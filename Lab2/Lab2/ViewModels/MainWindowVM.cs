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

        #region CustomerRepository

        private CustomerRepository _customerRepo;

        public CustomerRepository CustomerRepo
        {
            get => _customerRepo;
            set => Set(ref _customerRepo, value);
        }

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
        #region SelectedCustomer

        private Customer _selectedCustomer = new Customer();

        public Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set=> Set(ref _selectedCustomer, value);
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

        #region UpdateCustomerCommand

        public ICommand UpdateCustomerCommand { get; }

        private bool CanUpdateCustomerCommandExecute(object p) => SelectedCustomer?.Id != 0 && !string.IsNullOrEmpty(SelectedCustomer?.Name) && !string.IsNullOrEmpty(SelectedCustomer?.ContactData);

        private void OnUpdateCustomerCommandExecuted(object p)
        {
            CustomerRepo.Update(SelectedCustomer);
            SelectedCustomer = new Customer();
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

        public MainWindowVM()
        {
            UpdateCustomerCommand = new LambdaCommand(OnUpdateCustomerCommandExecuted, CanUpdateCustomerCommandExecute);
            AddCustomerCommand = new LambdaCommand(OnAddCustomerCommandExecuted, CanAddCustomerCommandExecute);
            DeleteCustomerCommand = new LambdaCommand(OnDeleteCustomerCommandExecuted, CanDeleteCustomerCommandExecute);

            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            CustomerRepo = new CustomerRepository(connection);
            CustomerRepo.Get();
        }

    }
}
