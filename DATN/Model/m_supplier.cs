using System.ComponentModel.DataAnnotations;

namespace DATN.Model
{
    public class m_supplier
    {
        [Key]
        public int supplier_id { get; set; }
        [Required(ErrorMessage ="Vui lòng nhập tên NCC")]
        public string supplier_name { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số ĐT NCC")]
        public string supplier_phone_number { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập email NCC")]
        public string supplier_email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ NCC")]
        public string supplier_address { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public string supplier_status { get; set; }
        public DateTime create_at { get; set; }
        public DateTime update_at { get; set; }
    }
}
