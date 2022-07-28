using AzureStorageTodos.Models;

namespace AzureStorageTodos.Repository.Interface;

public interface IUsersRepository : IAzureStorageRepository<UserEntityModel>
{
}