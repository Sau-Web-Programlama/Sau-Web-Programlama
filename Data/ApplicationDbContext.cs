using SporSalonu2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema; // Eğer [Column] gibi attributelar kullanıyorsanız bu gerekli

namespace SporSalonu2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Service> Services { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Veri Tohumlama (Seed Data)
            modelBuilder.Entity<Service>().HasData(
                new Service { Id = 1, Name = "Fitness", Description = "Ağırlık ve kardiyo ekipmanları.", DurationMinutes = 60, Price = 350 },
                new Service { Id = 2, Name = "Yoga", Description = "Esneklik ve zihin dengesi.", DurationMinutes = 45, Price = 400 },
                new Service { Id = 3, Name = "Pilates", Description = "Core güçlendirme ve duruş düzeltme.", DurationMinutes = 50, Price = 450 }
            );

            // Veritabanı başlangıcı için 3 antrenör
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer { Id = 1, FirstName = "Ahmet", LastName = "Yılmaz", Specialty = "Fitness & Kilo Verme", Email = "ahmet@fitzone.com", Phone = "5551112233", Bio = "Kişisel antrenör ve beslenme danışmanı." },
                new Trainer { Id = 2, FirstName = "Zeynep", LastName = "Kara", Specialty = "Yoga & Meditasyon", Email = "zeynep@fitzone.com", Phone = "5554445566", Bio = "Uzman yoga eğitmeni." },
                new Trainer { Id = 3, FirstName = "Mehmet", LastName = "Demir", Specialty = "Pilates & Rehabilitasyon", Email = "mehmet@fitzone.com", Phone = "5557778899", Bio = "Reformer ve mat pilates uzmanı." }
            );

            // Ahmet Yılmaz için örnek müsaitlik (DayOfWeek çakışması düzeltildi)
            modelBuilder.Entity<Availability>().HasData(
               new Availability
               {
                   Id = 1,
                   TrainerId = 1,
                   DayOfWeek = SporSalonu2.Models.DayOfWeek.Pazartesi,
                   StartTime = new TimeSpan(9, 0, 0),
                   EndTime = new TimeSpan(12, 0, 0),
                   IsBookable = true
               },
               new Availability
               {
                   Id = 2,
                   TrainerId = 1,
                   DayOfWeek = SporSalonu2.Models.DayOfWeek.Sali,
                   StartTime = new TimeSpan(14, 0, 0),
                   EndTime = new TimeSpan(17, 0, 0),
                   IsBookable = true
               }
            );
        }
    }
}