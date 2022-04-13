use Storages;
go
alter table Storages
add LocationMap int null references gadm40_blr_1(ogr_fid);
go
alter table Storages
add Point geometry null
go
update Storages
set LocationMap = 4
where id = 6
insert into Storages values ('г.Брест',100,100,1);
insert into Storages values ('г.Гомель',100,100,2);

select [ogr_geometry],
		[ogr_geometry].ToString() as WKT,
		[ogr_geometry].STSrid as SRID
from [gadm40_blr_1]
where [ogr_fid] = 1;

declare @poly1 geometry = 'POLYGON ((1 1, 1 4, 4 4, 4 1, 1 1))';
declare @poly2 geometry = 'POLYGON ((2 2, 2 6, 6 6, 6 2, 2 2))';
declare @inters geometry = @poly1.STIntersection(@poly2);
declare @iskl geometry = @poly1.STDifference(@poly2);
select	@inters.STAsText() as 'Пересечение', 
		@iskl.STAsText() as 'Исключение';

declare @g geometry = geometry::STGeomFromText('POLYGON ((1 1, 1 4, 4 4, 4 1, 1 1))',0);
select @g.STBuffer(5) as Buffer, @g.BufferWithTolerance(5,0.5,0) as BufferWithTolerance;
declare @b geometry = geometry::STGeomFromText('POLYGON ((2 2, 2 6, 6 6, 6 2, 2 2))',0);
select @b.STBuffer(5) as Buffer, @b.BufferWithTolerance(5,0.5,0) as BufferWithTolerance;
declare @s geometry = geometry::STGeomFromText('POLYGON ((2 2, 4.0000000000000036 2.0000000000000107, 4 4, 2.0000000000000107 4.0000000000000036, 2 2))',0);
declare @f geometry = geometry::STGeomFromText('POLYGON ((1 1, 4 1, 4.0000000000000036 2.0000000000000107, 2 2, 2.0000000000000107 4.0000000000000036, 1 4, 1 1))',0);
select @s.STBuffer(5) as Buffer, @s.BufferWithTolerance(5,0.5,0) as BufferWithTolerance;
select @f.STBuffer(5) as Buffer, @f.BufferWithTolerance(5,0.5,0) as BufferWithTolerance;
--declare @poly1 geometry = 'POLYGON ((1 1, 1 4, 4 4, 4 1, 1 1))';
--declare @poly2 geometry = 'POLYGON ((2 2, 2 6, 6 6, 6 2, 2 2))';
--declare @inters geometry = @poly1.STIntersection(@poly2);
--declare @iskl geometry = @poly1.STDifference(@poly2);
--select @inters.STBuffer(5) as Buffer, @inters.BufferWithTolerance(5,0.5,0) as BufferWithTolerance;
--select @iskl.STBuffer(5) as Buffer, @iskl.BufferWithTolerance(5,0.5,0) as BufferWithTolerance;

declare @g geometry;
declare @h geometry;
declare @dist float;
select @g = ogr_geometry.STAsText() from gadm40_blr_1 where ogr_fid=4;
select @h = ogr_geometry.STAsText() from gadm40_blr_1 where ogr_fid=3;
select @g.STPointN(100).ToString();
select @h.STPointN(100).ToString();
select @dist = @g.STPointN(100).STDistance(@h.STPointN(100));
select @dist as 'Distance';
select @dist as 'Расстояние', (select name_1 from gadm40_blr_1 where ogr_fid=3) as 'Город1', 
		name_1 as 'Город2' from gadm40_blr_1 where ogr_fid=4;

