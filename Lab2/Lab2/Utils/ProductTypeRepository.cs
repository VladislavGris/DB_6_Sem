using Lab2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Windows;

namespace Lab2.Utils
{
    internal class ProductTypeRepository : BaseRepository<ProductType>, IRepository<ProductType>
    {

        public ProductTypeRepository(SqlConnection connection) : base(connection) { }

        public void Get()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                using (SqlDataReader reader = _getCommand.ExecuteReader())
                {
                    Items.Clear();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Items.Add(new ProductType(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }
                }
            }
        }

        public void Insert(ProductType entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _insertCommand.Parameters.Clear();
                SqlParameter typeParam = new SqlParameter
                {
                    ParameterName = "@type",
                    Value = entity.Name
                };
                _insertCommand.Parameters.Add(typeParam);
                entity.Id = (int)_insertCommand.ExecuteScalar();
                Items.Add(entity);
            }
        }

        public void Remove(int id)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
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
                        _removeCommand.ExecuteNonQuery();
                        Items.Remove(item);
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
        }

        public void Update(ProductType entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                var item = Items.FirstOrDefault(c => c.Id == entity.Id);
                if (item == null)
                {
                    return;
                }
                item = new ProductType(entity.Id, entity.Name);
                _updateCommand.Parameters.Clear();
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = entity.Id
                };
                SqlParameter typeParam = new SqlParameter
                {
                    ParameterName = "@type",
                    Value = entity.Name
                };
                _updateCommand.Parameters.Add(idParam);
                _updateCommand.Parameters.Add(typeParam);
                try
                {
                    _updateCommand.ExecuteNonQuery();
                    MessageBox.Show("Тип обновлен");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        protected sealed override string InsertProcedure => "AddProductType";
        protected sealed override string UpdateProcedure => "UpdateProductType";
        protected sealed override string RemoveProcedure => "RemoveProductType";
        protected sealed override string GetProcedure => "GetProductTypes";
    }
}
