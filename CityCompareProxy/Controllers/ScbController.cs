using System;
using System.Net.Http;
using System.Threading.Tasks;
using CityCompareProxy.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static YourNamespace.Controllers.ScbController;


namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScbController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.scb.se"; // API endpoint


        public ScbController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("houseprices/{city}")]
        public async Task<IActionResult> GetHousePrice(string city)
        {
            try
            {
                string url = "/OV0104/v1/doris/sv/ssd/START/BO/BO0501/BO0501B/FastprisSHRegionAr";
                //var cacheKey = $"houseprice-{city.ToLower()}";
                //var data = GetFromCache(cacheKey);

                //if (data == null)
                //{

                var requestBody = new
                {
                    query = new[]
                    {
                            new { code = "Region", selection = new { filter = "vs:RegionKommun07EjAggr", values = new[] { city } } },
                            new { code = "Fastighetstyp", selection = new { filter = "item", values = new[] { "220" } } },
                            new { code = "ContentsCode", selection = new { filter = "item", values = new[] { "BO0501C2" } } },
                            new { code = "Tid", selection = new { filter = "item", values = new[] { "2022" } } }//Skippar jag tid här så får jag alla år. Då kan jag göra ett linjediagram istället
                        },
                    response = new { format = "json" }
                };

                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl + url, requestContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<HousePriceResponse>(responseContent);

                //SaveToCache(cacheKey, data);
                // }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("income/{city}")]
        public async Task<IActionResult> GetIncomeData(string city)
        {
            try
            {
                var incomeUrl = "/OV0104/v1/doris/sv/ssd/START/HE/HE0110/HE0110A/SamForvInk2";
                //string cacheKey = $"income-{city.ToLower()}";
                //var data = GetFromCache(cacheKey); // Uncomment this if you have caching logic

                //if (data == null) // Uncomment this if you have caching logic
                //{

                var requestBody = new
                {
                    query = new[]
                    {
                            new { code = "Region", selection = new { filter = "vs:RegionKommun07EjAggr", values = new[] { city } } },
                            new { code = "Alder", selection = new { filter = "item", values = new[] { "20-64" } } },
                            new { code = "Inkomstklass", selection = new { filter = "item", values = new[] { "TOT" } } },
                            new { code = "ContentsCode", selection = new { filter = "item", values = new[] { "HE0110K1", "HE0110K2" } } },
                            new { code = "Tid", selection = new { filter = "item", values = new[] { "2020" } } }
                        },
                    response = new { format = "json" }
                };

                var requestBodyJson = JsonConvert.SerializeObject(requestBody);
                var requestContent = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_baseUrl + incomeUrl, requestContent);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var incomeData = JsonConvert.DeserializeObject<IncomeResponse>(responseContent);

                // SaveToCache(cacheKey, incomeData); // Uncomment this if you have caching logic
                return Ok(incomeData);
                //}
                //return Ok(data); // Uncomment this if you have caching logic
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //private object GetFromCache(string cacheKey)
        //{
        //    // Implement your cache retrieval logic here
        //    throw new NotImplementedException();
        //}

        //private void SaveToCache(string cacheKey, object data)
        //{
        //    // Implement your cache saving logic here
        //    throw new NotImplementedException();
        //}
    }
}
