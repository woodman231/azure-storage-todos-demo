using Azure.Storage.Blobs;
using AzureStorageTodos.Models;
using AzureStorageTodos.Repository.BaseRepositories;
using AzureStorageTodos.Repository.Interface;

namespace AzureStorageTodos.Repository.Repositories;

public class TodoListRepository : AzureBlobStorageRepository<TodoListEntityModel>, ITodoListRepository
{
    public TodoListRepository(BlobServiceClient blobServiceClient) : base(blobServiceClient, StaticBlobContainerNames.TodoLists, StaticBlobFolderNames.TodoListItems)
    {        
    }
}