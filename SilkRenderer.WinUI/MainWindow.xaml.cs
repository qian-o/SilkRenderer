using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SilkRenderer.WinUI.OpenGL.Sample;

namespace SilkRenderer.WinUI;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Materials_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        Materials materials = (Materials)sender;

        if (Grid.GetColumn(materials) == 1)
        {
            Grid.SetColumn(materials, 0);
            Grid.SetRow(materials, 0);

            Grid.SetColumnSpan(materials, 2);
            Grid.SetRowSpan(materials, 2);
        }
        else
        {
            Grid.SetColumn(materials, 1);
            Grid.SetRow(materials, 1);

            Grid.SetColumnSpan(materials, 1);
            Grid.SetRowSpan(materials, 1);
        }
    }
}