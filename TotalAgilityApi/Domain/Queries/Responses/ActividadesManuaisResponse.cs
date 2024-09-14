using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ActividadesManuaisResponse
    {
        public string NomeOperador { get; set; } = string.Empty;
        public string DataOperacao { get; set; } = string.Empty;
        //public string DataInicio { get; set; } = string.Empty;
        public string Msisdn { get; set; } = string.Empty;
        public string Actividade { get; set; } = string.Empty;
    }
}
