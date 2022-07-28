using AzureStorageTodos.Models;

namespace AzureStorageTodos.Web.Models;

public class TodosHomePageViewModel
{
    public IEnumerable<TodoEntityModel>? AllTodos {get; set;}
    public IEnumerable<TodoEntityModel>? CompletedTodos {get; set;}
    public IEnumerable<TodoEntityModel>? IncompleteTodos {get; set;}
}