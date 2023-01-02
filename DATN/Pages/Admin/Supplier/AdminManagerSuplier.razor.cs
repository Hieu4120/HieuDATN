using DATN.Model;
using DATN.Services;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Components;

namespace DATN.Pages.Admin.Supplier
{
    public partial class AdminManagerSuplier
    {
        [Inject]
        private ISupplierServices sups{ get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        public IEnumerable<m_supplier> supplis = Enumerable.Empty<m_supplier>();
        [Parameter]
        public int page { get; set; }

        private string CurrentUri = "manager-suplier";
        PagingInfo pagingInfo = new PagingInfo();
        private IEnumerable<m_supplier> supplis_i;
        private int ROW_INDEX = 1;
        /*private string status_sup = "";
        private string supId;*/

        protected override async Task OnInitializedAsync()
        {
            //supplis = await sups.GetAllSupp();
            supplis_i = await sups.GetAllSupp();
            
        }

        private void btn_pass_data(m_supplier ele)
        {
            string sup_id = ele.supplier_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"supplier_id", sup_id},
            };
            iredir.RedirectParameter("update-suplier", passData);
        }

        protected override void OnParametersSet()
        {
            CreatePagingInfo();
        }

        public async void CreatePagingInfo()
        {
            int PageSize = 3;
            pagingInfo = new PagingInfo();
            page = page == 0 ? 1 : page;
            pagingInfo.CurrentPage = page;
            pagingInfo.TotalItems = supplis_i.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            supplis = supplis_i.Skip(skip).Take(PageSize).ToList();
        }
    }
}
