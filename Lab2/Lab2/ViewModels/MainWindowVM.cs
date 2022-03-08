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
        #region StorageRepository

        private StorageRepository _storageRepository;

        public StorageRepository StorageRepository
        {
            get => _storageRepository;
            set => Set(ref _storageRepository, value);
        }

        #endregion
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
        #region ProductTypeProperties
        #region ProductTypeRepository

        private ProductTypeRepository _productTypeRepo;

        public ProductTypeRepository ProductTypeRepo
        {
            get => _productTypeRepo;
            set => Set(ref _productTypeRepo, value);
        }

        #endregion
        #region SelectedProductType

        private ProductType _selectedProductType = new ProductType();

        public ProductType SelectedProductType
        {
            get => _selectedProductType;
            set => Set(ref _selectedProductType, value);
        }

        #endregion
        #region ToAddType

        private ProductType _toAddType = new ProductType();

        public ProductType ToAddType
        {
            get => _toAddType;
            set => Set(ref _toAddType, value);
        }

        #endregion
        #endregion
        #region ProductTypeCommands
        #region UpdateProductTypeCommand

        public ICommand UpdateProductTypeCommand { get; }

        private bool CanUpdateProductTypeCommandExecute(object p) => SelectedProductType?.Id != 0 && !string.IsNullOrEmpty(SelectedProductType?.Name);

        private void OnUpdateProductTypeCommandExecuted(object p)
        {
            ProductTypeRepo.Update(SelectedProductType);
        }

        #endregion
        #region AddProductTypeCommand

        public ICommand AddProductTypeCommand { get; }

        private bool CanAddProductTypeCommandExecute(object p) => !string.IsNullOrEmpty(ToAddType?.Name);

        private void OnAddProductTypeCommandExecuted(object p)
        {
            ProductTypeRepo.Insert(ToAddType);
            ToAddType = new ProductType();
        }

        #endregion
        #region DeleteProductTypeCommand

        public ICommand DeleteProductTypeCommand { get; }

        private bool CanDeleteProductTypeCommandExecute(object p) => true;

        private void OnDeleteProductTypeCommandExecuted(object p)
        {
            ProductTypeRepo.Remove((int)p);
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
            #region ProductTypeCommands
            UpdateProductTypeCommand = new LambdaCommand(OnUpdateProductTypeCommandExecuted, CanUpdateProductTypeCommandExecute);
            AddProductTypeCommand = new LambdaCommand(OnAddProductTypeCommandExecuted, CanAddProductTypeCommandExecute);
            DeleteProductTypeCommand = new LambdaCommand(OnDeleteProductTypeCommandExecuted, CanDeleteProductTypeCommandExecute);
            #endregion
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            CustomerRepo = new CustomerRepository(connection);
            StorageRepository = new StorageRepository(connection);
            ProductTypeRepo = new ProductTypeRepository(connection);
            CustomerRepo.Get();
            StorageRepository.Get();
            ProductTypeRepo.Get();
        }

    }
}
