🏋️‍♂️ Fitness Center Yönetim ve Randevu Sistemi

Bu proje, Web Programlama dersi için geliştirdiğimiz bir Spor Salonu Yönetim ve Randevu Sistemi uygulamasıdır. Amaç; antrenörlerin, hizmetlerin ve üyelerin randevularının yönetilebildiği temel bir web sistemi oluşturmaktı. Projeyi ASP.NET Core MVC kullanarak yaptık.

📌 Proje Hakkında

Projede aşağıdaki ana bileşenler bulunuyor:

🏢 Spor Salonları

Tek veya birden fazla salon eklenebiliyor.

Salonların çalışma saatleri, hizmet türleri, süre ve ücret bilgileri yönetilebiliyor.

🧑‍🏫 Antrenör Yönetimi

Antrenör ekleme, düzenleme, silme (CRUD).

Uzmanlık alanı ekleme.

Müsaitlik saatleri sayesinde randevu takvimi oluşturma.

👤 Üye ve Randevu Sistemi

Üyeler sisteme kayıt olup giriş yapabiliyor.

Uygun zamana göre randevu alabiliyor.

Sistem randevu çakışmalarını kontrol ediyor.

Randevular admin tarafından onaylanabiliyor.

🔐 Rol Sistemi

Aşağıdaki gibi iki rol bulunuyor:

Rol	Mail	Şifre
Admin	ogrencinumarasi@sakarya.edu.tr
	sau
Üye	Kayıt ekranından oluşturulur	—
📡 API Kullanımı

Projede REST API ile veritabanından veri çektiğimiz bölümler var.
Örnek kullanım:

Tüm antrenörleri listeleme

Belirli bir tarihe göre uygun antrenörü getirme

Üyenin geçmiş randevularını API üzerinden çekme

LINQ filtrelemeleri API tarafında aktif olarak kullanıldı.

🛠️ Kullanılan Teknolojiler

ASP.NET Core MVC

C#

Entity Framework Core

LINQ

SQL Server veya PostgreSQL

Bootstrap 5

HTML, CSS, JavaScript, jQuery

🎨 Arayüz

Arayüzde Bootstrap 5 kullandık.
Admin ve üye panelleri için sade, kullanımı kolay sayfalar oluşturduk.

🔧 CRUD İşlemleri

Aşağıdaki varlıklar için tam CRUD işlemleri yapıldı:

Salon

Hizmet

Antrenör

Üyeler

Randevular

İstemci ve sunucu taraflı doğrulamalar (validation) eklendi.

👨‍💻 Geliştiriciler

Ad Soyad: Tunahan Avşar
Öğrenci No: G221210074

Ad Soyad: Anıl Gezertaşar
Öğrenci No: G221210041

Ders: Web Programlama
Dönem: 2025–2026 Güz
