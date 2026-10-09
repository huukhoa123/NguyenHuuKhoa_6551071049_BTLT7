using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cau3_SunriseHomestay.Models
{
    [Table("Phong")]
    public class Phong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaPhong { get; set; }

        [Required]
        [StringLength(10)]
        public string SoPhong { get; set; } = string.Empty;

        public int TangSo { get; set; } = 1;

        [StringLength(20)]
        public string TinhTrang { get; set; } = "Trống"; // Trống, Đang ở, Đang dọn

        [StringLength(255)]
        public string? HinhAnh { get; set; }

        public int MaLoai { get; set; }

        [ForeignKey("MaLoai")]
        public virtual LoaiPhong? MaLoaiNavigation { get; set; }
    }
}
