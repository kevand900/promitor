using System;
using System.ComponentModel;
using Azure.Identity;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.OpenApi.Extensions;
using Promitor.Core.Extensions;
using Promitor.Core.Serialization.Enum;
using Xunit;

namespace Promitor.Tests.Unit.Azure
{
    [Category("Unit")]
    public class AzureCloudUnitTests : UnitTest
    {
        [Fact]
        public void GetEnvironmentName_ForAzureCustomCloud_ProvidesCorrectDisplayNameEnvironmentInfo()
        {
            // Arrange
            var azureCloud = AzureCloud.Custom;
            var expectedDisplayName = "AzureCustomCloud";

            // Act
            var displayName = azureCloud.GetEnvironmentName();

            // Assert
            Assert.Equal(expectedDisplayName, displayName);
        }
    }
}
