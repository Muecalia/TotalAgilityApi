using Microsoft.EntityFrameworkCore;
using Prometheus;
using System.Globalization;
using TotalAgilityApi.Config;
using TotalAgilityApi.Domain.Queries.Requests;
using TotalAgilityApi.Infraestrutura.Context;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.RabbitMq;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Repositories
{
    public class ProcessoManualRepository : IProcessoManualRepository
    {
        readonly TotalAgilityContext _context;
        readonly ILogger<ProcessoManualRepository> _logger;
        private readonly CultureInfo customCulture;

        private readonly IRabbitMqService _rabbitMqService;

        private static readonly Counter RequestActividadeManualCounter = Metrics.CreateCounter("actividade_manual_total", "Requisições das actividades manuais", ["status_code"]);
        private static readonly Counter RequestProcessoManualCriadoDiaCounter = Metrics.CreateCounter("processo_manual_criado_dia_total", "Requisições de processos manuais criados no dia", ["status_code"]);
        private static readonly Counter RequestProcessoManualValidadoDiaCounter = Metrics.CreateCounter("processo_manual_validado_dia_total", "Requisições de processos manuais validado no dia", ["status_code"]);
        private static readonly Counter RequestProcessoManualTermiandoConcluidoDiaCounter = Metrics.CreateCounter("processo_manual_terminado_concluido_dia_total", "Requisições de processos manuais terminados e concluídos no dia", ["status_code"]);


        public ProcessoManualRepository(TotalAgilityContext context, ILogger<ProcessoManualRepository> logger, IRabbitMqService rabbitMqService)
        {
            _logger = logger;

            customCulture = new CultureInfo("en-US", false);
            customCulture = new CultureInfo("pt-PT", true);

            _rabbitMqService = rabbitMqService;

            customCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;

            _context = context;
        }

        /*************************************************************************************************
        * Objectivo: Listar as actividades manuais (Actividades que tiveram tratamento humano)
        * Parametros: request (DataInicial e DataFinal)
        * Retorno: A lista contendo os dados ou lista vazia
        ************************************************************************************************/
        public async Task<Response<string>> GetActividadesManuais(Request request, CancellationToken cancellationToken)
        {
            var Entidade = "Actividades Manuais";
            try
            {
                string Queue = "ActividadeManualQueue";
                var DataActual = DateTime.Now;
                //var End_Date = $"{request.DataFinal} 23:59:59";
                DateTime FirstDate = Convert.ToDateTime(request.DataInicial);
                DateTime EndDate = Convert.ToDateTime(request.DataFinal);

                if ((FirstDate.CompareTo(DataActual) > 0) || (EndDate.CompareTo(DataActual) > 0))
                    return new Response<string>(MessageError.DataError());

                if (FirstDate.CompareTo(EndDate) > 0)
                    return new Response<string>(MessageError.DataError(FirstDate.ToString("d"), EndDate.ToString("d")));

                var response = await _context.ActividadesManuais.FromSqlInterpolated($"EXEC [dbo].[sp_GetActividadesManuais] @FirstDate={FirstDate}, @EndDate={EndDate}").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestActividadeManualCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestActividadeManualCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        /*************************************************************************************************
         * Objectivo: Listar os processos manuais criados no dia
         * Parametros: DataCriacao
         * Retorno: A lista contendo os dados ou lista vazia
         ************************************************************************************************/
        public async Task<Response<string>> GetProcessosManuaisCriadosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var Entidade = "Processos Manuais criados no dia";
            try
            {
                string Queue = "ProcessoManualCriadoDiaQueue";
                var DataActual = DateTime.Now;
                DateTime CreatedDate = Convert.ToDateTime($"{DataCriacao}");

                if (CreatedDate.CompareTo(DataActual) > 0)
                    return new Response<string>(MessageError.DataError());

                //var response = await _context.ProcessosRecebidosDia.FromSql($"EXEC [dbo].[sp_GetProcessosManuaisCriadosDia] @DataOperacao={DataCriacao}").ToListAsync(cancellationToken);
                var response = await _context.ProcessosRecebidosDia.FromSqlInterpolated($"EXEC [dbo].[sp_GetProcessosManuaisCriadosDia] @DataOperacao={DataCriacao}").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestProcessoManualCriadoDiaCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestProcessoManualCriadoDiaCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        public async Task<Response<string>> GetProcessosManuaisTerminadosConcluidosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var Entidade = "Processos Manuais termiandos e concluídos no dia";
            try
            {
                string Queue = "ProcessoManualTerminadoConcluidoDiaQueue";
                var DataActual = DateTime.Now;
                DateTime CreatedDate = Convert.ToDateTime($"{DataCriacao}");

                if (CreatedDate.CompareTo(DataActual) > 0)
                    return new Response<string>(MessageError.DataError());

                //var response = await _context.ProcessosRecebidosDia.FromSql($"EXEC [dbo].[sp_GetProcessosManuaisCriadosDia] @DataOperacao={DataCriacao}").ToListAsync(cancellationToken);
                var response = await _context.ProcessosRecebidosDia.FromSqlInterpolated($"EXEC [dbo].[sp_GetProcessosManuaisTerminadosConcluidosDia] @DataOperacao={DataCriacao}").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestProcessoManualTermiandoConcluidoDiaCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestProcessoManualTermiandoConcluidoDiaCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        public async Task<Response<string>> GetProcessosManuaisValidadosDia(string DataCriacao, CancellationToken cancellationToken)
        {
            var Entidade = "Processos Manuais validados no dia";
            try
            {
                string Queue = "ProcessoManualValidadoDiaQueue";
                var DataActual = DateTime.Now;
                DateTime CreatedDate = Convert.ToDateTime($"{DataCriacao}");

                if (CreatedDate.CompareTo(DataActual) > 0)
                    return new Response<string>(MessageError.DataError());

                var response = await _context.ProcessosRecebidosDia.FromSqlInterpolated($"EXEC [dbo].[sp_GetProcessosManuaisValidadosDia] @DataOperacao={DataCriacao}").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestProcessoManualValidadoDiaCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestProcessoManualValidadoDiaCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }


    }
}
