using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Scoreboard : ContentPage
    {
        Services.Services _service = new Services.Services();
        public Scoreboard()
        {
            InitializeComponent();

            GetAll();
        }
        private async void GetAll()
        {
            var token = Application.Current.Properties["MyToken"].ToString();

            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetStringAsync("http://dearjean.ddns.net:44301/api/Rewards/GetAll");

            var tablicaW = JsonConvert.DeserializeObject<List<RewardsGetALL>>(response);

            scoreBoard.ItemsSource = tablicaW;


        } 
    }
}