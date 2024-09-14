using Microsoft.EntityFrameworkCore;
using Prometheus;
using System.Globalization;
using TotalAgilityApi.Config;
using TotalAgilityApi.Domain.Queries.Responses;
using TotalAgilityApi.Infraestrutura.Context;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.RabbitMq;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Repositories
{
    public class AgenteRepository : IAgenteRepository
    {
        readonly TotalAgilityContext _context;
        readonly ILogger<AgenteRepository> _logger;
        private readonly CultureInfo customCulture;

        private readonly IRabbitMqService _rabbitMqService;
        private static readonly Counter RequestAgenteValidacaoRevDocumentoCounter = Metrics.CreateCounter("agente_validacao_rev_documentos_total", "Total requisições agente validação e revisão de documentos endpoint", ["status_code"]);

        public AgenteRepository(TotalAgilityContext context, ILogger<AgenteRepository> logger, IRabbitMqService rabbitMqService)
        {
            _logger = logger;
            _rabbitMqService = rabbitMqService;
            customCulture = new CultureInfo("en-US", false);
            customCulture = new CultureInfo("pt-PT", true);

            customCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;

            _context = context;
        }


        /**********************************************************************************************************
        * Objectivo: Listar o Top 10 dos agentes que mais enviam processos para validação e revisão de documentos
        * Parametros: Nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        **********************************************************************************************************/
        public async Task<Response<string>> GetTopAgentesValidacaoRevisaoDocumentos(CancellationToken cancellationToken)
        {
            var Entidade = "Agente Validação e revisão de documentos";
            try
            {
                string Queue = "AgenteValidacaoRevisaoDocumentoQueue";
                var responseValidacao = await _context.TopAgentesValidacao.FromSqlInterpolated($"EXEC sp_GetTopAgentesValidacao").ToListAsync(cancellationToken);
                var responseRevDocumentos = await _context.TopAgentesValidacao.FromSqlInterpolated($"EXEC sp_GetTopAgentesRevisaoDocumentos").ToListAsync(cancellationToken);

                var response = new TopAgentesValidacaoRevisaoDocumentos
                {
                    TopValidacao = responseValidacao,
                    TopRevisaoDocumentos = responseRevDocumentos
                };

                _rabbitMqService.SendMessage(response, Queue);
                RequestAgenteValidacaoRevDocumentoCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, responseValidacao.Count+ responseRevDocumentos.Count));
                return new Response<string>(string.Empty, MessageError.RabbitCarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.RabbitCarregamentoErro(Entidade, ex.Message));
                RequestAgenteValidacaoRevDocumentoCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }
    }
}
