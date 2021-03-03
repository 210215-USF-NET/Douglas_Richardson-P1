drop table managers;
drop table cart;
drop table orderTable;
drop table customers;
drop table locationTable;
drop table product;
drop table item;

create table managers(
	id int identity primary key,
	firstName nvarchar(100) not null,
	lastName nvarchar(100) not null,
	emailAddress nvarchar(100) not null
); 
create table customers(
	id int identity primary key,
	firstName nvarchar(100),
	lastName nvarchar(100),
	emailAddress nvarchar(100)
); 
create table product(
	id int identity primary key,
	productName nvarchar(50),
	price float,
	category int
);
create table locationTable(
	id int identity primary key,
	locationName nvarchar(50),
	locationAddress nvarchar(50),
);
create table item(
	id int identity primary key,
	quantity int,
	location_id int references locationTable(id),
	product_id int references product(id)
);
create table cart(
	id int identity primary key,
	customer_id int references customers(id),
	quantity int,
	location_id int references locationTable(id),
	total float,
	item_id int references item(id),
); 
create table orderTable(
	id int identity primary key,
	customer_id int references customers(id),
	quantity int,
	location_id int references locationTable(id),
	total float,
	item_id int references item(id)
);  

select * from customers;
select * from managers;
select * from locationTable;
select * from item;
select * from product;
select * from cart;
select * from orderTable;

delete from customers where id = 12;
delete from customers where id = 2;
delete from customers where id = 5;
delete from customers where id = 6;