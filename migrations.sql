create database aksia_test collate SQL_Latin1_General_CP1_CI_AS
go

use aksia_test
go

create table dbo.Transactions
(
    Id              uniqueidentifier default newid() not null
        constraint Transactions_pk
            primary key,
    ApplicationName nvarchar(200)                    not null,
    Email           varchar(200)                     not null,
    Filename        nvarchar(300),
    Url             nvarchar(2000),
    Inception       date                             not null,
    Amount          decimal(14,2)                    not null,
    Currency        char(3)                          not null,
    Allocation      decimal(7,2)
)
go


create table dbo.Currencies
(
    Code           char(3)       not null
        constraint Currencies_pk
            primary key,
    Symbol         nvarchar(3)   not null,
    Name           nvarchar(100) not null,
    FractionDigits int           not null
)
go

INSERT INTO dbo.Currencies (Code, Symbol, Name, FractionDigits) VALUES (N'EUR', N'€', N'Euro', 2)
GO

INSERT INTO dbo.Currencies (Code, Symbol, Name, FractionDigits) VALUES (N'GBP', N'£', N'United Kingdom Pound', 2)
GO

INSERT INTO dbo.Currencies (Code, Symbol, Name, FractionDigits) VALUES (N'USD', N'$', N'United States Dollar', 2)
GO
