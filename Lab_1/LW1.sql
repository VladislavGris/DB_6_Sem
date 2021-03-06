use master;
go
create database Storages;
go
use Storages;
go
create table Storages(
id int primary key identity(1,1),
location nvarchar(300) not null,
capacity int not null,
free_space float not null
);
go
create table Customers(
id int primary key identity(1,1),
name nvarchar(100) not null,
contact_data nvarchar(300) not null
);
go
create table ProductTypes(
id int primary key identity(1,1),
type_name nvarchar(100) not null
);
go
create table StoredProducts(
id int primary key identity(1,1),
customer_id int,
type_id int,
name nvarchar(100) not null,
count int not null,
weight float not null
);
go
alter table StoredProducts
add constraint fk_StoredProducts_ProductTypes
foreign key (type_id) references ProductTypes(id) on delete no action;
create index SPProductIdx 
on StoredProducts(type_id);
alter table StoredProducts
add constraint fk_StoredProducts_Customers
foreign key(customer_id) references Customers(id) on delete no action;
create index SPCustomerIdx
on StoredProducts(customer_id);
go
create table Storages_StoredProducts(
id int primary key identity(1,1),
storage_id int,
product_id int
);
go
alter table Storages_StoredProducts
add constraint fk_Storages
foreign key(storage_id) references Storages(id) on delete cascade;
create index SSPStorageIdx
on Storages_StoredProducts(storage_id);
alter table Storages_StoredProducts
add constraint fk_Products
foreign key(product_id) references StoredProducts(id) on delete cascade;
create index SSPProductIdx
on Storages_StoredProducts(product_id);
go
create table TariffRates(
id int primary key identity(1,1),
storage_id int,
price decimal(18,3) not null,
weight_from float,
weight_to float
)
go
alter table TariffRates
add constraint fk_TariffRates_Storages
foreign key(storage_id) references Storages(id) on delete no action;
create index TRIdx
on TariffRates(storage_id);
go
create procedure StorageCapacityCheck	@capacity int,
										@freeSpace int
as
begin
	set nocount on;
	if @freeSpace > @capacity
		throw 50001, '????????? ????? ?? ????? ????????? ???????????',1;
end;
go
create procedure StorageIdCheck @id int
as
begin
	set nocount on;
	declare @idCount int;
	select @idCount = count(id)
	from Storages
	where id = @id;
	if @idCount = 0
		throw 50002, '?????? ? ????????? Id ?? ??????????', 1;
end;
go
create procedure AddStorage @location nvarchar(300),
							@capacity int,
							@freeSpace float
as
begin
	set nocount on;
	begin try
		exec StorageCapacityCheck @capacity, @freeSpace;
		insert into Storages(location, capacity, free_space)
		output Inserted.Id
		values (@location,@capacity,@freeSpace);
	end try
	begin catch
		print error_message();
		throw;
	end catch

end;
go
create procedure RemoveStorage @id int
as
begin
	set nocount on;
	begin try
		exec StorageIdCheck @id;
		delete Storages
		where id = @id;
	end try
	begin catch
		print error_message();
		throw;
	end catch
end;
go
create procedure UpdateStorages @id int,
								@location nvarchar(300),
								@capacity int
as
begin
	set nocount on;
	begin try
		exec StorageIdCheck @id;
		update Storages
		set location = @location,
		capacity = @capacity
		where id = @id;
	end try
	begin catch
		print error_message();
		throw;
	end catch
end;

go
create trigger UpdateStorageFreeSpace
on Storages
after update
as
begin
	declare @old int, @new int, @id int;
	select top 1 @old = capacity, @id = id
	from Deleted;
	select top 1 @new = capacity
	from Inserted;
	if @old <> @new
	begin
		update Storages
		set free_space = free_space - (@old - @new)
		where id = @id;
		select free_space
		from Storages
		where id = @id;
	end

end;
drop trigger UpdateStorageFreeSpace
go
create procedure GetStorages 
as
begin
	set nocount on;
	select id, location, capacity, free_space
	from Storages;
end;
go
create procedure CheckCustomerId @id int
as
begin
	set nocount on;
	declare @idCount int;
	select @idCount = count(id)
	from Customers
	where id = @id;
	if @idCount = 0
		throw 50003, '??????????? ? ????????? Id ?? ??????????', 1
end;
go
create procedure AddCustomer	@name nvarchar(100),
								@contactData nvarchar(300)
as
begin
	set nocount on;
	insert into Customers(name, contact_data)
	output Inserted.Id
	values (@name, @contactData);
end;
go
create procedure UpdateCustomer @id int,
								@name nvarchar(100),
								@contactData nvarchar(300)
as
begin
	set nocount on;
	exec CheckCustomerId @id;
	update Customers
	set name = @name,
		contact_data = @contactData
	where id = @id;
end;
go
create procedure RemoveCustomer @id int
as
begin
	set nocount on;
	exec CheckCustomerId @id;
	delete Customers
	where id = @id;
end;
go
create procedure GetCustomers
as
begin
	set nocount on;
	select id, name, contact_data
	from Customers;
end;
go
create procedure CheckProductTypeId @id int
as
begin
	set nocount on;
	declare @idCount int;
	select @idCount = count(id)
	from ProductTypes
	where id = @id;
	if @idCount = 0
		throw 50004, '???? ????????? ? ????????? Id ?? ??????????', 1;
end;
go
create procedure AddProductType @type nvarchar(100)
as
begin
	set nocount on;
	insert into ProductTypes(type_name)
	output Inserted.Id
	values (@type);
end;
go
create procedure UpdateProductType	@id int,
									@type nvarchar(100)
as
begin
	set nocount on;
	exec CheckProductTypeId @id;
	update ProductTypes
	set type_name = @type
	where id = @id;
end;
go
create procedure RemoveProductType @id int
as
begin
	set nocount on;
	exec CheckProductTypeId @id;
	delete ProductTypes
	where id = @id;
end;
go
create procedure GetProductTypes
as
begin
	set nocount on;
	select id, type_name
	from ProductTypes;
end;
go
create procedure CheckStoredProductId @id int
as
begin
	set nocount on;
	declare @idCount int;
	select @idCount = count(id)
	from StoredProducts
	where id = @id;
	if @idCount = 0
		throw 50005, '???????? ? ????????? Id ?? ??????????', 1;
end;
go
create procedure CheckStorageFreeSpace @storageId int, @space float
as
begin
	set nocount on;
	declare @freeSpace float;
	select top 1 @freeSpace = free_space
	from Storages
	where id = @storageId;
	if @freeSpace < @space
		throw 50006, '?? ?????? ?? ?????????? ????? ??? ?????????? ?????????',1;
end;
go
create procedure AddStoredProduct	@customerId int,
									@typeId int,
									@storageId int,
									@name nvarchar(100),
									@count int,
									@weight float
as
begin
	set nocount on;
	exec CheckProductTypeId @typeId;
	exec CheckCustomerId @customerId;
	exec StorageIdCheck @storageId;
	begin tran;
	declare @isertedProductId int;
	insert into StoredProducts(customer_id,type_id,name,count,weight)
	values (@customerId,@typeId,@name,@count,@weight)
	set @isertedProductId = @@IDENTITY;
	insert into Storages_StoredProducts(storage_id, product_id)
	values (@storageId,@isertedProductId);
	declare @needSpace float = @count*@weight;
	exec CheckStorageFreeSpace @storageId, @needSpace
	update Storages
	set free_space = free_space - @needSpace
	commit;
	select top 1 free_space, @isertedProductId
	from Storages
	where id = @storageId;
end;
drop procedure AddStoredProduct
go
create function CalculateCurrentSpace(@id int)
returns float
as
begin
	declare @currentSpace float, @currCount int, @currWeight float;
	select top 1 @currCount = count, @currWeight = weight
	from StoredProducts
	where id = @id;
	select @currentSpace = @currCount*@currWeight;
	return @currentSpace;
end
go
create procedure UpdateStoredProduct	@id int,
										@customerId int,
										@typeId int,
										@storageId int,
										@name nvarchar(100),
										@count int,
										@weight float
as
begin
	set nocount on;
	exec CheckProductTypeId @typeId;
	exec CheckCustomerId @customerId;
	exec StorageIdCheck @storageId;
	exec CheckStoredProductId @id;

	declare @currentSpace float;
	select @currentSpace = dbo.CalculateCurrentSpace(@id);

	declare @spaceDiff float = @currentSpace - (@count*@weight);
	print @spaceDiff
	exec CheckStorageFreeSpace @storageId, @spaceDiff;

	begin tran
	update StoredProducts
	set customer_id = @customerId,
		type_id = @typeId,
		name = @name,
		count = @count,
		weight = @weight;
	update Storages_StoredProducts
	set storage_id = @storageId
	where product_id = @id;
	update Storages
	set free_space = free_space + @spaceDiff
	where id = @storageId;
	commit;
	select top 1 free_space
	from Storages
	where id = @storageId;
end;
drop procedure UpdateStoredProduct
go
create procedure RemoveStoredProduct @id int
as
begin
	set nocount on;
	exec CheckStoredProductId @id;
	begin tran
	declare @space float, @storageId int;
	select top 1 @space = p.count*p.weight, @storageId = sp.storage_id
	from StoredProducts p inner join Storages_StoredProducts sp on p.id = sp.product_id
	where p.id = @id;

	update Storages
	set free_space = free_space + @space
	where id = @storageId;
	delete Storages_StoredProducts
	where product_id = @id;
	commit
	select top 1 free_space
	from Storages
	where id = @storageId;
end;
drop procedure RemoveStoredProduct
go
create procedure GetStoredProducts
as
begin
	set nocount on;
	select p.id, p.customer_id, c.name, p.type_id, pt.type_name, p.name, p.count, p.weight, sp.storage_id
	from StoredProducts p inner join Storages_StoredProducts sp on p.id = sp.product_id
	inner join Customers c on c.id = p.customer_id
	inner join ProductTypes pt on pt.id = p.type_id;
end;
go
create procedure GetProductsByStorage @storageId int
as
begin
	set nocount on;
	select p.id, p.customer_id, c.name, p.type_id, pt.type_name, p.name, p.count, p.weight, sp.storage_id
	from StoredProducts p inner join Storages_StoredProducts sp on p.id = sp.product_id
	inner join Customers c on c.id = p.customer_id
	inner join ProductTypes pt on pt.id = p.type_id
	where sp.storage_id = @storageId;
end;
go
create procedure CheckTariffRateId @id int
as
begin
	set nocount on;
	declare @idCount int;
	select @idCount = id
	from TariffRates
	where id = @id;
	if @idCount = 0
		throw 50007, '???????? ?????? ? ????????? Id ?? ??????????', 1;
end;
go
create procedure AddTariffRate	@storageId int,
								@price decimal(18,3),
								@weightFrom float,
								@weightTo float
as
begin
	set nocount on;
	exec StorageIdCheck @storageId;
	insert into TariffRates(storage_id, price, weight_from, weight_to)
	values (@storageId,@price,@weightFrom,@weightTo);
end;
go
create procedure UpdateTariffRate	@id int,
									@price decimal(18,3),
									@weightFrom float,
									@weightTo float
as
begin
	set nocount on;
	exec CheckTariffRateId @id;
	update TariffRates
	set price = @price,
		weight_from = @weightFrom,
		weight_to = @weightTo
	where id = @id;
end;
go
create procedure RemoveTariffRate @id int
as
begin
	set nocount on;
	exec CheckTariffRateId @id;
	delete TariffRates
	where id = @id;
end;
go
create procedure GetTariffRates
as
begin
	set nocount on;
	select id, storage_id, price, weight_from, weight_to
	from TariffRates;
end;