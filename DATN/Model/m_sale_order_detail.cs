using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_sale_order_detail
    {
        [Key]
        public int sale_order_detail_id { get; set; }
        public int sale_order_id { get; set; }
        public string book_name { get; set; }
        public int? amount { get; set; }
        public int? phone_number { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string address { get; set; }
        public decimal? price { get; set; }
        public DateTime create_at { get; set; }
    }
}
