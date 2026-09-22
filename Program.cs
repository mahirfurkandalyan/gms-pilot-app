using System.Reflection;

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

app.Run();
