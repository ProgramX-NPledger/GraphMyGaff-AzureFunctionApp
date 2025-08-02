namespace GraphMyGaff.Azure.FunctionApp.Core.Model;

public class Catalogue
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string name { get; set; }
    public string type { get; set; } = nameof(Catalogue).ToLowerInvariant();
}