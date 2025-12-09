// Program.cs
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenAI.Extensions.DependencyInjection; // OpenAI API servisini eklemek için gerekli
using System;

var builder = WebApplication.CreateBuilder(args);

// ====================================================================
// 🔽 SERVİS TANIMLAMALARI 🔽
// ====================================================================

// OpenAI Yapılandırması: IOpenAIClient servisini DI'a ekler
builder.Services.AddOpenAI(options =>
{
    // API Key appsettings.json dosyasından okunacak ("OpenAI:ApiKey" yolunu kullanır)
    options.ApiKey = builder.Configuration["OpenAI:ApiKey"];
});

// MVC Controller'ları ve View'leri etkinleştirir
builder.Services.AddControllersWithViews();

// Cookie Authentication Servisi
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    });

// Session Servisi (TempData ve diğer session işlemleri için opsiyonel)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// ====================================================================
// 🔽 UYGULAMA YAPILANDIRMASI (Middleware) 🔽
// ====================================================================

var app = builder.Build();

// Production error handler
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // HTTP Strict Transport Security ayarı
    app.UseHsts();
}

app.UseHttpsRedirection();

// Statik dosyaları (CSS, JS, resimler) kullanmayı sağlar
app.UseStaticFiles();

// Routing Middleware'i
app.UseRouting();

// Authentication (Kullanıcı kimliğini doğrular)
app.UseAuthentication();

// Authorization (Kullanıcının yetkilerini kontrol eder)
app.UseAuthorization();

// Session'ı etkinleştirir
app.UseSession();

// Default Route Tanımlaması
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();