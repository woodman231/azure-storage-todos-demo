using AzureStorageTodos.Models;
using AzureStorageTodos.Service.Interface;
using AzureStorageTodos.Service.BaseRepositoryClients;
using AzureStorageTodos.Repository.Interface;

namespace AzureStorageTodos.Service.RepositoryClients;

public class TodoListRepositoryClient : AzureStorageRepsoitoryClient<TodoListEntityModel>, ITodoListRepositoryClient
{
    public TodoListRepositoryClient(ITodoListRepository todoListRepository) : base(todoListRepository)
    {
    }
}