namespace NumericalIntegration.Core.Models;

public class IntegrationResult
{
    public double TrapezoidalResult { get; set; }
    public double SimpsonResult { get; set; }
    public double GaussianResult { get; set; }
    public double ExactValue { get; set; }
    public IntegrationErrors Errors { get; set; }
}