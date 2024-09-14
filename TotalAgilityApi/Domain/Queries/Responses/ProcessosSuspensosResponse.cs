using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosSuspensosResponse
    {
        public string Job_Id { get; set; } = string.Empty;
        public string Node_Name { get; set; } = string.Empty;
        public string Process_Name { get; set; } = string.Empty;
        public string Due_Date { get; set; } = string.Empty;
        public string Job_Note { get; set; } = string.Empty;
    }
}
