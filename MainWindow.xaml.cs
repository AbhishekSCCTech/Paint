using System.Printing;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Line currentLine;
        bool paintStart = false;
        Point StartPoint;
        string drawMode;
        Brush selectedColor;
        double strokeThickness;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            paintStart = true;
            StartPoint = e.GetPosition(this);
            if (drawMode == "Straight Line")
            {
                currentLine = new Line()
                {
                    X1 = StartPoint.X,
                    Y1 = StartPoint.Y,
                    X2 = StartPoint.X,
                    Y2 = StartPoint.Y,
                    Stroke = selectedColor,  // Set the stroke color to black
                    StrokeThickness = strokeThickness
                };
                Canvas.Children.Add(currentLine);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!paintStart) return;
            Point currentPoint = e.GetPosition(Canvas);
            if (drawMode == "Straight Line" && currentLine != null)
            {
                // Update the end point of the line
                currentLine.X2 = currentPoint.X;
                currentLine.Y2 = currentPoint.Y;
            }
            else if (drawMode == "Free Draw")
            {
                // Draw small lines for free drawing
                var freeLine = new Line()
                {
                    X1 = StartPoint.X,
                    Y1 = StartPoint.Y,
                    X2 = currentPoint.X,
                    Y2 = currentPoint.Y,
                    Stroke = selectedColor,
                    StrokeThickness = strokeThickness
                };
                Canvas.Children.Add(freeLine);
                StartPoint = currentPoint; // Update starting point for the next segment
            }

        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            paintStart = false;
            currentLine = null; ;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();

        }
        private void Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Mode.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content is string selectedText)
                {
                    if (selectedText == "Straight Line")
                    {
                        drawMode = "Straight Line";
                    }
                    else if (selectedText == "Free Draw")
                    {
                        drawMode = "Free Draw";
                    }
                    else
                    {
                        drawMode = string.Empty; 
                    }
                }
            }

        }
        private void BrushColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrushColor.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content is string selectedText)
                {
                    if (selectedText == "Red")
                    {
                        selectedColor = Brushes.Red;
                    }
                    else if (selectedText == "Green")
                    {
                        selectedColor = Brushes.Green;
                    }
                    else if (selectedText == "Yellow")
                    {
                        selectedColor = Brushes.Yellow;
                    }
                    else if (selectedText == "Blue")
                    {
                        selectedColor = Brushes.Blue;
                    }
                    else if (selectedText == "Black")
                    {
                        selectedColor = Brushes.Black;
                    }
                    else
                    {
                        selectedColor = Brushes.Black; // Default to black
                    }
                }
            }
        }
        private void StrokeThickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrushSize.SelectedItem is ComboBoxItem selectedItem)
            {
                if (selectedItem.Content is string selectedText)
                {
                    // Set strokeThickness based on selected value directly
                    switch (selectedText)
                    {
                        case "2":
                            strokeThickness = 2;
                            break;
                        case "4":
                            strokeThickness = 4;
                            break;
                        case "6":
                            strokeThickness = 6;
                            break;
                        case "8":
                            strokeThickness = 8;
                            break;
                        case "10":
                            strokeThickness = 10;
                            break;
                        default:
                            strokeThickness = 2; // Fallback to default if no match
                            break;
                    }
                }
            }
        }
    }
}