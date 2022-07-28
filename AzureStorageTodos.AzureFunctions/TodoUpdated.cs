using System;
using System.Linq;
using System.Threading.Tasks;
using AzureStorageTodos.Repository;
using AzureStorageTodos.Service.Interface;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureStorageTodos.AzureFunctions
{
    public class TodoUpdated
    {
        private readonly ITodoListManager _todoListManager;

        public TodoUpdated(ITodoListManager todoListManager)
        {
            _todoListManager = todoListManager;
        }

        [FunctionName("TodoUpdated")]
        public async Task Run([QueueTrigger(StaticQueueNames.TodoUpdated, Connection = "AzureWebJobsStorage")]string queueMessage, ILogger log)
        {
            string[] queueMessageParts = queueMessage.Split(": ");
            string updatedOwnerUserId = queueMessageParts.Last();

            await _todoListManager.RefreshTodoListsForUserAsync(updatedOwnerUserId);
        }
    }
}
