using System;

namespace Sharenian;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App() => Activated += StartElmish;

    private void StartElmish(object? sender, EventArgs e)
    {
        Activated -= StartElmish;
        Program.main(MainWindow);
    }
}
