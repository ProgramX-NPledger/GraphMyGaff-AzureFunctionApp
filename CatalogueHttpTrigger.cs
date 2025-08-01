using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GraphMyGaff.Azure.FunctionApp.Core;

public class CatalogueHttpTrigger
{
    private readonly ILogger<CatalogueHttpTrigger> _logger;

    public CatalogueHttpTrigger(ILogger<CatalogueHttpTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(CatalogueHttpTrigger.GetCatalogue))]
    public async Task<IActionResult> GetCatalogue([HttpTrigger(AuthorizationLevel.Function, "get", Route = "catalogue")] HttpRequest req)
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

    record NewCatalogue(
        string id,
        string name);

    [Function(nameof(CatalogueHttpTrigger.CreateCatalogue))]
    public async Task<IActionResult> CreateCatalogue([HttpTrigger(AuthorizationLevel.Function, "post", Route = "catalogue")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request to create a catalogue.");

        // Here you would typically read the request body and create a new catalogue item.
        // For simplicity, we are returning a static response.
        var newCatalogue = await req.ReadFromJsonAsync<NewCatalogue>();
        if (newCatalogue == null)
        {
            return new BadRequestObjectResult("Invalid catalogue data.");
        }

        return new CreatedResult($"/catalogue/{newCatalogue.id}", newCatalogue);
    }
}