using Humanizer;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using System;

namespace Promitor.Core.Extensions
{
    public static class AzureEnvironmentExtensions
    {
        /// <summary>
        ///     Get Azure environment information
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static string GetDisplayName(this AzureEnvironment azureCloud)
        {
            return azureCloud.Name.Replace("Azure", "").Replace("Cloud", "").Humanize(LetterCasing.Title);
        }


        public static AzureEnvironment GetAzureCustomCloud(IConfiguration configuration) {
            AzureEnvironment azureCustomCloud = configuration.GetSection("azureCustomCloud").Get<AzureEnvironment>();
            if (azureCustomCloud == null)
            {
                throw new InvalidOperationException("Custom Azure Cloud configuration is missing or invalid.");
            }
            if (string.IsNullOrWhiteSpace(azureCustomCloud.AuthenticationEndpoint) ||
               string.IsNullOrWhiteSpace(azureCustomCloud.ResourceManagerEndpoint) ||
               string.IsNullOrWhiteSpace(azureCustomCloud.ManagementEndpoint) ||
               string.IsNullOrWhiteSpace(azureCustomCloud.GraphEndpoint) ||
               string.IsNullOrWhiteSpace(azureCustomCloud.StorageEndpointSuffix) ||
               string.IsNullOrWhiteSpace(azureCustomCloud.KeyVaultSuffix))
            {
                throw new InvalidOperationException("One or more required Azure custom cloud configuration values are missing.");
            }
            azureCustomCloud.Name = "AzureCustomCloud";
            return azureCustomCloud;
        }
    }
}