using AzureStorageTodos.Models;
using AzureStorageTodos.Repository;
using AzureStorageTodos.Service.Interface;

namespace AzureStorageTodos.Service.Services;

public class TodoListManager : ITodoListManager
{
    private readonly ITodosRepositoryClient _todosRepositoryClient;
    private readonly ITodoListRepositoryClient _todoListRepositoryClient;

    public TodoListManager(ITodosRepositoryClient todosRepositoryClient, ITodoListRepositoryClient todoListRepositoryClient)
    {
        _todosRepositoryClient = todosRepositoryClient;
        _todoListRepositoryClient = todoListRepositoryClient;
    }

    public async Task RefreshTodoListsForUserAsync(string userId)
    {
        var allTodosInRepository = await _todosRepositoryClient.GetAllAsync();

        if(allTodosInRepository is not null) 
        {
            var allTodosForUser = allTodosInRepository.Where(w => w.OwnerUserId == userId).ToList();

            var allTodosListEntity = new TodoListEntityModel(userId, StaticTodoListIds.All);
            var completedTodosListEntity = new TodoListEntityModel(userId, StaticTodoListIds.Complete);
            var incompleteTodosListEntity = new TodoListEntityModel(userId, StaticTodoListIds.Incomplete);

            if(allTodosForUser is not null)
            {
                allTodosListEntity.Results = allTodosForUser;
                completedTodosListEntity.Results = allTodosForUser.Where(w => w.Completed == true).ToList();
                incompleteTodosListEntity.Results = allTodosForUser.Where(w => w.Completed == false).ToList();

                await _todoListRepositoryClient.UpsertAsync(allTodosListEntity);
                await _todoListRepositoryClient.UpsertAsync(completedTodosListEntity);
                await _todoListRepositoryClient.UpsertAsync(incompleteTodosListEntity);
            }
        }        
    }
}