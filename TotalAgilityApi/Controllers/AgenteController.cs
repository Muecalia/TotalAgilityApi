using Microsoft.AspNetCore.Mvc;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgenteController : ControllerBase
    {
        private readonly IAgenteRepository _iAgenteRepository;

        public AgenteController(IAgenteRepository iAgenteRepository)
        {
            _iAgenteRepository = iAgenteRepository;
        }

        [HttpGet("agentesValidacaoRevisaoDocumentos")]
        public async Task<ActionResult<Response<string>>> GetTopAgentesValidacaoRevisaoDocumentos(CancellationToken cancellationToken)
        {
            var response = await _iAgenteRepository.GetTopAgentesValidacaoRevisaoDocumentos(cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

    }
}
