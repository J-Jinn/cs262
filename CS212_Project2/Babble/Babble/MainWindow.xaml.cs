using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Babble
{
    /// <summary>
    /// Project 2: Babble (pseudo-randomized text generation)
    /// CS-212 Data Structures and Algorithms
    /// Section: A
    /// Instructor: Professor Plantinga
    /// Date: 10-4-18
    /// </summary>
    /// Babble framework
    /// Starter code for CS212 Babble assignment
    public partial class MainWindow : Window
    {
        // Class members.
        private bool debug1 = false;        // For debug purposes.
        private bool debug2 = false;         // For debug purposes.
        private bool debug3 = true;         // For debug purposes.
        private bool debug4 = true;         // For debug purposes.

        private string input;               // input file
        private string[] words;             // input file broken into array of words
        private int wordCount = 200;        // number of words to babble

        // Hash table to store the word keys and succeeding words used to randomize text.
        private Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();

        //// Hash table to store occurence frequencies of each word.
        //private Dictionary<string, int> wordDuplicate = new Dictionary<string, int>();

        /// <summary>
        /// Method initializes the GUI from MainWindows.xaml
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method shows open file dialog box upon clicking the load file button.
        /// Parses the selected file upon clicking open in file dialog box.
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
        /// Method displays a MessageBox showing the selected Order N.
        /// </summary>
        /// <param name="order">index value</param>
        private void analyzeInput(int order)
        {
            if (order > 0)
            {
                MessageBox.Show("Analyzing at order: " + order);
            }
        }

        /// <summary>
        /// Method tracks user selection of the Order N in the ComboBox widget.
        /// Passes the index value to the analyzeInput(int order) method.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            analyzeInput(orderComboBox.SelectedIndex);
        }

        /// <summary>
        /// Method responds to  Babble button click and displays the randomized text.
        /// Adds each elements of the string[] array to the TextBlock object in the GUI.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void babbleButton_Click(object sender, RoutedEventArgs e)
        {
            // Stores the current key being operated on.
            String wordkey;

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // The for loop creates keys, adds them to hash table, and adds words as elements of those keys to the hash table.
            // Based on the entirety of the input file or wordCount, if the input file is longer than the imposed limit.
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
                ///////////////////////////////////////////////////////////////////////////////////////

                if (!hashTable.ContainsKey(wordkey))
                {
                    // Add key to hash table.
                    hashTable.Add(wordkey, new ArrayList());
                }

                ///////////////////////////////////////////////////////////////////////////////////////
                // If key already exists, simply add the word as part of that key's ArrayList.
                // This is based on the selected order.
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

                //// Add word to ArrayList only if it doesn't already exist.
                //if (!hashTable[wordkey].Contains(words[i + 1]))
                //{
                //    if (i + 1 < Math.Min(wordCount, words.Length))
                //    {
                //        // Add word following key as the value attached to the key.
                //        hashTable[wordkey].Add(words[i + 1]);
                //    }
                //    else
                //    {
                //        // Add word at beginning of file as we have reached end of file.
                //        hashTable[wordkey].Add(words[0]);
                //    }
                //}
                //// Track the occurence of duplicate words.
                //else if (hashTable[wordkey].Contains(words[i + 1]))
                //{
                //    // Add word to hash table tracking duplicates if not already in table.
                //    if (!wordDuplicate.ContainsKey(words[i + 1]))
                //    {
                //        wordDuplicate.Add(words[i + 1], 1);
                //    }
                //    // If word is already in hash table tracking duplicates, simply increments its occurence value.
                //    else
                //    {
                //        wordDuplicate[words[i + 1]]++;
                //        Console.WriteLine("Word we are keeping track of: {0}", words[i + 1]);
                //        Console.WriteLine("Value of wordDuplicate counter: {0}", wordDuplicate[words[i + 1]]);
                //    }
                //}

            }

            // Create random number generator for word selection based on keys.
            Random random = new Random();

            // Clear textBlock of previous text.
            textBlock1.Text = " ";

            ////////////////////////////////////////////////////////////////////////////////////////////////////
            // Generate randomized text up to the # of words in the input file or the wordCount limit.
            ////////////////////////////////////////////////////////////////////////////////////////////////////

            // TODO : randomize the special cases more randomly!!!!!

            for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            {
                // Random number for the special cases at the beginning of each input file.
                int s = random.Next(0, words.Length - 1);

                if (orderComboBox.SelectedIndex == 0)
                {
                    // Condition for first word of the file.
                    // Because we can't look at the previous word at this index.
                    if (i == 0)
                    {
                        wordkey = words[s % words.Length];
                    }
                    // Use previous word in file to determine ArrayList of following words.
                    else
                    {
                        wordkey = words[i - 1];
                    }
                }
                else if (orderComboBox.SelectedIndex == 1)
                {
                    // Condition for first and second words of the file.
                    // Because we can't look at the previous two words at these indices.
                    if (i == 0 || i == 1)
                    {
                        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length];
                    }
                    // Use previous two words in file to determine key to use.
                    else
                    {
                        wordkey = words[i - 2] + " " + words[i - 1];
                    }
                }
                else if (orderComboBox.SelectedIndex == 2)
                {
                    // Condition for first, second, and third words of the file.
                    // Because we can't look at the previous three words at these indices.
                    if (i == 0 || i == 1 || i == 2)
                    {
                        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                            + " " + words[(s + 2) % words.Length];
                    }
                    // Use previous three words in file to determine key to use.
                    else
                    {
                        wordkey = words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                    }
                }
                else if (orderComboBox.SelectedIndex == 3)
                {
                    // Condition for first, second, third, and fourth words of the file.
                    // Because we can't look at the previous four words at these indices.
                    if (i == 0 || i == 1 || i == 2 || i == 3)
                    {
                        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                            + " " + words[(s + 2) % words.Length] + " " + words[(s + 3) % words.Length];
                    }
                    // Use previous four words in file to determine key to use.
                    else
                    {
                        wordkey = words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                    }
                }
                else if (orderComboBox.SelectedIndex == 4)
                {
                    // Condition for first, second, third, fourth, and fifth words of the file.
                    // Because we can't look at the previous five words at these indices.
                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
                    {
                        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                            + " " + words[(s + 2) % words.Length] + " " + words[(s + 3) % words.Length]
                            + " " + words[(s + 4) % words.Length];
                    }
                    // Use previous five words in file to determine key to use.
                    else
                    {
                        wordkey = words[i - 5] + " " + words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                    }
                }
                else if (orderComboBox.SelectedIndex == 5)
                {
                    // Condition for first, second, third, fourth, fifth, and sixth words of the file.
                    // Because we can't look at the previous six words at these indices.
                    if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5)
                    {
                        wordkey = words[s % words.Length] + " " + words[(s + 1) % words.Length]
                            + " " + words[(s + 2) % words.Length] + " " + words[(s + 3) % words.Length]
                            + " " + words[(s + 4) % words.Length] + " " + words[(s + 5) % words.Length];
                    }
                    // Use previous six words in file to determine key to use.
                    else
                    {
                        wordkey = words[i - 6] + " " + words[i - 5] + " " + words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
                    }
                }
                else
                {
                    Console.WriteLine("Something funky happened!");
                    return;
                }

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

                // Add the randomly selected word to the textblock object in GUI.
                textBlock1.Text += " " + items[r];
            }

            // Clear hash table after every randomization.
            hashTable.Clear();

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Ignore everything below this line.  It's code that was experimental/failed and of a previous iteration.
            // Ignore everything below this line.  It's code that was experimental/failed and of a previous iteration.
            // Ignore everything below this line.  It's code that was experimental/failed and of a previous iteration.
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //// Order 1 - two words
            //if (orderComboBox.SelectedIndex == 1)
            //{
            //    //for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    //{
            //    //    //// Current two successive elements of the string[] array to consider.
            //    //    //if (i + 1 < Math.Min(wordCount, words.Length))
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[i + 1];
            //    //    //}
            //    //    //else
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[(i + 1) % Math.Min(wordCount, words.Length)];
            //    //    //}

            //    //    //// Debug statement.
            //    //    //Console.WriteLine("value of wordkey: {0}", wordkey);

            //    //    // Add key to hash table if not already found in table.
            //    //    if (!hashTable.ContainsKey(wordkey))
            //    //    {
            //    //        //// Add key to hash table.
            //    //        //hashTable.Add(wordkey, new ArrayList());
            //    //        //if (i + 2 < Math.Min(wordCount, words.Length))
            //    //        //{
            //    //        //    // Add word following key as the value attached to the key.
            //    //        //    hashTable[wordkey].Add(words[i + 2]);
            //    //        //}
            //    //        //else
            //    //        //{
            //    //        //    // Add word based on modulus as we have reached end of file.
            //    //        //    hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //}
            //    //    }
            //    //    // If key already exists, simply add the word as part of that key's ArrayList
            //    //    else
            //    //    {
            //    //        //// Add word following key as the value attached to the key.
            //    //        //hashTable[wordkey].Add(words[i + 2]);

            //    //        //// Add word to ArrayList only if it doesn't already exist.
            //    //        //if (!hashTable[wordkey].Contains(words[i + 2]))
            //    //        //{
            //    //        //    if (i + 2 < Math.Min(wordCount, words.Length))
            //    //        //    {
            //    //        //        // Add word following key as the value attached to the key.
            //    //        //        hashTable[wordkey].Add(words[i + 2]);
            //    //        //    }
            //    //        //    else
            //    //        //    {
            //    //        //        // Add word based on modulus as we have reached end of file.
            //    //        //        hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //    }
            //    //        //}
            //    //    }
            //    //}

            //    Random random = new Random();

            //    // Clear textBlock of previous text.
            //    textBlock1.Text = "";

            //    for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    {
            //        //// TODO: randomize these cases.
            //        //// Condition for first and second words of the file.
            //        //if (i == 0 || i == 1)
            //        //{
            //        //    wordkey = words[i % Math.Min(wordCount, words.Length)] + " " + words[i + 1 % Math.Min(wordCount, words.Length)];
            //        //}
            //        //// Use previous two words in file to determine key to use.
            //        //else
            //        //{
            //        //    wordkey = words[i - 2] + " " + words[i - 1];
            //        //}

            //        // Get the ArrayList corresponding to the current key.
            //        ArrayList list = hashTable[wordkey];

            //        // Determine the # of items in the ArrayList.
            //        int numItems = list.Count;

            //        // Randomly select an item from the ArrayList.
            //        int r = random.Next(numItems);

            //        // Get the ArrayList associated with that key.
            //        ArrayList items = hashTable[wordkey];

            //        // Add the randomly selected word to the textblock object in GUI.
            //        textBlock1.Text += " " + items[r];
            //    }
            //}

            //// Order 2 - three words
            //if (orderComboBox.SelectedIndex == 2)
            //{
            //    //for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    //{
            //    //    //// Current three successive elements of the string[] array to consider.
            //    //    //if (i + 2 < Math.Min(wordCount, words.Length))
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2];
            //    //    //}
            //    //    //else
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[(i + 1) % Math.Min(wordCount, words.Length)] + " " + words[(i + 2) % Math.Min(wordCount, words.Length)];
            //    //    //}

            //    //    //// Debug statement.
            //    //    //Console.WriteLine("value of wordkey: {0}", wordkey);

            //    //    // Add key to hash table if not already found in table.
            //    //    if (!hashTable.ContainsKey(wordkey))
            //    //    {
            //    //        //// Add key to hash table.
            //    //        //hashTable.Add(wordkey, new ArrayList());
            //    //        //if (i + 3 < Math.Min(wordCount, words.Length))
            //    //        //{
            //    //        //    // Add word following key as the value attached to the key.
            //    //        //    hashTable[wordkey].Add(words[i + 3]);
            //    //        //}
            //    //        //else
            //    //        //{
            //    //        //    // Add word based on modulus as we have reached end of file.
            //    //        //    hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //}
            //    //    }
            //    //    // If key already exists, simply add the word as part of that key's ArrayList
            //    //    else
            //    //    {

            //    //        //// Add word following key as the value attached to the key.
            //    //        //hashTable[wordkey].Add(words[i + 3]);

            //    //        //// Add word to ArrayList only if it doesn't already exist.
            //    //        //if (!hashTable[wordkey].Contains(words[i + 3]))
            //    //        //{
            //    //        //    if (i + 3 < Math.Min(wordCount, words.Length))
            //    //        //    {
            //    //        //        // Add word following key as the value attached to the key.
            //    //        //        hashTable[wordkey].Add(words[i + 3]);
            //    //        //    }
            //    //        //    else
            //    //        //    {
            //    //        //        // Add word based on modulus as we have reached end of file.
            //    //        //        hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //    }
            //    //        //}
            //    //    }
            //    //}

            //    Random random = new Random();

            //    // Clear textBlock of previous text.
            //    textBlock1.Text = "";

            //    for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    {
            //        //// Condition for first, second, and third words of the file.
            //        //if (i == 0 || i == 1 || i == 2)
            //        //{
            //        //    wordkey = words[i % Math.Min(wordCount, words.Length)] + " " + words[i + 1 % Math.Min(wordCount, words.Length)]
            //        //        + " " + words[i + 2 % Math.Min(wordCount, words.Length)];
            //        //}
            //        //// Use previous three words in file to determine key to use.
            //        //else
            //        //{
            //        //    wordkey = words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
            //        //}

            //        // Get the ArrayList corresponding to the current key.
            //        ArrayList list = hashTable[wordkey];

            //        // Determine the # of items in the ArrayList.
            //        int numItems = list.Count;

            //        // Randomly select an item from the ArrayList.
            //        int r = random.Next(numItems);

            //        // Get the ArrayList associated with that key.
            //        ArrayList items = hashTable[wordkey];

            //        // Add the randomly selected word to the textblock object in GUI.
            //        textBlock1.Text += " " + items[r];
            //    }
            //}

            //// Order 3 - four words
            //if (orderComboBox.SelectedIndex == 3)
            //{
            //    //for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    //{
            //    //    //// Current four successive elements of the string[] array to consider.
            //    //    //if (i + 3 < Math.Min(wordCount, words.Length))
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3];
            //    //    //}
            //    //    //else
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[(i + 1) % Math.Min(wordCount, words.Length)] + " " + words[(i + 2) % Math.Min(wordCount, words.Length)]
            //    //    //        + " " + words[(i + 3) % Math.Min(wordCount, words.Length)];
            //    //    //}

            //    //    //// Debug statement.
            //    //    //Console.WriteLine("value of wordkey: {0}", wordkey);

            //    //    // Add key to hash table if not already found in table.
            //    //    if (!hashTable.ContainsKey(wordkey))
            //    //    {
            //    //        //// Add key to hash table.
            //    //        //hashTable.Add(wordkey, new ArrayList());
            //    //        //if (i + 4 < Math.Min(wordCount, words.Length))
            //    //        //{
            //    //        //    // Add word following key as the value attached to the key.
            //    //        //    hashTable[wordkey].Add(words[i + 4]);
            //    //        //}
            //    //        //else
            //    //        //{
            //    //        //    // Add word based on modulus as we have reached end of file.
            //    //        //    hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //}
            //    //    }
            //    //    // If key already exists, simply add the word as part of that key's ArrayList
            //    //    else
            //    //    {
            //    //        //// Add word following key as the value attached to the key.
            //    //        //hashTable[wordkey].Add(words[i + 4]);

            //    //        //// Add word to ArrayList only if it doesn't already exist.
            //    //        //if (!hashTable[wordkey].Contains(words[i + 4]))
            //    //        //{
            //    //        //    if (i + 4 < Math.Min(wordCount, words.Length))
            //    //        //    {
            //    //        //        // Add word following key as the value attached to the key.
            //    //        //        hashTable[wordkey].Add(words[i + 4]);
            //    //        //    }
            //    //        //    else
            //    //        //    {
            //    //        //        // Add word based on modulus as we have reached end of file.
            //    //        //        hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //    }
            //    //        //}
            //    //    }
            //    //}

            //    Random random = new Random();

            //    // Clear textBlock of previous text.
            //    textBlock1.Text = "";

            //    for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    {
            //        //// Condition for first, second, and third words of the file.
            //        //if (i == 0 || i == 1 || i == 2 || i == 3)
            //        //{
            //        //    wordkey = words[i % Math.Min(wordCount, words.Length)] + " " + words[i + 1 % Math.Min(wordCount, words.Length)]
            //        //        + " " + words[i + 2 % Math.Min(wordCount, words.Length)] + " " + words[i + 3 % Math.Min(wordCount, words.Length)];
            //        //}
            //        //// Use previous four words in file to determine key to use.
            //        //else
            //        //{
            //        //    wordkey = words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
            //        //}

            //        // Get the ArrayList corresponding to the current key.
            //        ArrayList list = hashTable[wordkey];

            //        // Determine the # of items in the ArrayList.
            //        int numItems = list.Count;

            //        // Randomly select an item from the ArrayList.
            //        int r = random.Next(numItems);

            //        // Get the ArrayList associated with that key.
            //        ArrayList items = hashTable[wordkey];

            //        // Add the randomly selected word to the textblock object in GUI.
            //        textBlock1.Text += " " + items[r];
            //    }
            //}

            //// Order 4 - five words
            //if (orderComboBox.SelectedIndex == 4)
            //{
            //    //for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    //{
            //    //    //// Current five successive elements of the string[] array to consider.
            //    //    //if (i + 4 < Math.Min(wordCount, words.Length))
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4];
            //    //    //}
            //    //    //else
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[(i + 1) % Math.Min(wordCount, words.Length)] + " " + words[(i + 2) % Math.Min(wordCount, words.Length)]
            //    //    //        + " " + words[(i + 3) % Math.Min(wordCount, words.Length)] + " " + words[(i + 4) % Math.Min(wordCount, words.Length)];
            //    //    //}

            //    //    //// Debug statement.
            //    //    //Console.WriteLine("value of wordkey: {0}", wordkey);

            //    //    // Add key to hash table if not already found in table.
            //    //    if (!hashTable.ContainsKey(wordkey))
            //    //    {
            //    //        //// Add key to hash table.
            //    //        //hashTable.Add(wordkey, new ArrayList());
            //    //        //if (i + 5 < Math.Min(wordCount, words.Length))
            //    //        //{
            //    //        //    // Add word following key as the value attached to the key.
            //    //        //    hashTable[wordkey].Add(words[i + 5]);
            //    //        //}
            //    //        //else
            //    //        //{
            //    //        //    // Add word based on modulus as we have reached end of file.
            //    //        //    hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //}
            //    //    }
            //    //    // If key already exists, simply add the word as part of that key's ArrayList
            //    //    else
            //    //    {
            //    //        //// Add word following key as the value attached to the key.
            //    //        //hashTable[wordkey].Add(words[i + 5]);

            //    //        //// Add word to ArrayList only if it doesn't already exist.
            //    //        //if (!hashTable[wordkey].Contains(words[i + 5]))
            //    //        //{
            //    //        //    if (i + 4 < Math.Min(wordCount, words.Length))
            //    //        //    {
            //    //        //        // Add word following key as the value attached to the key.
            //    //        //        hashTable[wordkey].Add(words[i + 5]);
            //    //        //    }
            //    //        //    else
            //    //        //    {
            //    //        //        // Add word based on modulus as we have reached end of file.
            //    //        //        hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //    }
            //    //        //}
            //    //    }
            //    //}

            //    Random random = new Random();

            //    // Clear textBlock of previous text.
            //    textBlock1.Text = "";

            //    for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    {
            //        //// Condition for first, second, third, and fourth words of the file.
            //        //if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4)
            //        //{
            //        //    wordkey = words[i % Math.Min(wordCount, words.Length)] + " " + words[i + 1 % Math.Min(wordCount, words.Length)]
            //        //        + " " + words[i + 2 % Math.Min(wordCount, words.Length)] + " " + words[i + 3 % Math.Min(wordCount, words.Length)]
            //        //        + " " + words[i + 4 % Math.Min(wordCount, words.Length)];
            //        //}
            //        //// Use previous five words in file to determine key to use.
            //        //else
            //        //{
            //        //    wordkey = words[i - 5] + " " + words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
            //        //}

            //        // Get the ArrayList corresponding to the current key.
            //        ArrayList list = hashTable[wordkey];

            //        // Determine the # of items in the ArrayList.
            //        int numItems = list.Count;

            //        // Randomly select an item from the ArrayList.
            //        int r = random.Next(numItems);

            //        // Get the ArrayList associated with that key.
            //        ArrayList items = hashTable[wordkey];

            //        // Add the randomly selected word to the textblock object in GUI.
            //        textBlock1.Text += " " + items[r];
            //    }
            //}

            //// Order 5 - six words
            //if (orderComboBox.SelectedIndex == 5)
            //{
            //    //for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    //{
            //    //    //// Current five successive elements of the string[] array to consider.
            //    //    //if (i + 5 < Math.Min(wordCount, words.Length))
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[i + 1] + " " + words[i + 2] + " " + words[i + 3] + " " + words[i + 4] + " " + words[i + 5];
            //    //    //}
            //    //    //else
            //    //    //{
            //    //    //    wordkey = words[i] + " " + words[(i + 1) % Math.Min(wordCount, words.Length)] + " " + words[(i + 2) % Math.Min(wordCount, words.Length)]
            //    //    //        + " " + words[(i + 3) % Math.Min(wordCount, words.Length)] + " " + words[(i + 4) % Math.Min(wordCount, words.Length)]
            //    //    //        + " " + words[(i + 5) % Math.Min(wordCount, words.Length)];
            //    //    //}

            //    //    //// Debug statement.
            //    //    //Console.WriteLine("value of wordkey: {0}", wordkey);

            //    //    // Add key to hash table if not already found in table.
            //    //    if (!hashTable.ContainsKey(wordkey))
            //    //    {
            //    //        //// Add key to hash table.
            //    //        //hashTable.Add(wordkey, new ArrayList());
            //    //        //if (i + 6 < Math.Min(wordCount, words.Length))
            //    //        //{
            //    //        //    // Add word following key as the value attached to the key.
            //    //        //    hashTable[wordkey].Add(words[i + 6]);
            //    //        //}
            //    //        //else
            //    //        //{
            //    //        //    // Add word based on modulus as we have reached end of file.
            //    //        //    hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //}
            //    //    }
            //    //    // If key already exists, simply add the word as part of that key's ArrayList
            //    //    else
            //    //    {
            //    //        ////// Add word following key as the value attached to the key.
            //    //        //hashTable[wordkey].Add(words[i + 6]);

            //    //        //// Add word to ArrayList only if it doesn't already exist.
            //    //        //if (!hashTable[wordkey].Contains(words[i + 6]))
            //    //        //{
            //    //        //    if (i + 4 < Math.Min(wordCount, words.Length))
            //    //        //    {
            //    //        //        // Add word following key as the value attached to the key.
            //    //        //        hashTable[wordkey].Add(words[i + 6]);
            //    //        //    }
            //    //        //    else
            //    //        //    {
            //    //        //        // Add word based on modulus as we have reached end of file.
            //    //        //        hashTable[wordkey].Add(words[i % words.Length]);
            //    //        //    }
            //    //        //}
            //    //    }
            //    //}

            //    Random random = new Random();

            //    // Clear textBlock of previous text.
            //    textBlock1.Text = "";

            //    for (int i = 0; i < Math.Min(wordCount, words.Length); i++)
            //    {
            //        //// Condition for first, second, third, fourth, and fifth words of the file.
            //        //if (i == 0 || i == 1 || i == 2 || i == 3 || i == 4 || i == 5)
            //        //{
            //        //    wordkey = words[i % Math.Min(wordCount, words.Length)] + " " + words[i + 1 % Math.Min(wordCount, words.Length)]
            //        //        + " " + words[i + 2 % Math.Min(wordCount, words.Length)] + " " + words[i + 3 % Math.Min(wordCount, words.Length)]
            //        //        + " " + words[i + 4 % Math.Min(wordCount, words.Length)] + " " + words[i + 5 % Math.Min(wordCount, words.Length)];
            //        //}
            //        //// Use previous six words in file to determine key to use.
            //        //else
            //        //{
            //        //    wordkey = words[i - 6] + " " + words[i - 5] + " " + words[i - 4] + " " + words[i - 3] + " " + words[i - 2] + " " + words[i - 1];
            //        //}

            //        // Get the ArrayList corresponding to the current key.
            //        ArrayList list = hashTable[wordkey];

            //        // Determine the # of items in the ArrayList.
            //        int numItems = list.Count;

            //        // Randomly select an item from the ArrayList.
            //        int r = random.Next(numItems);

            //        // Get the ArrayList associated with that key.
            //        ArrayList items = hashTable[wordkey];

            //        // Add the randomly selected word to the textblock object in GUI.
            //        textBlock1.Text += " " + items[r];
            //    }
            //}
        }
    }

    /// <summary>
    /// Class Hash hashes the string[] containing the contents of the input file.
    /// </summary>
    //class Hash
    //{
    //    static Dictionary<string, ArrayList> makeHashtable()
    //    {
    //        String[] names = { "one", "two", "three", "four", "five", "six",
    //                         "seven", "two", "ten", "four" };
    //        Dictionary<string, ArrayList> hashTable = new Dictionary<string, ArrayList>();

    //        foreach (string name in names)
    //        {
    //            string firstLetter = name.Substring(0, 1);
    //            if (!hashTable.ContainsKey(firstLetter))
    //                hashTable.Add(firstLetter, new ArrayList());
    //            hashTable[firstLetter].Add(name);
    //        }
    //        return hashTable;
    //    }

    //    static void dump(Dictionary<string, ArrayList> hashTable)
    //    {
    //        foreach (KeyValuePair<string, ArrayList> entry in hashTable)
    //        {
    //            Console.Write("{0} -> ", entry.Key);
    //            foreach (string name in entry.Value)
    //                Console.Write("{0} ", name);
    //            Console.WriteLine();
    //        }
    //    }

    //    //static void Main(string[] args)
    //    //{
    //    //    Dictionary<string, ArrayList> hashTable = makeHashtable();
    //    //    dump(hashTable);
    //    //    Console.Write("\nPress enter to exit: ");
    //    //    Console.ReadLine();
    //    //}
    //}
}
