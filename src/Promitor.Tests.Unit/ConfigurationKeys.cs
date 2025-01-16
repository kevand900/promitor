namespace Promitor.Tests.Unit
{
    public class ConfigurationKeys
    {
        public class Authentication
        {
            public const string IdentityId = "authentication:IdentityId";
            public const string Mode = "authentication:Mode";
            public const string SecretFilePath = "authentication:SecretFilePath";
            public const string SecretFileName = "authentication:SecretFileName";
        }
        
        public class AzureCustomCloud
        {
            public const string Name = nameof(AzureCustomCloud);
            public const string AuthenticationEndpoint = "azureCustomCloud:authenticationEndpoint";
            public const string GraphEndpoint = "azureCustomCloud:graphEndpoint";
            public const string ResourceManagerEndpoint = "azureCustomCloud:resourceManagerEndpoint";
            public const string ManagementEndpoint = "azureCustomCloud:managementEndpoint";
            public const string StorageEndpointSuffix = "azureCustomCloud:storageEndpointSuffix";
            public const string KeyVaultSuffix = "azureCustomCloud:keyVaultSuffix";
        }
    }
}
