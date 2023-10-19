namespace Scrapify.ConsoleApp;

public class ProgressLogger
{
    private readonly ProgressBar _progressBar;
    public ProgressLogger()
    {
        _progressBar = new ProgressBar();
    }
    public void OnProgressReported(object sender, double progress)
    {
        _progressBar.Report(progress);
    }
}
