using Lab2.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Lab2.Utils
{
    internal class CustomerRepository : IRepository<Customer>
    {
        private static string InsertProcedure = "AddCustomer";
        private static string UpdateProcedure = "UpdateCustomer";
        private static string RemoveProcedure = "RemoveCustomer";
        private static string GetProcedure = "GetCustomers";

        private SqlConnection _connection;
        private SqlCommand _insertCommand;
        private SqlCommand _updateCommand;
        private SqlCommand _removeCommand;
        private SqlCommand _getCommand;

        private ObservableCollection<Customer> _customers;

        public ObservableCollection<Customer> Customers 
        {
            get { return _customers; }
        }

        public CustomerRepository(SqlConnection connection)
        {
            _customers = new ObservableCollection<Customer>();
            _connection = connection;

            _insertCommand = new SqlCommand(InsertProcedure, connection);
            _insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _updateCommand = new SqlCommand(UpdateProcedure, connection);
            _updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _removeCommand = new SqlCommand(RemoveProcedure, connection);
            _removeCommand.CommandType= System.Data.CommandType.StoredProcedure;
            _getCommand = new SqlCommand(GetProcedure, connection);
            _getCommand.CommandType = System.Data.CommandType.StoredProcedure;
        }

        public void Get()
        {
            if(_connection.State == System.Data.ConnectionState.Open)
            {
                using(SqlDataReader reader = _getCommand.ExecuteReader())
                {
                    _customers.Clear();
                    if(reader.HasRows)
                    {
                        while(reader.Read())
                        {
                            _customers.Add(new Customer(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                        }
                    }
                }
            }
        }

        public void Update(Customer entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                var customer = _customers.FirstOrDefault(c => c.Id == entity.Id);
                if(customer == null)
                {
                    return;
                }
                customer = new Customer(entity.Id, entity.Name, entity.ContactData);
                _updateCommand.Parameters.Clear();
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = customer.Id
                };
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = customer.Name
                };
                SqlParameter contactDataParam = new SqlParameter
                {
                    ParameterName = "@contactData",
                    Value = customer.ContactData
                };
                _updateCommand.Parameters.Add(idParam);
                _updateCommand.Parameters.Add(nameParam);
                _updateCommand.Parameters.Add(contactDataParam);
                try
                {
                    _updateCommand.ExecuteNonQuery();
                    MessageBox.Show("Потребитель обновлен");
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        public void Insert(Customer entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _insertCommand.Parameters.Clear();
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = entity.Name
                };
                SqlParameter contactDataParam = new SqlParameter
                {
                    ParameterName = "@contactData",
                    Value = entity.ContactData
                };
                _insertCommand.Parameters.Add(nameParam);
                _insertCommand.Parameters.Add(contactDataParam);
                entity.Id = (int)_insertCommand.ExecuteScalar();
                _customers.Add(entity);
            }
        }

        public void Remove(int id)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                var customer = _customers.Where(c => c.Id == id).FirstOrDefault();
                if(customer != null)
                {
                    
                    _removeCommand.Parameters.Clear();
                    SqlParameter idParam = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = id
                    };
                    _removeCommand.Parameters.Add(idParam);
                    try
                    {
                        _removeCommand.ExecuteNonQuery();
                        _customers.Remove(customer);
                    }
                    catch(SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                }
            }
        }
    }
}
