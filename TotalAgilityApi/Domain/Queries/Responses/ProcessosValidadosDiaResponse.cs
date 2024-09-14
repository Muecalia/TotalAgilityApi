using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosValidadosDiaResponse
    {
        public int Qtd { get; set; } = 0;
        public string Actividade { get; set; } = string.Empty;
        public string CreationDate { get; set; } = string.Empty;
    }
}
