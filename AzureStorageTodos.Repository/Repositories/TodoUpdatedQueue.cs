using Azure.Storage.Queues;
using AzureStorageTodos.Repository.Interface;
using AzureStorageTodos.Repository.BaseRepositories;

namespace AzureStorageTodos.Repository.Repositories;

public class TodoUpdatedQueue : AzureQueueStorageService, ITodoUpdatedQueue
{
    public TodoUpdatedQueue(QueueServiceClient queueServiceClient) : base (queueServiceClient, StaticQueueNames.TodoUpdated)
    {        
    }
}
