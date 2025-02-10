namespace NumericalIntegration.Core.Models.Requests;

public class CalculationRequest
{
    public string Function { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public string Method { get; set; }
    public int? Segments { get; set; }
}