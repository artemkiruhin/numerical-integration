namespace NumericalIntegration.Core.Models;

public class IntegrationRequest
{
    public string Function { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public int N { get; set; } = 100;
}