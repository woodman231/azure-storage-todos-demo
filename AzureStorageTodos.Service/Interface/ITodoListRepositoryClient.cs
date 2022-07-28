using AzureStorageTodos.Models;

namespace AzureStorageTodos.Service.Interface;

public interface ITodoListRepositoryClient : IAzureStorageRepositoryClient<TodoListEntityModel>
{
}