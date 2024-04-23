
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System.ComponentModel;
using System.Windows.Input;
namespace FlowZone.Views;

public partial class Pomodoro : ContentPage, INotifyPropertyChanged
{
	private bool isRunning;
	private TimeSpan elapsedTime;
	private DateTime startTime;
	public Pomodoro()
	{
		InitializeComponent();
		BindingContext = this;

		StartCommand = new Command(StartTimer, () => !isRunning);
		StopCommand = new Command(StopTimer, () => isRunning);
		ResetCommand = new Command(ResetTimer);

		Device.StartTimer(TimeSpan.FromSeconds(1), () =>
		{
			if (isRunning)
			{
				elapsedTime = DateTime.Now - startTime;
				TimerText = elapsedTime.ToString(@"hh\:mm\:ss");
			}
			return true;
		});
	}
	private Command startCommand;
	public Command StartCommand
	{
		get => startCommand;
		set
		{
			if (startCommand != value)
			{
				startCommand = value;
				OnPropertyChanged(nameof(StartCommand));
			}
		}
	}

	private Command stopCommand;
	public Command StopCommand
	{
		get => stopCommand;
		set
		{
			if (stopCommand != value)
			{
				stopCommand = value;
				OnPropertyChanged(nameof(StopCommand));
			}
		}
	}

	private Command resetCommand;
	public Command ResetCommand
	{
		get => resetCommand;
		set
		{
			if (resetCommand != value)
			{
				resetCommand = value;
				OnPropertyChanged(nameof(ResetCommand));
			}
		}
	}

	private string timerText;
	public string TimerText
	{
		get => timerText;
		set
		{
			if (timerText != value)
			{
				timerText = value;
				OnPropertyChanged(nameof(TimerText));
			}
		}
	}

	private void StartTimer()
	{
		isRunning = true;
		startTime = DateTime.Now;
		StartCommand.ChangeCanExecute();
		StopCommand.ChangeCanExecute();
	}

	private void StopTimer()
	{
		isRunning = false;
		StartCommand.ChangeCanExecute();
		StopCommand.ChangeCanExecute();
	}

	private void ResetTimer()
	{
		isRunning = false;
		elapsedTime = TimeSpan.Zero;
		TimerText = "00:00:00";
		StartCommand.ChangeCanExecute();
		StopCommand.ChangeCanExecute();
	}


	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}



}

