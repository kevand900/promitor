using System;
using System.Collections.Generic;
using System.ComponentModel;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Extensions.Configuration;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureCloudUnitTests : UnitTest
    {
        [Fact]
        public void GetAzureEnvironment_ForAzureGlobalCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Global;
            var expectedEnvironment = AzureEnvironment.AzureGlobalCloud;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(config);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureChinaCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.China;
            var expectedEnvironment = AzureEnvironment.AzureChinaCloud;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(config);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureGermanCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Germany;
            var expectedEnvironment = AzureEnvironment.AzureGermanCloud;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(config);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureUSGovernmentCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.UsGov;
            var expectedEnvironment = AzureEnvironment.AzureUSGovernment;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(config);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForAzureCustomCloud_ProvidesCorrectEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Custom;
            AzureEnvironment expectedEnvironment = new AzureEnvironment
            {
                Name = "AzureCustomCloud",
                AuthenticationEndpoint = "https://login.microsoftonline.com/custom",
                ResourceManagerEndpoint = "https://management.azure.com/custom",
                ManagementEndpoint = "https://management.core.windows.net/custom",
                GraphEndpoint = "https://graph.windows.net/custom",
                StorageEndpointSuffix = "core.windowscustom.net",
                KeyVaultSuffix = "vault.azurecustom.net"
            };

            var config = CreateConfiguration(new Dictionary<string, string> {
                { ConfigurationKeys.AzureCustomCloud.Name, expectedEnvironment.Name },
                { ConfigurationKeys.AzureCustomCloud.AuthenticationEndpoint, expectedEnvironment.AuthenticationEndpoint },
                {ConfigurationKeys.AzureCustomCloud.ResourceManagerEndpoint, expectedEnvironment.ResourceManagerEndpoint },
                {ConfigurationKeys.AzureCustomCloud.ManagementEndpoint, expectedEnvironment.ManagementEndpoint },
                { ConfigurationKeys.AzureCustomCloud.GraphEndpoint , expectedEnvironment.GraphEndpoint },
                {ConfigurationKeys.AzureCustomCloud.StorageEndpointSuffix, expectedEnvironment.StorageEndpointSuffix },
                {ConfigurationKeys.AzureCustomCloud.KeyVaultSuffix, expectedEnvironment.KeyVaultSuffix },
            });


            // Act
            var azureEnvironment = azureCloud.GetAzureEnvironment(config);

            // Assert
            PromitorAssert.ContainsSameAzureEnvironmentInfo(expectedEnvironment, azureEnvironment);
        }

        [Fact]
        public void GetAzureEnvironment_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var azureCloud = AzureCloud.Unspecified;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(()=> azureCloud.GetAzureEnvironment(config));
        }


        [Fact]
        public void GetAzureAuthorityHost_ForAzureGlobalCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.Global;
            var expectedAuthorityHost = AzureAuthorityHosts.AzurePublicCloud;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(config);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureChinaCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.China;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureChina;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(config);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGermanCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.Germany;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGermany;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(config);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureUSGovernmentCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureCloud = AzureCloud.UsGov;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGovernment;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act
            var actualAuthorityHost = azureCloud.GetAzureAuthorityHost(config);

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var azureCloud = AzureCloud.Unspecified;
            var config = CreateConfiguration(new Dictionary<string, string> { });

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => azureCloud.GetAzureAuthorityHost(config));
        }

        private IConfigurationRoot CreateConfiguration(Dictionary<string, string> inMemoryConfiguration)
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemoryConfiguration)
                .Build();
        }
    }
}
