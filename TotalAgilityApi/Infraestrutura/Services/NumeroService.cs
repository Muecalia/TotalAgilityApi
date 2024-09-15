using TotalAgilityApi.Infraestrutura.Interfaces;

namespace TotalAgilityApi.Infraestrutura.Services
{
    public class NumeroService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public NumeroService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var datetime = DateTime.Now;
                using var scope = _serviceScopeFactory.CreateScope();
                var _iNumeroRepository = scope.ServiceProvider.GetRequiredService<INumeroRepository>();

                await GetNumerosRemovidos(_iNumeroRepository, datetime, stoppingToken);
                await GetNumerosTerminados(_iNumeroRepository, datetime, stoppingToken);
                await GetNumerosEsperaImagens(_iNumeroRepository, datetime, stoppingToken);
                await GetEstatisticaNumerosEsperaImagens(_iNumeroRepository, datetime, 7, stoppingToken);

                System.Diagnostics.Debug.Print("NumeroService -> Executando aplicação em segundo plano");

                // Intervalo de tempo de execução
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
                
        }

        private static async Task GetNumerosEsperaImagens(INumeroRepository iNumeroRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            if ((int)Enuns.TimeEnuns.Times.HORA6 == datetime.Hour && datetime.Minute == 10)
                await iNumeroRepository.GetNumerosEsperaImagens(cancellationToken);
        }

        private static async Task GetNumerosTerminados(INumeroRepository iNumeroRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            var times = Enum.GetValues(typeof(Enuns.TimeEnuns.Times)).Cast<int>().ToList();

            if (times.Any(t => t == datetime.Hour && datetime.Minute == 15))
                await iNumeroRepository.GetNumerosTerminados(cancellationToken);
        }

        private static async Task GetNumerosRemovidos(INumeroRepository iNumeroRepository, DateTime datetime, CancellationToken cancellationToken)
        {
            if ((int)Enuns.TimeEnuns.Times.HORA6 == datetime.Hour && datetime.Minute == 20)
                await iNumeroRepository.GetNumerosRemovidos(cancellationToken);
        }

        private static async Task GetEstatisticaNumerosEsperaImagens(INumeroRepository iNumeroRepository, DateTime datetime, int NumeroDias, CancellationToken cancellationToken)
        {
            if ((int)Enuns.TimeEnuns.Times.HORA6 == datetime.Hour && datetime.Minute == 5)
                await iNumeroRepository.GetEstatisticaNumerosEsperaImagens(NumeroDias, cancellationToken);
        }

    }
}
