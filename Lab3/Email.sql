exec SendMail 'Hello from SQL';
go
create trigger AnyChangeTrigger on Storages
for insert, delete, update
as
begin
	declare @insName nvarchar(300);
	declare @delName nvarchar(300);
	select top 1 @insName = location
	from inserted
	select top 1 @delName = location
	from deleted;
	declare @message nvarchar(600) = concat(@insName, ' ', @delName);
	exec SendMail @message;
end
insert into Storages(location, capacity, free_space) values('Mogilev', 200,200);
update Storages
set location = 'Borisov'
where location = 'Mogilev'
select * from Storages;
delete Storages
where location = 'Borisov'