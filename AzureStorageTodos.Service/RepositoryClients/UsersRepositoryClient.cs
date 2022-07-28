using AzureStorageTodos.Models;
using AzureStorageTodos.Service.Interface;
using AzureStorageTodos.Service.BaseRepositoryClients;
using AzureStorageTodos.Repository.Interface;

namespace AzureStorageTodos.Service.RepositoryClients;

public class UsersRepositoryClient : AzureStorageRepsoitoryClient<UserEntityModel>, IUsersRepositoryClient
{
    public UsersRepositoryClient(IUsersRepository usersRepository) : base(usersRepository)
    {        
    }
}