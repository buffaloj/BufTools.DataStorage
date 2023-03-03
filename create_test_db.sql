CREATE DATABASE testdb;

go

USE testdb;

go

create table make (
id int NOT NULL PRIMARY KEY IDENTITY(1,1),
manufacturer varchar(255)
)
go

create table model (
id int NOT NULL PRIMARY KEY IDENTITY(1,1),
name varchar(255),
make_id int NOT NULL FOREIGN KEY REFERENCES make(id)
)
go

create table vehicle
(
id int NOT NULL PRIMARY KEY IDENTITY(1,1),
model_id int NOT NULL FOREIGN KEY REFERENCES model(id),
vin varchar(17) NOT NULL
)
go

create table person (
id int NOT NULL PRIMARY KEY IDENTITY(1,1),
first_name varchar(255),
last_name varchar(255)
)
go

create table person_car (
person_id int NOT NULL FOREIGN KEY REFERENCES person(id),
vehicle_id int NOT NULL FOREIGN KEY REFERENCES vehicle(id)
PRIMARY KEY (person_id, vehicle_id)
)
go

insert into dbo.make (manufacturer)
values ('Ford')
go

insert into make (manufacturer)
values ('General Motors')
go

insert into make (manufacturer)
values ('Chevy')
go

insert into make (manufacturer)
values ('Toyota')
go

insert into model (name, make_id)
values ('Mustang', 1)
go

insert into model (name, make_id)
values ('Taurus', 1)
go

insert into vehicle (model_id, vin)
values (1, '12345678901234567')
go

insert into vehicle (model_id, vin)
values (1, '22222222222222222')
go

insert into person (first_name, last_name)
values ('Jeremy', 'Shull')
go

insert into person_car (person_id, vehicle_id)
values (1, 1)
go

insert into person_car (person_id, vehicle_id)
values (1, 2)
go

create function number_of_cars_owned(@person_id int)
returns int as 
begin

DECLARE @count int
SELECT @count = COUNT(*)
    FROM person_car as pc
WHERE pc.person_id = @person_id
RETURN(@count)

end
go


CREATE FUNCTION owners_of_vehicle (@vin varchar(17))
  RETURNS Table
AS
return 
select person.first_name, person.last_name from person
inner join person_car on person_car.person_id = person.id
inner join vehicle on vehicle.id = person_car.vehicle_id
where vehicle.vin = @vin
go

create view vehicledata
as
select v.id, mak.manufacturer as make, m.name as model, v.vin from dbo.vehicle as v
inner join dbo.model m on v.model_id = m.id
inner join dbo.make mak on mak.id = m.make_id
go

create or alter procedure get_owners
(
	@vin varchar(17)
)
as 
select person.first_name, person.last_name from person
inner join person_car on person_car.person_id = person.id
inner join vehicle on vehicle.id = person_car.vehicle_id
where vehicle.vin = @vin

go


