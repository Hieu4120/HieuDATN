using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_genre
    {
        [Key]
        public int genre_id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên thể loại") ]
        public string genre_name { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public string status { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
    }
}
