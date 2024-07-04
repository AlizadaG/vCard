# vCard Oluşturma

**Proje açıklaması:**
İlk önce kullanıcıdan kaç adet user istediği soruldu, girdiği değer sayı olmazsa hata yaşanmaması için `try-catch` kullanıldı. `HttpClient` için de yine `try-catch` kullanılarak önlem alındı. Daha sonra Kullanıcının girdiği değerin 0 ile 2000(opsiyonel olarak dahil edildi api request zamani sorun yaşanmasın diye) arasinda olup-olmadığı kontrol edildi eğer deyilse, hata mesajı geri döndürüldü.

**Sonraki Adımlar:**
- RandomUser.me API'sinden  `HttpClient` kullanarak API'den veriler alındı
- Alınan veriler VCard sınıfına deserialize edildi
- Daha sonra aldığımız değerlerle `vCard` oluşturulması için wikipediadan card oluşturulma kodları kullanıldı
- Dosyayı kaydetmek için dosya yolu belirlendi
- Oluşturulan vCard'ı `.vcf` ve `.txt` olarak kaydedildi.