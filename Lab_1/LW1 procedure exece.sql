use Storages;
go
exec AddStorage '�.�����', 100,100;
exec UpdateStorages 5, '�.�����, ��-�.������������',600;
exec RemoveStorage 3;
exec GetStorages;
go
exec AddCustomer '������ ���� ��������11', 'email: iavnov@gmai.com, tel: +375291234567';
exec UpdateCustomer 1, '������ ���� ��������', 'email: iavnov.ivan@gmai.com, tel: +375291234567'
exec RemoveCustomer 1;
exec GetCustomers;
go
exec AddProductType '�������� �������';
exec UpdateProductType 1, '������� �������';
exec RemoveProductType 1;
exec GetProductTypes;
exec GetProductsByStorage 5;
go
exec AddStoredProduct 2,3,5,'������',10,1;
exec UpdateStoredProduct 1,1,1,1,'������',10,1;
exec RemoveStoredProduct 10;
exec GetStoredProducts;
go
exec AddTariffRate 1,0.5,0,1;
exec AddTariffRate 1,1.2,1,5;
exec UpdateTariffRate 1,0.5,0,1;
exec RemoveTariffRate 2;
exec GetTariffRates;
exec GetProductTypes