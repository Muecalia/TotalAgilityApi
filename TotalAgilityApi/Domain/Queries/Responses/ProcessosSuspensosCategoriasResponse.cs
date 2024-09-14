using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosSuspensosCategoriasResponse
    {
        public int Qtd { get; set; }
        public string Node_Name { get; set; } = string.Empty;
    }
}
