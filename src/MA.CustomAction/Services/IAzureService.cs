using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA.CustomAction.Services
{
    public interface IAzureService
    {
        System.Threading.Tasks.Task SendAzureQueueAsync(string body);
    }
}
