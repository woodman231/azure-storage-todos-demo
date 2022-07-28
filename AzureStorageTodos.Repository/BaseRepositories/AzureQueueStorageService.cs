using Azure.Storage.Queues;
using AzureStorageTodos.Repository.Interface;

namespace AzureStorageTodos.Repository.BaseRepositories;

public abstract class AzureQueueStorageService : IAzureQueueStorageService
{
    private readonly QueueServiceClient _queueServiceClient;
    private readonly string _queueName;

    public AzureQueueStorageService(QueueServiceClient queueServiceClient, string queueName)
    {
        _queueServiceClient = queueServiceClient;
        _queueName = queueName;
    }

    public async Task SendMessageAsync(string message)
    {
        var queueClient = await GetQueueClientAsync();        
        await queueClient.SendMessageAsync(message);
    }

    private async Task<QueueClient> GetQueueClientAsync()
    {
        var queueClient = _queueServiceClient.GetQueueClient(_queueName);        
        await queueClient.CreateIfNotExistsAsync();

        return queueClient;                
    }
}