using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_review
    {
        public int review_id { get; set; }
        public int book_id { get; set; }
        public int rating { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập nội dung")]
        public string review_content { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string user_name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email")]
        public string email { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }

    }
}
