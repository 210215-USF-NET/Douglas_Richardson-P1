drop table batch;
drop table trainers;
drop table associates;

create table associates(
	id int identity primary key,
	associateName nvarchar(100) not null,
	associateLocation varchar(2) not null,
	revaPoints int not null
);
create table trainers(
	id int identity primary key,
	trainerName nvarchar(100) not null,
	campusLocation varchar(3) not null,
	caffeineLevel int not null
);
create table batch(
	id int identity primary key,
	associateID int references associates(id),
	trainerID int references trainers(id),
);

insert into associates(associateName,associateLocation,revaPoints) 
values ('john', 'ga', -10),('mary', 'ga', 10),('kerry', 'ga', 5),('michael', 'ga', 30),('nick', 'tx', 45);
insert into trainers(trainerName,campusLocation,caffeineLevel) 
values ('master', 'UNG', 50),('yeehaw', 'UT', 20);
insert into batch(associateID,trainerID) 
values (1,1),(2,1),(3,1);

--2
select associateName from associates where associateLocation='ga';
--3
select associateLocation, count(associateName) as associateAmount from associates group by associateLocation;
--4a
select trainerName from trainers left outer join batch on trainers.id=trainerID where trainerID is null;
--4b
select associateName from associates  left outer join batch on associates.id=associateID where trainerID is not null;
--4c
select associateName from associates left outer join batch on associates.id=associateID where associateID is null;
