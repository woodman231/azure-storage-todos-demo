namespace AzureStorageTodos.Service.Interface;

public interface ITodoListManager
{
    public Task RefreshTodoListsForUserAsync(string userId);
}