using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_cart
    {
        [Key]
        public int cart_id { get; set; }
        public int customer_id { get; set; }
        public int book_id { get; set; }
        public int amount { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }

    }
}
