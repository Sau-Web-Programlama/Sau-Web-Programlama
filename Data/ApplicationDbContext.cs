using SporSalonu2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SporSalonu2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // KULLANILAN TABLOLAR:
        public DbSet<User> Users { get; set; }       // Üyeler ve Adminler
        public DbSet<Service> Services { get; set; } // Hizmetler (Boks, Pilates vb.)
        public DbSet<Trainer> Trainers { get; set; } // Antrenörler
        public DbSet<Booking> Bookings { get; set; } // Randevular

        // 'Availability' (Müsaitlik) tablosunu sildik, çünkü artık sabit saat sistemi kullanıyoruz.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}