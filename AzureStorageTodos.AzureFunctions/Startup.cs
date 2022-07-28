using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using AzureStorageTodos.Repository.Interface;
using AzureStorageTodos.Repository.Repositories;
using AzureStorageTodos.Service.Interface;
using AzureStorageTodos.Service.RepositoryClients;
using AzureStorageTodos.Service.Services;
using System;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

[assembly: FunctionsStartup(typeof(AzureStorageTodos.AzureFunctions.Startup))]

namespace AzureStorageTodos.AzureFunctions;

public class Startup : FunctionsStartup
{
    public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
    {
        string appConfigConnectionString = Environment.GetEnvironmentVariable("APPCONFIG_CONNECTION_STRING");
        string aspNetCoreEnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        builder.ConfigurationBuilder.AddAzureAppConfiguration(azureAppConfigurationBuilder => {
            azureAppConfigurationBuilder.Connect(appConfigConnectionString)
            .Select(KeyFilter.Any, LabelFilter.Null)
            .Select(KeyFilter.Any, aspNetCoreEnvironmentName)
            .ConfigureKeyVault(kv =>
            {
                kv.SetCredential(new DefaultAzureCredential());
            });           
        });
                
        base.ConfigureAppConfiguration(builder);
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = builder.GetContext().Configuration;

        string azureWebJobsStorage = config.GetValue<string>("AzureWebJobsStorage");

        // Register Azure Clients
        builder.Services.AddAzureClients(azureClientsBuilder => {
            azureClientsBuilder.AddTableServiceClient(azureWebJobsStorage);
            azureClientsBuilder.AddBlobServiceClient(azureWebJobsStorage);
            
            azureClientsBuilder.UseCredential(new DefaultAzureCredential());
        });

        // Register Repositories
        builder.Services.AddTransient<ITodosRepository, TodosRepository>();
        builder.Services.AddTransient<ITodoListRepository, TodoListRepository>();

        // Register Services
        builder.Services.AddTransient<ITodosRepositoryClient, TodosRepositoryClient>();
        builder.Services.AddTransient<ITodoListRepositoryClient, TodoListRepositoryClient>();
        builder.Services.AddTransient<ITodoListManager, TodoListManager>();
    }
}