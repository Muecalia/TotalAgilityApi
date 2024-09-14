using TotalAgilityApi.Domain.Queries.Requests;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Interfaces
{
    public interface IProcessoManualRepository
    {
        Task<Response<string>> GetActividadesManuais(Request request, CancellationToken cancellationToken);
        Task<Response<string>> GetProcessosManuaisCriadosDia(string DataCriacao, CancellationToken cancellationToken);
        Task<Response<string>> GetProcessosManuaisTerminadosConcluidosDia(string DataCriacao, CancellationToken cancellationToken);
        Task<Response<string>> GetProcessosManuaisValidadosDia(string DataCriacao, CancellationToken cancellationToken);
    }
}
