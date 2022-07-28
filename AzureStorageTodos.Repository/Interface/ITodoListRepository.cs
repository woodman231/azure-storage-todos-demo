using AzureStorageTodos.Models;

namespace AzureStorageTodos.Repository.Interface;

public interface ITodoListRepository : IAzureStorageRepository<TodoListEntityModel>
{    
}