using Azure.Data.Tables;
using AzureStorageTodos.Models;
using AzureStorageTodos.Repository.BaseRepositories;
using AzureStorageTodos.Repository.Interface;

namespace AzureStorageTodos.Repository.Repositories;

public class TodosRepository : AzureTableStorageRepository<TodoEntityModel>, ITodosRepository
{
    public TodosRepository(TableServiceClient tableServiceClient) : base(tableServiceClient, StaticTableNames.Todos, StaticTableRowKeys.TodoItems)
    {        
    }
}