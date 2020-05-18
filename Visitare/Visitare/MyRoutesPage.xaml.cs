using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Visitare.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyRoutesPage : ContentPage
    {
        public List<Routes> trasy;
        public MyRoutesPage()
        {
            InitializeComponent();
            GetRoutes();
        }

        private async void GetRoutes()
        {
            try
            {
                var token = Application.Current.Properties["MyToken"].ToString();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetStringAsync("http://dearjean.ddns.net:44301/api/Routes/GetMine");
                trasy = JsonConvert.DeserializeObject<List<Routes>>(response);
                myroutesList.ItemsSource = trasy;
            }
            catch(Exception xde)
            {
                 await DisplayAlert("Uwaga!", "Nie masz odpowiednich uprawnień na kreatora! Skontaktuj się z administratorem", "Anuluj");
            }
        }
        private async void MethodSearchRoutes(object sender, TextChangedEventArgs e)
        {
            try
            {
                var token = Application.Current.Properties["MyToken"].ToString();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetStringAsync("http://dearjean.ddns.net:44301/api/Routes/GetMine");
                trasy = JsonConvert.DeserializeObject<List<Routes>>(response);
                myroutesList.ItemsSource = trasy;

                var keyword = SearchRoutes.Text;
                var result = trasy.Where(x => x.Name.ToLower().Contains(keyword.ToLower()));
                myroutesList.ItemsSource = result;
            }
            catch(Exception)
            {
                await DisplayAlert("Uwaga!", "Nie masz odpowiednich uprawnień na kreatora! Skontaktuj się z administratorem", "Anuluj");
            }
        }
        private async void OnTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var tapped = (Routes)e.Item;
                var token = Application.Current.Properties["MyToken"].ToString();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetStringAsync("http://dearjean.ddns.net:44301/api/points3");
                List<Points> punkty = JsonConvert.DeserializeObject<List<Points>>(response);
                List<Points> list = new List<Points>();
                foreach (Points tmp in punkty)
                {
                    if (tmp.RouteId == tapped.Id)
                    {
                        list.Add(tmp);
                    }
                }
                await Navigation.PushAsync(new MainPage(new RoutePoints(list)));
            }
            catch(Exception xd)
            {
                await DisplayAlert("Uwaga!", "Nie masz odpowiednich uprawnień na kreatora! Skontaktuj się z administratorem", "Anuluj");
            }
        }

        private void OnXClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var route = button?.BindingContext as Routes;
            var vm = BindingContext as MyRoutesViewModel;
            vm.RoutesList = trasy;
            vm?.RemoveCommand.Execute(route);
            
        }
    }
}