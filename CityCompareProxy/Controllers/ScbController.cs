using System;
using System.Net.Http;
using System.Threading.Tasks;
using CityCompareProxy.Models;
using CityCompareProxy.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace CityCompareProxy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScbController : ControllerBase
    {
        private readonly IScbService _scbService;

        public ScbController(IScbService scbService)
        {
            _scbService = scbService;
        }

        [HttpGet("houseprices/{cityId}")]
        public async Task<IActionResult> GetHousePrice(string cityId)
        {
            try
            {
                ScbResponse? data = await _scbService.GetHousePrice(cityId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("income/{cityId}")]
        public async Task<IActionResult> GetIncomeData(string cityId)
        {
            try
            {
                ScbResponse? data = await _scbService.GetIncomeData(cityId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("growth/{cityId}")]
        public async Task<IActionResult> GetGrowthData(string cityId)
        {
            try
            {
                ScbResponse? growthResponse = await _scbService.GetGrowthData(cityId);

                return Ok(growthResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("population/{cityId}")]
        public async Task<IActionResult> GetPopulationData(string cityId)
        {
            try
            {
                ScbResponse? electionResponse = await _scbService.GetPopulationData(cityId);

                return Ok(electionResponse);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }


        [HttpGet("election/{cityId}")]
        public async Task<IActionResult> GetElectionData(string cityId)
        {
            try
            {
                ScbResponse? electionResponse = await _scbService.GetElectionData(cityId);

                return Ok(electionResponse);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }



        [HttpGet("election-municipality/{cityId}")]
        public async Task<IActionResult> GetMunicipalityElectionData(string cityId)
        {
            try
            {
                ScbResponse? electionResponse = await _scbService.GetMunicipalityElectionData(cityId);

                return Ok(electionResponse);
            }
            catch (Exception ex) { return StatusCode(500, $"Internal server error: {ex.Message}"); }
        }

    }
}

