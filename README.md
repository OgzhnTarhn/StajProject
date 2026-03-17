<div align="center">
  <h1>🚀 StajProject</h1>
  <p>Kurumsal ihtiyaçlara yönelik, SAP destekli, modern ve ölçeklenebilir bir ASP.NET web çözümü.</p>
</div>

---

## 📖 Projenin Amacı
Bu proje, kurumsal veri yönetimi ile web tabanlı arayüzleri bir araya getirmek üzere geliştirilmiş kapsamlı bir web uygulamasıdır. İçerisindeki **SAP DotNet Connector 3** entegrasyonu sayesinde doğrudan SAP sistemleriyle haberleşebilir, veri alışverişi yapabilir ve veritabanı işlemlerini güvenilir bir şekilde yönetebilir. Temel amacı işletmelerin iç süreçlerini dijitalleştirip verimli ve hızlı bir sistem mimarisine dönüştürmektir.

## 🛠 Teknik Özellikler
Modern bir altyapı kullanılarak inşa edilen bu projede kullanılan temel teknolojiler şunlardır:

### 🎨 Frontend (Önyüz)
- **ASP.NET MVC (Razor Views):** Sunucu taraflı, dinamik ve güçlü görünümler.
- **Bootstrap 5:** Tamamen duyarlı (responsive) ve modern arabirim tasarımı.
- **jQuery ve AJAX:** Asenkron veri yönetimi ve akıcı bir kullanıcı deneyimi.

### ⚙ Backend (Arkayüz) & Durum Yönetimi
- **C# & .NET Framework 4.7.2:** Güçlü, güvenli ve yüksek performanslı çekirdek altyapı.
- **ASP.NET Web API & MVC:** RESTful servisler ve model-view-controller (MVC) mimarisi ile temiz ve anlaşılır kod yapısı.

### 🗄 Veritabanı & Entegrasyonlar
- **Entity Framework 6:** ORM (Object-Relational Mapping) ile veritabanı işlemlerinde maksimum hız ve kolay yönetim.
- **SAP .NET Connector 3.0:** SAP ERP sistemleriyle gerçek zamanlı çift yönlü haberleşme altyapısı.

### 🌍 Sunucu ve Barındırma (Hosting)
- **Natro:** Projenin canlı ortama taşınması ve kesintisiz yayını yüksek performanslı ve güvenilir **Natro** hosting altyapısı ile sağlanmaktadır.

---

## ✨ Öne Çıkan Özellikler
- **Gerçek Zamanlı SAP Bağlantısı:** Özel parametreler ile (Client, SystemNumber, PoolSize vb.) SAP Application Server üzerinden yüksek hızlı iletişim.
- **Esnek Katmanlı Mimari:** Model, View ve Controller (MVC) mantığı sayesinde kolayca genişletilebilir modüller.
- **Kullanıcı Dostu Arayüz:** Gelişmiş form doğrulamaları (jQuery.Validation, Unobtrusive) ile hatasız ve sorunsuz deneyim.

---

## 📂 Proje Yapısı

```text
StajProject/
├── 📁 App_Start/       # Yönlendirme (Routing), Bundle ve genel uygulama filtreleri
├── 📁 Controllers/     # HTTP isteklerini işleyen ve görünümleri yöneten sınıflar
├── 📁 Models/          # Veritabanı Entity modelleri, ViewModel'ler ve iş mantığı
├── 📁 Views/           # Razor (.cshtml) sayfaları ve HTML arayüzleri
├── 📁 Scripts/         # jQuery ve projenin özel JavaScript kütüphaneleri
├── 📁 Content/         # CSS stilleri ve Bootstrap dosyaları
├── Web.config          # SAP bağlantı bilgileri ve genel konfigürasyon ayarları
└── packages.config     # Kullanılan dış NuGet bağımlılıkları listesi
```

---

## 🛠 Kurulum ve Çalıştırma (Adım Adım)
Projeyi kendi yerel ortamınızda ayağa kaldırmak için aşağıdaki adımları sırasıyla uygulayın:

1. **Gereksinimleri Kurun:**
   - Bilgisayarınızda [Visual Studio 2022](https://visualstudio.microsoft.com/) yüklü ve güncel olmalıdır.
   - .NET Framework 4.7.2 SDK dosyalarının kurulu olduğuna dikkat edin.

2. **Projeyi Klonlayın & Açın:**
   ```bash
   git clone <proje-git-adresi>
   ```
   İndirme tamamlandıktan sonra `StajProject.sln` çözüm dosyasına çift tıklayarak projeyi Visual Studio üzerinden açın.

3. **Bağımlılıkları Yükleyin:**
   - Visual Studio içerisinde `Tools > NuGet Package Manager > Manage NuGet Packages for Solution` (veya projeye sağ tıklayarak *Restore NuGet Packages*) sekmesinden eksik olan Entity Framework, SAP Connector ve Bootstrap gibi paketlerin yüklenmesini sağlayın.

4. **Konfigürasyonları Ayarlayın:**
   - Ekrandaki `Web.config` dosyasını açıp `<appSettings>` alanındaki **SAP bağlantı bilgilerini** (Client, IP, Kullanıcı adı, Şifre vb.) kendi SAP sunucunuza uygun şekilde güncelleyin.
   - Ayrıca MS SQL veritabanı kullanacaksanız `<connectionStrings>` tarafında kullanacağınız veritabanı yolunu girin.

5. **Projeyi Başlatın:**
   - Klavyeden `F5` tuşuna basarak veya yukarıdaki yeşil "Start" butonuna tıklayarak IIS Express aracılığıyla projeyi tarayıcınızda hemen başlatabilirsiniz. 🎉

---

## 🔮 Geleceğe İlham: Sırada Ne Var?
> *“En yenilikçi sistemler, sürekli gelişime açık esnek bir temelin üzerinde yükselir.”*

Bu proje, güçlü C# çekirdeği ve SAP entegrasyon özellikleriyle çok daha büyük vizyonlara ulaşmaya son derece hazırdır. İlerleyen süreçte aşağıdaki adımlar geliştirme heyecanımızı taze tutacaktır:

- **Büyük Veri (Big Data) & Yapay Zeka (AI) Entegrasyonu:** SAP sisteminden çekilen kurumsal loglar üzerinde makine öğrenmesi modelleri çalıştırılarak geleceğe yönelik kestirimsel (predictive) veri analizi oluşturulması.
- **İleri Seviye Dashboard Yönetimi:** Arayüze dahil edilecek interaktif grafikler sayesinde saniyeler içerisinde göz alıcı raporlama panellerinin sisteme eklenmesi.
- **PWA (Progressive Web Application):** Tüm projenin PWA standartlarında modernize edilerek çevrimdışı ve mobil tarayıcıda bile yerel bir uygulama gibi kesintisiz çalışması. 

Geliştirmeye ve inovasyon sınırlarını zorlamaya her zaman devam! Yalnızca var olanı inşa etmekle kalmıyor, kurumsallaşmanın geleceğini yazıyoruz. 🌟

---
<p align="center">Made with ❤️ for an advanced software architecture.</p>
