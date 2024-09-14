using TotalAgilityApi.Wrappers;

namespace TotalAgilityApi.Infraestrutura.Interfaces
{
    public interface IAgenteRepository
    {
        Task<Response<string>> GetTopAgentesValidacaoRevisaoDocumentos(CancellationToken cancellationToken);
    }
}
