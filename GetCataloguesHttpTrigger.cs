using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphMyGaff.Azure.FunctionApp.Core;

public class GetCataloguesHttpTrigger
{
    private readonly ILogger<GetCataloguesHttpTrigger> _logger;

    public GetCataloguesHttpTrigger(ILogger<GetCataloguesHttpTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(GetCataloguesHttpTrigger))]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}