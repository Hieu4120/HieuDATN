using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Model
{
    public class m_customer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int checkout_id { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public DateTime update_at { get; set; }
        public int? customer_phone_number { get; set; }
        public int customer_id { get; set; }
    }
}
