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

        private IEnumerable<m_genre> genres { get; set; }
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
    }
}
