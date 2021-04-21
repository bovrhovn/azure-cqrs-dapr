using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CityApp.Electricity.Controllers
{
    [ApiController]
    [Route("electricity")]
    [Produces("application/json")]
    public class ElectricityController : ControllerBase
    {
        private readonly ILogger<ElectricityController> logger;
        private readonly IElectricityRepository electricityRepository;
        private readonly IElectricityMeasurementRepository electricityMeasurementRepository;
        private readonly IMemoryCache memoryCache;

        public ElectricityController(ILogger<ElectricityController> logger,
            IElectricityRepository electricityRepository,
            IElectricityMeasurementRepository electricityMeasurementRepository,
            IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.electricityRepository = electricityRepository;
            this.electricityMeasurementRepository = electricityMeasurementRepository;
            this.memoryCache = memoryCache;
        }

        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetElectricityAsync()
        {
            logger.LogInformation("Check, if in memory");
            if (!memoryCache.TryGetValue<List<Models.Electricity>>("electricitydata", out var list))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                list = (await electricityRepository.GetAllAsync()).ToList();
                memoryCache.Set(memoryCache, list, cacheEntryOptions);
            }

            logger.LogInformation($"Loaded {list.Count} items from database");
            return Ok(list);
        }

        [Route("search/{cityUserId}/{electricityId}/{page}/{pageCount}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPagedAsync(int cityUserId, int electricityId, int page, int pageCount)
        {
            int? electricityValue = null;
            if (electricityId != -1) electricityValue = electricityId;
            var data = await electricityMeasurementRepository.GetPagedForUserAsync(cityUserId, electricityValue, page,
                pageCount);
            return Ok(data);
        }
    }
}