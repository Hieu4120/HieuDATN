using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_feedback
    {
        [Key]
        public int feedback_id { get; set; }
        public int customer_id { get; set; }
        public string email { get; set; }
        public string message { get; set; }
        public string user_name { get; set; }
        public string status { get; set; }
        public DateTime update_at { get; set; }
    }
}
