namespace DAL.Utilities
{
    public interface IResponseAPI
    {
        System.Net.Http.HttpResponseMessage GetResponseAPI(string ur);
        System.Net.Http.HttpResponseMessage ReadAsStringAsyncAPI(string url, object obj);
    }
}
