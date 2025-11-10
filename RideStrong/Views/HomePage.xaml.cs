namespace RideStrong.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnStartWorkoutClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//WorkoutPage");
    }
}
