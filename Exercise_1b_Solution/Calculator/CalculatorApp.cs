using System.ComponentModel;

namespace Calculator;

public partial class CalculatorApp : Form
{
    private BackgroundWorker worker = new BackgroundWorker();
    public CalculatorApp()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        // TODO: Solve the freeze by use the backgroundworker.
        if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b))
        {
            worker.WorkerReportsProgress = true;
            worker.DoWork += (s, arg) => {
                var sender = s as BackgroundWorker;
                int result = LongAdd(a, b, (ss, aa) => {
                    sender?.ReportProgress(aa.ProgressPercentage);
                });
                arg.Result = result;
            };
            worker.ProgressChanged += (a, arg)=>UpdateProgress(arg.ProgressPercentage);
            worker.RunWorkerCompleted += (s, arg) => UpdateAnswer(arg.Result);
           

            worker.RunWorkerAsync();
        }
    }

    private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void UpdateAnswer(object? result)
    {
        lblAnswer.Text = result?.ToString();
    }
    private void UpdateProgress(object? result)
    {
        pgBar.Value = (int)result!;
    }

    private int LongAdd(int a, int b, ProgressChangedEventHandler? progress = default)
    {
       for(int i = 1; i <= 100; i++)
        {
            Thread.Sleep(100);
            if (progress != null) progress(this, new ProgressChangedEventArgs(i, i));
        }
        return a + b;
    }
}