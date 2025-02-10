namespace NumericalIntegration.Core.Models.Results;

public class ComparisonResult
{
    public Dictionary<string, MethodResult> Results { get; set; }
    public Dictionary<string, double> Errors { get; set; }
    public string BestMethod { get; set; }
    public Dictionary<string, double> LeastSquaresAnalysis { get; set; }
}