using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Interfaces
{
    public interface IProcessoRepository
    {
        Task<Response<string>> GetProcessosPendentes(CancellationToken cancellationToken);
        Task<Response<string>> GetEstatisticaProcessosSuspensos(CancellationToken cancellationToken);
        Task<Response<string>> GetProcessosCriadosDia(string DataCriacao, CancellationToken cancellationToken);
        Task<Response<string>> GetEstadosDataBases(CancellationToken cancellationToken);
    }
}
