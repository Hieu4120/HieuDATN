using ClosedXML.Excel;
using DATN.Model;

namespace DATN.Services
{
    public class ExcelProcessSevices
    {
        private XLWorkbook wb;
        private int START_ROW = 0;
        private int START_COL = 0;
        public ExcelProcessSevices()
        {
            //If you don't need this feature then you can turn it off to save memory and
            //increase performance. Just open your workbook with the option XLEventTracking.Disabled.
            wb = new XLWorkbook(XLEventTracking.Disabled);
        }

        // 業務経歴登録/更新の出力
        public async Task<string> ExportExcelImportorder(
            List<m_import_order> import_Orders,
           ISupplierServices sups
        )
        {         
            var ws = wb.Worksheets.Add("ExportData");

            string contentBase64;

            Dictionary<string, string> titles_target_key = new Dictionary<string, string> {
                {"import_order_id", "Mã đơn hàng"},
                {"supplier_id","Nhà cung cấp"},
                {"price",  "Giá"},
                {"create_at","Ngày tạo"},
                {"receive_at",  "Ngày nhận"},
            };
            
            List<string> import_order_key = new List<string>
            {
                "import_order_id",
                "supplier_id",
                "price",
                "create_at",
                "receive_at",
            };
         
            var RowData = (import_Orders != null) ? import_Orders : null;
            int cols = import_order_key.Count;
            int rows = 0;
            if (RowData != null)
            {
                rows = RowData.Count + 1;
            }

            int start_row = START_ROW + 1;
            int start_col = START_COL + 1;
       
            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
                {
  
                    if (r == 0)
                    {
                        ws.Cell(r + start_row, c + start_col).Value = titles_target_key[import_order_key[c]];
                        continue;
                    }
                    
                    var propertyInfo = RowData[r - 1].GetType().GetProperty(import_order_key[c]);
                    var value = propertyInfo.GetValue(RowData[r - 1], null);
                    if (value == null)
                    {
                        value = "";
                    }
                    if (import_order_key[c] == "number_row")
                    {
                        value = r + 1;
                    }

                    if (import_order_key[c] == "supplier_id")
                    {
                        var Supp = await sups.GetSuppById((int)value);
                        value = (Supp != null) ? Supp.supplier_name : "";
                    }
                    ws.Cell(r + start_row, c + start_col).Value = value;
                }
            }
            var firstCell = ws.Cell(start_row, start_col);
            var lastCell = ws.Cell(start_row + rows - 1, start_col + cols - 1);
           
            ws.Range(firstCell, lastCell).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            ws.Range(firstCell, lastCell).Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            ws.Range(firstCell, lastCell).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(firstCell, lastCell).Style.Border.LeftBorder = XLBorderStyleValues.Thin;
            ws.Range(firstCell, lastCell).Style.Border.RightBorder = XLBorderStyleValues.Thin;
            ws.Range(firstCell, lastCell).Style.Border.TopBorder = XLBorderStyleValues.Thin;

            ws.Columns().AdjustToContents();
            using (var stream = new MemoryStream())
            {
                wb.SaveAs(stream);
                contentBase64 = Convert.ToBase64String(stream.ToArray());
                //var fileName = "Countries.xlsx";
                //await JSRuntime.InvokeAsync<bool>("saveAsFile", "Countries.xlsx", Convert.ToBase64String(content));
            }
            //wb.SaveAs("InsertingData.xlsx");
            return contentBase64;
        }
    }
}
