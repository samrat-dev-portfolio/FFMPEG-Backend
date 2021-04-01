USE [FFMpeg]
GO

INSERT INTO [dbo].[tblChapter]
           ([chapterName]
           ,[subjectId]
           ,[classId])
     VALUES
           (N'পরিবেশ ও উৎপাদন (কৃষি ও মৎস্য উৎপাদন)'
           ,5
           ,1)
GO

SELECT * FROM [dbo].[tblChapter] order by id desc

--SELECT * FROM [dbo].[tblClass]
--SELECT * FROM [dbo].[tblSubject]
--SELECT * FROM [dbo].[tblChapter]




