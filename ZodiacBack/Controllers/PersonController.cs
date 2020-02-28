using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZodiacBack.Core.CustomResponses;
using ZodiacBack.Core.Enums;
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

        [Route("prop")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetProperties()
        {
            var response = await Task.Run(() => _people.GetProperties());
            return Ok(response);
        }

        [Route("save")]
        [HttpGet]
        public async Task<ActionResult<string>> Save()
        {
            await Task.Run(() => _people.SaveData());
            return Ok("Дані були збережені");
        }

        [Route("fs")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetFilSorted
        ([FromQuery(Name = "pf")] PersonProperties propertyF,
            [FromQuery(Name = "value")] string value, [FromQuery(Name = "ps")] PersonProperties propertyS,
            [FromQuery(Name = "desc")] bool desc)
        {
            var response =
                await Task.Run(() =>
                    _people.GetFilteredAndOrderedPeople(propertyF, value, propertyS, desc));
            return Ok(response);
        }

        [Route("filter")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetFiltered
        ([FromQuery(Name = "pf")] PersonProperties property,
            [FromQuery(Name = "value")] string value)
        {
            var response =
                await Task.Run(() => _people.GetFilteredPeople(property, value, true));
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetAll(
            [FromQuery(Name = "ps")] PersonProperties property,
            [FromQuery(Name = "desc")] bool desc)
        {
            var response = await Task.Run(() => _people.GetOrderedList(property, desc));
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