using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using RideStrong.Models;
using RideStrong.Services;

namespace RideStrong.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    private readonly ISettingsService _settingsService;
    private double _userWeight;
    private int _totalWorkouts;

    public event PropertyChangedEventHandler? PropertyChanged;

    public SettingsViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        // Load saved settings
        var settings = _settingsService.LoadSettings();
        _userWeight = settings.UserWeight;
        _totalWorkouts = _settingsService.GetTotalWorkoutsCount();

        // Initialize commands
        SaveSettingsCommand = new Command(OnSaveSettings);
        ExportWorkoutsCommand = new Command(OnExportWorkouts);
        DeleteAllWorkoutsCommand = new Command(OnDeleteAllWorkouts);
    }

    public double UserWeight
    {
        get => _userWeight;
        set
        {
            if (Math.Abs(_userWeight - value) > 0.01)
            {
                _userWeight = value;
                OnPropertyChanged();
            }
        }
    }

    public int TotalWorkouts
    {
        get => _totalWorkouts;
        set
        {
            if (_totalWorkouts != value)
            {
                _totalWorkouts = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveSettingsCommand { get; }
    public ICommand ExportWorkoutsCommand { get; }
    public ICommand DeleteAllWorkoutsCommand { get; }

    private async void OnSaveSettings()
    {
        var settings = new UserSettings
        {
            UserWeight = UserWeight
        };

        _settingsService.SaveSettings(settings);

        var page = Application.Current?.Windows[0]?.Page;
        if (page != null)
        {
            await page.DisplayAlert("Settings Saved", "Your settings have been saved successfully.", "OK");
        }
    }

    private async void OnExportWorkouts()
    {
        try
        {
            var json = _settingsService.ExportWorkoutsToJson();

            if (string.IsNullOrEmpty(json) || json == "[]")
            {
                var page = Application.Current?.Windows[0]?.Page;
                if (page != null)
                {
                    await page.DisplayAlert("No Data", "There are no workouts to export.", "OK");
                }
                return;
            }

            // Get the documents folder path
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var fileName = $"RideStrong_Workouts_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(documentsPath, fileName);

            // Write the JSON to file
            await File.WriteAllTextAsync(filePath, json);

            var page2 = Application.Current?.Windows[0]?.Page;
            if (page2 != null)
            {
                await page2.DisplayAlert(
                    "Export Successful",
                    $"Workouts exported to:\n{filePath}",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            var page = Application.Current?.Windows[0]?.Page;
            if (page != null)
            {
                await page.DisplayAlert("Export Failed", $"Failed to export workouts: {ex.Message}", "OK");
            }
        }
    }

    private async void OnDeleteAllWorkouts()
    {
        var page = Application.Current?.Windows[0]?.Page;
        if (page != null)
        {
            var result = await page.DisplayAlert(
                "Delete All Workouts",
                "Are you sure you want to delete all workout data? This action cannot be undone.",
                "Delete",
                "Cancel");

            if (result)
            {
                _settingsService.DeleteAllWorkouts();
                TotalWorkouts = 0;

                await page.DisplayAlert("Deleted", "All workout data has been deleted.", "OK");
            }
        }
    }

    public void RefreshWorkoutCount()
    {
        TotalWorkouts = _settingsService.GetTotalWorkoutsCount();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
