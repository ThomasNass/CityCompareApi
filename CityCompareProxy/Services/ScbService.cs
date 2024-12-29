using CityCompareProxy.Helper;
using CityCompareProxy.Models;
using CityCompareProxy.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;

namespace CityCompareProxy.Services
{
    public class ScbService : IScbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.scb.se";
        private readonly IScbRepository _scbRepository;

        public ScbService(IHttpClientFactory httpClientFactory, IScbRepository scbRepository)
        {
            _httpClient = httpClientFactory.CreateClient();
            _scbRepository = scbRepository;
        }

        public async Task<City> GetCityAsync(string cityId)
        {
            return _scbRepository.GetCity(cityId);
        }
        public async Task<City?> GetHousePrice(string lauCode)
        {
            string url = "/OV0104/v1/doris/sv/ssd/START/BO/BO0501/BO0501B/FastprisSHRegionAr";

            var requestBody = new
            {
                query = new[]
                {
                       new { code = "Region", selection = new { filter = "vs:RegionKommun07EjAggr", values = new[] { lauCode } } },
                       new { code = "Fastighetstyp", selection = new { filter = "item", values = new[] { "220" } } },
                       new { code = "ContentsCode", selection = new { filter = "item", values = new[] { "BO0501C2" } } }
                },
                response = new { format = "json" }
            };


            var city = _scbRepository.GetCity(lauCode);
            if (city != null)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                if (city.HousePrices != null && city.Income.LastUpdateDate! > lastWeek)
                {
                    return city;
                }
            }
            var result = await PostScb(url, requestBody);
            HousePrices housePrices = Mapper.MapScbResponseToEntity(result, (response) =>
            {
                return new HousePrices
                {
                    Id = Guid.NewGuid(),
                    LastUpdateDate = DateTime.Now,
                    Items = new List<Data>()
                };
            });
            housePrices.CityId = city.Id;
            city.HousePrices = housePrices;
            _scbRepository.Add(housePrices);
            return city;

        }



        public async Task<City?> GetIncomeData(string lauCode)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/HE/HE0110/HE0110A/SamForvInk2";

            var requestBody = new
            {
                query = new[]
                {
                    new { code = "Region", selection = new { filter = "vs:RegionKommun07EjAggr", values = new[] { lauCode } } },
                    new {code = "Kon", selection = new {filter = "item", values = new[] {"1","2"}}},
                    new { code = "Alder", selection = new { filter = "item", values = new[] { "20-64" } } },
                    new { code = "Inkomstklass", selection = new { filter = "item", values = new[] { "TOT" } } },
                    new { code = "ContentsCode", selection = new { filter = "item", values = new[] { "HE0110K1", "HE0110K2" } } }
                },
                response = new { format = "json" }
            };


            var city = _scbRepository.GetCity(lauCode);
            if (city != null)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                if (city.Income != null && city.HousePrices.LastUpdateDate! > lastWeek)
                {
                    return city;
                }
            }
            var result = await PostScb(url, requestBody);
            Income income = Mapper.MapScbResponseToEntity(result, (response) =>
            {
                return new Income
                {
                    Id = Guid.NewGuid(),
                    LastUpdateDate = DateTime.Now,
                    Items = new List<Data>()
                };
            });
            income.CityId = city.Id;
            city.Income = income;
            _scbRepository.Add(income);
            return city;
        }

        public async Task<City?> GetGrowthData(string lauCode)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101A/BefolkningNy";

            var requestBody = new
            {
                query = new[]
                {
                       new { code = "Region", selection = new { filter = "vs:RegionKommun07", values = new[] { lauCode } } },
                       new { code = "ContentsCode", selection = new  { filter = "item", values = new[] { "BE0101N1" } } }
                },
                response = new { format = "json" }
            };
            var city = _scbRepository.GetCity(lauCode);
            if (city != null)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                if (city.PopulationGrowth != null && city.PopulationGrowth.LastUpdateDate! > lastWeek)
                {
                    return city;
                }
            }
            var result = await PostScb(url, requestBody);
            PopulationGrowth populationGrowth = Mapper.MapScbResponseToEntity(result, (response) =>
            {
                return new PopulationGrowth
                {
                    Id = Guid.NewGuid(),
                    LastUpdateDate = DateTime.Now,
                    Items = new List<Data>()
                };
            });
            populationGrowth.CityId = city.Id;
            city.PopulationGrowth = populationGrowth;

            _scbRepository.Add(populationGrowth);

            return city;
        }

        public async Task<City?> GetPopulationGenderData(string lauCode)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/BE/BE0101/BE0101A/BefolkningNy";

            var requestBody = new
            {
                query = new[]
                {
                    new { code = "Region", selection = new{ filter ="vs:RegionKommun07", values = new[] { lauCode } } },
                    new { code = "ContentsCode", selection = new{ filter = "item", values = new[] { "BE0101N1" } } },
                    new { code = "Kon", selection = new{ filter = "item", values = new[] { "1","2" } } },
                    new { code = "Tid", selection = new{ filter = "item", values = new[] { "2022" } } },
                },
                response = new { format = "json" }
            };

            var city = _scbRepository.GetCity(lauCode);
            if (city != null)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                if (city.PopulationGenderData != null && city.PopulationGenderData.LastUpdateDate! > lastWeek)
                {
                    return city;
                }
            }
            var result = await PostScb(url, requestBody);
            PopulationGenderData populationGenderData = Mapper.MapScbResponseToEntity(result, (response) =>
            {
                return new PopulationGenderData
                {
                    Id = Guid.NewGuid(),
                    LastUpdateDate = DateTime.Now,
                    Items = new List<Data>()
                };
            });
            populationGenderData.CityId = city.Id;
            city.PopulationGenderData = populationGenderData;

            _scbRepository.Add(populationGenderData);

            return city;
        }

        public async Task<City?> GetElectionData(string lauCode)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/ME/ME0104/ME0104C/ME0104T3";

            var requestBody = new
            {
                query = new[]
                {   new{ code = "Region", selection = new{ filter ="vs:RegionKommun07+BaraEjAggr", values = new[] { lauCode } } },
                    new {code = "ContentsCode", selection = new{ filter = "item", values = new[] { "ME0104B7" } } },
                    new {code = "Partimm", selection = new{ filter = "item", values = new[] { "V","MP","S","C","FP","KD","M","SD","ÖVRIGA" } } },
                    new {code = "Tid", selection = new{ filter = "item", values = new[] { "2022" } } },
                },
                response = new { format = "json" }
            };

            var city = _scbRepository.GetCity(lauCode);
            if (city != null)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                if (city.ElectionData != null && city.ElectionData.LastUpdateDate! > lastWeek)
                {
                    return city;
                }
            }
            var result = await PostScb(url, requestBody);
            ElectionData electionData = Mapper.MapScbResponseToEntity(result, (response) =>
            {
                return new ElectionData
                {
                    Id = Guid.NewGuid(),
                    LastUpdateDate = DateTime.Now,
                    Items = new List<Data>()
                };
            });
            electionData.CityId = city.Id;
            city.ElectionData = electionData;

            _scbRepository.Add(electionData);

            return city;
        }

        public async Task<City?> GetMunicipalityElectionData(string lauCode)
        {
            var url = "/OV0104/v1/doris/sv/ssd/START/ME/ME0104/ME0104A/ME0104T1";

            var requestBody = new
            {
                query = new[]
                {   new{ code = "Region", selection = new{ filter ="vs:RegionKommun07+BaraEjAggr", values = new[] { lauCode } } },
                    new {code = "ContentsCode", selection = new{ filter = "item", values = new[] { "ME0104B2" } } },
                    new {code = "Partimm", selection = new{ filter = "item", values = new[] { "V","MP","S","C","FP","KD","M","SD","ÖVRIGA" } } },
                    new {code = "Tid", selection = new{ filter = "item", values = new[] { "2022" } } },
                },
                response = new { format = "json" }
            };

            var city = _scbRepository.GetCity(lauCode);
            if (city != null)
            {
                var lastWeek = DateTime.Now.AddDays(-7);
                if (city.MunicipalityElectionData != null && city.MunicipalityElectionData.LastUpdateDate! > lastWeek)
                {
                    return city;
                }
            }
            var result = await PostScb(url, requestBody);
            MunicipalityElectionData municiplaityElectionData = Mapper.MapScbResponseToEntity(result, (response) =>
            {
                return new MunicipalityElectionData
                {
                    Id = Guid.NewGuid(),
                    LastUpdateDate = DateTime.Now,
                    Items = new List<Data>()
                };
            });
            municiplaityElectionData.CityId = city.Id;
            city.MunicipalityElectionData = municiplaityElectionData;

            _scbRepository.Add(municiplaityElectionData);

            return city;
        }

        public async Task PopulateCityWithData(string lauCode)
        {
            await GetHousePrice(lauCode);
            await GetGrowthData(lauCode);
            await GetIncomeData(lauCode);
            await GetPopulationGenderData(lauCode);
            await GetElectionData(lauCode);
            await GetMunicipalityElectionData(lauCode);
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
