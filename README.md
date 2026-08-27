# GMS Pilot Uygulaması

GMS'in uçtan uca çalışıp çalışmadığını sınamak için yazılmış küçük bir ASP.NET Core
uygulaması. Tek işi var: **dağıtımın gerçekten olduğunu gözle görülür kılmak.**

## İçinde ne var

- `wwwroot/index.html` — sekmeli bir navbar ve üç bilgi kutusu
- `Program.cs` — statik dosyaları sunar, `/health` ucundan sürüm bilgisi döner
- `GmsPilotApp.csproj` — sürüm numarası burada (`<Version>`)

## Niye sürüm numarası var

Sayfadaki sürüm kutusu `/health` ucundan okunuyor, sayfaya gömülü değil.

Sebep şu: dağıtım "başarılı" görünüp aslında eski dosyaları bırakabilir. Numara
sayfaya gömülü olsaydı eski sayfa eski numarayı gösterir, kimse farkı anlamazdı.
Çalışan uygulamadan okunduğunda numara değişmiyorsa **dosyalar değişmemiş** demektir.

Her değişiklikte `.csproj` içindeki `<Version>` bir artırılır.

## Yerelde çalıştırma

```
dotnet run
```

Sonra `http://localhost:5000` (ya da konsolun yazdığı adres).

## Pilot senaryosu

1. Jira'da görev açılır: navbara yeni bir sekme eklenmesi
2. GMS'te değişiklik kaydı açılır, dal ve PR oluşur
3. PR birleştirilir, sürüm planlanır
4. Dağıtım ajanı sunucuya kurar
5. Site açılır: **yeni sekme görünüyor mu, sürüm numarası arttı mı**
