-- 001: Messages tablosu.
-- Idempotent: tablo varsa dokunmaz. Ayni betik ayni veritabaninda iki kez kosarsa zarar vermez;
-- GMS'in "duplicate execution" kapisi zaten ikinci kosuyu sorar, bu ikinci emniyet.
IF OBJECT_ID(N'dbo.Messages', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Messages
    (
        Id        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Messages PRIMARY KEY,
        Title     NVARCHAR(200)     NOT NULL,
        Body      NVARCHAR(2000)    NOT NULL,
        CreatedAt DATETIME2(0)      NOT NULL CONSTRAINT DF_Messages_CreatedAt DEFAULT (SYSUTCDATETIME())
    );
END
GO
