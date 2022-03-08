using Lab2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal class StorageRepository : BaseRepository<Storage>, IRepository<Storage>
    {
        public StorageRepository(SqlConnection connection) : base(connection) { }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public void Insert(Storage entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Storage entity)
        {
            throw new NotImplementedException();
        }

        protected override string InsertProcedure => "AddStorage";
        protected override string UpdateProcedure => "UpdateStorages";
        protected override string RemoveProcedure => "RemoveStorage";
        protected override string GetProcedure => "GetStorages";
    }
}
