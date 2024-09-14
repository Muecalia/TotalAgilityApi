using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosCursoTerminadosResponse
    {
        public string Job_Id { get; set; } = string.Empty;
        public string Creation_Time { get; set; } = string.Empty;
        public string Process_Name { get; set; } = string.Empty;
        public string Var_Id { get; set; } = string.Empty;
        public string Msisdn { get; set; } = string.Empty;
        public string Job_Status { get; set; } = string.Empty;
    }
}
