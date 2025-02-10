namespace NumericalIntegration.Core.Models.Results;

public class SegmentResult
{
    public int Segments { get; set; }
    public Dictionary<string, MethodResult> MethodResults { get; set; }
}