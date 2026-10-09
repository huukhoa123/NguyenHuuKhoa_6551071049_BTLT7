using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cau4_AnKhangClinic.Models
{
    [Table("BacSi")]
    public class BacSi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MaBS { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ChuyenKhoa { get; set; }

        [StringLength(15)]
        public string? SDT { get; set; }

        public virtual ICollection<LichKham> LichKhams { get; set; } = new List<LichKham>();

        [NotMapped]
        public string DisplayText => $"BS. {HoTen}" + (string.IsNullOrEmpty(ChuyenKhoa) ? "" : $" - {ChuyenKhoa}");
    }
}
