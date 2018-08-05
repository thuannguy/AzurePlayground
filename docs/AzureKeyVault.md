# How to use Azure Key Vault

This application requires Azure Key Vault. Firstly, you need to create an Azure Key Vault and put the secret to access your Azure AD B2C using OpenID Connect in a key named "AzurePlaygroundOAuthSecret". After that, you need to install [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest).

- Open C:\Program Files\Microsoft SDKs\Azure\.NET SDK\v2.9>
- Type: az logout
- The command will open your default browser for you to login to your Azure subscription. The thing is that if you already logged in on that browser using other subcriptions, you may need to logout of all of them.
- Try: az keyvault secret show --vault-name [yourkeyvaultname] --name [yoursecretname]