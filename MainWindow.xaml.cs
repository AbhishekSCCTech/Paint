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
        private bool isDrawing = false; 
        private Point startPoint;       
        private Brush selectedColor = Brushes.Black; 
        private double selectedBrushSize = 2;        
        private string drawingMode = "Free Draw";    

        public MainWindow()
        {
            InitializeComponent();
            BrushSize.SelectedIndex = 0;
            BrushColor.SelectedIndex = 4;
            Mode.SelectedIndex = 0;

            BrushSize.SelectionChanged += BrushSize_SelectionChanged;
            BrushColor.SelectionChanged += BrushColor_SelectionChanged;
            Mode.SelectionChanged += Mode_SelectionChanged;
        }
        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDrawing = true;
                startPoint = e.GetPosition(Canvas);

                if (drawingMode == "Straight Line")
                {
                    // Start line drawing
                    var line = new Line
                    {
                        Stroke = selectedColor,
                        StrokeThickness = selectedBrushSize,
                        X1 = startPoint.X,
                        Y1 = startPoint.Y,
                        X2 = startPoint.X,
                        Y2 = startPoint.Y
                    };

                    Canvas.Children.Add(line);
                    line.CaptureMouse();
                }
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDrawing || e.LeftButton != MouseButtonState.Pressed)
                return;

            if (drawingMode == "Free Draw")
            {
                // Draw freehand line
                var currentPoint = e.GetPosition(Canvas);
                var line = new Line
                {
                    Stroke = selectedColor,
                    StrokeThickness = selectedBrushSize,
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = currentPoint.X,
                    Y2 = currentPoint.Y
                };

                Canvas.Children.Add(line);
                startPoint = currentPoint;
            }
            else if (drawingMode == "Straight Line")
            {
                if (Canvas.Children[Canvas.Children.Count - 1] is Line lastLine)
                {
                    var currentPoint = e.GetPosition(Canvas);
                    lastLine.X2 = currentPoint.X;
                    lastLine.Y2 = currentPoint.Y;
                }
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                isDrawing = false;

                if (drawingMode == "Straight Line")
                {
                    // Release mouse capture for line drawing
                    if (Canvas.Children[Canvas.Children.Count - 1] is Line lastLine)
                        lastLine.ReleaseMouseCapture();
                }
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas.Children.Clear();

        }
        private void BrushColor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrushColor.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedColor = selectedItem.Content.ToString() switch
                {
                    "Red" => Brushes.Red,
                    "Green" => Brushes.Green,
                    "Yellow" => Brushes.Yellow,
                    "Blue" => Brushes.Blue,
                    "Black" => Brushes.Black,
                    _ => Brushes.Black,
                };
            }

        }

        private void BrushSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrushSize.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
            {
                selectedBrushSize = double.Parse(selectedItem.Content.ToString());
            }
        }

        private void Mode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Mode.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content != null)
            {
                drawingMode = selectedItem.Content.ToString();
            }
        }





    }
}