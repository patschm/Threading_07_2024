using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Form1 : Form
    {
        private readonly SynchronizationContext _main;
        private readonly BackgroundWorker _worker;

        public Form1()
        {
            InitializeComponent();
            _main = SynchronizationContext.Current;
            _worker = new BackgroundWorker { WorkerReportsProgress = true };
        }

        private async void btbPlus_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b))
            {
                //int result = await LongAddAsync(a, b);
                //UpdateAnswer(result);
                var t1 = DoeIets(a, b);//.ConfigureAwait(false);
                int result = t1.Result;
                UpdateAnswer(result);

                //Task.Run(() => LongAdd(a, b))
                //    .ContinueWith(pt => _main.Post(UpdateAnswer, pt.Result));

                //Task.Run(() => LongAdd(a, b))
                //    .ContinueWith(pt => _main.Post(UpdateAnswer, pt.Result + 1));

                //Func<int, int, int> fn = LongAdd;

                //fn.BeginInvoke(a, b, ar => { 
                //    var fsub = ar.AsyncState as Func<int, int, int>;    
                //    int result = fsub.EndInvoke(ar);
                //    _main.Send(UpdateAnswer, result);
                //}, fn);

                //_worker.DoWork += (o, arg) =>
                //{
                //    (int aa, int bb) = (ValueTuple<int, int>)arg.Argument;
                //    arg.Result = LongAdd(aa, bb);
                //};
                //_worker.RunWorkerCompleted += (s, arg) =>
                //{
                //    UpdateAnswer(arg.Result);
                //};
                //_worker.ProgressChanged += (s, arg) =>
                //{
                //    UpdateProgress(arg.ProgressPercentage);
                //};
                //_worker.RunWorkerAsync((a, b));

                //ThreadPool.QueueUserWorkItem(_ => {                 
                //    int result = LongAdd(a, b);
                //    _main.Send(UpdateAnswer, result);
                //});

                //ThreadPool.QueueUserWorkItem(args => {
                //    (int aa, int bb) = (ValueTuple<int, int>)args;
                //    int result = LongAdd(aa, bb);
                //    _main.Send(UpdateAnswer, result);
                //}, (a, b));

                //int result = LongAdd(a,b);
                //UpdateAnswer(result);
                //new Thread(() => {
                //    int result = LongAdd(a,b);
                //    //_main.Post(UpdateAnswer, result);
                //    //this.Invoke((Action<object>)UpdateAnswer, result);

                //    //UpdateAnswer(result);
                //}).Start();
            }
        }

        private  Task<int> DoeIets(int a, int b)
        {
            return  LongAddAsync(a, b);
        }

        private void UpdateAnswer(object result)
        {
            lblAnswer.Text = result.ToString();
        }
        private void UpdateProgress(object percentage)
        {
            pgbCalc.Value = (int)percentage;
        }

        private int LongAdd(int a, int b)
        {
            for (int i = 1; i <= 50; i++)
            {
                Thread.Sleep(100);
                // _worker.ReportProgress(2 * i);
                _main.Post(UpdateProgress, i * 2);
            }
            return a + b;
        }
        private Task<int> LongAddAsync(int a, int b)
        {
            return Task.Run(() => LongAdd(a, b));
        }
    }
}
