using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace fbsandboxtests.Utils
{
    public class REST
    {
        public static HttpResponseMessage POST(string url, string email, UserDetails usersDetails)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var count = 0;
            do
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + email);
                        response = client.PostAsJsonAsync(url, usersDetails).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            return response;
                        }
                    }
                }
                catch (Exception)
                {

                }

                count++;
                Thread.Sleep(10);
            } while (count < 500);
            return response;
        }

        public static HttpResponseMessage PATCH(string url, string email, UserDetails userDetails)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            dynamic userDetailsDynamic = new ExpandoObject();
            userDetailsDynamic.name = userDetails.Name;
            userDetailsDynamic.balance = userDetails.Balance;

            var content = new StringContent(JsonConvert.SerializeObject(userDetailsDynamic),
                    Encoding.UTF8,
                   "application/json");
            var count = 0;
            do
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + email);
                        response = client.PatchAsync(url, content).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            return response;
                        }
                    }
                }
                catch (Exception)
                {

                }

                count++;
                Thread.Sleep(10);
            } while (count < 50);
            return response;
        }


        public static HttpResponseMessage GET(string url)
        {
            var response = new HttpResponseMessage();
            try
            {
                using (var client = new HttpClient())
                {
                    response = client.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return response;
        }
    }
}

