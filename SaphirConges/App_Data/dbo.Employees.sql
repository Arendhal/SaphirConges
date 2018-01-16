CREATE TABLE [dbo].[Employees] (
    [EmployeeId]      INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]       NVARCHAR (MAX) NULL,
    [LastName]        NVARCHAR (MAX) NULL,
    [BirthDate]       DATETIME       NOT NULL,
    [Gender]          INT            NOT NULL,
    [HireDate]        DATETIME2       NOT NULL,
    [TerminationDate] DATETIME       NULL,
    [Address]         NVARCHAR (MAX) NULL,
    [ContactNumber]   BIGINT         NOT NULL,
    [PersonalEmail]   NVARCHAR (MAX) NULL,
    [Username]        NVARCHAR (MAX) NULL,
    [EmployeeType]    INT            NOT NULL,
    [JobTitle]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Employees] PRIMARY KEY CLUSTERED ([EmployeeId] ASC)
);

