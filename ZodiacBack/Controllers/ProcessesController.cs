using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZodiacBack.Core.Models;

namespace ZodiacBack.Controllers
{
    [ApiController]
    [Route("lab/tasks")]
    public class ProcessesController: ControllerBase
    {
        private readonly ILogger<Zodiac> _logger;

        private readonly CustomProcesses _processes;

        public ProcessesController(ILogger<Zodiac> logger, CustomProcesses processes)
        {
            _logger = logger;
            _processes = processes;
        }
        
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomProcess>>> Get()
        {
            var response = await Task.Run(() => _processes.GetResponseProcesses());
            return Ok(response);
        }
    }
}