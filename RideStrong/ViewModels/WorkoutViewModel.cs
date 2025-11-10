using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace RideStrong.ViewModels;

public class WorkoutViewModel : INotifyPropertyChanged
{
    private bool _isWorkoutActive;
    private TimeSpan _elapsedTime;
    private double _speed;
    private int _cadence;
    private int _heartRate = 60;
    private double _distance;
    private int _calories;
    private int _resistance = 5;
    private int _resistanceLevel = 5;
    private System.Timers.Timer? _workoutTimer;

    public event PropertyChangedEventHandler? PropertyChanged;

    public WorkoutViewModel()
    {
        ToggleWorkoutCommand = new Command(OnToggleWorkout);
        ResetWorkoutCommand = new Command(OnResetWorkout);
        FinishWorkoutCommand = new Command(OnFinishWorkout);
    }

    public bool IsWorkoutActive
    {
        get => _isWorkoutActive;
        set
        {
            if (_isWorkoutActive != value)
            {
                _isWorkoutActive = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(StartButtonText));
            }
        }
    }

    public string StartButtonText => IsWorkoutActive ? "Pause" : "Start";

    public TimeSpan ElapsedTime
    {
        get => _elapsedTime;
        set
        {
            if (_elapsedTime != value)
            {
                _elapsedTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ElapsedTimeFormatted));
            }
        }
    }

    public string ElapsedTimeFormatted => $"{(int)ElapsedTime.TotalHours:D2}:{ElapsedTime.Minutes:D2}:{ElapsedTime.Seconds:D2}";

    public double Speed
    {
        get => _speed;
        set
        {
            if (Math.Abs(_speed - value) > 0.01)
            {
                _speed = value;
                OnPropertyChanged();
            }
        }
    }

    public int Cadence
    {
        get => _cadence;
        set
        {
            if (_cadence != value)
            {
                _cadence = value;
                OnPropertyChanged();
            }
        }
    }

    public int HeartRate
    {
        get => _heartRate;
        set
        {
            if (_heartRate != value)
            {
                _heartRate = value;
                OnPropertyChanged();
            }
        }
    }

    public double Distance
    {
        get => _distance;
        set
        {
            if (Math.Abs(_distance - value) > 0.001)
            {
                _distance = value;
                OnPropertyChanged();
            }
        }
    }

    public int Calories
    {
        get => _calories;
        set
        {
            if (_calories != value)
            {
                _calories = value;
                OnPropertyChanged();
            }
        }
    }

    public int Resistance
    {
        get => _resistance;
        set
        {
            if (_resistance != value)
            {
                _resistance = value;
                OnPropertyChanged();
            }
        }
    }

    public int ResistanceLevel
    {
        get => _resistanceLevel;
        set
        {
            if (_resistanceLevel != value)
            {
                _resistanceLevel = value;
                Resistance = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ResistanceLevelText));
            }
        }
    }

    public string ResistanceLevelText => $"Resistance Level: {ResistanceLevel}";

    public ICommand ToggleWorkoutCommand { get; }
    public ICommand ResetWorkoutCommand { get; }
    public ICommand FinishWorkoutCommand { get; }

    private void OnToggleWorkout()
    {
        IsWorkoutActive = !IsWorkoutActive;

        if (IsWorkoutActive)
        {
            StartWorkout();
        }
        else
        {
            PauseWorkout();
        }
    }

    private void StartWorkout()
    {
        if (_workoutTimer == null)
        {
            _workoutTimer = new System.Timers.Timer(1000); // Update every second
            _workoutTimer.Elapsed += OnWorkoutTimerElapsed;
        }

        _workoutTimer.Start();
    }

    private void PauseWorkout()
    {
        _workoutTimer?.Stop();
    }

    private void OnWorkoutTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        // Update elapsed time
        ElapsedTime = ElapsedTime.Add(TimeSpan.FromSeconds(1));

        // Simulate workout metrics based on resistance level
        // In a real app, these would come from sensors or user input
        SimulateWorkoutMetrics();
    }

    private void SimulateWorkoutMetrics()
    {
        var random = new Random();

        // Speed increases with resistance (simulated)
        Speed = 15 + (ResistanceLevel * 2) + random.Next(-2, 3);

        // Cadence varies around 80 RPM
        Cadence = 70 + random.Next(0, 20);

        // Heart rate increases with effort
        var targetHeartRate = 100 + (ResistanceLevel * 5);
        HeartRate = Math.Min(180, targetHeartRate + random.Next(-5, 6));

        // Distance accumulates based on speed (km = speed * time in hours)
        Distance += Speed / 3600; // Convert km/h to km per second

        // Calories burn estimation (rough approximation)
        Calories = (int)(ElapsedTime.TotalMinutes * (5 + ResistanceLevel * 0.5));
    }

    private void OnResetWorkout()
    {
        _workoutTimer?.Stop();
        IsWorkoutActive = false;

        ElapsedTime = TimeSpan.Zero;
        Speed = 0;
        Cadence = 0;
        HeartRate = 60;
        Distance = 0;
        Calories = 0;
        ResistanceLevel = 5;
    }

    private async void OnFinishWorkout()
    {
        _workoutTimer?.Stop();
        IsWorkoutActive = false;

        // In a real app, this would save the workout to a database
        var page = Application.Current?.Windows[0]?.Page;
        if (page != null)
        {
            await page.DisplayAlert(
                "Workout Saved",
                $"Great job! You completed a {ElapsedTime.TotalMinutes:F1} minute workout.\n" +
                $"Distance: {Distance:F2} km\n" +
                $"Calories: {Calories} kcal",
                "OK");
        }

        // Reset after showing the summary
        OnResetWorkout();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
