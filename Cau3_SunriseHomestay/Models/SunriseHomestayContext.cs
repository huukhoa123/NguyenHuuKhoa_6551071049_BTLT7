using Microsoft.EntityFrameworkCore;

namespace Cau3_SunriseHomestay.Models
{
    public class SunriseHomestayContext : DbContext
    {
        public DbSet<LoaiPhong> LoaiPhongs { get; set; } = null!;
        public DbSet<Phong> Phongs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=SunriseHomestayDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoaiPhong>().HasData(
                new LoaiPhong { MaLoai = 1, TenLoai = "Phòng Đơn", GiaMoiDem = 350000, MoTa = "Phòng dành cho 1 người" },
                new LoaiPhong { MaLoai = 2, TenLoai = "Phòng Đôi", GiaMoiDem = 200000, MoTa = "Phòng dành cho 2 người" },
                new LoaiPhong { MaLoai = 3, TenLoai = "Phòng VIP", GiaMoiDem = 300000, MoTa = "Phòng VIP cao cấp" }
            );

            modelBuilder.Entity<Phong>().HasData(
                new Phong { MaPhong = 1, SoPhong = "1", TangSo = 1, TinhTrang = "Trống", HinhAnh = "room1.jpg", MaLoai = 1 },
                new Phong { MaPhong = 2, SoPhong = "2", TangSo = 1, TinhTrang = "Đang ở", HinhAnh = "room2.jpg", MaLoai = 2 },
                new Phong { MaPhong = 3, SoPhong = "3", TangSo = 2, TinhTrang = "Đang dọn", HinhAnh = "room1.jpg", MaLoai = 1 },
                new Phong { MaPhong = 4, SoPhong = "4", TangSo = 4, TinhTrang = "Đang dọn", HinhAnh = "room2.jpg", MaLoai = 2 },
                new Phong { MaPhong = 5, SoPhong = "5", TangSo = 5, TinhTrang = "Đang dọn", HinhAnh = "room3.jpg", MaLoai = 3 }
            );
        }
    }
}
