--DROP TABLE [USERS]
--USE [testStore]
--GO

--SELECT * FROM USERS
/*
USE [master]
GO
CREATE LOGIN [ShoeStoredbAppUsr] WITH PASSWORD = 'ShoeApp$123';
GO
CREATE USER [ShoeStoredbAppUsr] FOR LOGIN [ShoeStoredbAppUsr];  
GO 

USE [yourdb drop down]
GO
CREATE USER [ShoeStoredbAppUsr] FOR LOGIN [ShoeStoredbAppUsr]; 
GO

--DO NOT USE THIS
EXEC sp_addrolemember 'db_datareader', 'ShoeStoredbAppUsr'
EXEC sp_addrolemember 'db_datawriter', 'ShoeStoredbAppUsr'
EXEC sp_addrolemember 'db_ddladmin', 'ShoeStoredbAppUsr'

--USE THIS
ALTER ROLE db_datareader ADD MEMBER ShoeStoredbAppUsr
GO
ALTER ROLE db_datawriter ADD MEMBER ShoeStoredbAppUsr
GO
*/

--DROP TABLE [USERS]
CREATE TABLE [USERS] (
	[username] [VARCHAR](50) PRIMARY KEY,
	[password] [VARCHAR](50) NOT NULL,
)
ALTER TABLE [USERS] ADD  [role] [VARCHAR](50) NOT NULL;

--DROP TABLE [SHOES]
CREATE TABLE [SHOES] (
	[shoeID] [INT] IDENTITY(1,1) PRIMARY KEY ,
	[brand] [VARCHAR](50),
	[shoe_name] [VARCHAR](50) NOT NULL,
	[price] [SMALLMONEY] NOT NULL,
	[color] [VARCHAR](25),
	[shoe_description] [VARCHAR](1000)
)

----DROP TABLE [CART]
--CREATE TABLE [CART] (
--	[cartID] [INT] IDENTITY(1,1) PRIMARY KEY,
--	[cart_user] [VARCHAR](50) REFERENCES [USERS](username) ON DELETE CASCADE,
--)

--DROP TABLE [SHOE_IN_CART]
CREATE TABLE [SHOE_IN_CART] (
	[cartID] [INT] IDENTITY(1,1),
	[username] [VARCHAR](50) REFERENCES [USERS](username) ON DELETE CASCADE,
	[shoeID] [INT] REFERENCES [SHOES](shoeID) ON DELETE CASCADE
)

--DROP TABLE [REVIEWS]
CREATE TABLE [REVIEWS] (
	[reviewID] [INT] IDENTITY(1,1),
	[reviewed_shoe] [INT] REFERENCES [SHOES](shoeID) ON DELETE CASCADE,
	[reviewer] [VARCHAR](50) REFERENCES [USERS](username) ON DELETE CASCADE,
	[rating] [INT] NOT NULL,
	[rating_description] [VARCHAR](1000)
)



--INSERT INTO [SHOES] (brand, shoe_name, price, color, shoe_description)
--VALUES
--('Nike', 'Air Jordan IV', 400.00, 'Red', 'The Air Jordan 4 “Fire Red 2020” is a November 2020 release of an original colorway of Michael Jordan’s fourth signature shoe'),
--('Adidas', 'Ultra Boosts', 120.00, 'Black', 'Mens running shoes for dominant performance and all-day comfort'),
--('Nike', 'Dunk Low Retro', 115.00, 'Polar Blue', 'Classic Nike sneaker with colors representing top universities to show off some school spirit.')


--INSERT INTO [USERS] ([username], [password], [role])
--VALUES
--('user0', 'password0', 'appuser'),
--('user1', 'password1', 'appuser'),
--('user2', 'password2', 'appuser'),
--('manager1', 'passwordmanager', 'manager')

INSERT INTO [REVIEWS] ([reviewed_shoe], [reviewer], [rating], [rating_description])
VALUES
(3, 'user0', 6, 'cool shoe, liked it a lot!'),
(3, 'user0', 2, 'didnt work well on me'),
(3, 'user0', 10, 'my favorite pair! will buy more!'),
(3, 'user0', 6, 'super mid')
