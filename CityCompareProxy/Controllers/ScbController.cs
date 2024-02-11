using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static YourNamespace.Controllers.HousePriceController;


namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HousePriceController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string baseUrl = "https://api.scb.se"; // API endpoint

        public HousePriceController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("{city}")]
        public async Task<IActionResult> GetHousePrice(string city)
        {
            try
            {
                string url = "/OV0104/v1/doris/sv/ssd/START/BO/BO0501/BO0501B/FastprisSHRegionAr";
                //var cacheKey = $"houseprice-{city.ToLower()}";
                //var data = GetFromCache(cacheKey);

                //if (data == null)
                //{
                Console.WriteLine("API ANROP");

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

                var response = await _httpClient.PostAsync(baseUrl+url, requestContent);
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

        public class HousePriceResponse
        {
            public List<Column> Columns { get; set; }
            public List<DataItem> Data { get; set; }
            public List<MetadataItem> Metadata { get; set; }
        }

        public class Column
        {
            public string Code { get; set; }
            public string Text { get; set; }
            public string Type { get; set; }
        }

        public class DataItem
        {
            public List<string> Key { get; set; }
            public List<string> Values { get; set; }
        }

        public class MetadataItem
        {
            public string Infofile { get; set; }
            public DateTime Updated { get; set; }
            public string Label { get; set; }
            public string Source { get; set; }
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
