namespace TotalAgilityApi.Config
{
    public class MessageError
    {
        //NOT FOUND
        public static string NotFound(string str)
        {
            return $"Não existe {str} a serem exibidos.";
        }
        public static string NotFound(string str, string mensagem)
        {
            return $"Não existe {str} a serem exibidos. Mensagem: {mensagem}.";
        }

        //CARREGAR
        public static string CarregamentoSucesso(string str)
        {
            return $"Dados da(o) {str} carregada(o) com sucesso.";
        }
        public static string CarregamentoSucesso(string str, int Size)
        {
            return $"Dados da(o) {str} carregada(o) com sucesso.\nTotal de Itens carregados: {Size}";
        }

        public static string RabbitCarregamentoSucesso(string str)
        {
            return $"Dados da(o) {str} carregada(o) com sucesso no servidor rabbit.";
        }
        public static string RabbitCarregamentoErro(string str, string msn)
        {
            return $"Erro ao carregar os dados da(o) {str} no servidor rabbit.Mensagem: {msn}";
        }

        //DATA
        //
        public static string DataError()
        {
            return $"Erro! Data inserida não pode ser maior que a data actual.";
        }
        public static string DataError(string DataInicial, string DataFinal)
        {
            return $"Erro! a Data Inicial {DataInicial} não deve ser maior que a data final {DataFinal}.";
        }

        //BADREQUEST
        public static string BadRequest(string str)
        {
            return $"Erro ao realizar a operação.";
        }
        public static string BadRequest(string str, string mensagem)
        {
            return $"Erro ao carregar os dados do(a) {str}. Mensagem: {mensagem}";
        }

    }
}
