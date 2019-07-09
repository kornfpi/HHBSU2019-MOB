using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using HabitatBuddy;
using HabitatBuddy.Models;

public class HomeRegInfoService
    {
        HttpClient client;

        public List<HomeRegInfo> registrations { get; private set; }

        public HomeRegInfoService()
        {
            var authData = string.Format("{0}:{1}", TodoREST.Constants.Username, TodoREST.Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task<List<HomeRegInfo>> RefreshDataAsync()
        {
            registrations = new List<HomeRegInfo>();
            var uri = new Uri(string.Format(TodoREST.Constants.RegInfo, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    registrations = JsonConvert.DeserializeObject<List<HomeRegInfo>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return registrations;
        }

        public async Task SaveTodoItemAsync(HomeRegInfo item, bool isNewItem = true)
        {

            var uri = new Uri(string.Format(TodoREST.Constants.RegInfo, string.Empty));

            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (isNewItem)
                {
                    response = await client.PostAsync(uri, content);
                }
                else
                {
                    response = await client.PutAsync(uri, content);
                }
            Console.WriteLine("pppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppppp  " + content.Headers);
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP Reg successfully saved.");
                }
                else
                {
                    Debug.WriteLine(@"  -------------------------------------------------- Reg Error " + response);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"  -------------------------------------------------- Reg Error{0}", ex.Message);
            }
        }

        public async Task DeleteTodoItemAsync(string id)
        {

            var uri = new Uri(string.Format(TodoREST.Constants.RegInfo, id));

            try
            {
                var response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"             TodoItem successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
        }

  

}



