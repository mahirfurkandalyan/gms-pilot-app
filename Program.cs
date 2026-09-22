using System.Reflection;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

/*
  Sağlık kontrolü. GMS dağıtım profilinde "Health-check URL" alanı var; kurulumdan sonra
  buraya bakıp sitenin gerçekten ayakta olduğunu doğruluyor.

  Sürüm numarasını da döndürüyor: "site ayakta" ile "YENİ sürüm ayakta" ayrı şeyler.
  Yalnızca ayakta olduğuna bakan bir kontrol, eski dosyaların kaldığı bir dağıtımı
  başarılı sayardı.
*/
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    service = "GMS Pilot App",
    version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "bilinmiyor",
    timestamp = DateTime.UtcNow
}));

/*
  MESAJLAR — veritabanı pilotunun değişiklik noktası.

  Bağlantı dizesi kodda değil, sitenin ortamına göre makine değişkeninden gelir:
  GMS ajanı siteye GMS_ENVIRONMENT (TEST/UAT/PROD) yazar; uygulama PILOTAPP_DB_{ORTAM}
  değişkenini okur. Böylece aynı dosyalar üç ortamda üç ayrı veritabanına bakar —
  "hangi ortam hangi veritabanı" sorusunun cevabı sunucudadır, pakette değil.

  Hata sessizce yutulmaz: değişken yoksa ya da tablo yoksa sebebi yanıtta yazar.
  Dağıtımdan sonra sayfada "tablo yok" görmek, SQL adımının koşmadığının kanıtıdır.
*/
app.MapGet("/api/messages", async () =>
{
    var env = (Environment.GetEnvironmentVariable("GMS_ENVIRONMENT") ?? "").Trim().ToUpperInvariant();
    var varName = string.IsNullOrEmpty(env) ? "PILOTAPP_DB" : $"PILOTAPP_DB_{env}";
    var cs = Environment.GetEnvironmentVariable(varName);
    if (string.IsNullOrWhiteSpace(cs))
        return Results.Ok(new { environment = env, variable = varName, error = $"Bağlantı dizesi yok: {varName} makine değişkeni tanımlı değil." });

    try
    {
        await using var conn = new SqlConnection(cs);
        await conn.OpenAsync();
        var database = conn.Database;
        await using var cmd = new SqlCommand("SELECT TOP 50 Id, Title, Body, CreatedAt FROM dbo.Messages ORDER BY Id DESC", conn);
        var list = new List<object>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            list.Add(new { id = reader.GetInt32(0), title = reader.GetString(1), body = reader.GetString(2), createdAt = reader.GetDateTime(3) });
        return Results.Ok(new { environment = env, database, messages = list });
    }
    catch (SqlException ex) when (ex.Number == 208)
    {
        return Results.Ok(new { environment = env, error = "dbo.Messages tablosu yok — SQL betiği bu ortamda henüz koşmamış." });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { environment = env, error = "Veritabanına ulaşılamadı: " + ex.Message });
    }
});

app.Run();
