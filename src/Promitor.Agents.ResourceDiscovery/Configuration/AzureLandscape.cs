using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Storage.Fluent.Models;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Agents.ResourceDiscovery.Configuration
{
    public class AzureLandscape
    {
        public string TenantId { get; set; }
        public List<string> Subscriptions { get; set; }
        public AzureCloud Cloud { get; set; } = AzureCloud.Global;
        [JsonPropertyName("endpoints")]
        public AzureEnvironment? AzureEnvironment { get; set; }
    }
}
