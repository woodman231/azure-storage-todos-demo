using AzureStorageTodos.Models;
using AzureStorageTodos.Service.Interface;
using AzureStorageTodos.Service.BaseRepositoryClients;
using AzureStorageTodos.Repository.Interface;

namespace AzureStorageTodos.Service.RepositoryClients;

public class TodosRepositoryClient : AzureStorageRepsoitoryClient<TodoEntityModel>, ITodosRepositoryClient
{
    private readonly ITodosRepository _todosRepository;
    private readonly ITodoUpdatedQueue _todoUpdatedQueue;

    public TodosRepositoryClient(ITodosRepository todosRepository, ITodoUpdatedQueue todoUpdatedQueue) : base(todosRepository)
    {
        _todosRepository = todosRepository;
        _todoUpdatedQueue = todoUpdatedQueue;
    }

    public override async Task<TodoEntityModel?> UpsertAsync(TodoEntityModel entityDetails)
    {
        var results = await base.UpsertAsync(entityDetails);

        await _todoUpdatedQueue.SendMessageAsync($"Todo Updated For User: {entityDetails.OwnerUserId}");

        return results;
    }

    public override async Task DeleteAsync(string id)
    {
        var currentDetails = await this.GetOneAsync(id);

        if(currentDetails is not null) 
        {
            await base.DeleteAsync(id);

            await _todoUpdatedQueue.SendMessageAsync($"Todo Updated For User: {currentDetails.OwnerUserId}");
        }        
    }
}