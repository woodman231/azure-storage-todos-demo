﻿using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using AzureStorageTodos.Web.Models;

namespace AzureStorageTodos.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly GraphServiceClient _graphServiceClient;

    public HomeController(ILogger<HomeController> logger,
                        GraphServiceClient graphServiceClient)
    {
            _logger = logger;
        _graphServiceClient = graphServiceClient;
    }

    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public async Task<IActionResult> Index()
    {
        var user = await _graphServiceClient.Me.Request().GetAsync();
        ViewData["ApiResult"] = user.DisplayName;

        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
