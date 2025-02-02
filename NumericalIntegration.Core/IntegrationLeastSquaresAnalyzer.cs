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

        public record LinearSystemTestCase(
            double[,] Matrix,
            double[] Vector,
            double[] ExactSolution,
            string Name
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

        public record LinearSystemResult(
            string TestName,
            double[] ExactSolution,
            double[] CalculatedSolution,
            double Error
        );

        private readonly List<TestCase> _testCases = new()
        {
            new TestCase(x => x * x, 1.0/3.0, "x^2", 0, 1),
            new TestCase(x => Math.Sin(x), 1 - Math.Cos(1), "sin(x)", 0, 1),
            new TestCase(x => Math.Exp(x), Math.E - 1, "e^x", 0, 1),
            new TestCase(x => 1 / x, Math.Log(2), "1/x", 1, 2)
        };

        private readonly List<LinearSystemTestCase> _linearSystemTests = new()
        {
            new LinearSystemTestCase(
                new double[,] { { 2, 1 }, { 1, 3 } },
                new double[] { 5, 6 },
                new double[] { 2, 1 },
                "Simple 2x2 System"
            ),
            new LinearSystemTestCase(
                new double[,] { { 1, 1, 1 }, { 0, 2, 5 }, { 2, 5, -1 } },
                new double[] { 6, -4, 27 },
                new double[] { 5, 3, -2 },
                "3x3 System"
            )
        };

        public record ComprehensiveAnalysisResult(
            AnalysisResult Trapezoidal,
            AnalysisResult Simpson,
            List<LinearSystemResult> GaussResults
        );

        public ComprehensiveAnalysisResult AnalyzeAllMethods()
        {
            var (trapResult, simpResult) = AnalyzeMethods();
            var gaussResults = AnalyzeGaussMethod();

            return new ComprehensiveAnalysisResult(
                trapResult,
                simpResult,
                gaussResults
            );
        }

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

        public List<LinearSystemResult> AnalyzeGaussMethod()
        {
            var results = new List<LinearSystemResult>();

            foreach (var test in _linearSystemTests)
            {
                var calculatedSolution = NewtonCotesCalculator.GaussMethod(test.Matrix, test.Vector);

                // Вычисление среднеквадратичной ошибки для решения
                double error = 0;
                for (int i = 0; i < test.ExactSolution.Length; i++)
                {
                    error += Math.Pow(calculatedSolution[i] - test.ExactSolution[i], 2);
                }
                error = Math.Sqrt(error / test.ExactSolution.Length);

                results.Add(new LinearSystemResult(
                    test.Name,
                    test.ExactSolution,
                    calculatedSolution,
                    error
                ));
            }

            return results;
        }

        public void AddTestCase(TestCase testCase)
        {
            _testCases.Add(testCase);
        }

        public void AddLinearSystemTestCase(LinearSystemTestCase testCase)
        {
            _linearSystemTests.Add(testCase);
        }

        public IReadOnlyList<TestCase> GetTestCases() => _testCases.AsReadOnly();

        public IReadOnlyList<LinearSystemTestCase> GetLinearSystemTestCases() => _linearSystemTests.AsReadOnly();
    }
}