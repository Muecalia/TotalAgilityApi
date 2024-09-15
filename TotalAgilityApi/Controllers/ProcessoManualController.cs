using Microsoft.AspNetCore.Mvc;
using TotalAgilityApi.Domain.Queries.Requests;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoManualController : ControllerBase
    {
        private readonly IProcessoManualRepository _iProcessoManualRepository;
        

        public ProcessoManualController(IProcessoManualRepository iProcessoManualRepository)
        {
            _iProcessoManualRepository = iProcessoManualRepository;
        }

        [HttpGet("actividadesManuais")]
        public async Task<ActionResult<Response<string>>> GetActividadesManuais([FromQuery] Request request, CancellationToken cancellationToken)
        {
            var response = await _iProcessoManualRepository.GetActividadesManuais(request, cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("processosManuaisDia")]
        public async Task<ActionResult<Response<string>>> GetProcessosManuaisDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var response = await _iProcessoManualRepository.GetProcessosManuaisDia(DataCriacao, cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

    }
}
