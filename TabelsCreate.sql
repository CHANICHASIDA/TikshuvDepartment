create table Customer(
id varchar(50) primary key,
firstName varchar(50) NOT NULL,
lastName varchar(50) NOT NULL,
address varchar(50) NOT NULL,
numOfStreet int NOT NULL,
city varchar(50) NOT NULL,
mobile  varchar(50) NOT NULL,
phone varchar(50) NOT NULL,
birthday date NOT NULL);

create table Disease(
id varchar(50) primary key,
positiveDate date NOT NULL,
recoveryDate date NOT NULL);

create table Vaccination(
num int identity (1,1) primary key,
id varchar(50) NOT  NULL,
vaccinationDate date NOT NULL,
manufacturer varchar(50) NOT NULL,
 FOREIGN KEY (id) REFERENCES Customer(id)
);  
