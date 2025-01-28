using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Text.Json.Serialization;
using Promitor.Core.Serialization.Enum;

namespace Promitor.Core.Scraping.Configuration.Serialization.v1.Model
{
    public class AzureMetadataV1
    {
        public string TenantId { get; set; }
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public AzureCloud Cloud { get; set; }
        [JsonPropertyName("endpoints")]
        public AzureEnvironment? AzureEnvironment { get; set; }
    }
}
