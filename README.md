# PaparaStore API

## Genel Bakış

PaparaStore, e-ticaret işlevleri sunan, ürün yönetimi, kategori yönetimi, sepet işlemleri, sipariş işleme ve ödeme işlemleri gibi özellikleri içeren .NET Core tabanlı bir Web API'dir. API, JWT kimlik doğrulama, Redis ile önbellekleme, Serilog ile loglama ve MediatR kullanarak CQRS komut ve sorgu işlemlerini yönetir.

## Özellikler

- **Ürün Yönetimi**: Ürünler için CRUD işlemleri.
- **Kategori Yönetimi**: Kategoriler için CRUD işlemleri, etiket bazlı filtreleme.
- **Sepet İşlemleri**: Sepete ürün ekleme, güncelleme ve çıkarma.
- **Sipariş İşleme**: Sipariş oluşturma, indirim uygulama, cüzdan bakiyesi kullanma ve ödül puanı kazanma.
- **Ödeme İşlemleri**: Kupon kodları ve ödül puanlarını destekleyen ödeme işlemleri.
- **JWT Kimlik Doğrulama**: JWT tokenları ile API'yi güvence altına alma.
- **Önbellekleme**: Performansı artırmak için Redis ile önbellekleme.
- **Loglama**: Serilog ile merkezi loglama.
- **Swagger**: API dokümantasyonu ve testi.

## Kullanılan Teknolojiler

- **.NET Core**: API'nin oluşturulduğu framework.
- **Entity Framework Core**: Veritabanı işlemleri için kullanılan ORM.
- **AutoMapper**: Domain modelleri ile DTO'lar arasındaki eşlemeler için.
- **MediatR**: CQRS pattern'in uygulanması için.
- **FluentValidation**: Gelen isteklerin doğrulanması için.
- **Redis**: Dağıtık önbellekleme için.
- **Serilog**: Yapılandırılmış loglama için.
- **Swagger**: API dokümantasyonu ve testi için.

## Başlangıç

### Gereksinimler

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download)
- [RabbitMQ](https://www.rabbitmq.com/download.html)

### Konfigürasyon

Uygulamayı çalıştırmadan önce `appsettings.json` dosyasını aşağıdaki ayarlarla güncelleyin:

- **ConnectionStrings**
  - `MsSqlConnection`: SQL Server veritabanı için bağlantı dizesi.
- **Redis**
  - `Host`: Redis sunucu hostu (örn. `localhost`).
  - `Port`: Redis sunucu portu (varsayılan `6379`).
- **JwtConfig**
  - `Secret`: JWT tokenlarının imzalanması için kullanılan gizli anahtar.
  - `Issuer` ve `Audience`: Tokenın issuer ve audience bilgileri.
- **Serilog**
  - Loglama çıktıları, örneğin Konsol veya Dosya için yapılandırma.
- **RabbitMQ**
  - `Host`, `Username`, `Password`, `QueueName`: RabbitMQ yapılandırması.

### Veritabanı Kurulumu

1. Projeyi IDE'de açın.
2. Veritabanını oluşturmak ve migrationları uygulamak için aşağıdaki komutu çalıştırın:
   **dotnet ef database update**
   
SQL Server'ın çalıştığından ve appsettings.json dosyasındaki bağlantı dizesinin doğru olduğundan emin olun.
Uygulamayı Çalıştırma
Uygulamayı çalıştırmak için aşağıdaki komutu kullanın:
**dotnet run**

API varsayılan olarak http://localhost:5000 adresinde kullanılabilir olacaktır.

Swagger dokümantasyonu http://localhost:5000/swagger adresinde kullanılabilir.

**Test Etme**
API uç noktalarını test etmek için Swagger'ı veya Postman gibi bir API test aracı kullanabilirsiniz.

### Klasör Yapısı
Controllers: API denetleyicilerini içerir.
Business: Komut ve sorgu işleyicileri, iş mantığı ve doğrulayıcıları içerir.
Data: Varlık modellerini, yapılandırmaları ve repository implementasyonlarını içerir.
Schema: DTO'ları ve istek/yanıt modellerini içerir.
Base: BaseEntity, ApiResponse ve JWT yapılandırmaları gibi ortak sınıfları içerir.
Middleware: Hata işleme ve loglama için özel middleware'leri içerir.


Lisans
Bu proje MIT Lisansı altında lisanslanmıştır. Daha fazla ayrıntı için LICENSE dosyasına bakın.

### İletişim
Herhangi bir soru veya sorunuz için kenanakin6134@gmail.com ile iletişime geçin.
