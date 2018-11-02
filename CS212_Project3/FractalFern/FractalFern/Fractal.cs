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
/// Note: May need to adjust hard-coded canvas size depending on size of user screen.
///     (currently set to 1024x768)
/// Note: Background image may cause portability issues 
///     (.jpg is in same directory as .exe)
///     
/// Note: Weird color issue with SolidColorBrush where the color it draws doesn't really reflect
/// the range of the randomized ARGB values (which should be a hue variation of green).
/// But, the color it does draw adds a cool phosphorescent effect for the fern's "leaves".
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

        // Constants for testing purposes.
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
        /// <param name="depth">value of depth slider</param>
        /// <param name="size"> value of size slider</param>
        /// <param name="turnbias">value of turn bias slider</param>
        /// <param name="canvas">canvas object to draw on</param>
        public Fractal(double depth, double size, double turnbias, Canvas canvas)
        {
            // Clear canvas of old contents every iteration.
            canvas.Children.Clear();

            // Set-up background image for drawing on.
            setupBackground(canvas);

            // Get the slider value for depth from widget.
            double sliderDepth = depth;

            // Get the slider value for turn bias from widget.
            double sliderTurnBias = turnbias;

            // Get the slider value for TBD from widget.
            double sliderSize = size;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Initial parameters.
            double initialX1 = canvas.Width / 2 - 200;
            double initialY1 = canvas.Height;
            double initialTheta1 = 2 * Math.PI / 3;
            double initialStemThickness1 = 5.0;
            double initialLeafSize1 = 6.0;

            // Draw the recursive fractal fern using many randomized parameters.
            drawFractalFernRandomized(canvas, initialX1, initialY1, sliderSize, initialTheta1, sliderDepth, sliderTurnBias, initialStemThickness1, initialLeafSize1);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Initial parameters.
            double initialX2 = canvas.Width / 2 + 200;
            double initialY2 = canvas.Height;
            double initialTheta2 = Math.PI / 3;
            double initialStemThickness2 = 5.0;
            double initialLeafSize2 = 6.0;

            // Draw the recursive fractal fern using many randomized parameters.
            drawFractalFernRandomized(canvas, initialX2, initialY2, sliderSize, initialTheta2, sliderDepth, sliderTurnBias, initialStemThickness2, initialLeafSize2);

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Initial parameters.
            double initialX3 = canvas.Width / 2;
            double initialY3 = canvas.Height;
            double initialTheta3 = Math.PI / 2;
            double initialStemThickness3 = 5.0;
            double initialLeafSize3 = 6.0;

            // Draw the recursive fractal fern using mostly non-randomized parameters.
            drawFractalFernNonRandomized(canvas, initialX3, initialY3, sliderSize + 25, initialTheta3, sliderDepth, sliderTurnBias, initialStemThickness3, initialLeafSize3);

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Draw the fractal fern with many randomized parameters.
        /// </summary>
        /// <param name="canvas">canvas object to draw on</param>
        /// <param name="x">x-coordinate value</param>
        /// <param name="y">y-coordinate value</param>
        /// <param name="distance">length to draw</param>
        /// <param name="theta">angle to draw</param>
        /// <param name="depth">recursive depth value</param>
        /// <param name="turnbias">amount to bend the fern plant</param>
        /// <param name="stemThickness">thickness of the fern's stem</param>
        /// <param name="leafSize">radius of the fern's elliptical leaves</param>
        private void drawFractalFernRandomized(Canvas canvas, double x, double y, double distance, double theta, double depth, double turnBias, double stemThickness, double leafSize)
        {
            // Recursion base case.
            if (depth <= 0.5)
            {
                return;
            }

            // RNG.
            Random random = new Random();

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
            drawFernFrondDetail(canvas, x, y, distance / varyFrondDistanceRatio, theta + varyFrondAngleOffsetAmount, depth / varyFrondDepthReductionRatio, leafSize);

            // Calculate new coordinates.
            double varyDistance = random.Next(Convert.ToInt32(distance) - 3, Convert.ToInt32(distance) + 3);
            double newX = x + varyDistance * Math.Cos(theta);
            double newY = y - varyDistance * Math.Sin(theta);

            // Beginning of the stem of the fern.
            drawMyLine(canvas, x, newX, y, newY, stemThickness / 1.5);

            // Calculate new coordinates.
            double varyDistance2 = random.Next(Convert.ToInt32(distance) - 3, Convert.ToInt32(distance) + 3);
            double newX1 = x + varyDistance2 * Math.Cos(theta);
            double newY1 = y - varyDistance2 * Math.Sin(theta);

            // Draw the right fronds.
            drawFractalFernRandomized(canvas, newX1, newY1, distance / varyFrondDistanceRatio, theta + varyFrondAngleOffsetAmount, depth / varyFrondDepthReductionRatio,
                turnBias, stemThickness / 1.1, leafSize / 1.1);

            // Calculate new coordinates.
            double varyDistance3 = random.Next(Convert.ToInt32(distance) - 3, Convert.ToInt32(distance) + 3);
            double newX2 = x + varyDistance3 * Math.Cos(theta);
            double newY2 = y - varyDistance3 * Math.Sin(theta);

            // Draw the left fronds.
            drawFractalFernRandomized(canvas, newX2, newY2, distance / varyFrondDistanceRatio, theta - varyFrondAngleOffsetAmount, depth / varyFrondDepthReductionRatio,
                turnBias, stemThickness / 1.1, leafSize / 1.1);

            // Draw the remaining parts of the main stem of the fern.
            // (randomizes whether it turns left or right (so turnBias slider won't always make it go one direction or the other)
            int chooseTurnDirection = random.Next(1, 4);

            if (chooseTurnDirection == 1 || chooseTurnDirection == 2)
            {
                drawFractalFernRandomized(canvas, newX, newY, distance / varyStemDistanceRatio, theta + varyStemAngleOffsetAmount, depth / varyStemDepthReductionRatio,
                    turnBias, stemThickness / 1.1, leafSize / 1.1);
            }
            else
            {
                drawFractalFernRandomized(canvas, newX, newY, distance / varyStemDistanceRatio, theta - varyStemAngleOffsetAmount, depth / varyStemDepthReductionRatio,
                    turnBias, stemThickness / 1.1, leafSize / 1.1);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Draw the fractal fern without as much randomization. (a lot less randomized parameters)
        /// (aka a boring plain old stereotypical fern)
        /// </summary>
        /// <param name="canvas">canvas object to draw on</param>
        /// <param name="x">x-coordinate value</param>
        /// <param name="y">y-coordinate value</param>
        /// <param name="distance">length to draw</param>
        /// <param name="theta">angle to draw</param>
        /// <param name="depth">recursive depth value</param>
        /// <param name="turnbias">amount to bend the fern plant</param>
        /// <param name="stemThickness">thickness of the fern's stem</param>
        /// <param name="leafSize">radius of the fern's elliptical leaves</param>
        private void drawFractalFernNonRandomized(Canvas canvas, double x, double y, double distance, double theta, double depth, double turnBias, double stemThickness, double leafSize)
        {
            // Recursion base case.
            if (depth <= 0.5)
            {
                return;
            }

            // RNG.
            Random random = new Random();

            // Constraints on the randomness.
            //double ellipseRadiusMax = 6;
            //double ellipseRadiusMin = 3;

            // Random number generators.
            //double varyEllipseRadius = random.NextDouble() * (ellipseRadiusMax - ellipseRadiusMin) + ellipseRadiusMin;

            // "leaves" of the fern.
            drawFernFrondDetail(canvas, x, y, distance / 2.5, theta + Math.PI / 6, depth / 1.45, leafSize);

            // Calculate new coordinates.
            double newX = x + distance * Math.Cos(theta);
            double newY = y - distance * Math.Sin(theta);

            // Beginning of the stem of the fern.
            drawMyLine(canvas, x, newX, y, newY, stemThickness);

            // Calculate new coordinates.
            double newX1 = x + distance * Math.Cos(theta);
            double newY1 = y - distance * Math.Sin(theta);

            // Draw the right fronds.
            drawFractalFernNonRandomized(canvas, newX1, newY1, distance / 2.5, theta + Math.PI / 6, depth / 1.45, turnBias, stemThickness / 1.1, leafSize / 1.1);

            // Calculate new coordinates.
            double newX2 = x + distance * Math.Cos(theta);
            double newY2 = y - distance * Math.Sin(theta);

            // Draw the left fronds.
            drawFractalFernNonRandomized(canvas, newX2, newY2, distance / 2.5, theta - Math.PI / 6, depth / 1.45, turnBias, stemThickness / 1.1, leafSize / 1.1);

            // Draw the remaining parts of the main stem of the fern.
            drawFractalFernNonRandomized(canvas, newX, newY, distance / 1.15, theta + turnBias, depth / 1.25, turnBias, stemThickness / 1.1, leafSize / 1.1);

        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Draw the frond details of the fern at each level. (leaves, etc.)
        /// </summary>
        /// <param name="canvas">canvas object to draw on</param>
        /// <param name="x">x-coordinate value</param>
        /// <param name="y">y-coordinate value</param>
        /// <param name="distance">length to draw</param>
        /// <param name="theta">angle to draw</param>
        /// <param name="depth">recursive depth value</param>
        /// <param name="radius">radius of the elliptical object</param>
        private void drawFernFrondDetail(Canvas canvas, double x, double y, double distance, double theta, double depth, double radius)
        {
            drawMyEllipse(canvas, x, y, radius);
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Set-up a background image to draw on.
        /// Note: hopefully there's no portability issues with the background image.
        /// </summary>
        /// <param name="canvas">canvas object to draw on</param>
        private void setupBackground(Canvas canvas)
        {
            // Set canvas to the background image.
            try
            {
                ImageBrush myImageBrush = new ImageBrush();
                //myImageBrush.ImageSource = new BitmapImage(new Uri("D:/Dropbox/CS_262/CS212_Project3/FractalFern/patchygrass_1.jpg", UriKind.RelativeOrAbsolute));
                myImageBrush.ImageSource = new BitmapImage(new Uri("patchygrass_1.jpg", UriKind.RelativeOrAbsolute));

                canvas.Background = myImageBrush;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Source, "Argument exception: {0}");
                MessageBox.Show("Failed to load required files");
                return;
            }

            // This section doesn't do much at the moment.  Was going to try a different way of drawing.
            ///////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////

            // Declare objects necessary for the process.
            System.Drawing.Image sourceImage;
            Bitmap sourceBitmap;
            Graphics sourceGraphics;

            // Create Image object from user specified image file path.
            try
            {
                sourceImage = System.Drawing.Image.FromFile("patchygrass_1.jpg");
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

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method draws a ellipse with specified parameters.
        /// </summary>
        /// <param name="canvas">canvas object to draw on</param>
        /// <param name="x">x-coordinate value</param>
        /// <param name="y">y-coordinate value</param>
        /// <param name="radius">radius of the elliptical object</param>
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
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.GreenYellow, 0.0));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 0.25));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.ForestGreen, 0.75));
            radialGradientBrush.GradientStops.Add(new GradientStop(Colors.DarkSeaGreen, 1.0));

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

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method draws a line with specified parameters.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="x1">starting x-coordinate of line segment</param>
        /// <param name="x2">ending x-coordinate of line segment</param>
        /// <param name="y1">starting y-coordinate of line segment</param>
        /// <param name="y2">ending y-coordinate of line segment</param>
        /// <param name="thickness">stroke thickness of line segment</param>
        private static void drawMyLine(Canvas canvas, double x1, double x2, double y1, double y2, double thickness)
        {
            // RNG.
            Random random = new Random();

            // Constraints.
            double strokeThicknessMin = 1.0;
            double strokeThicknessMax = 1.2;

            // Generate random numbers. (not used in current implementation)
            double randomStrokeThickness = random.NextDouble() * (strokeThicknessMax - strokeThicknessMin) + strokeThicknessMin;

            int randomAlpha = random.Next(200, 250);
            int randomRed = random.Next(200, 220);
            int randomGreen = random.Next(245, 255);
            int randomBlue = random.Next(250, 255);

            // Create a horizontal linear gradient. 
            System.Windows.Media.LinearGradientBrush horizontalGradientBrush =
                new System.Windows.Media.LinearGradientBrush();

            // Set the starting and ending point of the stops. (controls angle of gradient)
            horizontalGradientBrush.StartPoint = new System.Windows.Point(0, 0.5);
            horizontalGradientBrush.EndPoint = new System.Windows.Point(1, 0.5);

            // Create four gradient stops.
            horizontalGradientBrush.GradientStops.Add(new GradientStop(Colors.GreenYellow, 0.0));
            horizontalGradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 0.25));
            horizontalGradientBrush.GradientStops.Add(new GradientStop(Colors.ForestGreen, 0.75));
            horizontalGradientBrush.GradientStops.Add(new GradientStop(Colors.DarkSeaGreen, 1.0));

            // Create a SolidColorBrush with a single color.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = System.Windows.Media.Color.FromArgb(Convert.ToByte(randomAlpha), Convert.ToByte(randomRed), Convert.ToByte(randomGreen), Convert.ToByte(randomBlue));

            // Settings for the line.
            Line myLine = new Line();
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            myLine.Stroke = horizontalGradientBrush;
            myLine.VerticalAlignment = VerticalAlignment.Center;
            myLine.HorizontalAlignment = HorizontalAlignment.Left;
            myLine.StrokeThickness = thickness;

            // Draw to canvas.
            canvas.Children.Add(myLine);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// This class is needed to enable us to set the center for an ellipse (not built in?!)
    /// </summary>
    public static class EllipseX
    {
        /// <summary>
        /// This method sets the center for an ellipse.
        /// </summary>
        /// <param name="ellipse">ellipse object</param>
        /// <param name="X">x-coordinate value</param>
        /// <param name="Y">y-coordinate value</param>
        public static void SetCenter(this Ellipse ellipse, double X, double Y)
        {
            Canvas.SetTop(ellipse, Y - ellipse.Height / 2);
            Canvas.SetLeft(ellipse, X - ellipse.Width / 2);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

}
