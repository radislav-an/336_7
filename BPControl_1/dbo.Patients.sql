CREATE TABLE [dbo].[Patients] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Surname]     NVARCHAR (50) NULL,
    [Name]        NVARCHAR (50) NULL,
    [Patronymic]  NVARCHAR (50) NULL,
    [DateOfBirth] DATE          NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

