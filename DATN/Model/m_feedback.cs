using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_feedback
    {
        [Key]
        public int feedback_id { get; set; }
        public int customer_id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập Email")]
        public string email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tin nhắn")]
        public string message { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string user_name { get; set; }
        public string status { get; set; }
        public DateTime update_at { get; set; }
    }
}
