--1. Создать базу данных.
/*
CREATE DATABASE KursachDB               
COLLATE Cyrillic_General_CI_AS
GO
*/
--2. Создать в базе данных таблицы
CREATE TABLE Users
(
	UserID int NOT NULL IDENTITY,
	RoleID int NOT NULL,
	Login nvarchar(35) NOT NULL UNIQUE,
	Password nvarchar(35) NOT NULL,
	LastName nvarchar(35) NOT NULL,
	FirstName nvarchar(35) NOT NULL,
	Patronymic nvarchar(35) NOT NULL,
	PassportData int NOT NULL,
	MemberTicketID int NULL,
	ConclusionDate date NULL,
)  
GO

CREATE TABLE MemberTickets
(
	MemberTicketID int NOT NULL IDENTITY,
	Name nvarchar(35) NOT NULL,
	Cost int DEFAULT 0,
	ValidityPeriod int NOT NULL,
	GymID int NOT NULL,
)  
GO

CREATE TABLE Gyms
(                                      
	GymID int NOT NULL IDENTITY,  
	Name nvarchar(35) NOT NULL,
	Address nvarchar(60) NOT NULL,
	PhoneNumber int NOT NULL,
)
GO	

CREATE TABLE Roles
(
	RoleID int NOT NULL IDENTITY,
	Name nvarchar(35) NOT NULL,
)
GO

CREATE TABLE Trainings
(
	TrainingID int NOT NULL IDENTITY,
	Name nvarchar(35) NOT NULL,
	GymID int NOT NULL,
	CoachID int NOT NULL,
	TimeOfStarting DATETIME NOT NULL,
	TrainingDuration int NOT NULL,
)
GO

CREATE TABLE TrainingRegistrations
(
	ClientID int NOT NULL,  
	TrainingID int NOT NULL	
)
GO


--3. Индексация БД.

CREATE INDEX IX_Clients_ID on Users(UserID);

CREATE INDEX IX_MemberTickets_ID on MemberTickets(MemberTicketID);

CREATE INDEX IX_Gyms_ID on Gyms(GymID);

CREATE INDEX IX_Coaches_ID on Roles(RoleID);

CREATE INDEX IX_Trainings_ID on Trainings(TrainingID);

CREATE INDEX IX_Trainings_CoachID on Trainings(CoachID);

CREATE INDEX IX_TrainingRegistrations_TrainingID on TrainingRegistrations(TrainingID);

CREATE INDEX IX_TrainingRegistrations_ClientID on TrainingRegistrations(ClientID);


--4. Установить связи между таблицами.

--Clients
ALTER TABLE Users ADD 
	CONSTRAINT PK_Users PRIMARY KEY(UserID) 
GO

--MemberTickets
ALTER TABLE MemberTickets ADD 
	CONSTRAINT PK_MemberTickets PRIMARY KEY(MemberTicketID) 
GO

--Gyms
ALTER TABLE Gyms ADD 
	CONSTRAINT PK_Gyms PRIMARY KEY(GymID)
GO

--Roles
ALTER TABLE Roles ADD 
	CONSTRAINT PK_Roles PRIMARY KEY(RoleID)
GO

--Trainings
ALTER TABLE Trainings ADD 
	CONSTRAINT PK_Trainings PRIMARY KEY(TrainingID)
GO

--TrainingRegistrations
ALTER TABLE TrainingRegistrations ADD CONSTRAINT
	PK_TrainingRegistrations PRIMARY KEY
	(TrainingID,ClientID) 
GO

--Clients

ALTER TABLE Users ADD CONSTRAINT
	FK_Users_MemberTicketID FOREIGN KEY(MemberTicketID) 
	REFERENCES MemberTickets(MemberTicketID) 
	ON DELETE CASCADE  
GO

ALTER TABLE Users ADD CONSTRAINT
	FK_Users_RoleID FOREIGN KEY(RoleID) 
	REFERENCES Roles(RoleID) 
	ON DELETE CASCADE  
GO

--MemberTickets
ALTER TABLE MemberTickets ADD CONSTRAINT
	FK_MemberTickets_GymID FOREIGN KEY(GymID) 
	REFERENCES Gyms(GymID) 
	ON DELETE CASCADE  
GO


--Trainings
ALTER TABLE Trainings ADD CONSTRAINT
	FK_Trainings_GymID FOREIGN KEY(GymID) 
	REFERENCES Gyms(GymID) 
	ON DELETE CASCADE  
GO

ALTER TABLE Trainings ADD CONSTRAINT
	FK_Trainings_CoachID FOREIGN KEY(CoachID) 
	REFERENCES Users(UserID) 
	ON DELETE NO ACTION  
GO

--TrainingRegistrations
ALTER TABLE TrainingRegistrations ADD CONSTRAINT
	FK_TrainingRegistrations_TrainingID FOREIGN KEY(TrainingID) 
	REFERENCES Trainings(TrainingID) 
	ON DELETE NO ACTION  
GO

ALTER TABLE TrainingRegistrations ADD CONSTRAINT
	FK_TrainingRegistrations_ClientID FOREIGN KEY(ClientID) 
	REFERENCES Users(UserID) 
	ON DELETE CASCADE  
GO


/*

--5. Установить ограничения в таблицах.

--Products
ALTER TABLE Products ADD CONSTRAINT CK_Products 
	CHECK((RNumber !='')AND(RNumber >0) AND (RNumber<=9999999999)) 
GO


--Warehouses
ALTER TABLE Warehouses ADD CONSTRAINT CK_Warehouses 
	CHECK((WNumber>0) AND (WNumber<=9999999999) AND (Building>0) AND (Building<=99999))
GO


--Stores
ALTER TABLE Stores ADD CONSTRAINT CK_Stores 
	CHECK((Building>0) AND (Building<=99999))
GO


--StoreAccounting
ALTER TABLE StoreAccounting ADD CONSTRAINT CK_StoreAccounting
	CHECK((Quantity>=0) AND (Quantity<=9999999999) AND (Cost>=0) AND (Cost<=9999999999))
GO


--WarehouseAccounting
ALTER TABLE WarehouseAccounting ADD CONSTRAINT CK_WarehouseAccounting 
	CHECK((Quantity>=0) AND (Quantity<=9999999999) AND (Cost>=0) AND (Cost<=9999999999))
GO
*/