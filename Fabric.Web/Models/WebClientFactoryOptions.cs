using System;
using System.Fabric;
using System.Fabric.Description;

namespace Fabric.Web.Models
{
    public class WebClientFactoryOptions
    {
        public string ContentDirectory { get; set; }

        public string EndpointName { get; set; }

        public Action<StatelessServiceContext, string> LogAction { get; set; }

        public ConfigurationSettings FabricConfiguration { get; set; }
    }
}
