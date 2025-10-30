USE BlocketDB;
GO

-- Users
INSERT INTO Users (Username, PasswordHash, Email)
VALUES
('alice', 'hash123', 'alice@example.com'),
('bob', 'hash456', 'bob@example.com'),
('charlie', 'hash789', 'charlie@example.com');
GO

-- Categories
INSERT INTO Categories (Name)
VALUES ('Electronics'), ('Vehicles'), ('Furniture');
GO

-- Advertisements
INSERT INTO Advertisements (Title, Description, Price, SellerId, CategoryId)
VALUES
('iPhone 14 Pro', 'Brand new, sealed box', 1100.00, 1, 1),
('Used Honda Civic', '2014 model, good condition', 9000.00, 2, 2),
('Wooden Dining Table', '6-seater, solid oak', 500.00, 3, 3);
GO
