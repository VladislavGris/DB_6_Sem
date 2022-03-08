using Lab2.Models;
using Microsoft.Data.SqlClient;
using System;

namespace Lab2.Utils
{
    internal class ProductTypeRepository : BaseRepository<ProductType>, IRepository<ProductType>
    {

        public ProductTypeRepository(SqlConnection connection) : base(connection) { }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public void Insert(ProductType entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductType entity)
        {
            throw new NotImplementedException();
        }


        protected sealed override string InsertProcedure => "AddProductType";
        protected sealed override string UpdateProcedure => "UpdateProductType";
        protected sealed override string RemoveProcedure => "RemoveProductType";
        protected sealed override string GetProcedure => "GetProductTypes";
    }
}
