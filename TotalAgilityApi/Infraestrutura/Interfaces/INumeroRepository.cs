using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Interfaces
{
    public interface INumeroRepository
    {
        Task<Response<string>> GetNumerosRemovidos(CancellationToken cancellationToken);
        Task<Response<string>> GetNumerosTerminados(CancellationToken cancellationToken);
        Task<Response<string>> GetNumerosEsperaImagens(CancellationToken cancellationToken);
        Task<Response<string>> GetEstatisticaNumerosEsperaImagens(int NumeroDias, CancellationToken cancellationToken);
    }
}
