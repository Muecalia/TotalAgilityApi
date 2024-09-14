using Microsoft.AspNetCore.Mvc;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroController : ControllerBase
    {        
        private readonly INumeroRepository _iNumeroRepository;
        
        public NumeroController(INumeroRepository iNumeroRepository)
        {            
            _iNumeroRepository = iNumeroRepository;
        }

        [HttpGet("numerosEsperaImagens")]
        public async Task<ActionResult<Response<string>>> GetNumerosEsperaImagens(CancellationToken cancellationToken)
        {
            var response = await _iNumeroRepository.GetNumerosEsperaImagens(cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("estatisticaNumerosEsperaImagens")]
        public async Task<ActionResult<Response<string>>> GetEstatisticaNumerosEsperaImagens(int NumeroDias, CancellationToken cancellationToken)
        {
            var response = await _iNumeroRepository.GetEstatisticaNumerosEsperaImagens(NumeroDias, cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("numerosRemovidos")]
        public async Task<ActionResult<Response<string>>> GetNumerosRemovidos(CancellationToken cancellationToken)
        {
            var response = await _iNumeroRepository.GetNumerosRemovidos(cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

        [HttpGet("numerosTerminados")]
        public async Task<ActionResult<Response<string>>> GetNumerosTerminados(CancellationToken cancellationToken)
        {
            var response = await _iNumeroRepository.GetNumerosTerminados(cancellationToken);
            if (response.Succeeded)
                return Ok(response.Message);
            return BadRequest(response.Message);
        }

    }
}
