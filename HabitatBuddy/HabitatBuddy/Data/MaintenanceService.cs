using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HabitatBuddy.Models;
using Newtonsoft.Json;

namespace TodoREST
{
    public class MaintenanceService : IMaintenance
    {
        HttpClient client;

        public List<Maintenance> Items { get; private set; }

        public MaintenanceService()
        {
            var authData = string.Format("{0}:{1}", Constants.Username, Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task<List<Maintenance>> RefreshDataAsync()
        {
            Items = new List<Maintenance>();

            // RestUrl = http://developer.xamarin.com:8081/api/todoitems
            var uri = new Uri(string.Format(Constants.mUrl, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<Maintenance>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }

            return Items;
        }


        

       
    }
}
