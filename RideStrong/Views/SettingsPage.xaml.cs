using RideStrong.ViewModels;

namespace RideStrong.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Refresh workout count when page appears
        if (BindingContext is SettingsViewModel viewModel)
        {
            viewModel.RefreshWorkoutCount();
        }
    }
}
