using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosSuspensosMsisdnResponse
    {
        public string Job_Id { get; set; } = string.Empty;
        public string Job_Note { get; set; } = string.Empty;
        //c.JOB_ID, a.JOB_NOTE
    }
}
