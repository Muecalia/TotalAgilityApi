using Microsoft.AspNetCore.Mvc;
using TotalAgilityApi.Domain.Queries.Responses;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcessoController : ControllerBase
    {
        private readonly IProcessoRepository _iProcessoRepository;
        
        public ProcessoController(IProcessoRepository iProcessoRepository)
        {
            _iProcessoRepository = iProcessoRepository;
        }

        [HttpGet("processosCriadosDia")]
        public async Task<ActionResult<Response<string>>> GetProcessosCriadosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var response = await _iProcessoRepository.GetProcessosCriadosDia(DataCriacao, cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("estadosDataBases")]
        public async Task<ActionResult<PagedResponse<TopAgentesValidacaoResponse>>> GetEstadosDataBases(CancellationToken cancellationToken)
        {
            var response = await _iProcessoRepository.GetEstadosDataBases(cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("estatisticaProcessosSuspensos")]
        public async Task<ActionResult<PagedResponse<TopAgentesValidacaoResponse>>> GetEstatisticaProcessosSuspensos(CancellationToken cancellationToken)
        {
            var response = await _iProcessoRepository.GetEstatisticaProcessosSuspensos(cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("processosPendentes")]
        public async Task<ActionResult<PagedResponse<TopAgentesValidacaoResponse>>> GetProcessosPendentes(CancellationToken cancellationToken)
        {
            var response = await _iProcessoRepository.GetProcessosPendentes(cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

    }
}
