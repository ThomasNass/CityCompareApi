using CityCompareProxy.Models;
using CityCompareProxy.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace CityCompareProxy.Services
{
    public class ScbService : IScbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.scb.se"; // API endpoint
        private readonly IScbRepository _scbRepository;

        public ScbService(IHttpClientFactory httpClientFactory, IScbRepository scbRepository)
        {
            _httpClient = httpClientFactory.CreateClient();
            _scbRepository = scbRepository;
        }
        public async Task<ScbResponse?> GetHousePrice(string cityId)
        {
            string url = "/OV0104/v1/doris/sv/ssd/START/BO/BO0501/BO0501B/FastprisSHRegionAr";

            var requestBody = new
            {
                query = new[]
                {
                       new { code = "Region", selection = new { filter = "vs:RegionKommun07EjAggr", values = new[] { cityId } } },
                       new { code = "Fastighetstyp", selection = new { filter = "item", values = new[] { "220" } } },
                       new { code = "ContentsCode", selection = new { filter = "item", values = new[] { "BO0501C2" } } },
                       new { code = "Tid", selection = new { filter = "item", values = new[] { "2022" } } }//Skippar jag tid här så får jag alla år. Då kan jag göra ett linjediagram istället
                },
                response = new { format = "json" }
            };

            var result = await PostScb(url, requestBody);
            await _scbRepository.StoreResponse(result);
            return result;
        }



        public async Task<ScbResponse?> GetIncomeData(string cityId)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/HE/HE0110/HE0110A/SamForvInk2";

            var requestBody = new
            {
                query = new[]
                {
                    new { code = "Region", selection = new { filter = "vs:RegionKommun07EjAggr", values = new[] { cityId } } },
                    new { code = "Alder", selection = new { filter = "item", values = new[] { "20-64" } } },
                    new { code = "Inkomstklass", selection = new { filter = "item", values = new[] { "TOT" } } },
                    new { code = "ContentsCode", selection = new { filter = "item", values = new[] { "HE0110K1", "HE0110K2" } } },
                    new { code = "Tid", selection = new { filter = "item", values = new[] { "2022" } } }
                },
                response = new { format = "json" }
            };

            var result = await PostScb(url, requestBody);
            await _scbRepository.StoreResponse(result);
            return result;

        }

        public async Task<ScbResponse?> GetGrowthData(string cityId)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101A/BefolkningNy";

            var requestBody = new
            {
                query = new[]
                {
                       new { code = "Region", selection = new { filter = "vs:RegionKommun07", values = new[] { cityId } } },
                       new { code = "ContentsCode", selection = new  { filter = "item", values = new[] { "BE0101N1" } } }
                },
                response = new { format = "json" }
            };

            var result = await PostScb(url, requestBody);
            await _scbRepository.StoreResponse(result);
            return result;
        }

        public async Task<ScbResponse?> GetPopulationData(string cityId)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101A/BefolkningNy";

            var requestBody = new
            {
                query = new[]
                {
                    new { code = "Region", selection = new{ filter ="vs:RegionKommun07", values = new[] { cityId } } },
                    new { code = "ContentsCode", selection = new{ filter = "item", values = new[] { "BE0101N1" } } },
                    new { code = "Kon", selection = new{ filter = "item", values = new[] { "1","2" } } },
                    new { code = "Tid", selection = new{ filter = "item", values = new[] { "2022" } } },
                },
                response = new { format = "json" }
            };
            var result = await PostScb(url, requestBody);
            await _scbRepository.StoreResponse(result);
            return result;
        }

        public async Task<ScbResponse?> GetElectionData(string cityId)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/ME/ME0104/ME0104C/ME0104T3";

            var requestBody = new
            {
                query = new[]
                {   new{ code = "Region", selection = new{ filter ="vs:RegionKommun07+BaraEjAggr", values = new[] { cityId } } },
                    new {code = "ContentsCode", selection = new{ filter = "item", values = new[] { "ME0104B7" } } },
                    new {code = "Partimm", selection = new{ filter = "item", values = new[] { "V","MP","S","C","FP","KD","M","SD","ÖVRIGA" } } },
                    new {code = "Tid", selection = new{ filter = "item", values = new[] { "2022" } } },
                },
                response = new { format = "json" }
            };
            var result = await PostScb(url, requestBody);
            await _scbRepository.StoreResponse(result);
            return result;
        }

        public async Task<ScbResponse?> GetMunicipalityElectionData(string cityId)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/ME/ME0104/ME0104A/ME0104T1";

            var requestBody = new
            {
                query = new[]
                {   new{ code = "Region", selection = new{ filter ="vs:RegionKommun07+BaraEjAggr", values = new[] { cityId } } },
                    new {code = "ContentsCode", selection = new{ filter = "item", values = new[] { "ME0104B2" } } },
                    new {code = "Partimm", selection = new{ filter = "item", values = new[] { "V","MP","S","C","FP","KD","M","SD","ÖVRIGA" } } },
                    new {code = "Tid", selection = new{ filter = "item", values = new[] { "2022" } } },
                },
                response = new { format = "json" }
            };

            var result = await PostScb(url, requestBody);
            await _scbRepository.StoreResponse(result);
            return result;
        }

        private async Task<ScbResponse?> PostScb(string url, object requestBody)
        {
            var requestBodyJson = JsonConvert.SerializeObject(requestBody);
            var requestContent = new StringContent(requestBodyJson, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_baseUrl + url, requestContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ScbResponse>(responseContent);

            return data;
        }
    }

}
