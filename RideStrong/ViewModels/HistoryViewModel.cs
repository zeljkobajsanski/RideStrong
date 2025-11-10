using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using RideStrong.Models;

namespace RideStrong.ViewModels;

public class HistoryViewModel : INotifyPropertyChanged
{
    private ObservableCollection<WorkoutHistory> _workoutHistory;

    public event PropertyChangedEventHandler? PropertyChanged;

    public HistoryViewModel()
    {
        _workoutHistory = new ObservableCollection<WorkoutHistory>();
        LoadSampleData();
    }

    public ObservableCollection<WorkoutHistory> WorkoutHistory
    {
        get => _workoutHistory;
        set
        {
            if (_workoutHistory != value)
            {
                _workoutHistory = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalWorkouts));
                OnPropertyChanged(nameof(TotalDistance));
                OnPropertyChanged(nameof(TotalDistanceFormatted));
                OnPropertyChanged(nameof(TotalCalories));
                OnPropertyChanged(nameof(TotalTime));
                OnPropertyChanged(nameof(TotalTimeFormatted));
                OnPropertyChanged(nameof(AverageSpeed));
                OnPropertyChanged(nameof(AverageSpeedFormatted));
            }
        }
    }

    public int TotalWorkouts => WorkoutHistory.Count;

    public double TotalDistance => WorkoutHistory.Sum(w => w.Distance);

    public string TotalDistanceFormatted => $"{TotalDistance:F1}\nkm";

    public int TotalCalories => WorkoutHistory.Sum(w => w.Calories);

    public string TotalCaloriesFormatted => $"{TotalCalories}\nkcal";

    public TimeSpan TotalTime => TimeSpan.FromSeconds(WorkoutHistory.Sum(w => w.Duration.TotalSeconds));

    public string TotalTimeFormatted => $"{(int)TotalTime.TotalHours}h";

    public double AverageSpeed => WorkoutHistory.Any() ? WorkoutHistory.Average(w => w.AverageSpeed) : 0;

    public string AverageSpeedFormatted => $"{AverageSpeed:F1}\nkm/h";

    private void LoadSampleData()
    {
        // Sample workout data
        // In a real app, this would be loaded from a database
        _workoutHistory.Add(new WorkoutHistory
        {
            Id = 1,
            WorkoutDate = DateTime.Parse("2025-11-10 10:51:00"),
            Duration = TimeSpan.FromSeconds(4),
            Distance = 0.01,
            Calories = 0,
            AverageSpeed = 5.5,
            AverageCadence = 75,
            AverageHeartRate = 125
        });

        // Notify all properties that depend on the collection
        OnPropertyChanged(nameof(WorkoutHistory));
        OnPropertyChanged(nameof(TotalWorkouts));
        OnPropertyChanged(nameof(TotalDistance));
        OnPropertyChanged(nameof(TotalDistanceFormatted));
        OnPropertyChanged(nameof(TotalCalories));
        OnPropertyChanged(nameof(TotalCaloriesFormatted));
        OnPropertyChanged(nameof(TotalTime));
        OnPropertyChanged(nameof(TotalTimeFormatted));
        OnPropertyChanged(nameof(AverageSpeed));
        OnPropertyChanged(nameof(AverageSpeedFormatted));
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
