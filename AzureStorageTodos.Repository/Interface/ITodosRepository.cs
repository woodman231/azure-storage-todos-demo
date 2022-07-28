using AzureStorageTodos.Models;

namespace AzureStorageTodos.Repository.Interface;

public interface ITodosRepository : IAzureStorageRepository<TodoEntityModel>
{
}