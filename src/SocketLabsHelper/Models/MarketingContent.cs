using System;

namespace SocketLabsHelper.Models
{

    public class MarketingContent
    {
        public int id { get; set; }
        public string name { get; set; }
        public string thumbnailUrl { get; set; }
        public DateTime thumbnailExpiration { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public override string ToString()
        {
            return $"{id},{name},{created},{updated}";
        }
    }

}
