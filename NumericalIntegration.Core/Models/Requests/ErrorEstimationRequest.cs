namespace NumericalIntegration.Core.Models.Requests;

public class ErrorEstimationRequest
{
    public string Function { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public string Method { get; set; }
    public int Segments { get; set; }
}