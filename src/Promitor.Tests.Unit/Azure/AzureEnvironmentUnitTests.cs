using System;
using System.ComponentModel;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureEnvironmentUnitTests : UnitTest
    {
        [Fact]
        public void GetDisplayName_ForAzureGlobalCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureGlobalCloud;
            var expectedDisplayName = "Global";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }

        [Fact]
        public void GetDisplayName_ForAzureChinaCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureChinaCloud;
            var expectedDisplayName = "China";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }

        [Fact]
        public void GetDisplayName_ForAzureGermanCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureGermanCloud;
            var expectedDisplayName = "German";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }

        [Fact]
        public void GetDisplayName_ForUSGovernmentCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureUSGovernment;
            var expectedDisplayName = "US Government";

            // Act
            var displayName = azureEnvironment.GetDisplayName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }


        [Fact]
        public void GetAzureAuthorityHost_ForAzureGlobalCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureGlobalCloud;
            var expectedAuthorityHost = AzureAuthorityHosts.AzurePublicCloud;

            // Act
            var actualAuthorityHost = azureEnvironment.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureChinaCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureChinaCloud;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureChina;

            // Act
            var actualAuthorityHost = azureEnvironment.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureGermanCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureGermanCloud;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGermany;

            // Act
            var actualAuthorityHost = azureEnvironment.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForAzureUSGovernmentCloud_ProvidesCorrectAuthorityHost()
        {
            // Arrange
            var azureEnvironment = AzureEnvironment.AzureUSGovernment;
            var expectedAuthorityHost = AzureAuthorityHosts.AzureGovernment;

            // Act
            var actualAuthorityHost = azureEnvironment.GetAzureAuthorityHost();

            // Assert
            Assert.True(expectedAuthorityHost.Equals(actualAuthorityHost));
        }

        [Fact]
        public void GetAzureAuthorityHost_ForUnspecifiedAzureCloud_ThrowsException()
        {
            // Arrange
            var azureEnvironment = new AzureEnvironment 
            { 
                Name = "AzureCustomCloud"
            };


            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => azureEnvironment.GetAzureAuthorityHost());
        }
    }
}
