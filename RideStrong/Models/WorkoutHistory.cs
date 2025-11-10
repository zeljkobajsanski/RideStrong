namespace RideStrong.Models;

public class WorkoutHistory
{
    public int Id { get; set; }
    public DateTime WorkoutDate { get; set; }
    public TimeSpan Duration { get; set; }
    public double Distance { get; set; }
    public int Calories { get; set; }
    public double AverageSpeed { get; set; }
    public int AverageCadence { get; set; }
    public int AverageHeartRate { get; set; }

    public string FormattedDate => WorkoutDate.ToString("MMM dd, yyyy");
    public string FormattedTime => WorkoutDate.ToString("hh:mm tt");
    public string FormattedDuration => $"{(int)Duration.TotalHours:D2}:{Duration.Minutes:D2}:{Duration.Seconds:D2}";
    public string FormattedDistance => $"{Distance:F2} km";
    public string FormattedCalories => $"{Calories} kcal";
    public string FormattedAverageSpeed => $"{AverageSpeed:F1} km/h";
}
