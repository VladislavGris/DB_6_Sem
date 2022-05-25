create table Report(
id int primary key identity(1,1),
xml_data XML
);
go
select * from Storages
select * from StoredProducts;
select * from Storages_StoredProducts
select * from Customers

exec AddStoredProduct 2,3,31,'Молоко', 10,1;
exec AddStoredProduct 2,3,32,'Хлеб', 20,1;
exec AddStoredProduct 2,3,33,'Творог', 30,1;
go
select s.id, location, capacity, free_space, 
product_id, customer_id, type_id, name, count, weight from Storages s
inner join Storages_StoredProducts ssp on ssp.storage_id = s.id
inner join StoredProducts sp on ssp.product_id = sp.id
for xml path('Storage');
go
create procedure GenerateXML(@xml_data XML output)
as
begin
	set nocount on;
	declare @x XML;
	set @x = (select s.id, location, capacity, free_space, 
		product_id, customer_id, type_id, name, count, weight, count*weight as total_weight, GETDATE() as date from Storages s
		inner join Storages_StoredProducts ssp on ssp.storage_id = s.id
		inner join StoredProducts sp on ssp.product_id = sp.id
		for xml path('Storage'));
	select @xml_data = @x;
end
go
declare @xml xml;
exec GenerateXML @xml output
select @xml
go
create procedure InsertXML
as
begin
	set nocount on;
	declare @xml xml;
	exec GenerateXML @xml output;
	insert into Report values(@xml)
end
go
exec InsertXML
select * from Report
delete Report;
go
create primary xml index PrimaryXML on Report(xml_data);

create xml index SecondaryPathIndex on Report(xml_data)
using xml index PrimaryXML for path;-- value, PROPERTY 
go
create table #Temp_Storages(
	location nvarchar(max),
	capacity int,
	free_space float,
)
insert into #Temp_Storages
select * 
from (
	select X.Y.value('(location)[1]','nvarchar(MAX)')  as location,
		   X.Y.value('(capacity)[1]','int')  as capacity,
		   X.Y.value('(free_space)[1]','float')  as free_space
	from Report r cross apply r.xml_data.nodes('Storage') as X(Y)
) as T;
select * from #Temp_Storages;
drop table #Temp_Storages