using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZodiacBack.Core.CustomResponses;

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
        public async Task<ActionResult<ZodiacResponse>> Get(DateTime birthday)
        {
            var response = await Task.Run( () => new ZodiacResponse(birthday));

            if (response.ErrorMessages.Any()) return BadRequest(response.ErrorMessages);
            
            return Ok(response);
        }
    }
}