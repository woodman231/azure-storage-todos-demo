namespace AzureStorageTodos.Models;

public class TodoEntityModel : BaseAzureStorageEntityModel
{
    public TodoEntityModel()
    {
        OwnerUserId = "TBD";
    }

    public TodoEntityModel(string ownerUserId)
    {
        this.OwnerUserId = ownerUserId;        
    }

    public string OwnerUserId {get; set;} = null!;
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool Completed { get; set; } = false;    
}