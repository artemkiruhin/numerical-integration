namespace NumericalIntegration.Core.Models.Results;

public class ErrorEstimationResult
{
    public double ActualError { get; set; }
    public double EstimatedError { get; set; }
    public double RelativeError { get; set; }
    public double TruncationError { get; set; }
    public double RoundingError { get; set; }
}