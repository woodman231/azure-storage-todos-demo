using AzureStorageTodos.Models;

namespace AzureStorageTodos.Service.Interface;

public interface IUsersRepositoryClient : IAzureStorageRepositoryClient<UserEntityModel>
{
}