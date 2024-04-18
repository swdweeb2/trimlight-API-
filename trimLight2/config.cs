using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace trimLight2
{
    public class config
    {
        string clientId;
        string clientSecret;
        string baseURI;
        public string ClientId { get => clientId; set => clientId = value; }
        public string ClientSecret { get => clientSecret; set => clientSecret = value; }
        public string BaseURI { get => baseURI; set => baseURI = value; }
        public config() 
        {
            clientId = ConfigurationManager.AppSettings["clientId"];
            clientSecret = ConfigurationManager.AppSettings["clientSecret"];
            baseURI = ConfigurationManager.AppSettings["baseURI"];
        }
    }
}
