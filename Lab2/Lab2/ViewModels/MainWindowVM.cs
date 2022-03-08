using Lab2.Models;
using Lab2.Utils;
using Lab2.Utils.Commands;
using Lab2.ViewModels.Base;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Windows.Input;

namespace Lab2.ViewModels
{
    internal class MainWindowVM : ViewModelBase
    {
        private string _connectionString = "Server=DESKTOP-LRSVKUO;Database=Storages;Trusted_Connection=True;Encrypt=false;";
        #region CustomerProperties
        #region CustomerRepository

        private CustomerRepository _customerRepo;

        public CustomerRepository CustomerRepo
        {
            get => _customerRepo;
            set => Set(ref _customerRepo, value);
        }

        #endregion
        #region SelectedCustomer

        private Customer _selectedCustomer = new Customer();

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set => Set(ref _selectedCustomer, value);
        }

        #endregion
        #region ToAddCustomer

        private Customer _toAddCustomer = new Customer();

        public Customer ToAddCustomer
        {
            get => _toAddCustomer;
            set => Set(ref _toAddCustomer, value);
        }

        #endregion
        #endregion
        #region CustomerCommands
        #region UpdateCustomerCommand

        public ICommand UpdateCustomerCommand { get; }

        private bool CanUpdateCustomerCommandExecute(object p) => SelectedCustomer?.Id != 0 && !string.IsNullOrEmpty(SelectedCustomer?.Name) && !string.IsNullOrEmpty(SelectedCustomer?.ContactData);

        private void OnUpdateCustomerCommandExecuted(object p)
        {
            CustomerRepo.Update(SelectedCustomer);
        }

        #endregion
        #region AddCustomerCommand

        public ICommand AddCustomerCommand { get; }

        private bool CanAddCustomerCommandExecute(object p) => !string.IsNullOrEmpty(ToAddCustomer?.Name) && !string.IsNullOrEmpty(ToAddCustomer?.ContactData);

        private void OnAddCustomerCommandExecuted(object p)
        {
            CustomerRepo.Insert(new Customer(ToAddCustomer.Name, ToAddCustomer.ContactData));
            ToAddCustomer = new Customer();
        }

        #endregion
        #region DeleteCustomerCommand

        public ICommand DeleteCustomerCommand { get; }

        private bool CanDeleteCustomerCommandExecute(object p) => true;

        private void OnDeleteCustomerCommandExecuted(object p)
        {
            CustomerRepo.Remove((int)p);
        }

        #endregion
        #endregion
        #region StorageProperties
        #region SelectedStorage

        private Storage _selectedStorage = new Storage();

        public Storage SelectedStorage
        {
            get => _selectedStorage;
            set => Set(ref _selectedStorage, value);
        }

        #endregion
        #region ToAddStorage

        private Storage _toAddStorage = new Storage();

        public Storage ToAddStorage
        {
            get => _toAddStorage;
            set => Set(ref _toAddStorage, value);
        }

        #endregion
        #endregion
        #region StorageCommands
        #region UpdateStorageCommand

        public ICommand UpdateStorageCommand { get; }

        private bool CanUpdateStorageCommandExecute(object p) => SelectedStorage?.Id != 0 && !string.IsNullOrEmpty(SelectedStorage?.Location) && SelectedStorage?.Capacity != 0;

        private void OnUpdateStorageCommandExecuted(object p)
        {
            StorageRepository.Update(SelectedStorage);
        }

        #endregion
        #region AddStorageCommand

        public ICommand AddStorageCommand { get; }

        private bool CanAddStorageCommandExecute(object p) => !string.IsNullOrEmpty(ToAddStorage?.Location) && ToAddStorage?.Capacity != 0;

        private void OnAddStorageCommandExecuted(object p)
        {
            StorageRepository.Insert(ToAddStorage);
            ToAddStorage = new Storage();
        }

        #endregion
        #region DeleteStorageCommand

        public ICommand DeleteStorageCommand { get; }

        private bool CanDeleteStorageCommandExecute(object p) => true;

        private void OnDeleteStorageCommandExecuted(object p)
        {
            StorageRepository.Remove((int)p);
        }

        #endregion
        #endregion

        #region ProductRepository

        private ProductRepository _productRepo;

        public ProductRepository ProductRepo
        {
            get => _productRepo;
            set => Set(ref _productRepo, value);
        }

        #endregion
        #region ProductTypeRepository

        private ProductTypeRepository _productTypeRepo;

        public ProductTypeRepository ProductTypeRepo
        {
            get => _productTypeRepo;
            set => Set(ref _productTypeRepo, value);
        }

        #endregion
        #region StorageRepository

        private StorageRepository _storageRepository;

        public StorageRepository StorageRepository
        {
            get => _storageRepository;
            set => Set(ref _storageRepository, value);
        }

        #endregion
        #region TariffRateRepository

        private TariffRateRepository _tariffRateRepo;

        public TariffRateRepository TariffRateRepo
        {
            get => _tariffRateRepo;
            set => Set(ref _tariffRateRepo, value);
        }

        #endregion

        public MainWindowVM()
        {
            #region CustomerCommands
            UpdateCustomerCommand = new LambdaCommand(OnUpdateCustomerCommandExecuted, CanUpdateCustomerCommandExecute);
            AddCustomerCommand = new LambdaCommand(OnAddCustomerCommandExecuted, CanAddCustomerCommandExecute);
            DeleteCustomerCommand = new LambdaCommand(OnDeleteCustomerCommandExecuted, CanDeleteCustomerCommandExecute);
            #endregion
            #region StorageCommands
            UpdateStorageCommand = new LambdaCommand(OnUpdateStorageCommandExecuted, CanUpdateStorageCommandExecute);
            AddStorageCommand = new LambdaCommand(OnAddStorageCommandExecuted, CanAddStorageCommandExecute);
            DeleteStorageCommand = new LambdaCommand(OnDeleteStorageCommandExecuted, CanDeleteStorageCommandExecute);
            #endregion

            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            CustomerRepo = new CustomerRepository(connection);
            StorageRepository = new StorageRepository(connection);
            CustomerRepo.Get();
            StorageRepository.Get();
        }

    }
}
