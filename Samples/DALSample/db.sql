-- run this in master db
create database dals
go
use dals
go
create table humans
(
id int identity primary key,
firstName nvarchar(20),
lastName nvarchar(20),
dateRegistered Date
)

go
create proc deleteAll
as
truncate table humans