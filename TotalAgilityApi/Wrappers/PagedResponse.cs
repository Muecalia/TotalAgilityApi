namespace TotalAgilityApi.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public List<T> Datas { get; private set; }

        public PagedResponse() { }

        public PagedResponse(string message)
        {
            Datas = new List<T>();
            Message = message;
            Succeeded = false;
        }

        public PagedResponse(IEnumerable<T> data, string message)
        {
            Datas = new List<T>(data);
            Message = message; 
            Succeeded = true;
        }
    }
}
