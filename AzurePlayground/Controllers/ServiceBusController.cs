using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Options;

namespace AzurePlayground.Controllers
{
    public class ServiceBusController : Controller
    {
        private readonly ServiceBusOptions serviceBusOptions;

        public ServiceBusController(IOptions<ServiceBusOptions> serviceBusOptions)
        {
            if (serviceBusOptions == null)
                throw new ArgumentNullException(nameof(serviceBusOptions));

            this.serviceBusOptions = serviceBusOptions.Value;
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            // In reality, sending and receiving message never goes linear like this.
            // Also the try/finally pattern is not the best way to close the channels.
            // Check out https://github.com/Azure/azure-service-bus/blob/master/samples/DotNet/Microsoft.Azure.ServiceBus/SendersReceiversWithQueues/Program.cs
            // for a better sample.
            MessageReceiver messageReceiver = new MessageReceiver(new ServiceBusConnectionStringBuilder(serviceBusOptions.ConnectionString));
            
            try
            {
                IList<Message> messages = await messageReceiver.ReceiveAsync(1, TimeSpan.FromSeconds(5));
                if (messages == null || messages.Count == 0)
                {
                    // for the sake of learning only, assume that no error happened!
                    ViewData["QueueMessage"] = "Found no message in queue. Refresh to check again.";
                    // add a message to queue
                    MessageSender messageSender = new MessageSender(new ServiceBusConnectionStringBuilder(serviceBusOptions.ConnectionString));

                    try
                    {
                        await messageSender.SendAsync(new Message(Encoding.UTF8.GetBytes("ServiceBusController added a new message to queue at " + DateTime.Now.ToLongDateString())));
                    }
                    finally
                    {
                        await messageSender.CloseAsync();
                    }
                }
                else
                {
                    ViewData["QueueMessage"] = Encoding.UTF8.GetString(messages[0].Body);
                }
            }
            finally
            {
                await messageReceiver.CloseAsync();
            }
            return View();
        }
    }
}