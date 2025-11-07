USE BlocketDB;
GO

-- Users
INSERT INTO Users (Username, PasswordHash, Email)
VALUES
('alice', 'hash123', 'alice@example.com'),
('bob', 'hash456', 'bob@example.com'),
('charlie', 'hash789', 'charlie@example.com');
GO

-- Insert sample Categories
SET IDENTITY_INSERT Categories ON;
INSERT INTO Categories (Id, Name) VALUES
(1, 'men''s clothing'),
(2, 'women''s clothing'),
(3, 'electronics'),
(4, 'jewelery');
SET IDENTITY_INSERT Categories OFF;
GO

-- Insert sample Advertisements
SET IDENTITY_INSERT Advertisements ON;
INSERT INTO Advertisements (Id, Title, Description, Price, SellerId, CategoryId, ImageUrl) VALUES
(1, 'Fjallraven - Foldsack No. 1 Backpack, Fits 15 Laptops', 'Your perfect pack for everyday use and walks in the forest.', 109.95, 1, 1, 'https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_t.png'),
(2, 'Mens Casual Premium Slim Fit T-Shirts', 'Slim-fitting style, contrast raglan long sleeve, breathable and comfortable.', 22.30, 1, 1, 'https://fakestoreapi.com/img/71-3HjGNDUL._AC_SY879._SX._UX._SY._UY_t.png'),
(3, 'Mens Cotton Jacket', 'Great outerwear jacket suitable for all seasons.', 55.99, 1, 1, 'https://fakestoreapi.com/img/71li-ujtlUL._AC_UX679_t.png'),
(4, 'Mens Casual Slim Fit', 'Slim fit casual shirt with detailed stitching.', 15.99, 1, 1, 'https://fakestoreapi.com/img/71YXzeOuslL._AC_UY879_t.png'),
(5, 'John Hardy Women''s Legends Naga Bracelet', 'Inspired by the mythical water dragon that protects the ocean''s pearl.', 695.00, 1, 4, 'https://fakestoreapi.com/img/71pWzhdJNwL._AC_UL640_QL65_ML3_t.png'),
(6, 'Solid Gold Petite Micropave', 'Satisfaction guaranteed. Return or exchange within 30 days.', 168.00, 1, 4, 'https://fakestoreapi.com/img/61sbMiUnoGL._AC_UL640_QL65_ML3_t.png'),
(7, 'White Gold Plated Princess Ring', 'Classic wedding engagement solitaire diamond promise ring.', 9.99, 1, 4, 'https://fakestoreapi.com/img/71YAIFU48IL._AC_UL640_QL65_ML3_t.png'),
(8, 'Pierced Owl Rose Gold Earrings', 'Rose Gold Plated Double Flared Tunnel Plug Earrings.', 10.99, 1, 4, 'https://fakestoreapi.com/img/51UDEzMJVpL._AC_UL640_QL65_ML3_t.png'),
(9, 'WD 2TB Elements Portable External Hard Drive', 'USB 3.0 and USB 2.0 compatibility with fast data transfers.', 64.00, 1, 3, 'https://fakestoreapi.com/img/61IBBVJvSDL._AC_SY879_t.png'),
(10, 'SanDisk SSD PLUS 1TB Internal SSD', 'Boosts performance and reliability with read/write speeds up to 535MB/s.', 109.00, 1, 3, 'https://fakestoreapi.com/img/61U7T1koQqL._AC_SX679_t.png'),
(11, 'Silicon Power 256GB SSD 3D NAND', 'High transfer speeds and advanced SLC cache technology.', 109.00, 1, 3, 'https://fakestoreapi.com/img/71kWymZ+c+L._AC_SX679_t.png'),
(12, 'WD 4TB Gaming Drive for Playstation 4', 'Expand your PS4 gaming experience, play anywhere.', 114.00, 1, 3, 'https://fakestoreapi.com/img/61mtL65D4cL._AC_SX679_t.png'),
(13, 'Acer SB220Q 21.5 inch Full HD IPS Monitor', 'Ultra-thin IPS display with Radeon FreeSync technology.', 599.00, 1, 3, 'https://fakestoreapi.com/img/81QpkIctqPL._AC_SX679_t.png'),
(14, 'Samsung 49-Inch CHG90 Curved Gaming Monitor', 'Super ultrawide screen with HDR support and QLED technology.', 999.99, 1, 3, 'https://fakestoreapi.com/img/81Zt42ioCgL._AC_SX679_t.png'),
(15, 'BIYLACLESEN Women''s 3-in-1 Snowboard Jacket', 'Detachable design suitable for different seasons.', 56.99, 1, 2, 'https://fakestoreapi.com/img/51Y5NI-I5jL._AC_UX679_t.png'),
(16, 'Lock and Love Women''s Faux Leather Jacket', 'Faux leather material for style and comfort.', 29.95, 1, 2, 'https://fakestoreapi.com/img/81XH0e8fefL._AC_UY879_t.png'),
(17, 'Rain Jacket Women Windbreaker', 'Lightweight, adjustable drawstring waist, hooded raincoat.', 39.99, 1, 2, 'https://fakestoreapi.com/img/71HblAHs5xL._AC_UY879_-2t.png'),
(18, 'MBJ Women''s Solid Short Sleeve Boat Neck', 'Lightweight fabric with great stretch for comfort.', 9.85, 1, 2, 'https://fakestoreapi.com/img/71z3kpMAYsL._AC_UY879_t.png'),
(19, 'Opna Women''s Short Sleeve Moisture T-Shirt', 'Lightweight, breathable fabric with moisture wicking.', 7.95, 1, 2, 'https://fakestoreapi.com/img/51eg55uWmdL._AC_UX679_t.png'),
(20, 'DANVOUY Women''s Casual Cotton Short T-Shirt', 'Soft cotton blend casual short sleeve V-neck.', 12.99, 1, 2, 'https://fakestoreapi.com/img/61pHAEJ4NML._AC_UX679_t.png');
SET IDENTITY_INSERT Advertisements OFF;
GO
