﻿CREATE TABLE [Tag_Task] (
	Id int NOT NULL IDENTITY(1,1),
	TagId int NOT NULL,
	TaskId int NOT NULL,
  CONSTRAINT [PK_TAG_TASK] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Tag_Task] WITH CHECK ADD CONSTRAINT [Tag_Task_fk0] FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Tag_Task] CHECK CONSTRAINT [Tag_Task_fk0]
GO
ALTER TABLE [Tag_Task] WITH CHECK ADD CONSTRAINT [Tag_Task_fk1] FOREIGN KEY ([TaskId]) REFERENCES [Task]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Tag_Task] CHECK CONSTRAINT [Tag_Task_fk1]
GO
ALTER TABLE [dbo].[Tag_Task]
ADD CONSTRAINT UC_TagId_TaskId UNIQUE(TagId, TaskId)
GO