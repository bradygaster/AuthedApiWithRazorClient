using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;

namespace MyUX.Pages
{
    [AuthorizeForScopes(Scopes = new string[] {
                "api://795c6798-129c-4dc5-815a-e0d7727b9993/access_as_logged_in_user"})]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ITokenAcquisition tokenAcquisition;
        private HttpClient HttpClient;

        public string Result { get; set; }

        public IndexModel(ILogger<IndexModel> logger, 
            IHttpClientFactory httpClientFactory,
            ITokenAcquisition tokenAcquisition)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.tokenAcquisition = tokenAcquisition;

            this.HttpClient = this.httpClientFactory.CreateClient();
        }
        
        public async Task OnGet()
        {
            var accessToken = await this.tokenAcquisition.GetAccessTokenForUserAsync(new string[] {
                "api://795c6798-129c-4dc5-815a-e0d7727b9993/access_as_logged_in_user"
            });

            this.HttpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", accessToken);

            var json = await this.HttpClient.GetStringAsync("https://localhost:5001/weatherforecast");

            Result = json;
        }
    }
}
