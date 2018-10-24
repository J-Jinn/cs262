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

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

/// <summary>
/// Namespace the solution belongs to.
/// </summary>
namespace FractalFern
{
    /// <summary>
    /// The class constructs a fern via a recursive fractal algorithm.
    /// </summary>
    class Fractal
    {

        // Class constants.
        private static double MAX_DEPTH = 4;

        /// <summary>
        /// Constructor method for the Fractal class.
        /// 
        /// 1) Clears the canvas every iteration of fractal generation.
        /// 2) Sets the background image for the canvas.
        /// 3) Establishes initial parameters for the fractal fern generation.
        /// 4) Calls recursive method to generate the fern.
        /// 
        /// </summary>
        /// <param name="size"></param>
        /// <param name="redux"></param>
        /// <param name="turnbias"></param>
        /// <param name="canvas"></param>
        public Fractal(double size, double redux, double turnbias, Canvas canvas)
        {
            // Clear canvas of old contents every iteration.
            canvas.Children.Clear();

            // Set-up background image for drawing on.
            setupBackground(canvas);


            // Initial parameters.
            double initialX1 = canvas.Width / 2;
            double initialY1 = canvas.Height;
            double initialDistance = 75;
            double initialTheta = Math.PI / 2;

            // Draw the recursive fractal fern.
            drawFractalFern(canvas, initialX1, initialY1, initialDistance, initialTheta, MAX_DEPTH + 1);

            // Initial parameters.
            double initialX2 = canvas.Width / 2 + 25;
            double initialY2 = canvas.Height;
            double initialDistance2 = 100;
            double initialTheta2 = Math.PI / 3;

            // Draw the recursive fractal fern.
            drawFractalFern(canvas, initialX2, initialY2, initialDistance2, initialTheta2, MAX_DEPTH);

            // Initial parameters.
            double initialX3 = canvas.Width / 2 - 25;
            double initialY3 = canvas.Height;
            double initialDistance3 = 125;
            double initialTheta3 = Math.PI / 4;

            // Draw the recursive fractal fern.
            drawFractalFern(canvas, initialX3, initialY3, initialDistance3, initialTheta3, MAX_DEPTH - 1);

        }

        /// <summary>
        /// Draw the fractal fern.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="distance"></param>
        /// <param name="theta"></param>
        /// <param name="depth"></param>
        private void drawFractalFern(Canvas canvas, double x, double y, double distance, double theta, double depth)
        {
            if (depth <= 0.5)
            {
                return;
            }

            Random random = new Random();
            double varyDistance = random.Next(Convert.ToInt32(distance) - 3, Convert.ToInt32(distance) + 3);

            // Constraints on the randomness.
            double frondDistanceRatioMax = 3.0;
            double frondDistanceRatioMin = 2.0;

            double stemDistanceRatioMax = 1.2;
            double stemDistanceRatioMin = 1.1;

            double frondDepthRatioMax = 1.5;
            double frondDepthRatioMin = 1.4;

            double stemDepthRatioMax = 1.3;
            double stemdDepthRatioMin = 1.2;

            double stemAngleOffsetMax = Math.PI / 12;
            double stemAngleOffsetMin = Math.PI / 24;

            double frondAngleOffsetMax = Math.PI / 6;
            double frondAngleOffsetMin = Math.PI / 12;

            // Random number generators.
            double varyFrondDistanceRatio = random.NextDouble() * (frondDistanceRatioMax - frondDistanceRatioMin) + frondDistanceRatioMin;
            double varyStemDistanceRatio = random.NextDouble() * (stemDistanceRatioMax - stemDistanceRatioMin) + stemDistanceRatioMin;
            double varyFrondDepthReductionRatio = random.NextDouble() * (frondDepthRatioMax - frondDepthRatioMin) + frondDepthRatioMin;
            double varyStemDepthReductionRatio = random.NextDouble() * (stemDepthRatioMax - stemdDepthRatioMin) + stemdDepthRatioMin;
            double varyStemAngleOffsetAmount = random.NextDouble() * (stemAngleOffsetMax - stemAngleOffsetMin) + stemAngleOffsetMin;
            double varyFrondAngleOffsetAmount = random.NextDouble() * (frondAngleOffsetMax - frondAngleOffsetMin) + frondAngleOffsetMin;

            // "leaves" of the fern.
            drawMyEllipse(canvas, x, y, 3);

            // Calculate new coordinates.
            double newX = x + varyDistance * Math.Cos(theta);
            double newY = y - varyDistance * Math.Sin(theta);

            // Beginning of the stem of the fern.
            drawMyLine(canvas, x, newX, y, newY);

            // Calculate new coordinates.
            double varyDistance2 = random.Next(Convert.ToInt32(distance) - 3, Convert.ToInt32(distance) + 3);
            double newX1 = x + varyDistance2 * Math.Cos(theta);
            double newY1 = y - varyDistance2 * Math.Sin(theta);

            // Draw the right fronds.
            drawFractalFern(canvas, newX1, newY1, distance / varyFrondDistanceRatio, theta + varyFrondAngleOffsetAmount, depth / varyFrondDepthReductionRatio);

            // Calculate new coordinates.
            double varyDistance3 = random.Next(Convert.ToInt32(distance) - 3, Convert.ToInt32(distance) + 3);
            double newX2 = x + varyDistance3 * Math.Cos(theta);
            double newY2 = y - varyDistance3 * Math.Sin(theta);

            // Draw the left fronds.
            drawFractalFern(canvas, newX2, newY2, distance / varyFrondDistanceRatio, theta - varyFrondAngleOffsetAmount, depth / varyFrondDepthReductionRatio);

            // Draw the remaining parts of the main stem of the fern.

            int chooseTurnDirection = random.Next(1, 4);

            if (chooseTurnDirection == 1)
            {
                drawFractalFern(canvas, newX, newY, distance / varyStemDistanceRatio, theta + varyStemAngleOffsetAmount, depth / varyStemDepthReductionRatio);
            }
            else
            {
                drawFractalFern(canvas, newX, newY, distance / varyStemDistanceRatio, theta - varyStemAngleOffsetAmount, depth / varyStemDepthReductionRatio);
            }
        }

        /// <summary>
        /// Draw the frond details of the fern at each level.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        /// <param name="depth"></param>
        private void drawFernFrondDetail(Canvas canvas, double x, double y, double distance, double theta, int depth)
        {

        }

        /// <summary>
        /// Set-up a background image to draw on.
        /// </summary>
        private void setupBackground(Canvas canvas)
        {
            // Set canvas to the background image.
            try
            {
                ImageBrush myImageBrush = new ImageBrush();
                myImageBrush.ImageSource = new BitmapImage(new Uri("D:/Dropbox/CS_262/CS212_Project3/FractalFern/patchygrass_1.jpg", UriKind.RelativeOrAbsolute));

                canvas.Background = myImageBrush;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source, "Argument exception: {0}");
                MessageBox.Show("Failed to load required files");
            }

            // Declare objects necessary for the process.
            System.Drawing.Image sourceImage;
            Bitmap sourceBitmap;
            Graphics sourceGraphics;

            // Create Image object from user specified image file path.
            try
            {
                sourceImage = System.Drawing.Image.FromFile("D:/Dropbox/CS_262/CS212_Project3/FractalFern/patchygrass_1.jpg");
            }
            catch (ArgumentException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source, "Argument exception: {0}");
                MessageBox.Show("Failed to load required files");
                return;
            }

            // Create Bitmap object from the Image object.
            sourceBitmap = new Bitmap(sourceImage.Width, sourceImage.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // Create Graphics object from the Bitmap object.
            sourceGraphics = Graphics.FromImage(sourceBitmap);

            // Set anti-aliasing mode.
            sourceGraphics.SmoothingMode = SmoothingMode.HighQuality;

            // Draw Image object to the Graphics object. (use source image as the canvas upon which to draw the watermark image)
            sourceGraphics.DrawImage(sourceImage, new System.Drawing.Rectangle(0, 0, sourceImage.Width, sourceImage.Height), 0, 0, sourceImage.Width, sourceImage.Height, GraphicsUnit.Pixel);

            // Save image to specific file format.
            sourceImage = sourceBitmap;

            // De-allocate resources.
            sourceGraphics.Dispose();
            sourceImage.Dispose();
            sourceBitmap.Dispose();
        }

        /// <summary>
        /// Method draws a ellipse with specified parameters.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="radius"></param>
        private static void drawMyEllipse(Canvas canvas, double x1, double y1, double radius)
        {
            // RNG.
            Random random = new Random();

            // Constraints.
            double strokeThicknessMin = 1.0;
            double strokeThicknessMax = 1.2;

            double ellipseModifierMin = 1.0;
            double ellipseModifierMax = 1.5;

            // Generate random numbers.
            double randomStrokeThickness = random.NextDouble() * (strokeThicknessMax - strokeThicknessMin) + strokeThicknessMin;
            double randomEllipseDimensions = random.NextDouble() * (ellipseModifierMax - ellipseModifierMin) + ellipseModifierMin;
            int randomAlpha = random.Next(200, 250);
            int randomRed = random.Next(200, 220);
            int randomGreen = random.Next(245, 255);
            int randomBlue = random.Next(250, 255);

            // Create a RadialGradientBrush with four gradient stops.
            RadialGradientBrush radialGradientBrush = new RadialGradientBrush();

            // Set the GradientOrigin to the center of the area being painted.
            radialGradientBrush.GradientOrigin = new System.Windows.Point(0.5, 0.5);

            // Set the gradient center to the center of the area being painted.
            radialGradientBrush.Center = new System.Windows.Point(0.5, 0.5);

            // Set the radius of the gradient circle so that it extends to
            // the edges of the area being painted.
            radialGradientBrush.RadiusX = 0.5;
            radialGradientBrush.RadiusY = 0.5;

            // Create four gradient stops.
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));

            // Freeze the brush (make it unmodifiable) for performance benefits.
            radialGradientBrush.Freeze();

            // Create a SolidColorBrush with a single color.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(Convert.ToByte(randomAlpha), Convert.ToByte(randomRed), Convert.ToByte(randomGreen), Convert.ToByte(randomBlue));

            // Settings for the ellipse.
            Ellipse myEllipse = new Ellipse();
            myEllipse.Fill = radialGradientBrush;
            myEllipse.StrokeThickness = randomStrokeThickness;
            myEllipse.Stroke = mySolidColorBrush;

            myEllipse.HorizontalAlignment = HorizontalAlignment.Center;
            myEllipse.VerticalAlignment = VerticalAlignment.Center;

            myEllipse.Width = randomEllipseDimensions * radius;
            myEllipse.Height = randomEllipseDimensions * radius;
            myEllipse.SetCenter(x1, y1);

            // Draw to canvas.
            canvas.Children.Add(myEllipse);
        }

        /// <summary>
        /// Method draws a line with specified parameters.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="y1"></param>
        /// <param name="y2"></param>
        private static void drawMyLine(Canvas canvas, double x1, double x2, double y1, double y2)
        {
            // RNG.
            Random random = new Random();
            
            // Constraints.
            double strokeThicknessMin = 1.0;
            double strokeThicknessMax = 1.2;

            // Generate random numbers.
            double randomStrokeThickness = random.NextDouble() * (strokeThicknessMax - strokeThicknessMin) + strokeThicknessMin;
            int randomAlpha = random.Next(200, 250);
            int randomRed = random.Next(200, 220);
            int randomGreen = random.Next(245, 255);
            int randomBlue = random.Next(250, 255);

            // Create a SolidColorBrush with a single color.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(Convert.ToByte(randomAlpha), Convert.ToByte(randomRed), Convert.ToByte(randomGreen), Convert.ToByte(randomBlue));

            // Settings for the line.
            Line myLine = new Line();
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = mySolidColorBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = randomStrokeThickness;

            // Draw to canvas.
            canvas.Children.Add(myLine);
        }         
    }

    /// <summary>
    /// This class is needed to enable us to set the center for an ellipse (not built in?!)
    /// </summary>
    public static class EllipseX
    {
        /// <summary>
        /// This method sets the center for an ellipse.
        /// </summary>
        /// <param name="ellipse"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        public static void SetCenter(this Ellipse ellipse, double X, double Y)
        {
            Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
            Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
        }
    }
}
// TODO: fix color issue.
// TODO: add 3 slider controls.
