sp_configure'clr enabled', 1
GO
RECONFIGURE
GO
use Storages;
alter database Storages
set trustworthy on;
go
create assembly Storages_Assembly from N'G:\6_Sem\Lab3.dll' with permission_set=unsafe;
go
create procedure WriteFile @filename nvarchar(max), @data nvarchar(max)
as
external name Storages_Assembly.StoredProcedures.WriteProcedure;
go
create type dbo.StoragePlace
external name Storages_Assembly.[StorageType];
go
exec WriteFile @filename='G:\data2.txt', @data='datafdfd';
declare @place StoragePlace;
set @place = cast('15,2' as StoragePlace);
select @place.Rack as Rack, @place.Shelf as Shelf;