using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Visitare.Services
{
    class Services
    {
        public async Task<string> LoginAsync(string userName, string password)
        {

            string URL = "http://dearjean.ddns.net:44301/Token";
            var accessToken = string.Empty;
            await Task.Run( () =>
            {
                try
                {
                    var keyValues = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("Username", userName),
                        new KeyValuePair<string, string>("Password", password),
                        new KeyValuePair<string, string>("grant_type", "password")
                    };
                    var request = new HttpRequestMessage(
                        HttpMethod.Post, URL);

                    request.Content = new FormUrlEncodedContent(keyValues);

                    var client = new HttpClient();
                    client.Timeout = TimeSpan.FromSeconds(7);
                    var response = client.SendAsync(request).Result;

                    using (HttpContent content = response.Content)
                    {
                        var json = content.ReadAsStringAsync();
                        JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(json.Result);
                        var accessTokenExpiration = jwtDynamic.Value<DateTime>(".expires");
                        accessToken = jwtDynamic.Value<string>("access_token");

                        var Username = jwtDynamic.Value<string>("userName");
                        var AccessTokenExpirationDate = accessTokenExpiration;
                        Application.Current.Properties["MyToken"] = $"{accessToken}";
                    }
                }
                catch (Exception ex)
                {
                    accessToken = "1";
                }
            });
            return accessToken;
            

        }
    }
}
