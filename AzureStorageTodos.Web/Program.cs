using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Graph = Microsoft.Graph;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using AzureStorageTodos.Repository.Interface;
using AzureStorageTodos.Repository.BaseRepositories;
using AzureStorageTodos.Repository.Repositories;
using AzureStorageTodos.Service.Interface;
using AzureStorageTodos.Service.RepositoryClients;
using AzureStorageTodos.Service.Services;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

var builder = WebApplication.CreateBuilder(args);

string? appConfigConnectionString = Environment.GetEnvironmentVariable("APPCONFIG_CONNECTION_STRING");
string? aspNetCoreEnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Host.ConfigureAppConfiguration(configurationBuilder => {
    if(appConfigConnectionString is not null && aspNetCoreEnvironmentName is not null) 
    {
        configurationBuilder.AddAzureAppConfiguration(azureAppConfigurationOptions => {
            azureAppConfigurationOptions.Connect(appConfigConnectionString)
            .Select(KeyFilter.Any, LabelFilter.Null)
            .Select(KeyFilter.Any, aspNetCoreEnvironmentName)
            .ConfigureKeyVault(kv =>
            {
                kv.SetCredential(new DefaultAzureCredential());
            });
        });
    }
});

// Add services to the container.
var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ');

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(options => {
        options.Instance = builder.Configuration["AzureAd:Instance"];
        options.Domain = builder.Configuration["AzureAd:Domain"];
        options.TenantId = builder.Configuration["AzureAd:TenantId"];
        options.ClientId = builder.Configuration["AzureAd:ClientId"];
        options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"];
        options.CallbackPath = builder.Configuration["AzureAd:CallbackPath"];

        options.Events = new WebAppOpenIdConnectEvents();
    })
    .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddMicrosoftGraph(builder.Configuration.GetSection("DownstreamApi"))
        .AddInMemoryTokenCaches();

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});
builder.Services.AddRazorPages()
    .AddMicrosoftIdentityUI();

// Register Azure Clients
builder.Services.AddAzureClients(azureClientsBuilder => {
    string azureStorageConnectionString = builder.Configuration.GetConnectionString("AzureStorage");    

    azureClientsBuilder.AddTableServiceClient(azureStorageConnectionString);
    azureClientsBuilder.AddBlobServiceClient(azureStorageConnectionString);
    azureClientsBuilder.AddQueueServiceClient(azureStorageConnectionString).ConfigureOptions(queueOptions => {
        queueOptions.MessageEncoding = Azure.Storage.Queues.QueueMessageEncoding.Base64;
    });    
    
    azureClientsBuilder.UseCredential(new DefaultAzureCredential());
});

// Register Repositories
builder.Services.AddTransient<IUsersRepository, UsersRepository>();
builder.Services.AddTransient<ITodosRepository, TodosRepository>();
builder.Services.AddTransient<ITodoListRepository, TodoListRepository>();
builder.Services.AddTransient<ITodoUpdatedQueue, TodoUpdatedQueue>();

// Register Services
builder.Services.AddTransient<IUsersRepositoryClient, UsersRepositoryClient>();
builder.Services.AddTransient<ITodosRepositoryClient, TodosRepositoryClient>();
builder.Services.AddTransient<ITodoListRepositoryClient, TodoListRepositoryClient>();
builder.Services.AddTransient<ITodoListManager, TodoListManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
