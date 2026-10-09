using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cau1_TriThucBooks.Models
{
    [Table("TheLoaiSach")]
    public class TheLoaiSach
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaTL { get; set; }

        [Required]
        [StringLength(100)]
        public string TenTheLoai { get; set; } = string.Empty;

        [StringLength(255)]
        public string? MoTa { get; set; }

        public int SoLuongSach { get; set; } = 0;

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}
