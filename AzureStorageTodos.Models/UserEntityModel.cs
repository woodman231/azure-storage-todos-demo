namespace AzureStorageTodos.Models;

public class UserEntityModel : BaseAzureStorageEntityModel
{    
    public string DisplayName {get; set;} = null!;
    public string EmailAddress {get; set;} = null!;        
}