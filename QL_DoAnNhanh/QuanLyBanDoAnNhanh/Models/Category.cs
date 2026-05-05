using System.ComponentModel.DataAnnotations;

namespace QuanLyBanDoAnNhanh.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tên loại")]
        public string Name { get; set; } = string.Empty;
        // Một danh mục có nhiều sản phẩm
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}