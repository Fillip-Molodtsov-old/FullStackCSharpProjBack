using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ZodiacBack.Core.Enums;
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
        
        [Route("prop")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetProperties()
        {
            var response = await Task.Run(() => _processes.GetProperties());
            return Ok(response);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomProcess>>>
            Get([FromQuery] ProcessProperties property, [FromQuery] bool desc)
        {
            var response = await 
                Task.Run(() => _processes.GetResponseProcesses(property,desc));
            return Ok(response);
        }

        [Route("info")]
        [HttpGet]
        public async Task<ActionResult<CustomProcess>> GetDetails([FromQuery] int id)
        {
            var response = await Task.Run(()=>_processes.GetAddInfo(id));
            return Ok(response);
        }
        
        [Route("kill")]
        [HttpGet]
        public async Task<ActionResult<string>> Kill([FromQuery] int id)
        {
            await Task.Run(() => CustomProcesses.KillProcess(id));
            return Ok("The process was killed");
        }

        [Route("open")]
        [HttpPost]
        public async Task<ActionResult<int>> OpenDirectory([FromBody] PathWrapperObject path)
        {
            await Task.Run(() => CustomProcesses.OpenDirectory(path.Path));
            return Ok();
        }
    }
}