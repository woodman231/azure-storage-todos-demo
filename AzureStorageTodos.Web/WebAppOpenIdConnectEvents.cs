using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using AzureStorageTodos.Models;
using AzureStorageTodos.Service.Interface;

public class WebAppOpenIdConnectEvents : OpenIdConnectEvents
{
    public WebAppOpenIdConnectEvents()
    {
        this.OnTicketReceived = async ctx => {
            if(ctx.Principal is not null) 
            {
                var myPrincipal = ctx.Principal;

                var uidClaim = myPrincipal.Claims.FirstOrDefault(w => w.Type == "uid");
                var nameClaim = myPrincipal.Claims.FirstOrDefault(w => w.Type == "name");
                var emailClaim = myPrincipal.Claims.FirstOrDefault(w => w.Type == "preferred_username");

                if (uidClaim is not null && nameClaim is not null && emailClaim is not null)
                {
                    var usersRepositoryClient = ctx.HttpContext.RequestServices.GetRequiredService<IUsersRepositoryClient>();

                    var userEntity = new UserEntityModel
                    {
                        Id = uidClaim.Value,
                        DisplayName = nameClaim.Value,
                        EmailAddress = emailClaim.Value
                    };

                    await usersRepositoryClient.UpsertAsync(userEntity);
                }
            }

            await Task.Yield();
        };     
    }
}