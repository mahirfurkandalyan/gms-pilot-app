-- 002: karsilama mesaji. Idempotent: basligi ayni kayit varsa eklemez.
IF NOT EXISTS (SELECT 1 FROM dbo.Messages WHERE Title = N'GMS pilotu')
    INSERT INTO dbo.Messages (Title, Body)
    VALUES (N'GMS pilotu', N'Bu mesaj veritabanina GMS dagitim ajaninin kosturdugu SQL betigiyle yazildi.');
GO
