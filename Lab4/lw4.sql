drop procedure AddStorage;
go
create procedure AddStorage @location nvarchar(300),
							@capacity int,
							@freeSpace float,
							@point nvarchar(100)
as
begin
	set nocount on;
	begin try
		exec StorageCapacityCheck @capacity, @freeSpace;
		declare @g geometry = GEOMETRY::STGeomFromText(@point,0);
		set @g.STSrid = 4326;
		insert into Storages(location, capacity, free_space, point)
		output Inserted.Id
		values (@location,@capacity,@freeSpace,GEOMETRY::STGeomFromText(@point,0));
	end try
	begin catch
		print error_message();
		throw;
	end catch

end;
go
--53.945802, 27.603850
exec AddStorage 'г.Минск', '100','100','POINT(27.603850 53.945802)';
--53.669765, 23.787200
exec AddStorage 'г.Гродно', '100','100','POINT(23.787200 53.669765)';
--52.422623, 30.970893
exec AddStorage 'г.Гомель', '100','100','POINT(30.970893 52.422623)';
--52.099986, 23.751585
exec AddStorage 'г.Брест', '100','100','POINT(23.751585 52.099986)';
go

go
--DECLARE @g geometry= 'POINT(23.751585 52.099986)'; 
--select @g.STSrid;
--set @g.STSrid = 4326;
--select @g.STSrid;
--select @g;
--select ogr_geometry from gadm40_blr_0
--union all
--SELECT @g.STBuffer(0.05);
----4326
--select ogr_geometry from gadm40_blr_2
--union all
--select Point from storages

go
drop procedure GetStorageMap
go
create procedure GetStorageMap
as
begin
	set nocount on;
	create table #Points(point geometry);
	declare points_cursor cursor for
	select Point from storages;
	declare @g geometry;
	open points_cursor
	fetch next from points_cursor
	into @g;
	while @@FETCH_STATUS = 0
	begin
		set @g.STSrid = 4326;
		insert into #Points values (@g);
		fetch next from points_cursor
		into @g;
	end
	select ogr_geometry from gadm40_blr_2
	union all
	select point.STBuffer(0.07) from #Points;
	
	close points_cursor
	drop table #Points;
end;

exec GetStorageMap

go
drop procedure GetCoverageMap
go
create procedure GetCoverageMap
as
begin
	declare points_cursor cursor for
	select Point from storages;
	declare @cur geometry;
	declare @union geometry = null;
	declare @firstTime bit = 1;
	open points_cursor
	fetch next from points_cursor
	into @cur;
	while @@FETCH_STATUS = 0
	begin
		if @firstTime <> 1
			set @union = @union.STUnion(@cur.STBuffer(1.2))
		else
		begin
			set @union = @cur.STBuffer(1.2);
			set @firstTime = 0;
		end
		fetch next from points_cursor
		into @cur;
	end
	set @union.STSrid = 4326;
	
	select ogr_geometry from gadm40_blr_2
	union all
	select @union;

	--declare @inter geometry;
	--select @inter = ogr_geometry from gadm40_blr_0
	--select @inter.STIntersection(@union)
	--set @inter.STSrid = 4326;
	
	close points_cursor
end

exec GetCoverageMap

go
drop procedure CalculateShortestPath;
go
create procedure CalculateShortestPath(@p1 geometry, @p2 geometry)
as
begin
	declare @distance float = @p1.STDistance(@p2);
	declare @line geometry;
	select @distance as 'Distance';
	select @line = @p1.ShortestLineTo(@p2);
	set @line.STSrid = 4326;
	select ogr_geometry from gadm40_blr_2
	union all
	select @line.STBuffer(0.03);
end

declare @pt1 geometry, @pt2 geometry;
select @pt1 = Point from Storages where id = 27;
select @pt2 = Point from Storages where id = 28;
exec CalculateShortestPath @pt1, @pt2;

create spatial index index_shape
on gadm40_blr_2(ogr_geometry)
using geometry_grid
with (bounding_box = (xmin=20, ymin = 50, xmax=35, ymax=58),
grids= (LOW,LOW,MEDIUM,HIGH),
PAD_INDEX = ON);