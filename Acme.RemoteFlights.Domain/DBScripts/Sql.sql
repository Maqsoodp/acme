CREATE TABLE [dbo].[Flight]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [FlightNumber] VARCHAR(20) NOT NULL, 
    [Capacity] INT NOT NULL, 
    [DepartureCity] VARCHAR(50) NOT NULL, 
    [ArrivalCity] VARCHAR(50) NOT NULL, 
    [StartTime] time NOT NULL, 
    [EndTime] time NOT NULL
);
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Flight_FlightNumber')   
DROP INDEX IX_Flight_FlightNumber ON [dbo].[Flight];   
GO  

CREATE NONCLUSTERED INDEX IX_Flight_FlightNumber
    ON [dbo].[Flight] (FlightNumber);   
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Flight_DepartureCity')   
DROP INDEX IX_Flight_DepartureCity ON [dbo].[Flight];   
GO  

CREATE NONCLUSTERED INDEX IX_Flight_DepartureCity
    ON [dbo].[Flight] (DepartureCity);   
GO

IF EXISTS (SELECT name FROM sys.indexes WHERE name = N'IX_Flight_ArrivalCity')   
DROP INDEX IX_Flight_ArrivalCity ON [dbo].[Flight];   
GO  

CREATE NONCLUSTERED INDEX IX_Flight_ArrivalCity
    ON [dbo].[Flight] (ArrivalCity);   
GO


CREATE TABLE [dbo].[FlightBooking]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[FlightId] UNIQUEIDENTIFIER,
	[FlightDate] DATE NOT NULL, 
	[PassengerName]  VARCHAR(100) NOT NULL
);

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_FlightBooking_FlightId')   
    DROP INDEX IX_FlightBooking_FlightId ON [dbo].[FlightBooking];   
GO  

CREATE NONCLUSTERED INDEX IX_FlightBooking_FlightId   
    ON [dbo].[FlightBooking] (FlightId);   
GO

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_FlightBooking_FlightDate')   
    DROP INDEX IX_FlightBooking_FlightDate ON [dbo].[FlightBooking];   
GO  

CREATE NONCLUSTERED INDEX IX_FlightBooking_FlightDate
    ON [dbo].[FlightBooking] (FlightDate);   
GO  

IF EXISTS (SELECT name FROM sys.indexes  
            WHERE name = N'IX_FlightBooking_PassengerName')   
    DROP INDEX IX_FlightBooking_PassengerName ON [dbo].[FlightBooking];   
GO  

CREATE NONCLUSTERED INDEX IX_FlightBooking_PassengerName
    ON [dbo].[FlightBooking] (PassengerName);   
GO  

