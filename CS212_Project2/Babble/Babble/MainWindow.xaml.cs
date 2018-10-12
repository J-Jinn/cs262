/// <summary>
/// Project 2: Babble (pseudo-randomized text generation)
/// CS-212 Data Structures and Algorithms
/// Section: A
/// Instructor: Professor Plantinga
/// Date: 10-4-18
/// </summary>
/// 
/// Babble framework
/// Using the starter code for CS212 Babble assignment
/// 

// Import statements.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

// Namespace of solution.
namespace Babble
{
    /// <summary>
    /// Class for MainWindow.xaml.
    /// Contains all relevant code for UI components and Babble algorithm.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Class members.
        private bool debug1 = false;         // For debug purposes.
        private bool debug2 = false;         // For debug purposes.
        private bool debug3 = false;         // For debug purposes.
        private bool debug4 = false;         // For debug purposes.

        private string input;               // input file
        private string[] words;             // input file broken into array of words
        private int wordCount = 200;        // number of words to babble

        // Hash table to store the word keys and succeeding words used to randomize text.
        private Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();

        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method initializes the GUI from MainWindows.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method shows open file dialog box upon clicking the load file button.
        /// Parses the selected file upon clicking open in file dialog box.
        /// 
        /// Also generates statistics for currently selected order.
        /// 
        /// Note: I'm using the default tokenization provided by Professor Plantinga.
        /// 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.FileName = "Sample"; // Default file name
            ofd.DefaultExt = ".txt"; // Default file extension
            ofd.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            if ((bool)ofd.ShowDialog())
            {
                textBlock1.Text = "Loading file " + ofd.FileName + "\n";
                input = System.IO.File.ReadAllText(ofd.FileName);  // read file
                words = Regex.Split(input, @"\s+");       // split into array of words

                // Method call to compute statistics upon loading a file with currently selected order.
                computeStatistics();
            }

            // Debug statement.  Shows content of parsed input file in textBlock1.
            if (debug1 == true)
            {
                // Clear textBlock of previous text.
                textBlock1.Text = "";

                // Display the contents of the source input file.
                // Note: Will cause hang-ups if file is very large!
                for (int i = 0; i < words.Length; i++)
                {
                    textBlock1.Text += " " + words[i];
                }
            }
        }

        /// <summary>
        /// Method compute N-th order statistics.
        /// 
        /// There's probably a better way to do this than manual string concatenation for each
        /// of the cases I have to take into account, but....I'm out of fresh ideas. So, please
        /// forgive the length of my code-base and the # of nested if, else ifs, and other 
        /// conditional checks/statements.
        /// 
        /// Hopefully, this isn't making grading too difficult.
        /// 
        /// </summary>
        private void computeStatistics()
        {
            // Clear hash table everytime we compute new statistics.
            hashTable.Clear();

            // Stores the current key being operated on.
            String wordkey;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // The for loop creates keys, adds them to hash table, and adds words as elements of those keys to the hash table.
            // Based on the entirety of the input file.
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < words.Length; i++)
            {

                /////////////////////////////////////////////////////////////////
                // Creates keys for hashtable based on the selected order.
                /////////////////////////////////////////////////////////////////

                if (orderComboBox.SelectedIndex == 0)
                {
                    // Current element of the string[] array to consider.
                    wordkey = words[i];
                }
                // Current two successive elements of the string[] array to consider.
                else if (orderComboBox.SelectedIndex == 1)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 1 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1];
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        wordkey = words[i] + " " + words[(i + 1) % words.Length];
                    }
                }
                // Current three successive elements of the string[] array to consider.
                else if (orderComboBox.SelectedIndex == 2)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 2 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2];
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        wordkey = words[i] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length];
                    }
                }
                // Current four successive elements of the string[] array to consider.
                else if (orderComboBox.SelectedIndex == 3)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 3 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3];
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        wordkey = words[i] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length]
                            + " " + words[(i + 3) % words.Length];
                    }
                }
                // Current five successive elements of the string[] array to consider.
                else if (orderComboBox.SelectedIndex == 4)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 4 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4];
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        wordkey = words[i] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length]
                            + " " + words[(i + 3) % words.Length] + " " + words[(i + 4) % words.Length];
                    }
                }
                // Current six successive elements of the string[] array to consider.
                else if (orderComboBox.SelectedIndex == 5)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 5 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4] + " " + words[i + 5];
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        wordkey = words[i] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length]
                            + " " + words[(i + 3) % words.Length] + " " + words[(i + 4) % words.Length]
                            + " " + words[(i + 5) % words.Length];
                    }
                }
                else
                {
                    Console.WriteLine("Something funky happened!");
                    return;
                }

                ///////////////////////////////////////////////////////////////////////////////////////
                // Add key to hash table if not already found in table.

                // Note: side-benefit of this is that all keys are unique and thus allows us to
                // determine the # of unique words or word sequences based on the selected order.
                ///////////////////////////////////////////////////////////////////////////////////////

                if (!hashTable.ContainsKey(wordkey))
                {
                    // Add key to hash table.
                    hashTable.Add(wordkey, new ArrayList());
                }

                ///////////////////////////////////////////////////////////////////////////////////////
                // If key already exists, simply add the word as part of that key's ArrayList.
                // This is based on the selected order.
                //
                // Note: side-benefit of this is that it allows addition of duplicate elements in the
                // ArrayList associated with each key.  Thus, random selection of a successor is based
                // on the weighted frequencey of occurence of the successor(s)
                ///////////////////////////////////////////////////////////////////////////////////////

                if (orderComboBox.SelectedIndex == 0)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 1 < words.Length)
                    {
                        // Add word following key as the value attached to the key.
                        hashTable[wordkey].Add(words[i + 1]);
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        // Add word at beginning of file as we have reached end of file.
                        hashTable[wordkey].Add(words[(i + 1) % words.Length]);
                    }
                }
                else if (orderComboBox.SelectedIndex == 1)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 2 < words.Length)
                    {
                        // Add word following key as the value attached to the key.
                        hashTable[wordkey].Add(words[i + 2]);
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        // Add word based on modulus as we have reached end of file.
                        hashTable[wordkey].Add(words[(i + 2) % words.Length]);
                    }
                }
                else if (orderComboBox.SelectedIndex == 2)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 3 < words.Length)
                    {
                        // Add word following key as the value attached to the key.
                        hashTable[wordkey].Add(words[i + 3]);
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        // Add word based on modulus as we have reached end of file.
                        hashTable[wordkey].Add(words[(i + 3) % words.Length]);
                    }
                }
                else if (orderComboBox.SelectedIndex == 3)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 4 < words.Length)
                    {
                        // Add word following key as the value attached to the key.
                        hashTable[wordkey].Add(words[i + 4]);
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        // Add word based on modulus as we have reached end of file.
                        hashTable[wordkey].Add(words[(i + 4) % words.Length]);
                    }
                }
                else if (orderComboBox.SelectedIndex == 4)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 5 < words.Length)
                    {
                        // Add word following key as the value attached to the key.
                        hashTable[wordkey].Add(words[i + 5]);
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        // Add word based on modulus as we have reached end of file.
                        hashTable[wordkey].Add(words[(i + 5) % words.Length]);
                    }
                }
                else if (orderComboBox.SelectedIndex == 5)
                {
                    // Condition where we haven't reached the end of the input file.
                    if (i + 6 < words.Length)
                    {
                        // Add word following key as the value attached to the key.
                        hashTable[wordkey].Add(words[i + 6]);
                    }
                    // Condition where we have reached the end of the input file.
                    else
                    {
                        // Add word based on modulus as we have reached end of file.
                        hashTable[wordkey].Add(words[(i + 6) % words.Length]);
                    }
                }

                ///////////////////////////////////////////////////////////////////////////////////////

                // Debug statement.
                // Note: Don't enable for large input files, otherwise it'll take forever!!!
                if (debug2 == true)
                {
                    // Get the ArrayList corresponding to the current key.
                    ArrayList list = hashTable[wordkey];

                    Console.WriteLine("Key is: {0}", wordkey);
                    Console.Write("Elements in ArrayList associated with key are: ");

                    for (int j = 0; j < list.Count; j++)
                    {
                        Console.Write(list[j]);
                        Console.Write("<=>");
                    }

                    Console.WriteLine("\n");
                }
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Display some statistics concerning the currently parsed and tokenized input file to the GUI.
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Get number of keys in hash table (corresponds to unique elements since I don't allow entry of duplicate keys)
            int distinctWords = hashTable.Count;

            // Clear textBlock each time.
            textBlock1.Text = "";

            // Display the # of words in the input file.
            textBlock1.Text += "\n" + "Number of words in input file: \n" + words.Length;

            // Determine the order that we have currently selected.
            int order = orderComboBox.SelectedIndex + 1;

            // Display the # of distinct words or word sequences in the input file depending on order selected.
            textBlock1.Text += "\n" + "Currently selected order is: " + order;
            textBlock1.Text += "\n" + "Number of unique words or word sequences in input file based on current order is: " + distinctWords;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method displays a MessageBox showing the selected Order N.
        /// </summary>
        /// 
        /// <param name="order">selected index value of orderComboBox UI widget.</param>
        private void analyzeInput(int order)
        {
            if (order > 0)
            {
                MessageBox.Show("Analyzing at order: " + (order + 1));
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method tracks user selection of the Order N in the ComboBox widget.
        /// Passes the index value to the analyzeInput(int order) method.
        /// </summary>
        /// 
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Event handler fires during creation, so make sure string[] words is not null before attempt to compute statistics.
            if (words != null)
            {
                // Method call to compute statistics.
                computeStatistics();
            }

            // Method call to display a message box showing the currently selected order.
            analyzeInput(orderComboBox.SelectedIndex);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method responds to Babble button click, computes, and displays the randomized text.
        /// 
        /// Randomization is achieved via a random number generator in the range of the ArrayList
        /// associated with the key in the Hash Table.
        /// </summary>
        /// 
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            // Condition for user clicking Babble without loading a input file first.
            if (words == null)
            {
                textBlock1.Text = "Please load an input file first before attempting to generate text!";
                return;
            }

            // Stores the current key being operated on.
            String wordkey;

            // Store all the randomized text before outputting to textBlock.
            String babble = "";

            // Create random number generator for word selection based on keys.
            Random random = new Random();

            // Clear textBlock of previous text.
            textBlock1.Text = " ";

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // For loop generates randomized text up to the # of words specified in the wordCount limit variable.
            //
            // This section determines the key to use per iteration of the loop to generate the randomized text.
            // Based on the order selected.
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////

            for (int i = 0; i < wordCount; i++)
            {
                if (orderComboBox.SelectedIndex == 0)
                {
                    // Use next word in file to determine ArrayList of following words.
                    if (i < words.Length)
                    {
                        wordkey = words[i];
                    }
                    // We have exceeded original length of input file, start over.
                    else
                    {
                        wordkey = words[(i + 1) % words.Length];
                    }
                }
                else if (orderComboBox.SelectedIndex == 1)
                {
                    // Use next succeeding two words in file to determine key to use.
                    if (i + 1 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1];
                    }
                    // We have exceeded original length of input file, start over.
                    else
                    {
                        wordkey = words[(i) % words.Length] + " " + words[(i + 1) % words.Length];
                    }
                }
                else if (orderComboBox.SelectedIndex == 2)
                {
                    // Use next succeeding three words in file to determine key to use.
                    if (i + 2 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2];
                    }
                    // We have exceeded original length of input file, start over.
                    else
                    {
                        wordkey = words[(i) % words.Length] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length];
                    }
                }
                else if (orderComboBox.SelectedIndex == 3)
                {
                    // Use next succeeding four words in file to determine key to use.
                    if (i + 3 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3];
                    }
                    // We have exceeded original length of input file, start over.
                    else
                    {
                        wordkey = words[(i) % words.Length] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length] + " "
                              + words[(i + 3) % words.Length];
                    }
                }
                else if (orderComboBox.SelectedIndex == 4)
                {
                    // Use next succeeding five words in file to determine key to use.
                    if (i + 4 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4];
                    }
                    // We have exceeded original length of input file, start over.
                    else
                    {
                        wordkey = words[(i) % words.Length] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length] + " "
                             + words[(i + 3) % words.Length] + " " + words[(i + 4) % words.Length];
                    }
                }
                else if (orderComboBox.SelectedIndex == 5)
                {
                    // Use next succeeding six words in file to determine key to use.
                    if (i + 5 < words.Length)
                    {
                        wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4] + " " + words[i + 5];
                    }
                    // We have exceeded original length of input file, start over.
                    else
                    {
                        wordkey = words[(i) % words.Length] + " " + words[(i + 1) % words.Length] + " " + words[(i + 2) % words.Length] + " "
                            + words[(i + 3) % words.Length] + " " + words[(i + 4) % words.Length] + " " + words[(i + 5) % words.Length];
                    }
                }
                else
                {
                    Console.WriteLine("Something funky happened!");
                    return;
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                // Different way of generating random text - deprecated in favor of the above.
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                //if (orderComboBox.SelectedIndex == 0)
                //{
                //    // Condition for first word of the file.
                //    // Because we can't look at the previous word at this index.
                //    if (i == 0)
                //    {
                //        wordkey = words[s % words.Length];
                //    }
                //    // Use previous word in file to determine ArrayList of following words.
                //    else if (i <= words.Length)
                //    {
                //        wordkey = words[i - 1];
                //    }
                //    // We have exceeded original length of input file, start over.
                //    else
                //    {
                //        wordkey = words[(i - 1) % words.Length];
                //    }
                //}
                //else if (orderComboBox.SelectedIndex == 1)
                //{
                //    // Condition for first and second words of the file.
                //    // Because we can't look at the previous two words at these indices.
                //    if (i == 0 || i == 1)
                //    {
                //        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length];
                //    }
                //    // Use previous two words in file to determine key to use.
                //    else if (i <= words.Length)
                //    {
                //        wordkey = words[i - 2] + " " + words[i - 1];
                //    }
                //    // We have exceeded original length of input file, start over.
                //    else
                //    {
                //        wordkey = words[(i - 2) % words.Length] + " " + words[(i - 1) % words.Length];
                //    }
                //}
                //else if (orderComboBox.SelectedIndex == 2)
                //{
                //    // Condition for first, second, and third words of the file.
                //    // Because we can't look at the previous three words at these indices.
                //    if (i == 0 || i == 1 || i == 2)
                //    {
                //        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                //            + " " + words[(s + 2) % words.Length];
                //    }
                //    // Use previous three words in file to determine key to use.
                //    else if (i <= words.Length)
                //    {
                //        wordkey = words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                //    }
                //    // We have exceeded original length of input file, start over.
                //    else
                //    {
                //        wordkey = words[(i - 3) % words.Length] + " " + words[(i - 2) % words.Length] + " " + words[(i - 1) % words.Length];
                //    }
                //}
                //else if (orderComboBox.SelectedIndex == 3)
                //{
                //    // Condition for first, second, third, and fourth words of the file.
                //    // Because we can't look at the previous four words at these indices.
                //    if (i == 0 || i == 1 || i == 2 || i == 3)
                //    {
                //        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                //            + " " + words[(s + 2) % words.Length] + " " + words[(s + 3) % words.Length];
                //    }
                //    // Use previous four words in file to determine key to use.
                //    else if (i <= words.Length)
                //    {
                //        wordkey = words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                //    }
                //    // We have exceeded original length of input file, start over.
                //    else
                //    {
                //        wordkey = words[(i - 4) % words.Length] + " " + words[(i - 3) % words.Length] + " " + words[(i - 2) % words.Length] + " "
                //            + words[(i - 1) % words.Length];
                //    }
                //}
                //else if (orderComboBox.SelectedIndex == 4)
                //{
                //    // Condition for first, second, third, fourth, and fifth words of the file.
                //    // Because we can't look at the previous five words at these indices.
                //    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
                //    {
                //        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                //            + " " + words[(s + 2) % words.Length] + " " + words[(s + 3) % words.Length]
                //            + " " + words[(s + 4) % words.Length];
                //    }
                //    // Use previous five words in file to determine key to use.
                //    else if (i <= words.Length)
                //    {
                //        wordkey = words[i - 5] + " " + words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                //    }
                //    // We have exceeded original length of input file, start over.
                //    else
                //    {
                //        wordkey = words[(i - 5) % words.Length] + " " + words[(i - 4) % words.Length] + " " + words[(i - 3) % words.Length] + " "
                //            + words[(i - 2) % words.Length] + " " + words[(i - 1) % words.Length];
                //    }
                //}
                //else if (orderComboBox.SelectedIndex == 5)
                //{
                //    // Condition for first, second, third, fourth, fifth, and sixth words of the file.
                //    // Because we can't look at the previous six words at these indices.
                //    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5)
                //    {
                //        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                //            + " " + words[(s + 2) % words.Length] + " " + words[(s + 3) % words.Length]
                //            + " " + words[(s + 4) % words.Length] + " " + words[(s + 5) % words.Length];
                //    }
                //    // Use previous six words in file to determine key to use.
                //    else if (i <= words.Length)
                //    {
                //        wordkey = words[i - 6] + " " + words[i - 5] + " " + words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                //    }
                //    // We have exceeded original length of input file, start over.
                //    else
                //    {
                //        wordkey = words[(i - 6) % words.Length] + " " + words[(i - 5) % words.Length] + " " + words[(i - 4) % words.Length] + " "
                //            + words[(i - 3) % words.Length] + " " + words[(i - 2) % words.Length] + " " + words[(i - 1) % words.Length];
                //    }
                //}
                //else
                //{
                //    Console.WriteLine("Something funky happened!");
                //    return;
                //}

                ////////////////////////////////////////////////////////////////////////////////////////////////////
                // Use a random number generator to randomly select the succeeding word to output to the GUI.
                // Per iteration of the for loop for each key.
                ////////////////////////////////////////////////////////////////////////////////////////////////////

                // Get the ArrayList corresponding to the current key.
                ArrayList list = hashTable[wordkey];

                // Determine the # of items in the ArrayList.
                // Note to dumb self: index of ArrayList starts at 0 and ends at list.Count - 1 for existing items;
                int numItems = list.Count - 1;

                // Randomly select an item from the ArrayList.
                // Note to self: Ensure range is between 0 to list.Count - 1;
                int r = random.Next(0, numItems);

                // Get the ArrayList associated with that key.
                ArrayList items = hashTable[wordkey];

                // Add the randomly selected word to the string variable.
                babble += " " + items[r];
            }

            // Display the randomized text to the textblock object in GUI
            textBlock1.Text = babble;
        }
    }
}
