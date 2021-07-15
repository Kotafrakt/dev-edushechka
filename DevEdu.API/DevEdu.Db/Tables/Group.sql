﻿CREATE TABLE [Group] (
	Id int NOT NULL IDENTITY(1,1),
	Name nvarchar(50) NOT NULL,
	CourseId int NOT NULL,
	GroupStatusId int NOT NULL,
	StartDate date NOT NULL,
	IsDeleted bit NOT NULL DEFAULT '0',
	Timetable nvarchar(500) NOT NULL,
	PaymentPerMonth decimal(6,2) NOT NULL,
  CONSTRAINT [PK_GROUP] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Group] WITH CHECK ADD CONSTRAINT [Group_fk0] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Group] CHECK CONSTRAINT [Group_fk0]
GO
ALTER TABLE [Group] WITH CHECK ADD CONSTRAINT [Group_fk1] FOREIGN KEY ([GroupStatusId]) REFERENCES [GroupStatus]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Group] CHECK CONSTRAINT [Group_fk1]
GO