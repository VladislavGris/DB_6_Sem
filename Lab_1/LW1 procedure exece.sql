use Storages;
go
exec AddStorage 'г.Минск', 100,100;
exec UpdateStorages 5, 'г.Минск, пр-т.Партизанский',600;
exec RemoveStorage 3;
exec GetStorages;
go
exec AddCustomer 'Иванов Иван Иванович11', 'email: iavnov@gmai.com, tel: +375291234567';
exec UpdateCustomer 1, 'Иванов Иван Иванович', 'email: iavnov.ivan@gmai.com, tel: +375291234567'
exec RemoveCustomer 1;
exec GetCustomers;
go
exec AddProductType 'Продукты питания';
exec UpdateProductType 1, 'Продукт питания';
exec RemoveProductType 1;
exec GetProductTypes;
exec GetProductsByStorage 5;
go
exec AddStoredProduct 2,3,5,'Молоко',10,1;
exec UpdateStoredProduct 1,1,1,1,'Молоко',10,1;
exec RemoveStoredProduct 10;
exec GetStoredProducts;
go
exec AddTariffRate 1,0.5,0,1;
exec AddTariffRate 1,1.2,1,5;
exec UpdateTariffRate 1,0.5,0,1;
exec RemoveTariffRate 2;
exec GetTariffRates;
exec GetProductTypes