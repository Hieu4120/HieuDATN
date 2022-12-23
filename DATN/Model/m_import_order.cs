using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_import_order
    {
        [Key]
        public int import_order_id { get; set; }
        public int supplier_id { get; set; }
        public decimal? price { get; set; }
        public DateTime create_at { get; set; }
        public DateTime receive_at { get; set; }

    }
}
