using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _6b_Synchro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private  void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b) )
            {
                Task.Run(() => LongAdd(a, b))
                    .ContinueWith(t =>
                    {
                        txtAnswer.Dispatcher.Invoke(UpdateAnswer, t.Result);
                    });
                //var res = await Task.Run(() => LongAdd(a, b));
                //UpdateAnswer(res);
            }
        }

        private void UpdateAnswer(object answer)
        {
            txtAnswer.Text = answer.ToString();
        }
        private int LongAdd(int a, int b)
        {
            Thread.Sleep(10000);
            return a + b;   
        }
    }
}