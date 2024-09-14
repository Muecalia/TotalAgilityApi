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
    public class NumeroRepository : INumeroRepository
    {
        readonly TotalAgilityContext _context;
        readonly ILogger<NumeroRepository> _logger;
        private readonly CultureInfo customCulture;
        private readonly IRabbitMqService _rabbitMqService;
        private static readonly Counter RequestNumeroImagensCounter = Metrics.CreateCounter("numeros_espera_imagens_total", "Números a espera de imagens", ["status_code"]);
        private static readonly Counter RequestEstatisticaNumeroImagensCounter = Metrics.CreateCounter("estatistica_numeros_espera_imagens_total", "Estatística de Números a espera de imagens", ["status_code"]);
        private static readonly Counter RequestNumeroRemovidosCounter = Metrics.CreateCounter("numeros_removidos_total", "Números removidos", ["status_code"]);
        private static readonly Counter RequestNumeroTerminadosCounter = Metrics.CreateCounter("numeros_terminados_total", "Números terminados", ["status_code"]);


        public NumeroRepository(TotalAgilityContext context, ILogger<NumeroRepository> logger, IRabbitMqService rabbitMqService)
        {
            _logger = logger;

            customCulture = new CultureInfo("en-US", false);
            customCulture = new CultureInfo("pt-PT", true);

            customCulture.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";

            Thread.CurrentThread.CurrentCulture = customCulture;
            Thread.CurrentThread.CurrentUICulture = customCulture;

            _context = context;
            _rabbitMqService = rabbitMqService;
        }

        /*************************************************************************************************
        * Objectivo: Listar os números que estão a espera de imagens (Documento do Cliente)
        * Parametros: Nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        *************************************************************************************************/
        public async Task<Response<string>> GetNumerosEsperaImagens(CancellationToken cancellationToken)
        {
            var Entidade = "Números a espera de imagens";
            try
            {
                string Queue = "NumeroEsperaImagemQueue";
                var response = await _context.NumerosEsperaImagens.FromSqlRaw($"EXEC [dbo].[sp_GetNumerosEsperaImagens]").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestNumeroImagensCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();

                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestNumeroImagensCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        /*************************************************************************************************
        * Objectivo: Listar os números que foram removidos no KTA
        * Parametros: Nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        *************************************************************************************************/
        public async Task<Response<string>> GetNumerosRemovidos(CancellationToken cancellationToken)
        {
            var Entidade = "Números removidos";
            try
            {
                string Queue = "NumeroRemovidoQueue";
                var response = await _context.NumerosRemovidos.FromSqlRaw($"EXEC [dbo].[sp_GetNumerosRemovidos]").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestNumeroRemovidosCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestNumeroRemovidosCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        /*************************************************************************************************
        * Objectivo: Listar os números que terminados no KTA
        * Parametros: Nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        *************************************************************************************************/
        public async Task<Response<string>> GetNumerosTerminados(CancellationToken cancellationToken)
        {
            var Entidade = "Números terminados";
            try
            {
                string Queue = "NumeroTerminadoQueue";
                var response = await _context.NumerosTerminados.FromSqlRaw($"EXEC [dbo].[sp_GetNumerosTerminados]").ToListAsync(cancellationToken);

                if (response.Count > 0)
                    _rabbitMqService.SendMessage(response, Queue);
                RequestNumeroRemovidosCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestNumeroRemovidosCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }

        /*************************************************************************************************
        * Objectivo: Listar a estatística dos números a espera de imagens (Documento do Cliente) no KTA
        * Parametros: Nenhum
        * Retorno: A lista contendo os dados ou lista vazia
        *************************************************************************************************/
        public async Task<Response<string>> GetEstatisticaNumerosEsperaImagens(int NumeroDias, CancellationToken cancellationToken)
        {
            var Entidade = "Estatistica números a espera de imagem";
            try
            {
                //
                string Queue = "EstatisticaNumeroEsperaImagemQueue";
                var response = await _context.ProcessosRecebidosDia.FromSql($"EXEC [dbo].[sp_GetEstatisticaNumeroEsperaImagem] @Dias={NumeroDias}").ToListAsync(cancellationToken);

                if (response.Count > 0) 
                    _rabbitMqService.SendMessage(response, Queue);
                RequestEstatisticaNumeroImagensCounter.Labels(StatusCodes.Status200OK.ToString()).Inc();
                _logger.LogInformation(MessageError.CarregamentoSucesso(Entidade, response.Count));
                return new Response<string>(Entidade, MessageError.CarregamentoSucesso(Entidade));
            }
            catch (Exception ex)
            {
                _logger.LogError(MessageError.BadRequest(Entidade, ex.Message));
                RequestEstatisticaNumeroImagensCounter.Labels(StatusCodes.Status400BadRequest.ToString()).Inc();
                return new Response<string>(Entidade);
            }
        }


    }
}
