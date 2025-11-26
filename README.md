DepoAutomation - Mikroservis Tabanlı Depo Yönetim Sistemi 

Bu proje, üniversitede edindiğim teorik bilgileri pratiğe dökmek ve yetkinliğimi artırmak amacıyla geliştirdiğim kapsamlı bir Full-Stack projesidir.

## Teknoloji Yığını (Tech Stack)

### Backend (.Net 8 Core)
* **Mimari:** MicroServices, ClenArchitecture
* **İletişim:** REST API, YARP Reverse Proxy
* **Veritabanı & ORM:** PostgreSQL, Entity Framework Core (Code First)
* **Architecture Patterni:** CQRS (MediatR kütüphanesi ile)
* **Cache:** Redis (StackExchange.Redis)
* **Validasyon:** FluentValidation
* **Auth:** JWT (JSON Web Token) & Identity

### Frontend (Next.js)
* **Framework:** Next.js 16 (App Router yapısı)
* **Dil:** TypeScript
* **Styling:** Tailwind CSS
* **State & Fetch:** Axios (Custom Interceptors)
* **UI Bileşenleri:** Recharts (Grafikler için)

### Altyapı
* **Docker & Docker Compose:** Tüm servislerin ve veritabanlarının tek komutla ayağa kaldırılması.
* **PostgreSQL 16:** İlişkisel veri tutarlılığı için.
* **Redis 7:** Anlık stok takibi ve performans için.

## Mimari Yapı

Sistem, her biri kendi `DbContext`'ine ve sorumluluğuna sahip 4 ana mikroservis ve bir ağ geçidinden (APIGateway) oluşur:
| **ApiGateway** | Tüm dış istekleri karşılar, yükü dağıtır ve ilgili servise yönlendirir. ( YARP )
| **Identity** | Kullanıcı kaydı, girişi (Admin/Operator) ve JWT token üretimi. (ASP.NET Identity)
| **Catalog** | Ürün, kategori ve lokasyon tanımlamaları. ( EF Core, Postgres )
| **Inventory** | Stok giriş/çıkış hareketleri ve Redis ile hızlı stok sorgulama. ( Redis, CQRS )
| **Job** | Depo operasyonları (Toplama, Yerleştirme) için iş emirleri oluşturma.

**Admin Paneli:** Ürün ekleme, kategori yönetimi, lokasyon tanımlama ve dashboard raporları.

**Operasyon:** Mal kabul, transfer ve stok toplama süreçleri.

**Güvenlik:** Rol tabanlı yetkilendirme (Sadece Adminler ürün silebilir, Operatörler iş emri tamamlayabilir ama oluşturamazlar).
