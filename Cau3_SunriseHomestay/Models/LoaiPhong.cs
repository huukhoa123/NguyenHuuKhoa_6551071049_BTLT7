using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cau3_SunriseHomestay.Models
{
    [Table("LoaiPhong")]
    public class LoaiPhong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaLoai { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLoai { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal GiaMoiDem { get; set; }

        [StringLength(255)]
        public string? MoTa { get; set; }

        public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
    }
}
