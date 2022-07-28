namespace AzureStorageTodos.Models;

public class TodoListEntityModel : BaseAzureStorageEntityModel
{
    public TodoListEntityModel()
    {
        this.OwnerUserId = "TBD";
        this.Results = new List<TodoEntityModel>();
    }

    public TodoListEntityModel(string ownerUserId, string listType)
    {
        this.Id = $"{ownerUserId}-{listType}";
        this.OwnerUserId = ownerUserId;
        this.Results = new List<TodoEntityModel>();
    }

    public string OwnerUserId {get; set;} = null!;
    public IEnumerable<TodoEntityModel> Results {get; set;} = null!;
}