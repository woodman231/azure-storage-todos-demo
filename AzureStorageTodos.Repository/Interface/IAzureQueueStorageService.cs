namespace AzureStorageTodos.Repository.Interface;

public interface IAzureQueueStorageService
{
    public Task SendMessageAsync(string message);
}