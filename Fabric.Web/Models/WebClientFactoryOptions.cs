using Fabric.Core;
using System;
using System.Fabric;

namespace Fabric.Web.Models
{
    /// <summary>
    /// Encapsulates data outside the web tier needed to configur the web application.
    /// This is usually data in the fabric layer.
    /// </summary>
    public class WebClientFactoryOptions
    {
        public string EndpointName { get; set; }

        public Action<StatelessServiceContext, string> LogAction { get; set; }

        public FabricSettings FabricSettings { get; set; }

        public string ContentRoot { get; set; }
    }
}
