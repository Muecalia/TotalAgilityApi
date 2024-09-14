using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class EstadosDataBasesResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
