using Microsoft.EntityFrameworkCore;
using Prometheus;
using System.Globalization;
using TotalAgilityApi.Config;
using TotalAgilityApi.Infraestrutura.Context;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.RabbitMq;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Repositories
{
    public class ProcessoRepository : IProcessoRepository
    {
        private readonly TotalAgilityContext _context;
        private readonly CultureInfo customCulture;
        private readonly ILogger<ProcessoRepository> _logger;
        private readonly IRabbitMqService _rabbitMqService;
        private static readonly Counter RequestEstadosDataBasesCounter = Metrics.CreateCounter("estado_base_dados_total", "Requisições estados das bases de dados", ["status_code"]);
        private static readonly Counter RequestEstatisticaProcessoSuspensoCounter = Metrics.CreateCounter("estatistica_processo_suspenso_total", "Requisições estatistica de processos suspensos", ["status_code"]);
        private static readonly Counter RequestProcessosCriadoDiaCounter = Metrics.CreateCounter("processo_criado_dia_total", "Requisições de processos criados no dia", ["status_code"]);
        private static readonly Counter RequestProcessosPendenteCounter = Metrics.CreateCounter("processo_pendente_total", "Requisições de processos pendentes", ["status_code"]);

        public ProcessoRepository(TotalAgilityContext context, ILogger<ProcessoRepository> logger, IRabbitMqService rabbitMqService) 
        {
            _logger = logger;

            customCulture = new CultureInfo("en-US", false);
            customCulture = new CultureInfo("pt-PT", true);

            customCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;

            _rabbitMqService = rabbitMqService;
            _context = context;
        }

        /*************************************************************************************************
        * Objectivo: Listar o estado das bases de dados
        * Parametros: Nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        *************************************************************************************************/
        public async Task<Response<string>> GetEstadosDataBases(CancellationToken cancellationToken)
        {
            var Entidade = "Estados das Bases de Dados";
            try
            {
                string Queue = "EstadoDataBaseQueue";
                var response = await _context.EstadosDataBases.FromSqlRaw($"EXEC [dbo].[sp_GetEstadosDataBases]").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestEstadosDataBasesCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();

                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestEstadosDataBasesCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        /*************************************************************************************************
        * Objectivo: Listar a estatística dos processos suspensos
        * Parametros: Nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        *************************************************************************************************/
        public async Task<Response<string>> GetEstatisticaProcessosSuspensos(CancellationToken cancellationToken)
        {
            var Entidade = "Estatística de Processos Suspensos";
            try
            {
                string Queue = "EstatisticaProcessoSuspensoQueue";
                var response = await _context.EstatisticaProcessosSuspensos.FromSqlRaw($"EXEC [dbo].[sp_GetProcessosSuspensosCategorias]").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestEstatisticaProcessoSuspensoCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();

                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestEstatisticaProcessoSuspensoCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        /*************************************************************************************************
        * Objectivo: Listar os processos criados no dia no KTA
        * Parametros: DataCriacao
        * Retorno: A lista contendo os dados ou lista vazia
        *************************************************************************************************/
        public async Task<Response<string>> GetProcessosCriadosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var Entidade = "Processos criados no dia";
            try
            {
                string Queue = "ProcessosCriadosDiaQueue";

                var DataActual = DateTime.Now;
                DateTime FirstDate = Convert.ToDateTime(DataCriacao);

                if (FirstDate.CompareTo(DataActual) > 0)
                    return new Response<string>(MessageError.DataError());

                var response = await _context.ProcessosRecebidosDia.FromSqlInterpolated($"EXEC sp_GetProcessosCriadosDia @DataOperacao={FirstDate}").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestProcessosCriadoDiaCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();

                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestProcessosCriadoDiaCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        /***********************************************************************************************************
        * Objectivo: Listar os processos pendentes (processos a espera de tratamento manual) no KTA
        * Parametros: nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        ***********************************************************************************************************/
        public async Task<Response<string>> GetProcessosPendentes(CancellationToken cancellationToken)
        {
            var Entidade = "Processos Pendentes";
            try
            {
                string Queue = "ProcessoPendenteQueue";
                var response = await _context.ProcessosPendentes.FromSqlRaw($"EXEC sp_GetProcessosPendentes").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestProcessosPendenteCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();

                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestProcessosPendenteCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

    }
}
