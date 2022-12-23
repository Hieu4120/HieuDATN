using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_account
    {
        [Key]
        public int customer_id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string email { get; set; }
        public DateTime? create_at {get; set;}
    }
}
