namespace TotalAgilityApi.Domain.Queries.Responses
{
    public class TopAgentesValidacaoRevisaoDocumentos
    {
        public List<TopAgentesValidacaoResponse> TopValidacao { get; set; } = [];
        public List<TopAgentesValidacaoResponse> TopRevisaoDocumentos { get; set; } = [];
    }
}
