using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
namespace PSSClient
{
    [XmlType("InterfaceFilesLocation")]
    [Serializable]
    public class iFileConfig
    {
        public string serverName { get; set; }
        public string system { get; set; }
        public string path { get; set; }
        public string apiEndPoint { get; set; }
    }


    public class serverConfiguration
    {
        [System.Xml.Serialization.XmlArray]
        public List<iFileConfig> serverConfig = new List<iFileConfig>();
        private string _JobConfigurateFileName = "iFileConfiguration.xml";
        public string _AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public bool loadConfiguration()
        {

            return false;
        }

        public bool saveConfiguration()
        {
            return false;
        }

        public bool deleteConfiguration()
        {
            return false;
        }
    }
    

}
