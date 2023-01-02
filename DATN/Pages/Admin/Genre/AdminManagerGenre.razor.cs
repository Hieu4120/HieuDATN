using DATN.Model;
using DATN.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Reflection.Metadata;

namespace DATN.Pages.Admin.Genre
{
    public partial class AdminManagerGenre
    {
        [Inject]
        private IGenreServices ges { get; set; }
        [Inject]
        private IRedirectSevices? iredir { get; set; }
        [Parameter]
        public int page { get; set; }
        PagingInfo pagingInfo = new PagingInfo();
        private string CurrentUri = "manager-genre";
        private IEnumerable<m_genre> genres;
        private IEnumerable<m_genre> genres_p = Enumerable.Empty<m_genre>();
        private int ROW_INDEX = 1;
        private string status_gen = "";
        private string genId;
        private bool isLoading;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            genres = await ges.GetAllGenre();
            isLoading = false;
        }

        private void btn_pass_data(m_genre ele)
        {
            string gen_id = ele.genre_id.ToString();
            Dictionary<string, string> passData = new Dictionary<string, string>
            {
                {"genre_id", gen_id},
            };
            iredir.RedirectParameter("update-genre", passData);
        }

        protected override void OnParametersSet()
        {
            CreatePagingInfo();
        }
        public async void CreatePagingInfo()
        {
            int PageSize = 4;
            pagingInfo = new PagingInfo();
            page = page == 0 ? 1 : page;
            pagingInfo.CurrentPage = page;
            pagingInfo.TotalItems = genres.Count();
            pagingInfo.ItemsPerPage = PageSize;

            var skip = PageSize * (Convert.ToInt32(page) - 1);
            genres_p = genres.Skip(skip).Take(PageSize).ToList();
        }
    }
}
