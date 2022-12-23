


namespace DATN.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class m_book
    {
        [Key]
        public int book_id { get; set; }
  
        public int genre_id { get; set; }

        public int supplier_id { get; set; }
        [Required]
        public string book_name { get; set; }
        [Required]
        public string author { get; set; }
        [Required]
        public decimal? price { get; set; }
        [Required]
        public string book_content { get; set; }
        [Required]
        public int? amount { get; set; }
        [Required]
        public byte[] book_image { get; set; }
        public string status { get; set; }
        public int number_click { get; set; }
        public int page_number { get; set; }
        public DateTime update_at { get; set; }
        public DateTime release_date { get; set; }
    }
}
