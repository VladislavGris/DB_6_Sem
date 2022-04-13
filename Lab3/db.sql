use Storages;
go
alter table StoredProducts
add date_from date;
alter table StoredProducts
add date_to date;
update StoredProducts
set date_from = '2022-03-05',
	date_to = '2022-09-05';
