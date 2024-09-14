using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosDuplicadosResponse
    {
        public string Job_Status { get; set; } = string.Empty;
        public string Job_Id { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty; 
        public string CreatedOn { get; set; } = string.Empty;
        public string ModifiedBy { get; set; } = string.Empty;
        public string ModifiedOn { get; set; } = string.Empty;
        public string ControlActionReason { get; set; } = string.Empty;
        public string Msisdn { get; set; } = string.Empty;
    }
}
