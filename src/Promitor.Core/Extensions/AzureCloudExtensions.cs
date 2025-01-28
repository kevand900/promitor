using System;
using Azure.Identity;
using Azure.Monitor.Query;
using Humanizer;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Serialization.Enum;
using static Promitor.Core.EnvironmentVariables;

namespace Promitor.Core.Extensions
{
    public static class AzureCloudExtensions
    {
        public static string GetEnvironmentName(this AzureCloud azureCloud)
        {
            return "Azure"+ azureCloud + "Cloud";
        }
    }
}