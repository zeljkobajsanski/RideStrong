# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

RideStrong is an indoor bike workout application built with .NET 9.0 MAUI (Multi-platform App UI). It targets Android, iOS, macOS Catalyst, and Windows platforms, using a single codebase with platform-specific entry points and configurations.

## Build and Run Commands

### Building the project
```bash
dotnet build RideStrong.sln
```

### Build for specific platform
```bash
# Android
dotnet build RideStrong/RideStrong.csproj -f net9.0-android

# iOS
dotnet build RideStrong/RideStrong.csproj -f net9.0-ios

# macOS Catalyst
dotnet build RideStrong/RideStrong.csproj -f net9.0-maccatalyst

# Windows
dotnet build RideStrong/RideStrong.csproj -f net9.0-windows10.0.19041.0
```

### Running the application
```bash
# Run on specific platform (requires platform SDK installed)
dotnet run --project RideStrong/RideStrong.csproj -f net9.0-android
dotnet run --project RideStrong/RideStrong.csproj -f net9.0-ios
```

### Restore dependencies
```bash
dotnet restore RideStrong.sln
```

### Clean build artifacts
```bash
dotnet clean RideStrong.sln
```

## Architecture

### Application Structure

- **MauiProgram.cs**: Application entry point and dependency injection configuration. Configures the MAUI app builder, fonts, and debug logging.
- **App.xaml.cs**: Main application class that creates the initial window with AppShell.
- **AppShell.xaml**: Shell-based navigation structure defining the app's visual hierarchy and routing.
- **MainPage.xaml/MainPage.xaml.cs**: Default home page (template demo page).

### Platform-Specific Code

Platform-specific implementations are located in `RideStrong/Platforms/`:
- **Android/**: MainActivity, MainApplication, AndroidManifest.xml
- **iOS/**: AppDelegate, Program, Info.plist, PrivacyInfo.xcprivacy
- **MacCatalyst/**: AppDelegate, Program, Info.plist, Entitlements.plist
- **Windows/**: App.xaml, app.manifest, Package.appxmanifest
- **Tizen/**: Main.cs, tizen-manifest.xml (commented out by default)

### Resources

- **AppIcon/**: Application icon (SVG format)
- **Splash/**: Splash screen (SVG format)
- **Fonts/**: OpenSans font family
- **Images/**: Image assets
- **Styles/**: XAML resource dictionaries for Colors and Styles
- **Raw/**: Raw assets accessible at runtime

## MAUI Patterns

### XAML + Code-Behind Pattern
Pages use XAML for UI definition paired with C# code-behind (partial classes). The `InitializeComponent()` method is automatically generated and must be called in constructors to wire up XAML elements.

### Shell Navigation
Navigation is managed through the Shell pattern defined in AppShell.xaml. Routes can be registered for navigation using:
```csharp
Routing.RegisterRoute("routename", typeof(PageType));
```

### Dependency Injection
Services are registered in MauiProgram.cs using the builder pattern:
```csharp
builder.Services.AddSingleton<IService, ServiceImplementation>();
builder.Services.AddTransient<PageType>();
builder.Services.AddTransient<ViewModelType>();
```

## Configuration

### Target Frameworks
The project targets .NET 9.0 with multiple platform-specific frameworks:
- Android 21.0+
- iOS/macOS Catalyst 15.0+
- Windows 10.0.17763.0+
- Tizen 6.5+ (optional, commented out)

### Project Settings
- **ApplicationId**: com.companyname.ridestrong
- **ApplicationTitle**: RideStrong
- **Nullable**: Enabled
- **ImplicitUsings**: Enabled
- **SingleProject**: True (multi-targeting single project)

## Domain Context

RideStrong is an indoor cycling workout application. When developing features, consider:

- **Workout tracking**: Timer-based sessions, distance, speed, cadence, power metrics
- **Performance metrics**: Real-time data display during workouts
- **Workout plans**: Structured training programs, intervals, zones
- **Data persistence**: Workout history, progress tracking
- **Device integration**: Potential integration with smart trainers, sensors (Bluetooth, ANT+)
- **User experience**: Quick access to start workouts, clear metric displays, motivational elements