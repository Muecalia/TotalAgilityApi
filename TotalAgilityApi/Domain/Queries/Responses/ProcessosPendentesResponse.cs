using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosPendentesResponse
    {
        public int Quantidade { get; set; }
        public string NodeName { get; set; } = string.Empty;
    }
    
}
