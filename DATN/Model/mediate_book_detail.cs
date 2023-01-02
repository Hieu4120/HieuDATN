namespace DATN.Model
{
    public class mediate_book_detail
    {
        public int book_id { get; set; }
        public string book_name { get; set; }
        public decimal? price { get; set; }
        public string author { get; set; }
        public string book_content { get; set; }
        public byte[] book_image { get; set; }
        public int page_number { get; set; }
        public string supplier_name { get; set; }
        public string genre_name { get; set; }
        public string status { get; set; }
        public int number_click { get; set; }
        public DateTime release_date { get; set; }
        public int genre_id { get; set; }
        public int? amount { get; set; }
    }
}
