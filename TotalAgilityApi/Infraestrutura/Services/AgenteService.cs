using TotalAgilityApi.Infraestrutura.Interfaces;

namespace TotalAgilityApi.Infraestrutura.Services
{
    public class AgenteService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AgenteService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var datetime = DateTime.Now;
                await GetTopAgentesValidacaoRevisaoDocumentos(datetime, stoppingToken);

                System.Diagnostics.Debug.Print("AgenteService -> Executando aplicação em segundo plano");

                // Intervalo de tempo de execução
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task GetTopAgentesValidacaoRevisaoDocumentos(DateTime datetime, CancellationToken cancellationToken)
        {
            var times = Enum.GetValues(typeof(Enuns.TimeEnuns.Times)).Cast<int>().ToList();

            if (times.Any(t => t == datetime.Hour && datetime.Minute == 0))
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _iAgenteRepository = scope.ServiceProvider.GetRequiredService<IAgenteRepository>();
                await _iAgenteRepository.GetTopAgentesValidacaoRevisaoDocumentos(cancellationToken);
            }
        }

    }
}
