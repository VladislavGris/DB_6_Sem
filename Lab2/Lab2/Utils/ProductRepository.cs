using Lab2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab2.Utils
{
    internal class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(SqlConnection connection) : base(connection) { }

        public void Get(int storageId)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _getCommand.Parameters.Clear();
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@storageId",
                    Value = storageId
                };
                _getCommand.Parameters.Add(idParam);
                using (SqlDataReader reader = _getCommand.ExecuteReader())
                {
                    Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Items.Add(new Product(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetString(5), reader.GetInt32(6), reader.GetDouble(7), reader.GetInt32(8)));
                        }
                    }
                }
            }
        }

        public double Insert(Product entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _insertCommand.Parameters.Clear();
                SqlParameter customerParam = new SqlParameter
                {
                    ParameterName = "@customerId",
                    Value = entity.CustomerId
                };
                SqlParameter typeParam = new SqlParameter
                {
                    ParameterName = "@typeId",
                    Value = entity.ProductTypeId
                };
                SqlParameter storageParam = new SqlParameter
                {
                    ParameterName = "@storageId",
                    Value = entity.StorageId
                };
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = entity.Name
                };
                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count",
                    Value = entity.Count
                };
                SqlParameter weightParam = new SqlParameter
                {
                    ParameterName = "@weight",
                    Value = entity.Weight
                };
                _insertCommand.Parameters.Add(customerParam);
                _insertCommand.Parameters.Add(typeParam);
                _insertCommand.Parameters.Add(storageParam);
                _insertCommand.Parameters.Add(nameParam);
                _insertCommand.Parameters.Add(countParam);
                _insertCommand.Parameters.Add(weightParam);
                double freeSpace = 0;
                try
                {
                    
                    using (SqlDataReader reader = _insertCommand.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            reader.Read();
                            freeSpace = reader.GetDouble(0);
                            entity.Id = reader.GetInt32(1);
                        }
                    }
                    Items.Add(entity);
                    return freeSpace;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return -1;
            }
            return -1;
        }

        public double Remove(int id)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                double freeSpace = 0;
                var item = Items.Where(c => c.Id == id).FirstOrDefault();
                if (item != null)
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
                        freeSpace = (double)_removeCommand.ExecuteScalar();
                        Items.Remove(item);
                        return freeSpace;
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                return -1;
            }
            return -1;
        }

        public double Update(Product entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                var item = Items.FirstOrDefault(c => c.Id == entity.Id);
                if (item == null)
                {
                    return -1;
                }
                double freeSpace = 0;
                _updateCommand.Parameters.Clear();
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = entity.Id
                };
                SqlParameter customerParam = new SqlParameter
                {
                    ParameterName = "@customerId",
                    Value = entity.CustomerId
                };
                SqlParameter typeParam = new SqlParameter
                {
                    ParameterName = "@typeId",
                    Value = entity.ProductTypeId
                };
                SqlParameter storageParam = new SqlParameter
                {
                    ParameterName = "@storageId",
                    Value = entity.StorageId
                };
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = entity.Name
                };
                SqlParameter countParam = new SqlParameter
                {
                    ParameterName = "@count",
                    Value = entity.Count
                };
                SqlParameter weightParam = new SqlParameter
                {
                    ParameterName = "@weight",
                    Value = entity.Weight
                };
                _updateCommand.Parameters.Add(idParam);
                _updateCommand.Parameters.Add(customerParam);
                _updateCommand.Parameters.Add(typeParam);
                _updateCommand.Parameters.Add(storageParam);
                _updateCommand.Parameters.Add(nameParam);
                _updateCommand.Parameters.Add(countParam);
                _updateCommand.Parameters.Add(weightParam);
                try
                {
                    freeSpace = (double)_updateCommand.ExecuteScalar();
                    MessageBox.Show("Продукт обновлен");
                    return freeSpace;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                return -1;
            }
            return -1;
        }

        protected sealed override string InsertProcedure => "AddStoredProduct";
        protected sealed override string UpdateProcedure => "UpdateStoredProduct";
        protected sealed override string RemoveProcedure => "RemoveStoredProduct";
        protected sealed override string GetProcedure => "GetProductsByStorage";
    }
}
