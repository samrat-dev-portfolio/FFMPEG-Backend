USE [FFMpeg]
GO

/****** Object:  Table [dbo].[tblSubject]    Script Date: 30-03-2021 04:29:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblChapter](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[chapterName] [nvarchar](200) NOT NULL,
	[subjectId] [int] NOT NULL,
	CONSTRAINT PK_tblChapter PRIMARY KEY NONCLUSTERED (id),
    CONSTRAINT FK_tblChapter_tblSubject FOREIGN KEY (subjectId)
        REFERENCES tblSubject (id)
        ON DELETE CASCADE
        ON UPDATE CASCADE
)
GO


