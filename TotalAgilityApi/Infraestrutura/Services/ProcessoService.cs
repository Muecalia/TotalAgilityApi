using TotalAgilityApi.Infraestrutura.Interfaces;
using TotalAgilityApi.Wrappers;
using static TotalAgilityApi.Enuns.TimeEnuns;

namespace TotalAgilityApi.Infraestrutura.Services
{
    public class ProcessoService : BackgroundService
    {
        private static bool IsDbRecovery = false;
        private static bool IsFirstDbRecovery = false;
        private static int TimeEstadosDataBase = 0;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProcessoService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var datetime = DateTime.Now;
                using var scope = _serviceScopeFactory.CreateScope();
                var _iProcessoRepository = scope.ServiceProvider.GetRequiredService<IProcessoRepository>();

                await GetEstadosDataBases(_iProcessoRepository, datetime, stoppingToken);
                await GetProcessosPendentes(_iProcessoRepository, datetime, stoppingToken);
                await GetProcessosCriadosDia(_iProcessoRepository, datetime, stoppingToken);
                await GetProcessosCriadosDiaResumo(_iProcessoRepository, datetime, stoppingToken);
                await GetEstatisticaProcessosSuspensos(_iProcessoRepository, datetime, stoppingToken);

                System.Diagnostics.Debug.Print("ProcessoService -> Executando aplicação em segundo plano");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private static async Task GetProcessosPendentes(IProcessoRepository _iProcessoRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            if ((datetime.Hour >= (int)HoraAlertasGerais.HORA5) && (datetime.Hour <= (int)HoraAlertasGerais.HORA23) && (datetime.Minute == 5))
                await _iProcessoRepository.GetProcessosPendentes(cancellationToken);
        }

        private static async Task GetEstadosDataBases(IProcessoRepository _iProcessoRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            Response<string> result;
            if (!IsDbRecovery)
            {
                result = await _iProcessoRepository.GetEstadosDataBases(cancellationToken);
                IsDbRecovery = result.Succeeded;
                TimeEstadosDataBase = datetime.AddMinutes(2).Minute;
            }
            else if (IsDbRecovery && datetime.Minute == TimeEstadosDataBase)
            {
                TimeEstadosDataBase = datetime.AddMinutes(10).Minute;
                result = await _iProcessoRepository.GetEstadosDataBases(cancellationToken);
                IsDbRecovery = result.Succeeded;
            }
        }

        private static async Task GetProcessosCriadosDia(IProcessoRepository _iProcessoRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            var times = Enum.GetValues(typeof(Times)).Cast<int>().ToList();

            if (times.Any(t => t == datetime.Hour && datetime.Minute == 10))
                await _iProcessoRepository.GetProcessosCriadosDia(datetime.ToString("yyyy-MM-dd"), cancellationToken);
        }

        private async Task GetProcessosCriadosDiaResumo(IProcessoRepository _iProcessoRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            if (datetime.Hour == (int)HoraAlertasGerais.HORA0 & (datetime.Minute == 15))
                await _iProcessoRepository.GetProcessosCriadosDia(datetime.AddDays(-1).ToString("yyyy-MM-dd"), cancellationToken);
        }

        private static async Task GetEstatisticaProcessosSuspensos(IProcessoRepository _iProcessoRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            var times = Enum.GetValues(typeof(HoraEstatisticaProcessosSuspensos)).Cast<int>().ToList();

            if (times.Any(hora => hora == datetime.Hour && datetime.Minute == 5))
                await _iProcessoRepository.GetEstatisticaProcessosSuspensos(cancellationToken);
        }


    }
}
