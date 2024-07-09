using System;
using System.Threading;
using System.Windows.Forms;

namespace M6_Synchro
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Problem();
            Solution_1();
            //Solution_2();
            Console.ReadLine();
        }

        private static void Solution_2()
        {
            Form f1 = new Form();
            TextBox thisBelongsToMain = new TextBox();
            f1.Controls.Add(thisBelongsToMain);

            SynchronizationContext main = SynchronizationContext.Current;
            Thread t2 = new Thread(() =>
            {
                main.Send(v => thisBelongsToMain.Text = v.ToString(), "Hello");
            });
            t2.Start();

            f1.ShowDialog();
        }

        private static void Solution_1()
        {
            Form f1 = new Form();
            TextBox thisBelongsToMain = new TextBox();
            f1.Controls.Add(thisBelongsToMain);

            f1.Activated += (s, a) => {
                Thread t2 = new Thread(() =>
                {
                    // Wants to modify a field that doesn't belong to him;
                    //thisBelongsToMain.Invoke(new Action(() => { thisBelongsToMain.Text = "Hello"; }));
                    f1.Invoke(new Action(() => { thisBelongsToMain.Text = "Hello"; }));
                });
                t2.Start();
            };
            

            f1.ShowDialog();
        }

        static void Problem()
        {
            // Main thread owns this variable
            TextBox thisBelongsToMain = new TextBox();

            Thread t2 = new Thread(() =>
            {
                // Wants to modify a field that doesn't belong to him;
                thisBelongsToMain.Text = "Hello";
            });
            t2.Start();

            Console.WriteLine(thisBelongsToMain.Text);
        }
    }
}
