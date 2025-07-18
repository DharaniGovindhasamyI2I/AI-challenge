using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisTestController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public RedisTestController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet("set")] // api/RedisTest/set?key=foo&value=bar
        public async Task<IActionResult> Set(string key, string value)
        {
            await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });
            return Ok($"Set {key} = {value}");
        }

        [HttpGet("get")] // api/RedisTest/get?key=foo
        public async Task<IActionResult> Get(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (value == null)
                return NotFound($"Key '{key}' not found in cache.");
            return Ok($"{key} = {value}");
        }
    }
} 