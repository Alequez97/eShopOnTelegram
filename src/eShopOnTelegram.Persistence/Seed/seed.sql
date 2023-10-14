SET IDENTITY_INSERT ProductCategories ON;
INSERT INTO ProductCategories(Id, Name, IsDeleted)
VALUES
    (1, 'Electronics', 0),
    (2, 'Clothing', 0),
    (3, 'Home & Garden', 0);
SET IDENTITY_INSERT ProductCategories OFF;

SET IDENTITY_INSERT Products ON;
INSERT INTO Products (Id, Name, CategoryId, IsDeleted)
VALUES
    (1, 'Smartphone', 1, 0),
    (2, 'Laptop', 1, 0),
    (3, 'T-shirt', 2, 0),
    (4, 'Sofa', 3, 0),
    (5, 'TV', 1, 0);
SET IDENTITY_INSERT Products OFF;

SET IDENTITY_INSERT ProductAttributes ON;
INSERT INTO ProductAttributes (Id, Color, Size, OriginalPrice, PriceWithDiscount, QuantityLeft, ImageName, ProductId, IsDeleted)
VALUES
    (1, 'Black', 'Large', 699.99, NULL, 50, 'smartphone.jpg', 1, 0),
    (2, 'Silver', 'Medium', 1099.99, 999.99, 30, 'laptop.jpg', 2, 0),
    (3, 'Blue', 'Small', 19.99, NULL, 100, 'tshirt_blue.jpg', 3, 0),
    (4, 'Red', 'Large', 29.99, 24.99, 75, 'tshirt_red.jpg', 3, 0),
    (5, 'Gray', '2-Seater', 499.99, NULL, 10, 'sofa_gray.jpg', 4, 0),
    (6, 'Black', '65-inch', 799.99, NULL, 20, 'tv_black.jpg', 5, 0);
SET IDENTITY_INSERT ProductAttributes OFF;
