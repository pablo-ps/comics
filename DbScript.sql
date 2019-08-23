--create database comics; 
--use comics;
--go

create table configs(
	id int not null,
	publickey varchar(500) not null,
	privatekey varchar(500) not null,
	constraint PK_configs primary key(id),
);
go

-- create config
insert configs (id, publickey, privatekey) values (1, 'f9e1694096af6f326d1099e81ca61fb0', 'f011c73045fa6eefef31251d8ee56dfefc0debf9'); 
go

create table users(
	id int identity(1,1) not null,
	name varchar(50) not null,
	email varchar(150) not null,
	password varchar(150) not null,
	constraint PK_users primary key(id),
);
go

-- create user admin (password: abc123##)
insert users (name, email, password) values ('Admin', 'admin@comics.com', '01f41f6f596cf299400cbc4eea0b5213'); 
go

create table dbo.comics(
	id int not null,
	name varchar(500),
	likes int not null default 0,
	removed bit not null default 0,
	constraint PK_comics primary key(id)
);
go

-- insert comics (name, likes, removed) values ('Ghost-Spider (2019) #1', 1000, 0); 
-- insert comics (name, likes, removed) values ('Marvel Age Spider-Man Vol. 2: Everyday Hero (Digest)', 500, 0); 
-- insert comics (name, likes, removed) values ('Tony Stark: Iron Man #15', 600, 0); 
-- insert comics (name, likes, removed) values ('Invaders #7', 200, 0);
-- insert comics (name, likes, removed) values ('Absolute Carnage Vs. Deadpool #1', 180, 0); 
-- insert comics (name, likes, removed) values ('Deadpool Annual #1', 300, 0);
-- go