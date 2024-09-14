using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class NumerosTerminadosResponse
    {
        public string Job_Id { get; set; } = string.Empty;
        public string Msisdn { get; set; } = string.Empty;
        public string Sap_Agente { get; set; } = string.Empty;
        public string Estado_Numero { get; set; } = string.Empty;
    }
}
