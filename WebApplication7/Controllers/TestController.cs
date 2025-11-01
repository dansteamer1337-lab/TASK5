using Microsoft.AspNetCore.Mvc;

namespace WebApplication7.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static readonly Random _random = new Random();

        [HttpGet]
        public IActionResult GetRandomNumbers(int count)
        {
            if (count <= 0)
            {
                return BadRequest("Count must be greater than 0");
            }

            var numbers = new int[count];
            for (int i = 0; i < count; i++)
            {
                numbers[i] = _random.Next(1, 101);
            }

            return Ok(numbers);
        }
    }
}