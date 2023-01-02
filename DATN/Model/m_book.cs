using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Model
{
    
    public class m_book
    {
        [Key]
        public int book_id { get; set; }
        public int genre_id { get; set; }
        public int supplier_id { get; set; }
        public string book_name { get; set; }
        public string author { get; set; }
        public decimal? price { get; set; }
        [Required(ErrorMessage =" Vui lòng nhập nội dung sách")]
        public string book_content { get; set; }     
        public int? amount { get; set; }
        public byte[] book_image { get; set; }
        [Required(ErrorMessage = " Vui lòng chọn trạng thái sách")]
        public string status { get; set; }
        public int number_click { get; set; }
        public int page_number { get; set; }
        public DateTime update_at { get; set; }
        public DateTime release_date { get; set; }
    }
}
