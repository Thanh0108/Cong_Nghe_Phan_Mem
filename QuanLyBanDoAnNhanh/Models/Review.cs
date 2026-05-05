using System.ComponentModel.DataAnnotations;

namespace QuanLyBanDoAnNhanh.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Range(1, 5, ErrorMessage = "Vui lòng chọn từ 1 đến 5 sao")]
        public int Rating { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Bình luận không quá 500 ký tự")]
        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
