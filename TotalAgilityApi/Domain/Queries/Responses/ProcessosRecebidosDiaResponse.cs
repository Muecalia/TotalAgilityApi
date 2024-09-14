using Microsoft.EntityFrameworkCore;
//
namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosRecebidosDiaResponse
    {
        public int Qtd { get; set; } = 0;
        public string CreationDate { get; set; } = string.Empty;
    }
}
