using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class NumerosRemovidosResponse
    {
        public string Msisdn { get; set; } = string.Empty;
        public string Agente { get; set; } = string.Empty;
        public string Entrada { get; set; } = string.Empty;
        public string DataCriacao { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
