using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_import_order_detail
    {
        [Key]
        public int import_order_detail_id { get; set; }
        public int import_order_id { get; set; }
        public int book_id { get; set; }
        public int genre_id { get; set; }
        public int supplier_id { get; set; }
        public byte[] book_image { get; set; }
        public string book_name { get; set; }
        public string author { get; set; }
        public decimal? price { get; set; }
        public int? amount { get; set; }
        public int? page_number { get; set; }
        public DateTime update_at { get; set; }
        public DateTime create_at { get; set; }
    }
}
