using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPut("setdate")]
        public IActionResult SetDate()
        {
            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(5)
            });

            return Ok("👍");
        }

        [HttpGet("getdate")]
        public IActionResult GetDate()
        {
            return Ok(_memoryCache.Get<DateTime>("date"));
        }

        [HttpPut("setname")]
        public IActionResult SetName(string name)
        {
            _memoryCache.Set("name", name);
            return Ok("👍");

        }

        [HttpGet("getname")]
        public IActionResult GetName()
        {
            //string name = (string)_memoryCache.Get("name")!;

            if (_memoryCache.TryGetValue<string>("name", out string? name))
            {
                return Ok(name);
            }

            return NotFound();
        }

        [HttpDelete("removename")]
        public IActionResult RemoveName()
        {
            _memoryCache.Remove("name");
            return Ok("👍");
        }
    }
}
