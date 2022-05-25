use Storages;
go
-- Create teable of leders of storage with hierarchy id
create table Leaders(
node hierarchyid primary key clustered,
level as node.GetLevel() persisted,
leader_id int unique,
leader_name nvarchar(300) not null,
storage_id int references Storages(id)
)
go
-- Insert data into Storages table
select * from storages
insert into Storages(location, capacity, free_space) values('Minsk', 500,500);
insert into Storages(location, capacity, free_space) values('Grodno', 300,300);
insert into Storages(location, capacity, free_space) values('Brest', 200,200);
go
select  node.ToString() as NodeAsString,
		node as NodeAsBinary,
		level,
		leader_id,
		leader_name,
		storage_id
from Leaders;
-- Insert root leader into Leaders table
insert into Leaders(node, leader_id, leader_name, storage_id) values(hierarchyid::GetRoot(), 1, 'Avanov.I.I.', null);
go
-- Insert subleaders (level 2) into Leaders table
declare @LeaderNode hierarchyid;
select @LeaderNode = node from Leaders where leader_id = 1;
insert into Leaders(node, leader_id, leader_name, storage_id) values(@LeaderNode.GetDescendant(NULL, NULL), 2, 'Bvanov.I.I.', 31);

declare @LeaderLevel hierarchyid;
select @LeaderLevel = node from Leaders where leader_id = 2;
insert into Leaders(node, leader_id, leader_name, storage_id) values(@LeaderNode.GetDescendant(@LeaderLevel, NULL), 3, 'Cvanov.I.I.', 32);
select @LeaderLevel = node from Leaders where leader_id = 3;
insert into Leaders(node, leader_id, leader_name, storage_id) values(@LeaderNode.GetDescendant(@LeaderLevel, NULL), 4, 'Dvanov.I.I.', 33);
-- Insert level 3 leaders into Leaders table
select @LeaderLevel = node from Leaders where leader_id = 2;
insert into Leaders(node, leader_id, leader_name, storage_id) values(@LeaderLevel.GetDescendant(NULL, NULL), 5, 'Evanov.I.I.', 31);
select @LeaderLevel = node from Leaders where leader_id = 3;
insert into Leaders(node, leader_id, leader_name, storage_id) values(@LeaderLevel.GetDescendant(NULL, NULL), 6, 'Fvanov.I.I.', 32);
select @LeaderLevel = node from Leaders where leader_id = 4;
insert into Leaders(node, leader_id, leader_name, storage_id) values(@LeaderLevel.GetDescendant(NULL, NULL), 7, 'Gvanov.I.I.', 33);
go
-- Create procedure for all child nodes
create procedure GetNodesByParent @parentNode hierarchyid
as
begin
	set nocount on
	select  node.ToString() as NodeAsString,
		node as NodeAsBinary,
		level,
		leader_id,
		leader_name,
		storage_id
	from Leaders
	where node.IsDescendantOf(@parentNode) = 1;
end
go
-- Execute GetNodesByParent procedure
exec GetNodesByParent '/';
exec GetNodesByParent '/1/';
go
-- Create procedure to add child node
create procedure AddChildNode @leaderId int, @leader_name nvarchar(300), @storage_id int, @position hierarchyid
as
begin
	set nocount on;
	declare @level hierarchyid;
	select @level = max(node)
	from Leaders
	where node.GetAncestor(1) = @position;
	insert into Leaders(node, leader_id, leader_name, storage_id) values (@position.GetDescendant(@level,null), @leaderId, @leader_name, @storage_id);
end
go
-- Execute AddChildNode procedure
exec AddChildNode 8, 'Hvanov.I.I.', 31, '/1/1/';
exec AddChildNode 9, 'Ivanov.I.I.', 31, '/1/1/';
exec AddChildNode 10, 'Jvanov.I.I.', 31, '/1/1/';
go
-- Create procedure move branch
create procedure MoveBranch @oldParent hierarchyid, @newParent hierarchyid
as
begin
	set nocount on;
	create table #temp_table(
		node hierarchyid primary key clustered,
		level as node.GetLevel() persisted,
		leader_id int unique,
		leader_name nvarchar(300) not null,
		storage_id int references Storages(id)
	)

	insert into #temp_table(node, leader_id, leader_name, storage_id)
	select node, leader_id, leader_name, storage_id from Leaders
	where node.IsDescendantOf(@oldParent) = 1;

	declare @nnew hierarchyid = @newParent;
	set transaction isolation level serializable
	begin transaction
		select @nnew = @nnew.GetDescendant(max(node), NULL)
		from Leaders
		where node.GetAncestor(1) = @nnew;

		update #temp_table
		set node = node.GetReparentedValue(@oldParent, @nnew)
		where node.IsDescendantOf(@oldParent) = 1;
	commit transaction

	delete from Leaders
	where node.IsDescendantOf(@oldParent) = 1;

	insert into Leaders(node, leader_id, leader_name, storage_id)
	select node, leader_id, leader_name, storage_id 
	from #temp_table;

	drop table #temp_table
end
go
exec MoveBranch '/1/1/', '/2/';
