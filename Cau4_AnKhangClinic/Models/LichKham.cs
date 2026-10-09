using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cau4_AnKhangClinic.Models
{
    [Table("LichKham")]
    public class LichKham
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLich { get; set; }

        [Required]
        [StringLength(100)]
        public string TenBenhNhan { get; set; } = string.Empty;

        [StringLength(15)]
        public string? SDT { get; set; }

        public DateTime NgayKham { get; set; } = DateTime.Today;

        [StringLength(10)]
        public string GioKham { get; set; } = "08:00";

        public int MaBS { get; set; }

        [ForeignKey("MaBS")]
        public virtual BacSi? MaBSNavigation { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; } = "Chờ khám"; // Chờ khám, Đã khám, Đã hủy
    }
}
