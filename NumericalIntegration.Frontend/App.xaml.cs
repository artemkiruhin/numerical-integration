using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace NumericalIntegration.Frontend;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "Численное интегрирование" };
    }
}