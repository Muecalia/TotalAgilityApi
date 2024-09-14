using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class TopAgentesValidacaoResponse
    {
        public int Qtd { get; set; }
        public string Agente { get; set; } = string.Empty;
        //public decimal Percentagem { get; set; }
    }
}
