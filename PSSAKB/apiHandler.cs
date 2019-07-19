using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.ServiceModel;
using System.Configuration;
namespace PSSAKB
{
    public static class apiHandler
    {
        private static BasicHttpBinding binding = new BasicHttpBinding();
        private static BasicHttpBinding cresbinding = new BasicHttpBinding();
        private static EndpointAddress address;
        private static EndpointAddress cresapiaddr;
        public static ChannelFactory<GlobalAPI.IServerAdministration> WServices;
        public static ChannelFactory<CRESapi.ICRESapi> WServicesCRES;
        public static GlobalAPI.IServerAdministration gChannel;
        public static CRESapi.ICRESapi  gCRESChannel;
        public static void setEndPoint(string endPointAddr)
        {
            binding.Security.Mode = BasicHttpSecurityMode.None;
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            address = new EndpointAddress(string.Format(ConfigurationManager.AppSettings["globalapiuri"], ConfigurationManager.AppSettings["serverIP"]));
            WServices = new ChannelFactory<GlobalAPI.IServerAdministration>(binding, address);
            gChannel = WServices.CreateChannel();

            cresbinding.Security.Mode = BasicHttpSecurityMode.None;
            cresbinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            cresbinding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            cresbinding.OpenTimeout = new TimeSpan(0, 10, 0);
            cresbinding.CloseTimeout = new TimeSpan(0, 10, 0);
            cresapiaddr = new EndpointAddress(string.Format(ConfigurationManager.AppSettings["CRESapiUri"], ConfigurationManager.AppSettings["serverIP"]));
            WServicesCRES = new ChannelFactory<CRESapi.ICRESapi>(cresbinding, cresapiaddr);
            gCRESChannel = WServicesCRES.CreateChannel();
        }
    }
}
