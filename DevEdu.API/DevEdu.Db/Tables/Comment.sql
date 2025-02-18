﻿CREATE TABLE [Comment] (
	Id int NOT NULL IDENTITY(1,1),
	UserId int NOT NULL,
	LessonId int,
	StudentHomeworkId int,
	Text nvarchar(max) NOT NULL,
	Date datetime NOT NULL,
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_COMMENT] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
GO

ALTER TABLE [Comment] WITH CHECK ADD CONSTRAINT [Comment_fk0] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Comment] CHECK CONSTRAINT [Comment_fk0]
GO
ALTER TABLE [Comment] WITH CHECK ADD CONSTRAINT [Comment_fk1] FOREIGN KEY ([LessonId]) REFERENCES [Lesson]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Comment] CHECK CONSTRAINT [Comment_fk1]
GO
ALTER TABLE [Comment] WITH CHECK ADD CONSTRAINT [Comment_fk2] FOREIGN KEY ([StudentHomeworkId]) REFERENCES [Student_Homework]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Comment] CHECK CONSTRAINT [Comment_fk2]
GO