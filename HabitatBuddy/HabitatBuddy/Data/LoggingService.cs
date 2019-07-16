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

public class LoggingService
    {
        HttpClient client;

        public List<Log> logs { get; private set; }

        public LoggingService()
        {
            var authData = string.Format("{0}:{1}", TodoREST.Constants.Username, TodoREST.Constants.Password);
            var authHeaderValue = Convert.ToBase64String(Encoding.UTF8.GetBytes(authData));

            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);
        }

        public async Task<List<Log>> RefreshDataAsync()
        {
            logs = new List<Log>();
            var uri = new Uri(string.Format(TodoREST.Constants.LogInfo, string.Empty));

            try
            {
                var response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    logs = JsonConvert.DeserializeObject<List<Log>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
            return logs;
        }

        public async Task SaveTodoItemAsync(Log item, bool isNewItem = true)
        {

            var uri = new Uri(string.Format(TodoREST.Constants.LogInfo, string.Empty));

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
                    Debug.WriteLine(@"PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP Log successfully saved.");
                }
                else
                {
                    Debug.WriteLine(@"  -------------------------------------------------- Log Error " + response);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"  -------------------------------------------------- Reg Error{0}", ex.Message);
            }
        }

        public async Task DeleteTodoItemAsync(string id)
        {

            var uri = new Uri(string.Format(TodoREST.Constants.LogInfo, id));

            try
            {
                var response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"             Log item successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"             ERROR {0}", ex.Message);
            }
        }

  

}



