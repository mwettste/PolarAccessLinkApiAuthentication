using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PolarOAuthAuthentication.Controllers
{
    public class OauthCallbackController : Controller 
    {
        private readonly IHttpClientFactory httpClientFactory;
        // client id & secret which you received when registering your application with Polar Access Link
        private readonly string clientId = "YOUR_POLAR_ACCESSLINK_CLIENT_ID";
        private readonly string clientSecret = "YOUR_POLAR_ACCESSLINK_CLIENT_SECRET";
        private readonly string requestUri = "https://polarremote.com/v2/oauth2/token"; // URL to request the bearer token from

        public OauthCallbackController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }


        [Route("start_authentication")]
        public IActionResult StartAuthentication()
        {
            var state = Guid.NewGuid().ToString();   // not strictly necessary in this context, since we only manually authenticate for one user
            var polarScope = "accesslink.read_all";
            var redirectUrl = $"https://flow.polar.com/oauth2/authorization?response_type=code&scope={polarScope}&client_id={clientId}&state={state}";
            return Redirect(redirectUrl);
        }

        [Route("oauth2_callback")]
        public async Task<IActionResult> Callback(string? state, string? code, string? error)
        {
            var base64EncodedAuthCode = System.Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            var httpClient = httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthCode);
            
            var requestContent = new StringContent($"grant_type=authorization_code&code={code}", encoding: Encoding.UTF8, "application/json");
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = await httpClient.PostAsync(requestUri, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return Ok(responseContent);
            }
            else
            {
                return Ok("Failed to authorize: " + error);
            }
        }
    }
}
