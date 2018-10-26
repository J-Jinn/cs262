/// <summary>
/// Project 3: Fractal Fern
/// CS-212 Data Structures and Algorithms
/// Section: B
/// Instructor: Professor Plantinga
/// Date: 10-23-18
/// </summary>
/// 
/// Fractal Fern framework
/// Modified from the original template provided for this assignment.
/// 

using System.Windows;

/// <summary>
/// Namespace the solution belongs to.
/// </summary>
namespace FractalFern
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Constructor to initialize the MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// I'm not sure what this actually does.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Fractal f = new Fractal(depthSlider.Value, sizeSlider.Value, turnBiasSlider.Value, canvas);
        }
        
        /// <summary>
        /// Generate the fractal once user clicks the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Fractal f = new Fractal(depthSlider.Value, sizeSlider.Value, turnBiasSlider.Value, canvas);
        }
    }
}
