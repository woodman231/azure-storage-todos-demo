using AzureStorageTodos.Models.Interface;
using AzureStorageTodos.Repository.Interface;
using AzureStorageTodos.Service.Interface;

namespace AzureStorageTodos.Service.BaseRepositoryClients;

public abstract class AzureStorageRepsoitoryClient<T> : IAzureStorageRepositoryClient<T> where T : IBaseAzureStorageEntityModel
{
    private readonly IAzureStorageRepository<T> _repository;

    public AzureStorageRepsoitoryClient(IAzureStorageRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual Task DeleteAsync(string id)
    {
        try {
            return _repository.DeleteAsync(id);
        }
        catch {
            // Do nothing
        }
        
        return Task.CompletedTask;
    }

    public virtual async Task<List<T>?> GetAllAsync()
    {
        try {
            return await _repository.GetAllAsync();
        }
        catch {
            // Do nothing, return null
        }

        return null;
    }

    public virtual async Task<T?> GetOneAsync(string id)
    {
        try {
            return await _repository.GetOneAsync(id);
        }
        catch {
            // Do nothing, return null
        }

        return default(T);
    }

    public virtual async Task<T?> UpsertAsync(T entityDetails)
    {
        try {
            var results = await _repository.UpsertAsync(entityDetails);

            return results;
        }
        catch (Exception ex) {
            // Do nothing, return null
            Console.WriteLine(ex.Message);
        }

        return default(T);
    }
}