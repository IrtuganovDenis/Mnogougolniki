using Avalonia.Controls;

namespace Mnogougolniki.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(this);
        CustomControl CC = this.Find<CustomControl>("MyCC");
        if (point.Properties.IsLeftButtonPressed)
        {
            CC?.Click(e.GetPosition(CC).X, e.GetPosition(CC).Y);
        }
        else if (point.Properties.IsRightButtonPressed)
        {
            CC?.Delete(e.GetPosition(CC).X, e.GetPosition(CC).Y);
        }
        
    }
    private void Window_PointerMoved(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        CustomControl CC = this.Find<CustomControl>("MyCC");
        CC?.Move(e.GetPosition(CC).X, e.GetPosition(CC).Y);
    }

    private void Window_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        CustomControl CC = this.Find<CustomControl>("MyCC");
        CC?.Release(e.GetPosition(CC).X, e.GetPosition(CC).Y);
    }
}
