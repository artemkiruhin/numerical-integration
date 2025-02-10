namespace NumericalIntegration.Core.Models.Requests;

public class BenchmarkRequest
{
    public string Function { get; set; }
    public double A { get; set; }
    public double B { get; set; }
    public int[] SegmentsArray { get; set; }
}