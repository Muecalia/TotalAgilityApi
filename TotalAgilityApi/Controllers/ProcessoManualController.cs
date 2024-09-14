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

        [HttpGet("processosManuaisCriadosDia")]
        public async Task<ActionResult<Response<string>>> GetProcessosManuaisCriadosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var response = await _iProcessoManualRepository.GetProcessosManuaisCriadosDia(DataCriacao, cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("processosManuaisTerminadosConcluidosDia")]
        public async Task<ActionResult<Response<string>>> GetProcessosManuaisTerminadosConcluidosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var response = await _iProcessoManualRepository.GetProcessosManuaisTerminadosConcluidosDia(DataCriacao, cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("processosManuaisValidadosDia")]
        public async Task<ActionResult<Response<string>>> GetProcessosManuaisValidadosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var response = await _iProcessoManualRepository.GetProcessosManuaisValidadosDia(DataCriacao, cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

    }
}
