using Lab2.Models;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace Lab2.Utils
{
    internal class StorageRepository : BaseRepository<Storage>, IRepository<Storage>
    {
        public StorageRepository(SqlConnection connection) : base(connection) { }

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
                            Items.Add(new Storage(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDouble(3)));
                        }
                    }
                }
            }
        }

        public void Insert(Storage entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _insertCommand.Parameters.Clear();
                SqlParameter locationParam = new SqlParameter
                {
                    ParameterName = "@location",
                    Value = entity.Location
                };
                SqlParameter capacityParam = new SqlParameter
                {
                    ParameterName = "@capacity",
                    Value = entity.Capacity
                };
                SqlParameter freeSpaceParam = new SqlParameter
                {
                    ParameterName = "@freeSpace",
                    Value = (float)entity.Capacity
                };
                _insertCommand.Parameters.Add(locationParam);
                _insertCommand.Parameters.Add(capacityParam);
                _insertCommand.Parameters.Add(freeSpaceParam);
                try
                {
                    entity.Id = (int)_insertCommand.ExecuteScalar();
                    entity.FreeSpace = entity.Capacity;
                    Items.Add(entity);
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
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

        public void Update(Storage entity)
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                var item = Items.FirstOrDefault(c => c.Id == entity.Id);
                if (item == null)
                {
                    return;
                }
                double freeSpace = item.FreeSpace - (item.Capacity - entity.Capacity);
                item = new Storage(entity.Id, entity.Location, entity
                    .Capacity, freeSpace);
                _updateCommand.Parameters.Clear();
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    Value = entity.Id
                };
                SqlParameter locationParam = new SqlParameter
                {
                    ParameterName = "@location",
                    Value = entity.Location
                };
                SqlParameter capacityParam = new SqlParameter
                {
                    ParameterName = "@capacity",
                    Value = entity.Capacity
                };
                _updateCommand.Parameters.Add(idParam);
                _updateCommand.Parameters.Add(locationParam);
                _updateCommand.Parameters.Add(capacityParam);
                try
                {
                    item.FreeSpace = (double)_updateCommand.ExecuteScalar();
                    MessageBox.Show("Склад обновлен");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        protected override string InsertProcedure => "AddStorage";
        protected override string UpdateProcedure => "UpdateStorages";
        protected override string RemoveProcedure => "RemoveStorage";
        protected override string GetProcedure => "GetStorages";
    }
}
