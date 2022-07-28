using AzureStorageTodos.Models;

namespace AzureStorageTodos.Service.Interface;

public interface ITodosRepositoryClient : IAzureStorageRepositoryClient<TodoEntityModel>
{    
}