using Azure.Data.Tables;
using AzureStorageTodos.Models;
using AzureStorageTodos.Repository.BaseRepositories;
using AzureStorageTodos.Repository.Interface;

namespace AzureStorageTodos.Repository.Repositories;

public class UsersRepository : AzureTableStorageRepository<UserEntityModel>, IUsersRepository
{
    public UsersRepository(TableServiceClient tableServiceClient) : base(tableServiceClient, StaticTableNames.TodoUsers, StaticTableRowKeys.UserItems)
    {        
    }
}