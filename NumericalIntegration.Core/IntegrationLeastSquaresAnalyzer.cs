namespace NumericalIntegration.Core
{
    public class IntegrationLeastSquaresAnalyzer
    {
        public record TestCase(
            Func<double, double> Function,
            double ExactValue,
            string Name,
            double Start,
            double End
        );

        public record AnalysisResult(
            string MethodName,
            double MSE,
            IReadOnlyList<TestCaseResult> TestResults
        );

        public record TestCaseResult(
            string FunctionName,
            double ExactValue,
            double CalculatedValue,
            double SquaredError
        );

        private readonly List<TestCase> _testCases = new()
        {
            new TestCase(x => x * x, 1.0/3.0, "x^2", 0, 1),
            new TestCase(x => Math.Sin(x), 1 - Math.Cos(1), "sin(x)", 0, 1),
            new TestCase(x => Math.Exp(x), Math.E - 1, "e^x", 0, 1),
            new TestCase(x => 1 / x, Math.Log(2), "1/x", 1, 2)
        };

        public (AnalysisResult Trapezoidal, AnalysisResult Simpson) AnalyzeMethods()
        {
            var calculator = new NewtonCotesCalculator(x => x);
            
            var trapResults = new List<TestCaseResult>();
            var simpResults = new List<TestCaseResult>();
            double trapMSE = 0;
            double simpMSE = 0;

            foreach (var test in _testCases)
            {
                var trapValue = NewtonCotesCalculator.TrapezoidalRule(test.Function, test.Start, test.End);
                var simpValue = NewtonCotesCalculator.SimpsonsRule(test.Function, test.Start, test.End);

                var trapError = Math.Pow(trapValue - test.ExactValue, 2);
                var simpError = Math.Pow(simpValue - test.ExactValue, 2);

                trapResults.Add(new TestCaseResult(test.Name, test.ExactValue, trapValue, trapError));
                simpResults.Add(new TestCaseResult(test.Name, test.ExactValue, simpValue, simpError));

                trapMSE += trapError;
                simpMSE += simpError;
            }

            trapMSE /= _testCases.Count;
            simpMSE /= _testCases.Count;

            return (
                new AnalysisResult("Метод трапеций", trapMSE, trapResults),
                new AnalysisResult("Метод Симпсона", simpMSE, simpResults)
            );
        }

        public void AddTestCase(TestCase testCase)
        {
            _testCases.Add(testCase);
        }

        public IReadOnlyList<TestCase> GetTestCases() => _testCases.AsReadOnly();
    }
}