using GalaSoft.MvvmLight.Command;
using System.ComponentModel;
using System.Windows.Input;
using WpfCalculator.Services;

namespace WpfCalculator.ViewModels;

internal class CalculatorViewModel : INotifyPropertyChanged
{
    private int _a;
	private int _b;
	private int _answer;
	private readonly Calculator _calculator = new Calculator();
    public event PropertyChangedEventHandler? PropertyChanged;

    public int Answer
	{
		get { return _answer; }
		set 
		{ 
			_answer = value; 
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Answer)));
		}
	}
	public int B
	{
		get { return _b; }
		set { _b = value; }
	}
	public int A
	{
		get { return _a; }
		set { _a = value; }
	}

	public ICommand PlusCommand { get => new RelayCommand(Add); }

    private void Add()
    {
		Task.Run(() => _calculator.LongAdd(A, B))
			.ContinueWith(pt => { 
				Answer = pt.Result;
			});
    }
}
