using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZodiacBack.Models;

namespace ZodiacBack.Controllers
{
    [ApiController]
    [Route("lab/[controller]")]
    public class Zodiac : ControllerBase
    {
        private readonly ILogger<Zodiac> _logger;

        public Zodiac(ILogger<Zodiac> logger)
        {
            _logger = logger;
        }

        [HttpGet("{birthday}")]
        public async Task<ActionResult<ZodiacResponse>> Get(string birthday)
        {
            DateTime res;
            var date = DateTime.TryParse(birthday,
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, out res);
            if (!date)
            {
                return BadRequest($"{birthday} is not in the correct format \"yyyy-MM-dd\"");    
            }
            
            var response = await Task.Run( () => new ZodiacResponse(birthday));

            if (response.ErrorMessages.Any()) return BadRequest(response.ErrorMessages);
            
            return Ok(response);
        }
    }
}