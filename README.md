# vCard Oluşturma

**Proje açıklaması:**
İlk önce kullanıcıdan kaç adet user istediği soruldu, girdiği değer sayı olmazsa hata yaşanmaması için `try-catch` kullanıldı. `HttpClient` için de başverebilecek hatalar için yine `try-catch` kullanılarak önlem alındı. Daha sonra Kullanıcının girdiği değerin 0 ile 2000(opsiyonel olarak dahil edildi api request zamani sorun yaşanmasın diye) arasinda olup-olmadığı kontrol edildi eğer deyilse, hata mesajı geri döndürüldü.

**Sonraki Adımlar:**
-`RandomUser.me` API'sinden  `HttpClient` kullanarak  veriler alındı
- Veriler alinarken ulusal kimlikleri (us, fr, tr) olan kullanıcı profilleri olarak filtrelendi
- Alınan veriler `vCard` sınıfına deserialize edildi
- Dosyayı kaydetmek için dosya yolu belirlendi (`vCards(vcf)` ve `vCards(txt)` olarak iki ayrı dosya olarak belirlendi)
- Daha sonra aldığımız değerlerle `vCard` oluşturulması için wikipediadan card oluşturulma kodları kullanılarak `CreateCard` metodu yazildi 
- Cardin kaydedildiği ismin benzersiz olması için `RandomUser.me` den aldığımız değerlerdeki İd parametresi kullanıldı
- Oluşturulan vCard'ı `.vcf` ve `.txt` olarak kaydedildi ve kullaniciya kaydedilmiş olan userin İsim, Soyisim  ve dosya yolu ekrana çıkarıldı. 