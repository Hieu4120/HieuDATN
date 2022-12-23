using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_sale_order
    {
        [Key]
        public int sale_order_id { get; set; }
        public decimal price { get; set; }
        public DateTime purchase_date { get; set; }
        public DateTime delivery_date { get; set; }
        public string status { get; set; }
    }
}
