namespace NumericalIntegration.Core.Models.Results;

public class BenchmarkResult
{
    public List<SegmentResult> Results { get; set; }
    public string BestPerformingMethod { get; set; }
    public Dictionary<string, double> AverageErrors { get; set; }
}