namespace DATN.Model
{
    public class mediate_import_order_detail
    {
        public int import_order_id { get; set; }
        public int? book_id { get; set; }
        public byte[] book_image { get; set; }
        public string book_name { get; set; }
        public string author { get; set; }
        public decimal? price { get; set; }
        public int? amount { get; set; }
        public int? page_number { get; set; }
        public string genre_name { get; set; }
        public string supplier_name { get; set; }
    }
}
