using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_account
    {
        [Key]
        public int customer_id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên người dùng")]
        public string username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu ")]
        public string password { get; set; }
        public string role { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email ")]
        [EmailAddress(ErrorMessage = "Email bạn nhập không hợp lệ ")]
        public string email { get; set; }
        public DateTime? create_at {get; set;}
    }
}
