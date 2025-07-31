using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphMyGaff.Azure.FunctionApp.Core.Catalogues;

public class GetCataloguesHttpTrigger
{
    private readonly ILogger<GetCataloguesHttpTrigger> _logger;

    public GetCataloguesHttpTrigger(ILogger<GetCataloguesHttpTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(GetCataloguesHttpTrigger))]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var name = "anon";
        
        var stringValues = req.Query["name"];
        if (stringValues.Count > 0)
        {
            name = stringValues[0];
        }
        return new OkObjectResult($"Welcome {name}");
    }
}