using TotalAgilityApi.Domain.Queries.Requests;
using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Interfaces
{
    public interface IProcessoManualRepository
    {
        Task<Response<string>> GetActividadesManuais(Request request, CancellationToken cancellationToken);
        Task<Response<string>> GetProcessosManuaisDia(string DataCriacao, CancellationToken cancellationToken);
    }
}
