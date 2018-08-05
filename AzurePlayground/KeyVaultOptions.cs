using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzurePlayground
{
    public class KeyVaultOptions
    {
        public string VaultBaseUrl { get; set; }
        public string KeyName { get; set; }
        public string AzurePlaygroundOAuthSecretUrl { get; set; }
    }
}
