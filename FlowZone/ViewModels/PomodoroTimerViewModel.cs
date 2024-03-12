//using System;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Maui.Controls;


//namespace FlowZone.ViewModels
//{
//	public class PomodoroTimerViewModel : INotifyPropertyChanged
//	{
//		private TimeSpan _timerDuration;
//		private TimeSpan _remainingTime;
//		private bool _isRunning;
//		private bool _isConfiguring;

//		public PomodoroTimerViewModel()
//		{
//			_timerDuration = TimeSpan.FromMinutes(25); // Default timer duration
//			_remainingTime = _timerDuration;
//			_isRunning = false;
//			_isConfiguring = false;

//			StartCommand = new Command(Start);
//			StopCommand = new Command(Stop);
//			ConfigureCommand = new Command(ConfigureAsync);
//			ResetCommand = new Command(Reset);
//		}

//		public string TimerDisplay => _remainingTime.ToString(@"mm\:ss");

//		public Command StartCommand { get; }
//		public Command StopCommand { get; }
//		public Command ConfigureCommand { get; }
//		public Command ResetCommand { get; }

//		public async void Start()
//		{
//			_isRunning = true;

//			while (_isRunning && _remainingTime > TimeSpan.Zero)
//			{
//				await Task.Delay(1000);
//				_remainingTime = _remainingTime.Subtract(TimeSpan.FromSeconds(1));
//				OnPropertyChanged(nameof(TimerDisplay));
//			}
//		}

//		public void Stop()
//		{
//			_isRunning = false;
//		}

//		//public async Task ConfigureAsync()
//		//{
//		//	string input = await MauiWindow.Current.Page.DisplayPromptAsync("Configure Timer", "Enter the new timer duration in minutes", "OK", "Cancel", "5", -1, keyboard: Keyboard.Numeric);

//		//	if (!string.IsNullOrEmpty(input))
//		//	{
//		//		if (int.TryParse(input, out int minutes))
//		//		{
//		//			_timerDuration = TimeSpan.FromMinutes(minutes);
//		//			_remainingTime = _timerDuration;
//		//			OnPropertyChanged(nameof(TimerDisplay));
//		//		}
//		//		else
//		//		{
//		//			await MauiWindow.Current.Page.DisplayAlert("Error", "Invalid input. Please enter a valid number.", "OK");
//		//		}
//		//	}
//		//}

//		public void Reset()
//		{
//			_remainingTime = _timerDuration;
//			OnPropertyChanged(nameof(TimerDisplay));
//		}

//		public event PropertyChangedEventHandler PropertyChanged;

//		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//		{
//			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//		}
//	}
//}
