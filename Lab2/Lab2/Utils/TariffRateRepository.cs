using Lab2.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.Utils
{
    internal class TariffRateRepository : BaseRepository<TariffRate>, IRepository<TariffRate>
    {
        public TariffRateRepository(SqlConnection connection) : base(connection) { }

        public void Get()
        {
            throw new NotImplementedException();
        }

        public void Insert(TariffRate entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(TariffRate entity)
        {
            throw new NotImplementedException();
        }

        protected override string InsertProcedure => "AddTariffRate";
        protected override string UpdateProcedure => "UpdateTariffRate";
        protected override string RemoveProcedure => "RemoveTariffRate";
        protected override string GetProcedure => "GetTariffRates";
    }
}
