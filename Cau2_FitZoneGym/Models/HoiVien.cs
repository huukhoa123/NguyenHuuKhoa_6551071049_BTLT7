using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cau2_FitZoneGym.Models
{
    [Table("HoiVien")]
    public class HoiVien
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaHV { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        public bool GioiTinh { get; set; } = true; // true = Nam, false = Nữ

        public DateTime NgaySinh { get; set; } = DateTime.Now.AddYears(-20);

        [StringLength(15)]
        public string? SDT { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string HangThanhVien { get; set; } = "Basic"; // Basic, VIP, Premium

        public DateTime NgayDangKy { get; set; } = DateTime.Now;

        public bool TrangThai { get; set; } = true; // true = Đang hoạt động, false = Tạm ngưng
    }
}
