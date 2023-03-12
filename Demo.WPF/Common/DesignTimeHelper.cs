using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Demo.WPF.Common;

public static class DesignTimeHelper
{
    public static void DrawDesign(Control control, DrawingContext drawingContext)
    {
        if (control.Visibility == Visibility.Visible && control.ActualWidth > 0 && control.ActualHeight > 0)
        {
            drawingContext.DrawRectangle(Brushes.Black,
                new Pen(Brushes.DarkBlue, 2.0),
                new Rect(0, 0, control.ActualWidth, control.ActualHeight));

            drawingContext.DrawLine(new Pen(Brushes.DarkBlue, 2.0),
                new Point(0.0, 0.0),
                new Point(control.ActualWidth, control.ActualHeight));
            drawingContext.DrawLine(new Pen(Brushes.DarkBlue, 2.0),
                new Point(control.ActualWidth, 0.0),
                new Point(0.0, control.ActualHeight));
        }
    }
}
