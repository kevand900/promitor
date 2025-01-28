using Azure.Identity;
using Azure.Monitor.Query;
using Humanizer;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Serialization.Enum;
using System;

namespace Promitor.Core.Extensions
{
    public static class AzureEnvironmentExtensions
    {
        /// <summary>
        ///     Get Azure Cloud name
        /// </summary>
        /// <param name="azureEnvironment">Microsoft Azure Environment</param>
        /// <returns>Azure Cloud name information for specified Environment</returns>
        public static string GetDisplayName(this AzureEnvironment azureEnvironment)
        {
            return azureEnvironment.Name.Replace("Azure", "").Replace("Cloud", "").Humanize(LetterCasing.Title);
        }

        /// <summary>
        ///     Get Azure environment information for Azure.Monitor SDK single resource queries
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static MetricsQueryAudience DetermineMetricsClientAudience(this AzureEnvironment azureEnvironment)
        {
            if (azureEnvironment.Name == AzureEnvironment.AzureGlobalCloud.Name)
            {
                return MetricsQueryAudience.AzurePublicCloud;
            }
            else if (azureEnvironment.Name == AzureEnvironment.AzureUSGovernment.Name)
            {
                return MetricsQueryAudience.AzureGovernment;
            }
            else if (azureEnvironment.Name == AzureEnvironment.AzureChinaCloud.Name)
            {
                return MetricsQueryAudience.AzureChina;
            }
            throw new ArgumentOutOfRangeException(azureEnvironment.GetDisplayName(), "No Azure environment is known for"); // Azure.Monitory.Query package does not support any other sovereign regions
        }

        /// <summary>
        ///     Get Azure environment information for Azure.Monitor SDK batch queries
        /// </summary>
        /// <param name="azureCloud">Microsoft Azure cloud</param>
        /// <returns>Azure environment information for specified cloud</returns>
        public static MetricsClientAudience DetermineMetricsClientBatchQueryAudience(this AzureEnvironment azureEnvironment)
        {
            if (azureEnvironment.Name == AzureEnvironment.AzureGlobalCloud.Name)
            {
                return MetricsClientAudience.AzurePublicCloud;
            }
            else if (azureEnvironment.Name == AzureEnvironment.AzureUSGovernment.Name)
            {
                return MetricsClientAudience.AzureGovernment;
            }
            else if (azureEnvironment.Name == AzureEnvironment.AzureChinaCloud.Name)
            {
                return MetricsClientAudience.AzureChina;
            }
            throw new ArgumentOutOfRangeException(azureEnvironment.GetDisplayName(), "No Azure environment is known for"); // Azure.Monitory.Query package does not support any other sovereign regions
        }

        /// <summary>
        ///     Get authority hosts for the Azure Cloud
        /// </summary>
        /// <param name="azureEnvironment">Microsoft Azure environment</param>
        /// <returns>Authority hosts for the specified cloud</returns>
        public static Uri GetAzureAuthorityHost(this AzureEnvironment azureEnvironment)
        {
            if (azureEnvironment.Name == AzureEnvironment.AzureGlobalCloud.Name)
            {
                return AzureAuthorityHosts.AzurePublicCloud;
            }
            else if (azureEnvironment.Name == AzureEnvironment.AzureChinaCloud.Name)
            {
                return AzureAuthorityHosts.AzureChina;
            }
            else if (azureEnvironment.Name == AzureEnvironment.AzureGermanCloud.Name)
            {
                return AzureAuthorityHosts.AzureGermany;
            }
            else if (azureEnvironment.Name == AzureEnvironment.AzureUSGovernment.Name)
            {
                return AzureAuthorityHosts.AzureGovernment;
            }
            throw new ArgumentOutOfRangeException(azureEnvironment.GetDisplayName(), "No Azure environment is known for");
        }
    }
}