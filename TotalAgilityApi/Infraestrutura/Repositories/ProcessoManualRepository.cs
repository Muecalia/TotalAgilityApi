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
        private static readonly Counter RequestProcessoManualDiaCounter = Metrics.CreateCounter("processo_manual_dia_total", "Requisições de processos manuais criados, concluídos e terminados no dia", ["status_code"]);


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
        public async Task<Response<string>> GetProcessosManuaisDia(string DataRegisto, CancellationToken cancellationToken)
        {
            var Entidade = "Processos Manuais no dia";
            try
            {
                string Queue = "ProcessoManualDiaQueue";
                var DataActual = DateTime.Now;
                DateTime CreatedDate = Convert.ToDateTime($"{DataRegisto}");

                if (CreatedDate.CompareTo(DataActual) > 0)
                    return new Response<string>(MessageError.DataError());

                var response = await _context.ProcessosManuaisDia.FromSqlInterpolated($"EXEC [dbo].[sp_GetProcessosManuaisDia] @DataRegisto={CreatedDate}").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestProcessoManualDiaCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestProcessoManualDiaCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }


    }
}
