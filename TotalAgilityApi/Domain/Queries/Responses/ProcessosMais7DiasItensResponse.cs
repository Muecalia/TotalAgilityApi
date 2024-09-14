using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosMais7DiasItensResponse
    {
        public string Job_Id { get; set; } = string.Empty;
        public int CodeEstado { get; set; } = 0;
        public string Estado { get; set;} = string.Empty;
        public string Process_Name { get; set; } = string.Empty; 
        public string Msisdn { get; set; } = string.Empty; 
        public string Creation_Time { get; set; } = string.Empty; 
    }
}
