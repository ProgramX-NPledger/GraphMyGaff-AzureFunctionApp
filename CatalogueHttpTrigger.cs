using System.Net;
using GraphMyGaff.Azure.FunctionApp.Core.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
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
    public async Task<IActionResult> GetCatalogue([HttpTrigger(AuthorizationLevel.Function, "get", Route = "catalogue/{id:guid?}")] HttpRequest req, Guid? id=null)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        var name = "anon";

        var stringValues = req.Query["name"];
        if (stringValues.Count > 0)
        {
            name = stringValues[0];
        }
        
        // if not found, return a 404
        // return req.CreateResponse(HttpStatusCode.NotFound)
        return new OkObjectResult($"Welcome {name} {id ?? null}");
    }

    // public record NewCatalogue(
    //     string id,
    //     string name,
    //     string type);

    public class CreateCatalogueResponse
    {
        [CosmosDBOutput("graphmygaff","volatile",Connection = "CosmosDbConnection")]
        public Catalogue? NewCatalogue { get; set; }

        public HttpResponseData? HttpResponse { get; set; }
    }
    
    [Function(nameof(CatalogueHttpTrigger.CreateCatalogue))]
    public async Task<CreateCatalogueResponse> CreateCatalogue([HttpTrigger(AuthorizationLevel.Function, "post", Route = "catalogue")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request to create a catalogue.");

        // Here you would typically read the request body and create a new catalogue item.
        // For simplicity, we are returning a static response.
        var newCatalogue = await req.ReadFromJsonAsync<Catalogue>();
        if (newCatalogue == null)
        {
            var createCatalogueResponse = new CreateCatalogueResponse()
            {
                HttpResponse = req.CreateResponse(HttpStatusCode.BadRequest),
                NewCatalogue = null
            };
            createCatalogueResponse.HttpResponse.WriteString("Invalid request body");
            return createCatalogueResponse;
        }

        var httpResponse = req.CreateResponse(HttpStatusCode.Created);

        return new CreateCatalogueResponse
        {
            HttpResponse = httpResponse,
            NewCatalogue = newCatalogue
        };
        
        //return new CreatedResult($"/catalogue/{newCatalogue.id}", newCatalogue);
    }
}