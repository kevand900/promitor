using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Promitor.Agents.Core.Validation.Interfaces;
using Promitor.Agents.Core.Validation.Steps;
using Promitor.Core.Extensions;
using Promitor.Core.Scraping.Configuration.Model;
using Promitor.Core.Scraping.Configuration.Model.Metrics;
using Promitor.Core.Scraping.Configuration.Providers.Interfaces;
using Promitor.Core.Scraping.Configuration.Serialization;
using Promitor.Agents.Scraper.Validation.MetricDefinitions;
using ValidationResult = Promitor.Agents.Core.Validation.ValidationResult;
using Promitor.Core.Serialization.Enum;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace Promitor.Agents.Scraper.Validation.Steps
{
    public class MetricsDeclarationValidationStep : ValidationStep, IValidationStep
    {
        private readonly IMetricsDeclarationProvider _metricsDeclarationProvider;
        
        public MetricsDeclarationValidationStep(IMetricsDeclarationProvider metricsDeclarationProvider, ILogger<MetricsDeclarationValidationStep> logger) : base( logger)
        {
            _metricsDeclarationProvider = metricsDeclarationProvider;
        }

        public string ComponentName => "Metrics Declaration";

        public ValidationResult Run()
        {
            var errorReporter = new ErrorReporter();
            var metricsDeclaration = _metricsDeclarationProvider.Get(applyDefaults: true, errorReporter: errorReporter);
            if (metricsDeclaration == null)
            {
                return ValidationResult.Failure(ComponentName, "Unable to deserialize configured metrics declaration");
            }

            LogDeserializationMessages(errorReporter);

            if (errorReporter.HasErrors)
            {
                return ValidationResult.Failure(ComponentName, "Errors were found while deserializing the metric configuration.");
            }

            var validationErrors = new List<string>();
            var azureMetadataErrorMessages = ValidateAzureMetadata(metricsDeclaration.AzureMetadata);
            validationErrors.AddRange(azureMetadataErrorMessages);

            var metricDefaultErrorMessages = ValidateMetricDefaults(metricsDeclaration.MetricDefaults);
            validationErrors.AddRange(metricDefaultErrorMessages);

            var metricsErrorMessages = ValidateMetrics(metricsDeclaration.Metrics, metricsDeclaration.MetricDefaults);
            validationErrors.AddRange(metricsErrorMessages);

            return validationErrors.Any() ? ValidationResult.Failure(ComponentName, validationErrors) : ValidationResult.Successful(ComponentName);
        }

        private void LogDeserializationMessages(IErrorReporter errorReporter)
        {
            if (errorReporter.Messages.Any())
            {
                var combinedMessages = string.Join(
                    Environment.NewLine, errorReporter.Messages.Select(message => message.FormattedMessage));

                var deserializationProblemsMessage = $"The following problems were found with the metric configuration:{Environment.NewLine}{combinedMessages}";
                if (errorReporter.HasErrors)
                {
                    Logger.LogError(deserializationProblemsMessage);
                }
                else
                {
                    Logger.LogWarning(deserializationProblemsMessage);
                }
            }
        }
        
        private static IEnumerable<string> ValidateMetricDefaults(MetricDefaults metricDefaults)
        {
            if (string.IsNullOrWhiteSpace(metricDefaults.Scraping?.Schedule))
            {
                yield return @"No default metric scraping schedule is defined.";
            }

            if (metricDefaults.Limit > Promitor.Core.Defaults.MetricDefaults.Limit)
            {
                yield return $"Limit cannot be higher than {Promitor.Core.Defaults.MetricDefaults.Limit}";
            }

            if (metricDefaults.Limit <= 0)
            {
                yield return @"Limit has to be at least 1";
            }
        }

        private static IEnumerable<string> DetectDuplicateMetrics(List<MetricDefinition> metrics)
        {
            var duplicateMetricNames = metrics.GroupBy(metric => metric.PrometheusMetricDefinition?.Name)
                .Where(groupedMetrics => groupedMetrics.Count() > 1)
                .Select(groupedMetrics => groupedMetrics.Key);

            return duplicateMetricNames;
        }

        private static IEnumerable<string> ValidateAzureMetadata(AzureMetadata azureMetadata)
        {
            var errorMessages = new List<string>();

            if (azureMetadata == null)
            {
                errorMessages.Add("No azure metadata is configured");
                return errorMessages;
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.TenantId))
            {
                errorMessages.Add("No tenant id is configured");
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.SubscriptionId))
            {
                errorMessages.Add("No subscription id is configured");
            }

            if (string.IsNullOrWhiteSpace(azureMetadata.ResourceGroupName))
            {
                errorMessages.Add("No resource group name is not configured");
            }

            if (azureMetadata.Cloud == AzureCloud.Unspecified)
            {
                errorMessages.Add("No Azure cloud is configured");
            }

            if (azureMetadata.Cloud == AzureCloud.Global)
            {
                azureMetadata.AzureEnvironment = AzureEnvironment.AzureGlobalCloud;
            }
            else if (azureMetadata.Cloud == AzureCloud.China)
            {
                azureMetadata.AzureEnvironment = AzureEnvironment.AzureChinaCloud;
            }
            else if (azureMetadata.Cloud == AzureCloud.Germany)
            {
                azureMetadata.AzureEnvironment = AzureEnvironment.AzureGermanCloud;
            }
            else if (azureMetadata.Cloud == AzureCloud.UsGov)
            {
                azureMetadata.AzureEnvironment = AzureEnvironment.AzureUSGovernment;
            }
            else if (azureMetadata.Cloud == AzureCloud.Custom)
            {
                if (azureMetadata.AzureEnvironment == null)
                {
                    errorMessages.Add("AzureEnvironment configuration is missing for custom cloud.");
                }
                if (string.IsNullOrWhiteSpace(azureMetadata.AzureEnvironment.AuthenticationEndpoint) ||
                string.IsNullOrWhiteSpace(azureMetadata.AzureEnvironment.ResourceManagerEndpoint) ||
                string.IsNullOrWhiteSpace(azureMetadata.AzureEnvironment.ManagementEndpoint) ||
                string.IsNullOrWhiteSpace(azureMetadata.AzureEnvironment.GraphEndpoint) ||
                string.IsNullOrWhiteSpace(azureMetadata.AzureEnvironment.StorageEndpointSuffix) ||
                string.IsNullOrWhiteSpace(azureMetadata.AzureEnvironment.KeyVaultSuffix))
                {
                    errorMessages.Add("All custom cloud endpoints were not configured to query");
                }
                else
                {
                    azureMetadata.AzureEnvironment.Name = azureMetadata.Cloud.GetEnvironmentName();
                }
            }
            else
            {
                errorMessages.Add($"{nameof(azureMetadata.Cloud)} No Azure environment is known for in legacy SDK");
            }

            return errorMessages;
        }

        private static IEnumerable<string> ValidateMetrics(List<MetricDefinition> metrics, MetricDefaults metricDefaults)
        {
            var errorMessages = new List<string>();

            if (metrics == null)
            {
                errorMessages.Add("No metrics are configured");
                return errorMessages;
            }

            var metricsValidator = new MetricsValidator(metricDefaults);
            var metricErrorMessages = metricsValidator.Validate(metrics);
            errorMessages.AddRange(metricErrorMessages);

            // Detect duplicate metric names
            var duplicateMetrics = DetectDuplicateMetrics(metrics);
            errorMessages.AddRange(duplicateMetrics.Select(duplicateMetricName => $"Metric name '{duplicateMetricName}' is declared multiple times"));

            return errorMessages;
        }
    }
}