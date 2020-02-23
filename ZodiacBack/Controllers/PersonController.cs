using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZodiacBack.Core;
using ZodiacBack.Core.CustomResponses;
using ZodiacBack.Core.Models;

namespace ZodiacBack.Controllers
{
    [ApiController]
    [Route("lab/people")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<Zodiac> _logger;

        private readonly People _people;

        public PersonController(ILogger<Zodiac> logger, People people)
        {
            _logger = logger;
            _people = people;
        }
        
        [Route("save")]
        [HttpGet]
        public async Task<ActionResult<string>> Save()
        {
            await Task.Run(() => _people.SaveData());
            return Ok("Данні були збережені");
        }

        [HttpGet]
        public async Task<ActionResult<PersonResponse>> GetAll([FromQuery(Name = "prop")] PersonProperties property, [FromQuery(Name = "desc")] bool desc)
        {
            var response = await Task.Run(() => _people.GetList(property, desc));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<PersonResponse>> Post([FromBody] Person person)
        {
            var response = await Task.Run(() => _people.AddPerson(person));
            if (response.ErrorMessages.Any()) return BadRequest(response.ErrorMessages);
            return Ok(response);
        }
        
        [HttpPut]
        public async Task<ActionResult<PersonResponse>> Put([FromBody] Person person)
        {
            var response = await Task.Run(() => _people.PutPerson(person));
            if (response.ErrorMessages.Any()) return BadRequest(response.ErrorMessages);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<string>> Delete(string id)
        {
            await Task.Run(() => _people.DeletePerson(id));
            return Ok($"Person with id: {id} was deleted.");
        }
    }
}