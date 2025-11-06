-- Create database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BlocketDB')
BEGIN
    CREATE DATABASE BlocketDB;
END
GO

USE BlocketDB;
GO

-- Users Table
CREATE TABLE Users (
    Id INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Categories Table
CREATE TABLE Categories (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);
GO

-- Advertisements Table
CREATE TABLE Advertisements (
    Id INT IDENTITY PRIMARY KEY,
    Title NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Price DECIMAL(10,2) NOT NULL,
    SellerId INT NOT NULL,
    CategoryId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    ImageUrl NVARCHAR(100),
    FOREIGN KEY (SellerId) REFERENCES Users(Id),
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);
GO
