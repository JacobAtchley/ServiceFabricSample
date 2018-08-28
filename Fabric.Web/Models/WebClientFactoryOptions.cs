using Fabric.Core;
using System;
using System.Fabric;

namespace Fabric.Web.Models
{
    public class WebClientFactoryOptions
    {
        public string EndpointName { get; set; }

        public Action<StatelessServiceContext, string> LogAction { get; set; }

        public FabricSettings FabricSettings { get; set; }

        public string ContentRoot { get; set; }
    }
}
