using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZodiacBack.Core.CustomResponses;
using ZodiacBack.Core.HttpModels;

namespace ZodiacBack.Controllers
{
    [ApiController]
    [Route("lab/person")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<Zodiac> _logger;

        public PersonController(ILogger<Zodiac> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<PersonResponse>> Post([FromBody] PersonHttpBody person)
        {
            var response = await Task.Run(() => new PersonResponse(person));
            if (response.ErrorMessages.Any()) return BadRequest(response.ErrorMessages);
            return Ok(response);
        }
    }
}