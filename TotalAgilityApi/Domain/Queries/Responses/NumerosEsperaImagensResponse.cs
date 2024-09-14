using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class NumerosEsperaImagensResponse
    {
        public string JobId { get; set; } = string.Empty;
        public string Msisdn { get; set; } = string.Empty;
        public string SapAgente { get; set; } = string.Empty;
        public string Actividade { get; set; } = string.Empty;
    }
}
