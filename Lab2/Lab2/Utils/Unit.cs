using Lab2.Models;
using System.Collections.Generic;

namespace Lab2.Utils
{
    internal class Unit
    {
        private List<Customer> _customers;
        private List<Product> _products;
        private List<ProductType> _productTypes;
        private List<Storage> _storages;
        private List<TariffRate> _tariffRates;

        public Unit()
        {
            _customers = new List<Customer>();
            _products = new List<Product>();
            _storages = new List<Storage>();
            _tariffRates = new List<TariffRate>();
            _productTypes = new List<ProductType>();
        }

        public void LoadData()
        {

        }
    }
}
