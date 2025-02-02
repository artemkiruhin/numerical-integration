namespace NumericalIntegration.Core
{
    public class NewtonCotesCalculator
    {
        private readonly Func<double, double> _function;

        public NewtonCotesCalculator(Func<double, double> function)
        {
            _function = function ?? throw new ArgumentNullException(nameof(function));
        }

        private double Function(double x) => _function(x);

        public double TrapezoidalRule(double a, double b)
        {
            var h = b - a;
            return (h / 2) * (Function(a) + Function(b));
        }

        public double SimpsonsRule(double a, double b)
        {
            var h = (b - a) / 6;
            var x0 = a;
            var x1 = (a + b) / 2;
            var x2 = b;
            return h * (Function(x0) + 4 * Function(x1) + Function(x2));
        }

        public static double TrapezoidalRule(Func<double, double> function, double a, double b)
        {
            var h = b - a;
            return (h / 2) * (function(a) + function(b));
        }

        public static double SimpsonsRule(Func<double, double> function, double a, double b)
        {
            var h = (b - a) / 6;
            var x0 = a;
            var x1 = (a + b) / 2;
            var x2 = b;
            return h * (function(x0) + 4 * function(x1) + function(x2));
        }

        public static double[] GaussMethod(double[,] matrix, double[] vector)
        {
            int n = vector.Length;
            double[,] augmentedMatrix = new double[n, n + 1];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                    augmentedMatrix[i, j] = matrix[i, j];
                augmentedMatrix[i, n] = vector[i];
            }

            for (int k = 0; k < n - 1; k++)
            {
                int maxRow = k;
                double maxVal = Math.Abs(augmentedMatrix[k, k]);

                for (int i = k + 1; i < n; i++)
                {
                    if (Math.Abs(augmentedMatrix[i, k]) > maxVal)
                    {
                        maxVal = Math.Abs(augmentedMatrix[i, k]);
                        maxRow = i;
                    }
                }

                if (maxRow != k)
                {
                    for (int j = k; j <= n; j++)
                    {
                        double temp = augmentedMatrix[k, j];
                        augmentedMatrix[k, j] = augmentedMatrix[maxRow, j];
                        augmentedMatrix[maxRow, j] = temp;
                    }
                }

                for (int i = k + 1; i < n; i++)
                {
                    double factor = augmentedMatrix[i, k] / augmentedMatrix[k, k];
                    for (int j = k; j <= n; j++)
                        augmentedMatrix[i, j] -= factor * augmentedMatrix[k, j];
                }
            }

            double[] solution = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = 0;
                for (int j = i + 1; j < n; j++)
                    sum += augmentedMatrix[i, j] * solution[j];
                solution[i] = (augmentedMatrix[i, n] - sum) / augmentedMatrix[i, i];
            }

            return solution;
        }
    }
}