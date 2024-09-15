using Microsoft.EntityFrameworkCore;

namespace TotalAgilityApi.Domain.Queries.Responses
{
    [Keyless]
    public class ProcessosManuaisDiaResponse
    {
        public int QtdCriadosDia { get; set; } = 0;
        public int QtdManualCriadosDia { get; set; } = 0;
        public int QtdManualEmCursoDia { get; set; } = 0;
        public int QtdManualCompletosDia { get; set; } = 0;
        public int QtdManualRejeitadosDia { get; set; } = 0;
        public string DataRegisto { get; set; } = string.Empty;
    }
}
