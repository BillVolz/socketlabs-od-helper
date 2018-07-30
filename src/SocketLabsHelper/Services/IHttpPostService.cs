namespace SocketLabsHelper.Services
{
    public interface IHttpPostService
    {
        string Post(object postdata, string url);
        T PostAndGetResponse<T>(object payload, string url, string header);
        T Get<T>(string url, string header);
        dynamic Get(string url, string header);
        string GetResponseAsString(string url, string header);
  
    }
}
