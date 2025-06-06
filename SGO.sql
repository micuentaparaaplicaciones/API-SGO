BEGIN
    -- Crear el usuario
    EXECUTE IMMEDIATE 'CREATE USER C##SGO IDENTIFIED BY "123"';

    -- Asignar roles
    EXECUTE IMMEDIATE 'GRANT CONNECT TO C##SGO';
    EXECUTE IMMEDIATE 'GRANT RESOURCE TO C##SGO';

    -- Asignar privilegios del sistema
    EXECUTE IMMEDIATE 'GRANT CREATE PROCEDURE TO C##SGO';
    EXECUTE IMMEDIATE 'GRANT CREATE SEQUENCE TO C##SGO';
    EXECUTE IMMEDIATE 'GRANT CREATE TABLE TO C##SGO';
    EXECUTE IMMEDIATE 'GRANT CREATE TRIGGER TO C##SGO';
    EXECUTE IMMEDIATE 'GRANT CREATE VIEW TO C##SGO';
    EXECUTE IMMEDIATE 'GRANT UNLIMITED TABLESPACE TO C##SGO';
END;
/



CREATE TABLE Users (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR2(100) NOT NULL,
    Email VARCHAR2(100) NOT NULL UNIQUE,
    Phone VARCHAR2(15) NOT NULL,
    Address VARCHAR2(255) NOT NULL,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Password VARCHAR2(255) NOT NULL,
    Role VARCHAR2(100) NOT NULL,
    CreatedBy VARCHAR2(100) NOT NULL,
    ModifiedBy VARCHAR2(100) NOT NULL
);

CREATE SEQUENCE UserIdSeq
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE OR REPLACE TRIGGER Users_BI
BEFORE INSERT ON Users
FOR EACH ROW
BEGIN
    :NEW.Id := UserIdSeq.NEXTVAL;
END;



CREATE TABLE Suppliers (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    CreatedBy VARCHAR2(100) NOT NULL,
    ModifiedBy VARCHAR2(100) NOT NULL
);

CREATE SEQUENCE SupplierIdSeq
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE OR REPLACE TRIGGER Suppliers_BI
BEFORE INSERT ON Suppliers
FOR EACH ROW
BEGIN
    :NEW.Id := SupplierIdSeq.NEXTVAL;
END;



CREATE TABLE Categories (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    CreatedBy VARCHAR2(100) NOT NULL,
    ModifiedBy VARCHAR2(100) NOT NULL
);

CREATE SEQUENCE CategoryIdSeq
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE OR REPLACE TRIGGER Categories_BI
BEFORE INSERT ON Categories
FOR EACH ROW
BEGIN
    :NEW.Id := CategoryIdSeq.NEXTVAL;
END;



CREATE TABLE Products (
    Id NUMBER NOT NULL PRIMARY KEY,
    Name VARCHAR2(100) NOT NULL,
    Detail CLOB,
    Price NUMBER(10, 2) NOT NULL,
    AvailableQuantity NUMBER NOT NULL,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Supplier VARCHAR2(100) NOT NULL,
    Category VARCHAR2(100) NOT NULL,
    Image BLOB,
    CreatedBy VARCHAR2(100) NOT NULL,
    ModifiedBy VARCHAR2(100) NOT NULL
);

CREATE SEQUENCE ProductIdSeq
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE OR REPLACE TRIGGER Products_BI
BEFORE INSERT ON Products
FOR EACH ROW
BEGIN
    :NEW.Id := ProductIdSeq.NEXTVAL;
END;



CREATE TABLE Customers (
    Id INT NOT NULL PRIMARY KEY,
    Name VARCHAR2(100) NOT NULL,
    Email VARCHAR2(100) NOT NULL UNIQUE,
    Phone VARCHAR2(15) NOT NULL,
    Address VARCHAR2(255) NOT NULL,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CreatedBy VARCHAR2(100) NOT NULL,
    ModifiedBy VARCHAR2(100) NOT NULL
);



CREATE TABLE Orders (
    Id INT NOT NULL PRIMARY KEY,
    Status VARCHAR2(100) NOT NULL,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    DeliveryAddress VARCHAR2(255) NOT NULL,
    DeliveryDate TIMESTAMP NOT NULL,
    CustomerId VARCHAR2(100) NOT NULL,
    CustomerName VARCHAR2(100) NOT NULL,
    --UserName VARCHAR2(100) NOT NULL,
    Subtotal NUMBER(10, 2) NOT NULL,
    Discount NUMBER(10, 2) NOT NULL,
    Tax NUMBER(10, 2) NOT NULL,
    Total NUMBER(10, 2) NOT NULL,
    PaymentMethod VARCHAR2(50) NOT NULL,
    PaymentStatus VARCHAR2(50) NOT NULL,
    CreatedBy VARCHAR2(100) NOT NULL,
    ModifiedBy VARCHAR2(100) NOT NULL
);

CREATE SEQUENCE OrderIdSeq
    START WITH 1
    INCREMENT BY 1
    NOCACHE
    NOCYCLE;

CREATE OR REPLACE TRIGGER Orders_BI
BEFORE INSERT ON Orders
FOR EACH ROW
BEGIN
    :NEW.Id := OrderIdSeq.NEXTVAL;
END;



CREATE TABLE OrderDetails (
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    ProductName VARCHAR2(100) NOT NULL,    
    Status VARCHAR2(100) NOT NULL, 
    Notes CLOB,
    ProductRequestedQuantity INT NOT NULL,
    ProductPrice NUMBER(10, 2) NOT NULL,
    Subtotal NUMBER(10, 2),
    Discount NUMBER(10, 2),
    Tax NUMBER(10, 2),
    Total NUMBER(10, 2),
    CreatedBy VARCHAR2(100) NOT NULL,
    ModifiedBy VARCHAR2(100) NOT NULL,
    PRIMARY KEY (OrderId, ProductId)
);

CREATE OR REPLACE TRIGGER trg_order_details_before_ins_upd
BEFORE INSERT OR UPDATE ON OrderDetails
FOR EACH ROW
BEGIN
  IF :new.Notes IS NULL THEN
    :new.Notes := 'Sin notas'; -- Asigna un espacio para que no se guarde como null
  END IF;
END;
/


-- Consultas
SELECT RouteIdSeq.CURRVAL FROM dual;
SELECT RouteIdSeq.NEXTVAL FROM dual;
DELETE FROM OrderDetails WHERE OrderId = 1 AND ProductId = 3;
DELETE FROM OrderDetails;
DELETE FROM Orders;
DELETE FROM invoicedetails;
DELETE FROM invoices;
select * from orders where orderId = 382;
select * from orders;
select * from orderdetails;
select * from orderdetails where orderId = 1;
select * from users;
select * from products;
select * from customers;
select * from customers where customerId = 11;
select * from invoices;
select * from invoices where invoiceId = 14;
select * from invoicedetails;
select * from invoicedetails where invoicedetails.invoiceId = 17;
select * from routes;

UPDATE INVOICEDETAILS
SET INVOICEDETAILNOTES = 'Sin notas'
WHERE INVOICEDETAILNOTES IS NULL;

