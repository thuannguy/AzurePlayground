using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.KeyVault.WebKey;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Options;

namespace AzurePlayground.Controllers
{
    public class KeyVaultController : Controller
    {
        private readonly KeyVaultOptions keyVaultOptions;

        public KeyVaultController(IOptions<KeyVaultOptions> keyVaultOptions)
        {
            if (keyVaultOptions == null)
                throw new ArgumentNullException(nameof(keyVaultOptions));

            this.keyVaultOptions = keyVaultOptions.Value;
        }

        // GET: KeyVault
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Encrypt")]
        public async Task<ActionResult> EncryptAsync(string dataToEncrypt)
        {
            AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            
            // Demonstrate how to get a key
            KeyBundle key = await keyVaultClient.GetKeyAsync(keyVaultOptions.VaultBaseUrl, keyVaultOptions.KeyName);

            // Here you can use the (public) key to do encryption which is faster
            // Or encrypt on the Azure side using the EncryptAsync operation
            // Note that you can call the EncryptAsync operation directly without calling the GetKeyAsync method above
            KeyOperationResult keyOperationResult = await keyVaultClient.EncryptAsync(keyVaultOptions.VaultBaseUrl, key.KeyIdentifier.Name,
                key.KeyIdentifier.Version,
                JsonWebKeyEncryptionAlgorithm.RSAOAEP,
                Encoding.UTF8.GetBytes(dataToEncrypt));
            ViewData["Encrypted"] = Convert.ToBase64String(keyOperationResult.Result);
            return View("Index");
        }
    }
}