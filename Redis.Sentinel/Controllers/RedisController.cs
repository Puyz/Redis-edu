using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Redis.Sentinel.Services;
using StackExchange.Redis;

namespace Redis.Sentinel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetValue(string key)
        {
            IDatabase redisDB = await RedisService.GetRedisMasterDatabase();
            RedisValue value = await redisDB.StringGetAsync(key);
            return Ok(value.ToString());
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            IDatabase redisDB = await RedisService.GetRedisMasterDatabase();
            await redisDB.StringSetAsync(key, value);
            return Ok();
        }
    }
}
