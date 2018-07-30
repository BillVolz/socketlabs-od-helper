using System;
using System.Text;
using SocketLabsHelper.Models;
using SocketLabsHelper.Services;


namespace SocketLabsHelper
{
    public class Marketing
    {
        private static string _apiKey;
        private static int _serverId;

        public Marketing(string apiKey, int serverId)
        {
            _apiKey = apiKey;
            _serverId = serverId;
        }

        public MarketingContent[] GetContent()
        {
            try
            {
                var svcCredentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(_serverId + ":" + _apiKey));
                var httpPostServer = new HttpPostService();
                return httpPostServer.Get<MarketingContent[]>("https://api.socketlabs.com/marketing/v1/content", "Authorization: Basic " + svcCredentials);
                
               

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error, something bad happened: " + ex.Message);
                return null;
            }
        }
    }
}
