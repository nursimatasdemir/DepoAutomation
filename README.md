## DepoAutomation - Mikroservis Tabanlı Depo Yönetim Sistemi 

Bu proje, üniversitede edindiğim teorik bilgileri pratiğe dökmek ve yetkinliğimi artırmak amacıyla geliştirdiğim kapsamlı bir Full-Stack projesidir.

## Teknoloji Yığını
 
| Katman | Teknolojiler |
|---|---|
| Backend | .NET 8, ASP.NET Core Web API, Clean Architecture, CQRS (MediatR), FluentValidation |
| Veri | PostgreSQL 16, Entity Framework Core (Code First, migration'lar repoda), Redis 7 (StackExchange.Redis) |
| Kimlik | ASP.NET Identity, JWT (Bearer) |
| Ağ geçidi | YARP Reverse Proxy |
| Frontend | Next.js 16 (App Router), React 19, TypeScript, Tailwind CSS 4, Axios, Recharts |
| Altyapı | Docker Compose (PostgreSQL, Redis, RabbitMQ) |
 
RabbitMQ `docker-compose.yml` içinde tanımlıdır; servis kodunda şu an kullanılmamaktadır.

## Mimari Yapı

Sistem, her biri kendi `DbContext`'ine ve sorumluluğuna sahip 4 ana mikroservis ve bir ağ geçidinden (APIGateway) oluşur:
|---|---|
| **ApiGateway** | Tüm dış istekleri karşılar, yükü dağıtır ve ilgili servise yönlendirir. ( YARP ) | 
| **Identity** | Kullanıcı kaydı, girişi (Admin/Operator) ve JWT token üretimi. (ASP.NET Identity) |
| **Catalog** | Ürün, kategori ve lokasyon tanımlamaları. ( EF Core, Postgres ) |
| **Inventory** | Stok giriş/çıkış hareketleri ve Redis ile hızlı stok sorgulama. ( Redis, CQRS ) |
| **Job** | Depo operasyonları (Toplama, Yerleştirme) için iş emirleri oluşturma. |

**Admin Paneli:** Ürün ekleme, kategori yönetimi, lokasyon tanımlama ve dashboard raporları.

**Operasyon:** Mal kabul, transfer ve stok toplama süreçleri.

**Güvenlik:** Rol tabanlı yetkilendirme (Sadece Adminler ürün silebilir, Operatörler iş emri tamamlayabilir ama oluşturamazlar).

## Kurulum
 
### Gereksinimler
 
.NET 8 SDK, Node.js (Next.js 16 ile uyumlu bir LTS sürümü), Docker ve Docker Compose, `dotnet-ef` aracı.
 
### 1. Altyapıyı başlat
 
```bash
cp .env.example .env        # değerleri kendinize göre değiştirin
docker compose up -d
```
 
Bu komut yalnızca PostgreSQL, Redis ve RabbitMQ'yu başlatır. .NET servisleri ve frontend ayrıca çalıştırılır.
 
### 2. Servis ayarlarını oluştur
 
Her servis klasöründeki `appsettings.Development.json.example` dosyasını `appsettings.Development.json` olarak kopyalayın ve değerleri doldurun. Bu dosyalar `.gitignore` tarafından dışarıda tutulur. Tüm servislerde `JwtSettings` değerleri aynı olmalıdır.
 
ApiGateway'in `ReverseProxy` rota ayarları da `appsettings.Development.json` içinde tanımlanır. Örnek: `src/ApiGateway/ApiGateway/appsettings.Development.json.example`.
 
### 3. Veritabanı migration'larını uygula
 
```bash
dotnet ef database update --project src/services/Identity/Identity.Infrastructure --startup-project src/services/Identity/Identity.API
dotnet ef database update --project src/services/Catalog/Catalog.Infrastructure --startup-project src/services/Catalog/Catalog.API
dotnet ef database update --project src/services/Inventory/Inventory.Infrastructure --startup-project src/services/Inventory/Inventory.API
dotnet ef database update --project src/services/Job/Job.Infrastructure --startup-project src/services/Job/Job.API
```
 
### 4. Servisleri çalıştır
 
Her servisi ayrı terminalde başlatın: `dotnet run --project src/services/<Servis>/<Servis>.API` ve `dotnet run --project src/ApiGateway/ApiGateway`.
 
### 5. Frontend'i çalıştır
 
```bash
cd frontend
cp .env.local.example .env.local
npm install
npm run dev
```

**Admin Paneli Görünümü**
<img width="1280" height="710" alt="image" src="https://github.com/user-attachments/assets/eaf057a3-ea8a-4a93-99dc-f95234d77823" />

**Operatör Paneli Görünümü**
<img width="1280" height="301" alt="image" src="https://github.com/user-attachments/assets/830c91c9-1b17-43b5-bdee-1b60cb95c6e2" />


