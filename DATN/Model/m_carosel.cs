using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_carosel
    {
        [Key]
        public int carosel_id { get; set; }
        public byte[] carosel_image { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string tiltle { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string content { get; set; }
        public DateTime create_at { get; set; }

    }
}
