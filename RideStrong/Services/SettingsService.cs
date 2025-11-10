using System.Text.Json;
using RideStrong.Models;

namespace RideStrong.Services;

public interface ISettingsService
{
    UserSettings LoadSettings();
    void SaveSettings(UserSettings settings);
    List<WorkoutHistory> LoadWorkouts();
    void SaveWorkout(WorkoutHistory workout);
    void DeleteAllWorkouts();
    string ExportWorkoutsToJson();
    int GetTotalWorkoutsCount();
}

public class SettingsService : ISettingsService
{
    private const string SettingsKey = "UserSettings";
    private const string WorkoutsKey = "WorkoutHistory";

    public UserSettings LoadSettings()
    {
        var settingsJson = Preferences.Get(SettingsKey, string.Empty);
        if (string.IsNullOrEmpty(settingsJson))
        {
            return new UserSettings();
        }

        try
        {
            return JsonSerializer.Deserialize<UserSettings>(settingsJson) ?? new UserSettings();
        }
        catch
        {
            return new UserSettings();
        }
    }

    public void SaveSettings(UserSettings settings)
    {
        var settingsJson = JsonSerializer.Serialize(settings);
        Preferences.Set(SettingsKey, settingsJson);
    }

    public List<WorkoutHistory> LoadWorkouts()
    {
        var workoutsJson = Preferences.Get(WorkoutsKey, string.Empty);
        if (string.IsNullOrEmpty(workoutsJson))
        {
            return new List<WorkoutHistory>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<WorkoutHistory>>(workoutsJson) ?? new List<WorkoutHistory>();
        }
        catch
        {
            return new List<WorkoutHistory>();
        }
    }

    public void SaveWorkout(WorkoutHistory workout)
    {
        var workouts = LoadWorkouts();
        workout.Id = workouts.Count > 0 ? workouts.Max(w => w.Id) + 1 : 1;
        workouts.Add(workout);

        var workoutsJson = JsonSerializer.Serialize(workouts);
        Preferences.Set(WorkoutsKey, workoutsJson);
    }

    public void DeleteAllWorkouts()
    {
        Preferences.Remove(WorkoutsKey);
    }

    public string ExportWorkoutsToJson()
    {
        var workouts = LoadWorkouts();
        return JsonSerializer.Serialize(workouts, new JsonSerializerOptions
        {
            WriteIndented = true
        });
    }

    public int GetTotalWorkoutsCount()
    {
        return LoadWorkouts().Count;
    }
}
