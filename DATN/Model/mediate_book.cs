namespace DATN.Model
{
    public class mediate_book
    {
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string img_url { get; set; }
        public decimal? price { get; set; }
        public byte[] book_image { get; set; }
        public int number_click { get; set; }
        public int genre_id { get; set; }
    }
}
