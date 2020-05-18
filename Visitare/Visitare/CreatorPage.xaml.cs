using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Visitare.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Visitare
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreatorPage : ContentPage
    {
        public List<Points> routePoints = new List<Points>();

        public CreatorPage()
        {
            InitializeComponent();
            customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(53.010281, 18.604922), Distance.FromMiles(1.0)));
        }
        private void OnClearClicked(object sender, EventArgs e)
        {
            customMap.Pins.Clear();
            customMap.MapElements.Clear();
            routePoints.Clear();
        }
        private async void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(nazwaEntry.Text))
            {
                await DisplayAlert("Błąd", "Podaj nazwę punktu", "Ok");
                return;
            }

            CustomPin pin = new CustomPin
            {
                Type = PinType.SavedPin,
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
                Label = nazwaEntry.Text,
                Address = opisEntry.Text,
                Name = "Xamarin",
                Url = "http://xamarin.com/about/",
              
            };

            pin.MarkerClicked += async (s, args) =>
            {
                args.HideInfoWindow = true;
                string pinName = ((CustomPin)s).Label;
                // string pytanie = ((CustomPin)s).Question;
                string opis = ((CustomPin)s).Address;
                await DisplayAlert($"{pinName}, {e.Position.Latitude}, {e.Position.Longitude}", $"{opis}", "Anuluj");
                // string odpowiedz = ((CustomPin)s).Answer;
                /* if(await DisplayAlert($"{pinName}", $"{opis}","Quiz", "Anuluj"))
                 {
                     await Navigation.PushAsync(new QuestionPage(new Question()));
                 }*/


            };
            customMap.CustomPins = new List<CustomPin> { pin };
            customMap.Pins.Add(pin);
            routePoints.Add(new Points()
            {
                X = e.Position.Latitude,
                Y = e.Position.Longitude,
                RouteId = 1,
                Name = nazwaEntry.Text,
                Description = opisEntry.Text
            });
            if(customMap.Pins.Count > 10 && routePoints.Count > 10)
            {
                await DisplayAlert("Uwaga!", "Można do trasy dodać maksymalnie 10 punktów", "Ok");
                customMap.Pins.Remove(pin);
            }
        }
        private async void OnNewRouteClicked(object sender, EventArgs e)
        {
            if (routePoints.Count() == 0)
            {
                await DisplayAlert("Błąd", "Dodaj punkt na mapę", "Ok");
                return;
            }
            if (String.IsNullOrWhiteSpace(trasaEntry.Text))
            {
                await DisplayAlert("Błąd", "Podaj nazwę trasy", "Ok");
                return;
            }
            if(routePoints.Count() > 11) // dodatkowe zabezpieczenie na przycisk, nie moze byc > 10 bo wyswietla niepotrzebnie błąd
            {
                await DisplayAlert("Błąd", "Można do trasy dodać maksymalnie 10 punktów", "Ok");
                return;
            }

            try
            {
                List<int> idList = new List<int>();
                foreach (Points tmp in routePoints)
                {
                    var token1 = Application.Current.Properties["MyToken"].ToString();
                    var json = JsonConvert.SerializeObject(tmp);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token1);
                    var result = await client.PostAsync("http://dearjean.ddns.net:44301/api/Points3", content);
                    var pointResult = JsonConvert.DeserializeObject<Points>(result.Content.ReadAsStringAsync().Result);
                    if (result.StatusCode != HttpStatusCode.Created)
                    {
                        await DisplayAlert("Błąd", "Spróbuj ponownie później", "Ok");
                        return;
                    }
                    idList.Add(pointResult.Id);
                }
                var model = new ChangeModel
                {
                    UpdateList = idList,
                    Name = trasaEntry.Text,
                    Description = opistrasyEntry.Text
                };
                var json2 = JsonConvert.SerializeObject(model);
                var content2 = new StringContent(json2, Encoding.UTF8, "application/json");
                var token = Application.Current.Properties["MyToken"].ToString();
                HttpClient client2 = new HttpClient();
                client2.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var result2 = await client2.PostAsync("http://dearjean.ddns.net:44301/api/Points3/Change", content2);
                if (result2.StatusCode == HttpStatusCode.OK)
                    await DisplayAlert("Sukces", "Dodanie trasy przebiegło pomyślnie", "Ok");
                else
                    await DisplayAlert("Błąd", "Nie masz odpowiednich uprawnień na kreatora! Skontaktuj się z administratorem", "Anuluj");
            }
            catch(Exception xd)
            {

                await DisplayAlert("Uwaga!", "Nie masz odpowiednich uprawnień na kreatora! Skontaktuj się z administratorem", "Anuluj");
            }
       }
        private async void OnMyRoutesClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new MyRoutesPage());
            }
            catch(Exception xde)
            {
                await DisplayAlert("Uwaga!", "Nie masz odpowiednich uprawnień na kreatora! Skontaktuj się z administratorem", "Anuluj");
            }
        }
        private async void QuizForRoutes(object sender, EventArgs e)
        {

            await Navigation.PushAsync(new QuestionPage(new Question()));
        }
    }
}