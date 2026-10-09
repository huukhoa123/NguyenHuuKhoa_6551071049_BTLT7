using Microsoft.EntityFrameworkCore;
using System;

namespace Cau4_AnKhangClinic.Models
{
    public class AnKhangClinicContext : DbContext
    {
        public DbSet<BacSi> BacSis { get; set; } = null!;
        public DbSet<LichKham> LichKhams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=AnKhangClinicDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BacSi>().HasData(
                new BacSi { MaBS = 1, HoTen = "Nguyễn Văn A", ChuyenKhoa = "Nội tổng quát", SDT = "0901234567" },
                new BacSi { MaBS = 2, HoTen = "Nguyễn Văn B", ChuyenKhoa = "Nội tổng quát", SDT = "0902345678" },
                new BacSi { MaBS = 3, HoTen = "Nguyễn Văn C", ChuyenKhoa = "Nội tổng quát", SDT = "0903456789" },
                new BacSi { MaBS = 4, HoTen = "Nguyễn Văn D", ChuyenKhoa = "Nội tổng quát", SDT = "0904567890" }
            );

            modelBuilder.Entity<LichKham>().HasData(
                new LichKham { MaLich = 1, TenBenhNhan = "Tên bệnh nhân", SDT = "09725667894", NgayKham = new DateTime(2022, 7, 13), GioKham = "09:00", MaBS = 1, TrangThai = "Chờ khám" },
                new LichKham { MaLich = 2, TenBenhNhan = "Nguyễn Xinh", SDT = "09725667897", NgayKham = new DateTime(2022, 12, 29), GioKham = "13:00", MaBS = 1, TrangThai = "Đã khám" },
                new LichKham { MaLich = 3, TenBenhNhan = "Nguyễn Hạm", SDT = "09725667899", NgayKham = new DateTime(2022, 12, 23), GioKham = "16:00", MaBS = 1, TrangThai = "Đã hủy" },
                new LichKham { MaLich = 4, TenBenhNhan = "Nguyễn Tình", SDT = "09725667830", NgayKham = new DateTime(2022, 12, 23), GioKham = "16:00", MaBS = 1, TrangThai = "Đã hủy" },
                new LichKham { MaLich = 5, TenBenhNhan = "Nguyễn Hoan anh", SDT = "09725667899", NgayKham = new DateTime(2022, 12, 29), GioKham = "18:00", MaBS = 1, TrangThai = "Đã hủy" }
            );
        }
    }
}
