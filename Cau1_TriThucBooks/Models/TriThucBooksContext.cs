using Microsoft.EntityFrameworkCore;
using System;

namespace Cau1_TriThucBooks.Models
{
    public class TriThucBooksContext : DbContext
    {
        public DbSet<TheLoaiSach> TheLoaiSachs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TriThucBooksDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TheLoaiSach>(entity =>
            {
                entity.HasIndex(e => e.TenTheLoai).IsUnique();
                entity.Property(e => e.SoLuongSach).HasDefaultValue(0);
                entity.Property(e => e.NgayTao).HasDefaultValueSql("GETDATE()");
            });

            // Seed sample data matching exercise image
            modelBuilder.Entity<TheLoaiSach>().HasData(
                new TheLoaiSach { MaTL = 1, TenTheLoai = "Tiểu thuyết", MoTa = "Thân tiểu thuyết", SoLuongSach = 1, NgayTao = new DateTime(2022, 2, 20, 9, 57, 7) },
                new TheLoaiSach { MaTL = 2, TenTheLoai = "Kỹ năng sống", MoTa = "Kỹ năng sống nâng sống", SoLuongSach = 15, NgayTao = new DateTime(2022, 2, 20, 9, 57, 3) },
                new TheLoaiSach { MaTL = 3, TenTheLoai = "Thiếu nhi", MoTa = "Thiếu nhi củ có", SoLuongSach = 2, NgayTao = new DateTime(2022, 2, 20, 10, 52, 3) },
                new TheLoaiSach { MaTL = 4, TenTheLoai = "Sách giáo khoa", MoTa = "Sách giáo khoa chỉ đương phộp hơi vạo tạo", SoLuongSach = 2, NgayTao = new DateTime(2022, 2, 20, 10, 33, 7) },
                new TheLoaiSach { MaTL = 5, TenTheLoai = "Truyện tranh", MoTa = "Truyện tranh", SoLuongSach = 1, NgayTao = new DateTime(2022, 2, 20, 20, 42, 4) }
            );
        }
    }
}
