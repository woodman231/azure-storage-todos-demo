using AzureStorageTodos.Models;
using AzureStorageTodos.Repository;
using AzureStorageTodos.Service.Interface;
using AzureStorageTodos.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzureStorageTodos.Web.Controllers;

[Authorize]
public class TodosController : Controller
{
    private readonly ITodoListRepositoryClient _todoListRepositoryClient;
    private readonly ITodosRepositoryClient _todosRepositoryClient;

    public TodosController(ITodoListRepositoryClient todoListRepositoryClient, ITodosRepositoryClient todosRepositoryClient)
    {
        _todoListRepositoryClient = todoListRepositoryClient;
        _todosRepositoryClient = todosRepositoryClient;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        TodosHomePageViewModel todosHomePageViewModel = new TodosHomePageViewModel();

        
        var currentUser = this.HttpContext.User;

        var currentUserUserIdClaim = currentUser.Claims.Where(w => w.Type == "uid").FirstOrDefault();

        if(currentUserUserIdClaim is not null) 
        {
            var currentUserUserIdValue = currentUserUserIdClaim.Value;

            var allFileId = $"{currentUserUserIdValue}-{StaticTodoListIds.All}";
            var completedFileId = $"{currentUserUserIdValue}-{StaticTodoListIds.Complete}";
            var incompleteFileId = $"{currentUserUserIdValue}-{StaticTodoListIds.Incomplete}";

            var allTodos = await _todoListRepositoryClient.GetOneAsync(allFileId);
            var completedTods = await _todoListRepositoryClient.GetOneAsync(completedFileId);
            var incompleteTodos = await _todoListRepositoryClient.GetOneAsync(incompleteFileId);

            if(allTodos is not null) 
            {
                todosHomePageViewModel.AllTodos = allTodos.Results;                
            }

            if(completedTods is not null) 
            {
                todosHomePageViewModel.CompletedTodos = completedTods.Results;
            }

            if(incompleteTodos is not null) 
            {
                todosHomePageViewModel.IncompleteTodos = incompleteTodos.Results;
            }
        }        

        return View(todosHomePageViewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var currentDetails = await _todosRepositoryClient.GetOneAsync(id);

        if(currentDetails is null)
        {
            return NotFound();
        }

        return View(currentDetails);
    }

    [HttpGet]
    public IActionResult Create()
    {
        TodoEntityModel newTodoEntityModel = new TodoEntityModel();

        var currentUser = this.HttpContext.User;

        var currentUserUserIdClaim = currentUser.Claims.Where(w => w.Type == "uid").FirstOrDefault();

        if(currentUserUserIdClaim is not null) 
        {
            var currentUserUserIdValue = currentUserUserIdClaim.Value;

            newTodoEntityModel.OwnerUserId = currentUserUserIdValue;            
        }

        return View(newTodoEntityModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TodoEntityModel todoEntityModel)
    {
        if(ModelState.IsValid)
        {
            await _todosRepositoryClient.UpsertAsync(todoEntityModel);

            return RedirectToAction(nameof(Index));
        }

        return View(todoEntityModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit([FromRoute] string id)
    {
        var currentDetails = await _todosRepositoryClient.GetOneAsync(id);

        if(currentDetails is null)
        {
            return NotFound();            
        }

        return View(currentDetails);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromForm] TodoEntityModel todoEntityModel)
    {
        if(ModelState.IsValid)
        {
            await _todosRepositoryClient.UpsertAsync(todoEntityModel);

            return RedirectToAction(nameof(Index));
        }

        return View(todoEntityModel);
    }

    [HttpGet]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var currentDetails = await _todosRepositoryClient.GetOneAsync(id);

        if(currentDetails is null)
        {
            return NotFound();            
        }

        return View(currentDetails);                
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromForm] TodoEntityModel todoEntityModel)
    {
        await _todosRepositoryClient.DeleteAsync(todoEntityModel.Id);

        return RedirectToAction(nameof(Index));
    }
}