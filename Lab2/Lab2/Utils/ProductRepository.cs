using Lab2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal class ProductRepository : BaseRepository<Product>, IRepository<Product>
    {
        public ProductRepository(SqlConnection connection) : base(connection) { }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public void Insert(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }

        protected sealed override string InsertProcedure => "AddStoredProduct";
        protected sealed override string UpdateProcedure => "UpdateStoredProduct";
        protected sealed override string RemoveProcedure => "RemoveStoredProduct";
        protected sealed override string GetProcedure => "GetStoredProducts";
    }
}
