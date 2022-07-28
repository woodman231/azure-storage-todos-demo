using AzureStorageTodos.Models.Interface;

namespace AzureStorageTodos.Service.Interface;

public interface IAzureStorageRepositoryClient<T> where T : IBaseAzureStorageEntityModel
{
    Task<T?> UpsertAsync(T entityDetails);
    Task<T?> GetOneAsync(string id);
    Task<List<T>?> GetAllAsync();
    Task DeleteAsync(string id);
}