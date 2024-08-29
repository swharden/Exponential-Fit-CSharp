// start with original data points
double[] xs = [1, 2, 3, 4, 5, 6, 7];
double[] ys = [258, 183, 127, 89, 65, 48, 35];

// perform a linear fit on log-transformed Ys
double[] logYs = ys.Select(x => Math.Log(x)).ToArray();
(double slope, double intercept) = LeastSquaresFit(xs, logYs);

// convert coefficients back to exponential form
double a = Math.Exp(intercept);
Console.WriteLine($"y = {a}*e^({slope}*x)");

// plot the original data and the fitted curve
ScottPlot.Plot plot = new();
var dataMarkers = plot.Add.ScatterPoints(xs, ys);
dataMarkers.MarkerSize = 10;
dataMarkers.LegendText = "Raw Data";

// generate and plot the fitted curve
double[] fitXs = Enumerable.Range(0, 100).Select(x => x * .1).ToArray();
double[] fitYs = fitXs.Select(x => a * Math.Exp(slope * x)).ToArray();
var fitLine = plot.Add.ScatterLine(fitXs, fitYs);
fitLine.LineWidth = 2;
fitLine.LinePattern = ScottPlot.LinePattern.DenselyDashed;
fitLine.LegendText = "Fitted Curve";

// decorate the plot and save it
plot.Legend.Alignment = ScottPlot.Alignment.UpperRight;
plot.Title($"y = {a:N2}*e^({slope:N2}*x)");
plot.SavePng("fitSimple.png", 400, 300).LaunchInBrowser();

static (double slope, double intercept) LeastSquaresFit(double[] xs, double[] ys)
{
    double sumX = 0, sumY = 0, sumX2 = 0, sumXY = 0;

    for (int i = 0; i < xs.Length; i++)
    {
        sumX += xs[i];
        sumY += ys[i];
        sumX2 += xs[i] * xs[i];
        sumXY += xs[i] * ys[i];
    }

    double avgX = sumX / xs.Length;
    double avgY = sumY / xs.Length;
    double avgX2 = sumX2 / xs.Length;
    double avgXY = sumXY / xs.Length;
    double slope = (avgXY - avgX * avgY) / (avgX2 - avgX * avgX);
    double intercept = (avgX2 * avgY - avgXY * avgX) / (avgX2 - avgX * avgX);
    return (slope, intercept);
}

