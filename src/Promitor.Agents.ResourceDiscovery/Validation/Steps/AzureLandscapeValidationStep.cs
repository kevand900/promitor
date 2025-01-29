using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Promitor.Agents.Core.Validation;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Agents.ResourceDiscovery.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;
using static Promitor.Core.EnvironmentVariables;

namespace Promitor.Agents.ResourceDiscovery.Validation.Steps
{
    public class AzureLandscapeValidationStep : ValidationStep,
        IValidationStep
    {
        private readonly AzureLandscape _azureLandscape;

        public AzureLandscapeValidationStep(IOptions<AzureLandscape> azureLandscape, ILogger<AzureLandscapeValidationStep> logger) : base(logger)
        {
            _azureLandscape = azureLandscape.Value;
        }

        public string ComponentName { get; } = "Azure Landscape";

        public ValidationResult Run()
        {
            var errorMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(_azureLandscape.TenantId))
            {
                errorMessages.Add("No tenant id was configured");
            }

            if (_azureLandscape.Cloud == AzureCloud.Unspecified)
            {
                errorMessages.Add("No Azure cloud was configured");
            }

            errorMessages.AddRange(ValidateAzureCloudEnvironment());
            
            if (_azureLandscape.Subscriptions == null || _azureLandscape.Subscriptions.Count == 0)
            {
                errorMessages.Add("No subscription id(s) were configured to query");
            }
            else
            {
                if (_azureLandscape.Subscriptions.Distinct().Count() != _azureLandscape.Subscriptions.Count)
                {
                    errorMessages.Add("Duplicate subscription ids were configured to query");
                }

                if (_azureLandscape.Subscriptions.Any(string.IsNullOrWhiteSpace))
                {
                    errorMessages.Add("Empty subscription is configured to query");
                }
            }

            return errorMessages.Any() ? ValidationResult.Failure(ComponentName, errorMessages) : ValidationResult.Successful(ComponentName);
        }

        private IEnumerable<string> ValidateAzureCloudEnvironment()
        {
            var errorMessages = new List<string>();
            if (_azureLandscape.Cloud == AzureCloud.Global)
            {
                _azureLandscape.AzureEnvironment = AzureEnvironment.AzureGlobalCloud;
            }
            else if (_azureLandscape.Cloud == AzureCloud.China)
            {
                _azureLandscape.AzureEnvironment = AzureEnvironment.AzureChinaCloud;
            }
            else if (_azureLandscape.Cloud == AzureCloud.Germany)
            {
                _azureLandscape.AzureEnvironment = AzureEnvironment.AzureGermanCloud;
            }
            else if (_azureLandscape.Cloud == AzureCloud.UsGov)
            {
                _azureLandscape.AzureEnvironment = AzureEnvironment.AzureUSGovernment;
            }
            else if (_azureLandscape.Cloud == AzureCloud.Custom)
            {
                if (_azureLandscape.AzureEnvironment == null)
                {
                    errorMessages.Add("AzureEnvironment configuration is missing for custom cloud.");
                }
                else if (string.IsNullOrWhiteSpace(_azureLandscape.AzureEnvironment.AuthenticationEndpoint) ||
                string.IsNullOrWhiteSpace(_azureLandscape.AzureEnvironment.ResourceManagerEndpoint) ||
                string.IsNullOrWhiteSpace(_azureLandscape.AzureEnvironment.ManagementEndpoint) ||
                string.IsNullOrWhiteSpace(_azureLandscape.AzureEnvironment.GraphEndpoint) ||
                string.IsNullOrWhiteSpace(_azureLandscape.AzureEnvironment.StorageEndpointSuffix) ||
                string.IsNullOrWhiteSpace(_azureLandscape.AzureEnvironment.KeyVaultSuffix))
                {
                    errorMessages.Add("All custom cloud endpoints were not configured to query");
                }
                else
                {
                    _azureLandscape.AzureEnvironment.Name = _azureLandscape.Cloud.GetEnvironmentName();
                }
            }
            else
            {
                errorMessages.Add($"{nameof(_azureLandscape.Cloud)} No Azure environment is known for in legacy SDK");
            }
            return errorMessages;
        }
    }
}