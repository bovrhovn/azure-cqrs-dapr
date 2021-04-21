using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace CityApp.News.Controllers
{
    [ApiController]
    [Route("news")]
    [Produces("application/json")]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> logger;
        private readonly INewsRepository newsRepository;
        private readonly IMemoryCache memoryCache;

        public NewsController(ILogger<NewsController> logger, INewsRepository newsRepository, IMemoryCache memoryCache)
        {
            this.logger = logger;
            this.newsRepository = newsRepository;
            this.memoryCache = memoryCache;
        }

        [Route("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllNewsAsync()
        {
            logger.LogInformation("Check, if in memory");
            if (!memoryCache.TryGetValue<List<Models.News>>("newsdata", out var list))
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60));

                list = (await newsRepository.GetAllAsync()).ToList();
                memoryCache.Set(memoryCache, list, cacheEntryOptions);
            }
            logger.LogInformation($"Loaded {list.Count} items from database");
            return Ok(list);
        }

        [Route("search/{query}/{page}/{pageCount}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchPagedAsync(string query, int page, int pageCount)
        {
            var data = await newsRepository.SearchPagedAsync(query, page, pageCount);
            return Ok(data);
        }

        [Route("details/{newsId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDetailsAsync(int newsId)
        {
            logger.LogInformation("Getting news details");
            var news = await newsRepository.GetDetailsAsync(newsId);
            if (news == null)
            {
                logger.LogInformation("Information was not found");
                return NotFound();
            }
            return Ok(news);
        }
    }
}