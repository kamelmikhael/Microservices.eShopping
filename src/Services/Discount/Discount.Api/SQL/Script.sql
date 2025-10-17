Create Table Coupon(
    ID Serial Primary Key NOT NULL,
	ProductName VARCHAR(24) NOT NULL,
	Description TEXT,
	Amount INT
);

INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('IPhone X', 'IPhone Discount', 150);
INSERT INTO Coupon (ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100);