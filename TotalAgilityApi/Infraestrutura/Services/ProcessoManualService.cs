using static TotalAgilityApi.Enuns.TimeEnuns;
using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.Domain.Queries.Requests;

namespace TotalAgilityApi.Infraestrutura.Services
{
    public class ProcessoManualService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProcessoManualService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var datetime = DateTime.Now;
                using var scope = _serviceScopeFactory.CreateScope();
                var _iProcessoManualRepository = scope.ServiceProvider.GetRequiredService<IProcessoManualRepository>();

                await GetActividadesManuais(_iProcessoManualRepository, datetime, stoppingToken);
                await GetProcessosManuaisDia(_iProcessoManualRepository, datetime, stoppingToken);
                await GetProcessosManuaisResumoDia(_iProcessoManualRepository, datetime, stoppingToken);

                System.Diagnostics.Debug.Print("ProcessoManualService -> Executando aplicação em segundo plano");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task GetActividadesManuais(IProcessoManualRepository iProcessoManualRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            if ((datetime.DayOfWeek == DayOfWeek.Sunday) && (datetime.Hour == (int)Times.HORA6) && (datetime.Minute == 10))
                await iProcessoManualRepository.GetActividadesManuais(new Request { DataInicial = datetime.AddDays(-7).ToString("yyy-MM-dd"), DataFinal = datetime.AddDays(-1).ToString("yyy-MM-dd") }, cancellationToken);                
        }

        private static async Task GetProcessosManuaisDia(IProcessoManualRepository iProcessoManualRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            var horas = Enum.GetValues(typeof(HoraMensagensProcessosManualDia)).Cast<int>().ToList();
            if (horas.Any(hora => hora == datetime.Hour && datetime.Minute == 10))
                await iProcessoManualRepository.GetProcessosManuaisDia(datetime.ToString("yyy-MM-dd"), cancellationToken);
        }

        private static async Task GetProcessosManuaisResumoDia(IProcessoManualRepository iProcessoManualRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            if ((datetime.Hour == (int)HoraAlertasGerais.HORA0) && (datetime.Minute == 30))
                await iProcessoManualRepository.GetProcessosManuaisDia(datetime.AddDays(-1).ToString("yyy-MM-dd"), cancellationToken);
        }

    }
}
