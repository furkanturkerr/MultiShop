<div align="center">

# 🛒 MultiShop

### ASP.NET Core ile geliştirilmiş mikroservis tabanlı e-ticaret projesi

Ürün keşfinden kategoriye göre seçenek seçimine, sepette kupon uygulamadan  
adres ve sipariş yönetimine kadar birbirine bağlı bir alışveriş akışı.

<br/>

<img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 8" />
<img src="https://img.shields.io/badge/ASP.NET_Core-MVC_%26_Web_API-7B2CBF?style=for-the-badge" alt="ASP.NET Core MVC ve Web API" />
<img src="https://img.shields.io/badge/Ocelot-API_Gateway-FBBF24?style=for-the-badge" alt="Ocelot" />
<img src="https://img.shields.io/badge/Identity_%26_JWT-Authentication-111827?style=for-the-badge" alt="Identity ve JWT" />

<br/>

<img src="https://img.shields.io/badge/MongoDB-Catalog-47A248?style=for-the-badge&logo=mongodb&logoColor=white" alt="MongoDB" />
<img src="https://img.shields.io/badge/Redis-Basket-DC382D?style=for-the-badge&logo=redis&logoColor=white" alt="Redis" />
<img src="https://img.shields.io/badge/SQL_Server-Persistence-CC2927?style=for-the-badge" alt="SQL Server" />
<img src="https://img.shields.io/badge/RabbitMQ-Messaging-FF6600?style=for-the-badge&logo=rabbitmq&logoColor=white" alt="RabbitMQ" />
<img src="https://img.shields.io/badge/Docker-Container_Infrastructure-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker" />

<br/>

<img src="https://img.shields.io/badge/EF_Core_%26_Dapper-Data_Access-5C940D?style=for-the-badge" alt="EF Core ve Dapper" />
<img src="https://img.shields.io/badge/MediatR-CQRS-8B5CF6?style=for-the-badge" alt="MediatR" />
<img src="https://img.shields.io/badge/NUnit_%26_xUnit-Tests-2563EB?style=for-the-badge" alt="NUnit ve xUnit" />

<br/><br/>

<img src="docs/screenshots/home.png" alt="MultiShop ana sayfa" width="1000" />

</div>

---

## 📌 Proje Hakkında

**MultiShop**, ilk mikroservis projem olarak geliştirdiğim bir e-ticaret uygulaması. Murat Yücedağ'ın *ASP.NET Core MultiShop Mikroservis E-Ticaret* eğitimini temel alarak geliştirdim; eğitim akışının yanında arayüz, ürün seçenekleri, filtreleme, sipariş süreci, yetkilendirme ve testler üzerinde de çalıştım.

Bu projede amacım yalnızca ayrı API'ler oluşturmak değildi. Bir kullanıcının ürünü seçip sepete eklediği, kupon uyguladığı, adresini belirlediği ve siparişini geçmişinde gördüğü sürecin farklı servisler arasında nasıl yürüdüğünü öğrenmek istedim.

WebUI veritabanlarına doğrudan erişmez. HTTP isteklerini **Ocelot Gateway** üzerinden ilgili mikroservislere gönderir. Servisler kendi sorumluluklarına göre MongoDB, Redis veya SQL Server kullanır.

> Proje bir öğrenme ve portföy çalışmasıdır. Kart giriş ekranı demo alışveriş akışının parçasıdır; gerçek ödeme sağlayıcısı entegrasyonu ve karttan tahsilat bulunmaz.

## 🚀 Öne Çıkan Özellikler

### 👤 Hesap ve oturum yönetimi

- ASP.NET Core Identity ile kayıt ve giriş
- Yeni kayıtlarda otomatik `Customer` rolü atama
- Kayıtlı e-posta ve hatalı giriş kontrolleri
- Identity üzerinden parola hashleme ve parola kurallarının uygulanması
- Kullanıcı bilgileri ve rolleri içeren JWT üretimi
- WebUI'de Cookie Authentication ile oturum ve çıkış işlemi
- Giriş sonrasında uygulama içindeki hedef sayfaya geri yönlendirme
- Giriş ve kayıt endpoint'lerinde rate limiting

### 🛍️ Mağaza ve ürün keşfi

- Kategori, kampanya, marka ve ürün alanlarından oluşan ana sayfa
- Ürün arama, kategori ve fiyat filtreleme, sıralama ve sayfalama
- Katalog API'sinde MongoDB sorguları üzerinden filtreleme, sıralama ve sayfalama
- Çoklu görsel, açıklama ve yorum içeren ürün detay ekranları
- Admin tarafından kategoriye tanımlanan seçenek grupları ve ürüne özgü değerler; örneğin ayakkabıda renk / numara, bilgisayarda renk / depolama
- Giriş yapan kullanıcıların puan ve yorum bırakması; admin onayından sonra yorumların ürün detayında yayımlanması

### 🧺 Sepet ve kupon

- Giriş yapan kullanıcıya ait Redis tabanlı sepet
- Seçilen renk, beden, numara veya depolama gibi bilgilerin sepette korunması
- Aynı ürünün farklı seçimlerinin ayrı sepet satırları olarak tutulması
- AJAX ile adet değiştirme ve tutarların güncellenmesi
- Kupon kodu uygulama ve kaldırma
- Kuponun aktifliği, son kullanma tarihi ve indirim oranının API'de kontrol edilmesi
- Ürün fiyatlarının istemciden kabul edilmeden katalogdan doğrulanması

### 📦 Adres ve sipariş

- Kullanıcıya ait adresleri listeleme, ekleme, güncelleme ve silme
- Kayıtlı adres seçimi veya yeni adres ile devam etme
- Sepet, indirim ve ödenecek toplamın sipariş özetinde gösterilmesi
- Doğrulanmış sepetten sipariş ve sipariş kalemlerinin oluşturulması
- User Area içinde sipariş geçmişi; sipariş kalemleri, adetler ve seçilen seçeneklerin birlikte görüntülenmesi
- Admin panelinde sipariş listesi, sipariş detayı ve müşteri adresleri
- Sipariş oluştuğunda RabbitMQ üzerinden olay yayınlanması

### ⚙️ Admin paneli

| Alan | Yönetilen içerikler |
| --- | --- |
| Katalog | Kategoriler, seçenek grupları, ürünler, ürün görselleri, açıklamalar ve markalar |
| Kampanyalar | Kuponlar, indirim alanları ve özel teklifler |
| Ana sayfa | Slider ve vitrin özellikleri |
| Müşteri işlemleri | Siparişler, sipariş detayları, adresler ve yorum onayı |
| Lojistik | Kargo şirketleri |

## 🏗️ Mikroservis Mimarisi

```mermaid
flowchart TD
    User["Kullanıcı / Admin"] --> UI["ASP.NET Core MVC WebUI"]
    UI --> GW["Ocelot API Gateway"]
    GW --> ID["Identity API"]
    GW --> CAT["Catalog API"]
    GW --> BAS["Basket API"]
    GW --> ORD["Order API"]
    GW --> DIS["Discount API"]
    GW --> COM["Comment API"]
    GW --> CAR["Cargo API"]
    CAT --> MONGO[(MongoDB)]
    BAS --> REDIS[(Redis)]
    ID --> IDDB[(Identity SQL DB)]
    ORD --> ORDB[(Order SQL DB)]
    DIS --> DISDB[(Discount SQL DB)]
    COM --> COMDB[(Comment SQL DB)]
    CAR --> CARDB[(Cargo SQL DB)]
    BAS -. "Ürün / kupon doğrulama" .-> GW
    ORD -. "Sepet doğrulama" .-> GW
    ORD --> MQ["RabbitMQ"]
    MQ --> WORKER["OrderCreatedConsumer Worker"]
```

| Servis | Sorumluluk | Veri erişimi |
| --- | --- | --- |
| **Identity** | Kayıt, giriş, kullanıcı rolleri ve JWT üretimi | ASP.NET Core Identity + EF Core + SQL Server |
| **Catalog** | Ürün, kategori, seçenek, görsel, marka ve ana sayfa içerikleri | MongoDB Driver + AutoMapper |
| **Basket** | Kullanıcı sepeti, seçenek ve fiyat doğrulama | StackExchange.Redis |
| **Order** | Adres, sipariş ve sipariş kalemleri | EF Core + SQL Server; CQRS / MediatR |
| **Discount** | Kupon yönetimi ve kodla indirim sorgulama | Dapper + SQL Server; EF Core migrations |
| **Comment** | Ürün yorumları ve admin onay süreci | EF Core + SQL Server |
| **Cargo** | Kargo şirketi, müşteri ve kargo kayıtları | Katmanlı yapı + EF Core + SQL Server |
| **MessageBus / Consumer** | Sipariş olayını yayınlama ve kuyruktan okuma | RabbitMQ.Client + Worker Service |

Servisler sorumluluklarına uygun mimari yapılarla düzenlendi. Order tarafında Domain, Application, Persistence ve WebApi ayrımı; Cargo tarafında Entities, DataAccess, Business, Dtos ve WebApi katmanları bulunur. Diğer servislerde daha küçük, sorumluluğa göre ayrılmış yapılar kullanılır. WebUI tarafında mağaza, User ve Admin alanları ayrı layout'larla düzenlenir; API çağrıları HTTP servislerinde, ortak ekran parçaları ViewComponent ve Partial View'larda tutulur.

## 🔄 Alışveriş Akışı

```mermaid
flowchart LR
    A["Ürünü keşfet"] --> B["Giriş yap"]
    B --> C["Ürün seçeneklerini belirle"]
    C --> D["Sepete ekle"]
    D --> E["Kupon uygula"]
    E --> F["Adresi seç"]
    F --> G["Demo kart formu"]
    G --> H["Siparişi oluştur"]
    H --> I["Sipariş geçmişi"]
    H --> J["RabbitMQ olayı"]
```

**Ürün seçenekleri:** Kategorinin belirlediği gruplardan ürüne uygun değerler seçilir. Bu bilgiler sepetten sipariş geçmişine kadar korunur. Yapı seçenek seçimini destekler; gerçek stok takibi veya SKU bazlı varyant envanteri içermez.

**Sipariş tutarı:** Order API, kullanıcının doğrulanmış sepetini okuyarak ürünleri ve toplamı oluşturur. Tarayıcıdan gelen fiyat veya toplam sipariş tutarı kaynak olarak kullanılmaz.

## 🔐 Identity, JWT ve WebUI Oturumu

**Identity API**, kullanıcı hesapları ve parola kontrolünden sorumludur. **JWT**, API isteklerinde kullanıcının kimliğini taşır. **Cookie Authentication** ise kullanıcının WebUI'deki oturumunu yönetir. Bu üç yapı aynı giriş sürecinde farklı görevler üstlenir.

### Kayıt ve giriş süreci

```mermaid
sequenceDiagram
    actor User as Kullanıcı
    participant UI as MVC WebUI
    participant GW as Ocelot Gateway
    participant ID as Identity API
    participant DB as Identity SQL DB
    User->>UI: Ad soyad, e-posta ve parola ile kayıt
    UI->>GW: Register isteği
    GW->>ID: AuthController.Register
    ID->>DB: E-posta kontrolü ve UserManager.CreateAsync
    ID->>DB: Customer rolünü ata
    ID-->>GW: Kayıt sonucu
    GW-->>UI: Kayıt sonucu
    User->>UI: E-posta ve parola ile giriş
    UI->>GW: Login isteği
    GW->>ID: AuthController.Login
    ID->>DB: Kullanıcıyı bul ve parolayı doğrula
    ID->>DB: Kullanıcı rollerini oku
    ID-->>GW: JWT ve ExpiresAt
    GW-->>UI: Token yanıtı
    UI->>UI: Cookie oturumu oluştur ve access_token sakla
    UI-->>User: Hedef sayfaya yönlendir
```

Kayıtta `UserManager`, girişte `SignInManager` kullanılır. Parolalar Identity tarafından hashlenir. Giriş başarılı olduğunda `JwtTokenService`, kullanıcının bilgileri ve rolleriyle **HS256 imzalı** bir access token üretir.

| Token bilgisi | Kullanımı |
| --- | --- |
| `sub` | Kullanıcının Identity ID'si; sepet, adres ve sipariş sahipliği |
| `email` | Kullanıcının e-posta bilgisi |
| `name` | Kullanıcının adı soyadı |
| `role` | Customer, Admin veya Manager gibi kullanıcı rolleri |
| `jti` | Token için oluşturulan benzersiz tanımlayıcı |
| `iss`, `aud`, `exp` | Token'ı üreten uygulama, hedef API ve geçerlilik süresi |

WebUI, token yanıtındaki bilgileri oturum claim'lerine aktarır; `access_token` değerini authentication properties içinde saklar. Oturum cookie'si **HttpOnly** olarak yapılandırılmıştır. Cookie'nin süresi token'ın bitiş zamanı ile eşleştirilir; çıkış işleminde cookie oturumu kapatılır.

### Token mikroservise nasıl ulaşır?

```mermaid
sequenceDiagram
    actor User as Kullanıcı
    participant UI as MVC WebUI
    participant H as GatewayTokenHandler
    participant GW as Ocelot Gateway
    participant API as Basket / Order API
    participant DB as Redis / SQL DB
    User->>UI: Oturum cookie'si ile sayfayı aç
    UI->>H: HttpClient ile API isteği oluştur
    H->>H: Oturumdan access_token oku
    H->>GW: Authorization: Bearer JWT
    GW->>GW: Korumalı route için JWT doğrula
    GW->>API: İsteği Bearer token ile yönlendir
    API->>API: JWT doğrula, sub ve rolü oku
    API->>DB: Kullanıcıya ait kaydı sorgula
    DB-->>API: Sepet veya sipariş bilgisi
    API-->>GW: API yanıtı
    GW-->>UI: API yanıtı
    UI-->>User: Sayfayı göster
```

Basket ve Order, her istekte kullanıcıyı öğrenmek için Identity'ye tekrar gitmez. **Kendilerine ulaşan JWT'yi doğrulayıp `sub` claim'ini okurlar.** `ILoginService`, API tarafında bu kimliğin alınmasını ortak bir noktada tutar.

Örneğin sipariş geçmişinde tarayıcıdan gelen bir `userId` kullanılmaz. `MyOrders` endpoint'i token'daki kullanıcı ID'siyle sorgu oluşturur. Bir sipariş ID ile istendiğinde de müşterinin o siparişin sahibi olup olmadığı kontrol edilir.

### Erişim kuralları

| İşlem | Erişim |
| --- | --- |
| Ürün / kategori okuma, onaylı ürün yorumları | Giriş yapmadan görüntülenebilir |
| Sepet ve alışverişi tamamlama | Giriş yapan kullanıcı |
| Adresler ve sipariş geçmişi | Kullanıcının kendi kayıtları |
| Katalog yönetimi, yorum moderasyonu, tüm siparişler | Admin |
| Kargo ve kupon yönetimi API'leri | Admin veya Manager |

API'lerde imza, süre, issuer ve audience doğrulaması yapılır. Kayıt sahipliği ve rol kontrolleri, Gateway kontrolünün yanında ilgili API'de de uygulanır. WebUI'deki yönetim ekranlarında da ilgili controller'ın Admin / Manager rol kısıtları uygulanır.

JWT anahtarı ve bağlantı parolaları geliştirmede **.NET User Secrets**, sunucuda ortam değişkenleri üzerinden yapılandırılır. Ayrıntılar: [Güvenlik notları](SECURITY.md).

## 🐳 Docker ile Altyapı Yönetimi

Geliştirme ortamında **SQL Server, MongoDB, Redis ve RabbitMQ** Docker container'ları üzerinden çalıştırılır. Veritabanı ve mesajlaşma altyapısı Docker Desktop üzerinden yönetilir.

| Container altyapısı | Projedeki kullanım |
| --- | --- |
| SQL Server | Identity, Order, Discount, Comment ve Cargo veritabanları |
| MongoDB | Katalog ve ana sayfa içerikleri |
| Redis | Kullanıcıya ait sepet verileri |
| RabbitMQ | Sipariş olaylarının kuyruk üzerinden iletilmesi |

Repodaki `docker-compose.infrastructure.yml`, RabbitMQ container'ını, volume'unu ve healthcheck ayarlarını tanımlar. SQL Server, MongoDB ve Redis mevcut geliştirme ortamında ayrı container'lar olarak yönetilir.

<p align="center">
  <img src="docs/screenshots/docker-infrastructure.png" alt="Docker Desktop üzerinde geliştirme altyapısı container'ları" width="1000" />
</p>

## 📨 RabbitMQ ile Sipariş Olayı

```mermaid
flowchart LR
    A["Order API: siparişi kaydet"] --> B["OrderCreatedEvent"]
    B --> C["Exchange: multishop.events"]
    C -->|order.created| D["Queue: multishop.order.created"]
    D --> E["OrderCreatedConsumer"]
    E --> F["Mesajı oku, logla ve ACK gönder"]
```

Worker, mesajı okuyup sipariş bilgilerini loglar ve başarılı işlemde ACK gönderir. Bu bölüm servisler arası asenkron iletişimi öğrenmek için eklendi; otomatik e-posta veya kargo oluşturma işlemi yapmaz.

Mevcut sürümde **Outbox Pattern yoktur**. Sipariş kaydedildikten sonra RabbitMQ'ya yayın başarısız olursa olayın daha sonra otomatik gönderilmesi garanti edilmez.

## 🧪 Testler

Testleri bu projede adım adım öğrenerek ekledim. Basket ve Order testlerinde **NUnit**, Identity testlerinde **xUnit**, bağımlılıkları taklit etmek için **Moq** kullanılıyor.

| Test projesi | Başlıca senaryolar |
| --- | --- |
| `MultiShop.Basket.Tests` | Sepet toplamı, boş sepet, kullanıcıya ait sepet, fiyat/kupon/seçenek/adet doğrulama |
| `MultiShop.IdentityServer.Tests` | Başarılı ve hatalı giriş, kayıt, aynı e-posta, Customer rolü ve JWT sub claim'i |
| `MultiShop.Order.Application.Tests` | Sipariş oluşturma, ID ile sorgulama, sipariş ve kalemleri birlikte kaydetme |

```bash
dotnet test Tests/MultiShop.Basket.Tests/MultiShop.Basket.Tests.csproj
dotnet test Tests/MultiShop.IdentityServer.Tests/MultiShop.IdentityServer.Tests.csproj
dotnet test Tests/MultiShop.Order.Application.Tests/MultiShop.Order.Application.Tests.csproj
```

Bu testler seçilen iş kurallarını ve controller/handler davranışlarını kontrol eder. Tüm mikroservislerin birlikte çalışmasını veya gerçek ödeme akışını kapsayan uçtan uca bir test paketi değildir.

## 🗂️ Proje Yapısı

```text
MultiShop
├── ApiGateway
│   └── MultiShop.OcelotGateway
├── Frontends
│   ├── MultiShop.WebUI          # MVC, Razor, Areas, ViewComponents, HTTP servisleri
│   └── MultiShop.Dtos
├── IdentityServer
│   └── MultiShop.IdentityServer
├── Services
│   ├── Catalog
│   ├── Basket
│   ├── Discount
│   ├── Comment
│   ├── Cargo                   # Entities / Dtos / DataAccess / Business / WebApi
│   ├── Order                   # Domain / Application / Persistence / WebApi
│   └── Message                 # MessageBus / OrderCreatedConsumer
├── Tests
├── docs/screenshots
├── docker-compose.infrastructure.yml
└── SECURITY.md
```

## 🤝 Codex ile Geliştirme Deneyimim

Bu, Codex'i ilk kez bu kadar yoğun kullandığım projem oldu. Özellikle servislerin birbirine bağlanması, hata ayıklama, arayüz düzenlemeleri, güvenlik kontrolleri ve test senaryolarında Codex'ten destek aldım.

Süreçte benim için en önemli nokta yalnızca çalışan kod elde etmek değil, eklenen kodun neden gerekli olduğunu anlamaktı. Anlamadığım bölümlerde açıklama isteyerek ve özellikle testleri kendim yazarak ilerledim. Bu deneyim, AI ile çalışırken değişiklikleri küçük adımlarla incelemenin ve kendi anlayabildiğim kod yapısını korumanın önemini gösterdi.

## 🖼️ Ekran Görüntüleri

Görseller yerel demo verileriyle hazırlanmıştır.

<details>
<summary><strong>Ana sayfa</strong></summary>

### Anasayfa - 1

<p align="center">
  <img src="docs/screenshots/home-products.png" alt="Anasayfa - 1" width="1000" />
</p>

</details>

<details>
<summary><strong>Ürünler ve filtreleme</strong></summary>

### Ürünler

<p align="center">
  <img src="docs/screenshots/products.png" alt="Ürünler" width="1000" />
</p>

### Ürünler Filtreleme

<p align="center">
  <img src="docs/screenshots/product-filters.png" alt="Ürünler Filtreleme" width="1000" />
</p>

</details>

<details>
<summary><strong>Ürün detayları ve yorum</strong></summary>

### Ürün Detay - Ayakkabı

<p align="center">
  <img src="docs/screenshots/product-detail-shoes.png" alt="Ürün Detay - Ayakkabı" width="1000" />
</p>

### Ürün Detay - Laptop

<p align="center">
  <img src="docs/screenshots/product-detail-laptop.png" alt="Ürün Detay - Laptop" width="1000" />
</p>

### Ürün Detay - Yorum Yapma

<p align="center">
  <img src="docs/screenshots/product-review.png" alt="Ürün Detay - Yorum Yapma" width="1000" />
</p>

</details>

<details>
<summary><strong>Giriş ve kayıt</strong></summary>

### Giriş Yapma

<p align="center">
  <img src="docs/screenshots/login.png" alt="Giriş Yapma" width="1000" />
</p>

### Hesap Oluşturma

<p align="center">
  <img src="docs/screenshots/register.png" alt="Hesap Oluşturma" width="1000" />
</p>

</details>

<details>
<summary><strong>Sepet ve kupon</strong></summary>

### Sepet

<p align="center">
  <img src="docs/screenshots/cart.png" alt="Sepet" width="1000" />
</p>

### Sepette Kupon Uygulama

<p align="center">
  <img src="docs/screenshots/cart-coupon.png" alt="Sepette Kupon Uygulama" width="1000" />
</p>

</details>

<details>
<summary><strong>Adres, demo ödeme ve sipariş</strong></summary>

### Adres - Ödeme

<p align="center">
  <img src="docs/screenshots/checkout.png" alt="Adres - Ödeme" width="1000" />
</p>

### Siparişiniz Alındı

<p align="center">
  <img src="docs/screenshots/order-completed.png" alt="Siparişiniz Alındı" width="1000" />
</p>

### Siparişlerim

<p align="center">
  <img src="docs/screenshots/order-history.png" alt="Siparişlerim" width="1000" />
</p>

</details>

<details>
<summary><strong>Admin · Katalog yönetimi</strong></summary>

### Admin - Kategoriler

<p align="center">
  <img src="docs/screenshots/admin-categories.png" alt="Admin - Kategoriler" width="1000" />
</p>

### Admin - Kategori Güncelleme

<p align="center">
  <img src="docs/screenshots/admin-category-edit.png" alt="Admin - Kategori Güncelleme" width="1000" />
</p>

### Admin - Ürünler

<p align="center">
  <img src="docs/screenshots/admin-products.png" alt="Admin - Ürünler" width="1000" />
</p>

### Admin - Ürünü güncelle

<p align="center">
  <img src="docs/screenshots/admin-product-edit.png" alt="Admin - Ürünü güncelle" width="1000" />
</p>

### Admin - Ürün görselleri

<p align="center">
  <img src="docs/screenshots/admin-product-images.png" alt="Admin - Ürün görselleri" width="1000" />
</p>

### Admin - Ürün detayını güncelle

<p align="center">
  <img src="docs/screenshots/admin-product-description.png" alt="Admin - Ürün detayını güncelle" width="1000" />
</p>

### Admin - Markalar

<p align="center">
  <img src="docs/screenshots/admin-brands.png" alt="Admin - Markalar" width="1000" />
</p>

</details>

<details>
<summary><strong>Admin · Kampanya ve ana sayfa yönetimi</strong></summary>

### Admin - Kuponlar

<p align="center">
  <img src="docs/screenshots/admin-coupons.png" alt="Admin - Kuponlar" width="1000" />
</p>

### Admin - İndirim Alanları

<p align="center">
  <img src="docs/screenshots/admin-discount-banners.png" alt="Admin - İndirim Alanları" width="1000" />
</p>

### Admin - Özel Teklifler

<p align="center">
  <img src="docs/screenshots/admin-special-offers.png" alt="Admin - Özel Teklifler" width="1000" />
</p>

### Anasayfa - Slider

<p align="center">
  <img src="docs/screenshots/admin-sliders.png" alt="Anasayfa - Slider" width="1000" />
</p>

### Anasayfa - Vitrin Özellikleri

<p align="center">
  <img src="docs/screenshots/admin-featured.png" alt="Anasayfa - Vitrin Özellikleri" width="1000" />
</p>

</details>

<details>
<summary><strong>Admin · Sipariş, adres ve kargo</strong></summary>

### Admin - Siparişler

<p align="center">
  <img src="docs/screenshots/admin-orders.png" alt="Admin - Siparişler" width="1000" />
</p>

### Admin - Sipariş Detayı

<p align="center">
  <img src="docs/screenshots/admin-order-detail.png" alt="Admin - Sipariş Detayı" width="1000" />
</p>

### Admin - Müşteri adresleri

<p align="center">
  <img src="docs/screenshots/admin-customer-addresses.png" alt="Admin - Müşteri adresleri" width="1000" />
</p>

### Admin - Kargo Şirketleri

<p align="center">
  <img src="docs/screenshots/admin-cargo-companies.png" alt="Admin - Kargo Şirketleri" width="1000" />
</p>

</details>

<details>
<summary><strong>Admin · Yorum yönetimi</strong></summary>

### Admin - Yorumlar

<p align="center">
  <img src="docs/screenshots/admin-comments.png" alt="Admin - Yorumlar" width="1000" />
</p>

### Admin - Yorumu güncelle

<p align="center">
  <img src="docs/screenshots/admin-comment-edit.png" alt="Admin - Yorumu güncelle" width="1000" />
</p>

</details>

---

**Geliştirici:** [Furkan Türker](https://github.com/furkanturkerr)
