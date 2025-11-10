using RideStrong.ViewModels;

namespace RideStrong.Views;

public partial class WorkoutPage : ContentPage
{
    public WorkoutPage(WorkoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
