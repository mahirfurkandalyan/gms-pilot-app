-- 001 geri alma: tabloyu kaldirir. Veri kaybi bilincli — tablo bu degisiklikle geldi,
-- geri alinirsa icindekiler de gider. Uretimde bu betik ancak GMS rollback onayiyla kosar.
IF OBJECT_ID(N'dbo.Messages', N'U') IS NOT NULL
    DROP TABLE dbo.Messages;
GO
