using Microsoft.EntityFrameworkCore;
using System;

namespace Cau2_FitZoneGym.Models
{
    public class FitZoneContext : DbContext
    {
        public DbSet<HoiVien> HoiViens { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=FitZoneGymDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HoiVien>(entity =>
            {
                entity.Property(e => e.NgayDangKy).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.TrangThai).HasDefaultValue(true);
            });

            // Seed sample data as in page 3 of document screenshot
            modelBuilder.Entity<HoiVien>().HasData(
                new HoiVien { MaHV = 1, HoTen = "Họ tên Vốn", GioiTinh = true, NgaySinh = new DateTime(1999, 11, 7), SDT = "07382735879", Email = "von@gmail.com", HangThanhVien = "Basic", NgayDangKy = DateTime.Now, TrangThai = true },
                new HoiVien { MaHV = 2, HoTen = "Nguyễn Nnh", GioiTinh = true, NgaySinh = new DateTime(1999, 11, 10), SDT = "07382731034", Email = "nnh@gmail.com", HangThanhVien = "Premium", NgayDangKy = DateTime.Now, TrangThai = true },
                new HoiVien { MaHV = 3, HoTen = "Nguyễn Bộ Đôn", GioiTinh = true, NgaySinh = new DateTime(1999, 12, 29), SDT = "07882773727", Email = "don@gmail.com", HangThanhVien = "Basic", NgayDangKy = DateTime.Now, TrangThai = true },
                new HoiVien { MaHV = 4, HoTen = "Nguyễn Xim", GioiTinh = true, NgaySinh = new DateTime(1999, 11, 17), SDT = "07375856597", Email = "xim@gmail.com", HangThanhVien = "VIP", NgayDangKy = DateTime.Now, TrangThai = true },
                new HoiVien { MaHV = 5, HoTen = "Nguyễn Tương", GioiTinh = false, NgaySinh = new DateTime(1999, 1, 22), SDT = "07887775931", Email = "tuong@gmail.com", HangThanhVien = "Premium", NgayDangKy = DateTime.Now, TrangThai = true }
            );
        }
    }
}
