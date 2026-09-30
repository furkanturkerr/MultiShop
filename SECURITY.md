# Yerel güvenlik ayarları

JWT anahtarı ve veritabanı / RabbitMQ parolaları appsettings.json içinde tutulmaz.
Bu bilgisayarda mevcut değerler her çalıştırılabilir projenin .NET user-secrets deposuna taşındı.
Rider'ın Development profili bunları otomatik okur. JWT anahtarı yenilendiği için bütün API'ler,
Gateway ve WebUI yeniden başlatılmalı; ardından yeniden giriş yapılmalı.

Başka bilgisayarda her API, Identity ve Gateway için aynı güçlü `Jwt:Key` değerini
`dotnet user-secrets set "Jwt:Key" "<ortak-anahtar>" --project <proje-yolu>` ile tanımla.
Veritabanı kullanan projelerde `ConnectionStrings:DefaultConnection`, RabbitMQ kullanan
projelerde `RabbitMQ:Password` değerlerini de aynı yöntemle tanımla.
Sunucuda karşılıkları `Jwt__Key`, `ConnectionStrings__DefaultConnection`, `RabbitMQ__Password`
ortam değişkenleridir. User-secrets geliştirme içindir; şifreli bir kasa değildir.

Önceki commitlerdeki parolalar silinmiş sayılmaz: GitHub paylaşımından önce veritabanı ve
RabbitMQ parolalarını değiştir. JWT anahtarı bu düzenlemede yenilendi. Git geçmişi değiştirilmedi.
Gerçek kartlarla ödeme yapılmaz: mevcut kart formu demo akışıdır; gerçek ödeme için sağlayıcı
entegrasyonu ve ödeme sonucu doğrulaması gerekir. Kart numarası / CVV kaydedilmemelidir.

Katalog GET istekleri ve onaylı ürün yorumları herkese açıktır. Yönetim işlemleri Admin rolü
ister; kargo ve kupon yönetimi Admin veya Manager ister. Adres ve sipariş okuma/yazmalarında
kullanıcı JWT içindeki sub ile belirlenir, başka müşteriye ait kayıtlar reddedilir.
Sepet fiyatı katalogdan, indirim kupon servisinden alınır. Sipariş tutarı ve ürünleri Order API
kullanıcının doğrulanmış sepetinden oluşturur; istemciden gönderilen toplam kabul edilmez.

Üretimde HTTPS kullan, API/veritabanı/Redis/RabbitMQ portlarını internete açma ve yalnızca
Gateway'i dışarı sun. Local RabbitMQ portları compose içinde 127.0.0.1'e bağlıdır.
