namespace RideStrong;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Force dark mode
        UserAppTheme = AppTheme.Dark;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}