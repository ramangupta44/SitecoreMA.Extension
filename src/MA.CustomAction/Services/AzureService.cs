using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// install Microsoft.WindowsAzure.Storage nuget package

namespace MA.CustomAction.Services
{
    public class AzureService : IAzureService
    {
        public AzureService()
        {

        }
        public async System.Threading.Tasks.Task SendAzureQueueAsync(string body)
        {
            var azQueueAcName = ConfigurationManager.AppSettings["AzureStorageAccountName"];
            var azQueueAcKey = ConfigurationManager.AppSettings["AzureStorageAccountkey"];
            var azQueueName = ConfigurationManager.AppSettings["AzureStorageQueueName"];
            var saCred = new StorageCredentials(azQueueAcName, azQueueAcKey);
            var saConfig = new CloudStorageAccount(saCred, true);
            var azClient = saConfig.CreateCloudQueueClient();
            var azQueue = azClient.GetQueueReference(azQueueName);
            await azQueue.CreateIfNotExistsAsync();
            var msgToSend = new CloudQueueMessage(body);
            Task.Run(async () => { await azQueue.AddMessageAsync(msgToSend); }).Wait();
        }
    }
}
