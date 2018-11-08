/// <summary>
/// Project 4: Dutch Bingo
/// CS-212 Data Structures and Algorithms
/// Section: B
/// Instructor: Professor Plantinga
/// Date: 11-08-18
/// </summary>
/// 
/// Dutch Bingo Relationship Graph framework
/// Modified from the original template provided for this assignment.
/// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/// <summary>
/// Namespace the class belongs to.
/// </summary>
namespace Dutch_Bingo
{
    /// <summary>
    /// 
    /// Class models and represents a Dutch Bingo relationship graph.
    /// 
    /// </summary>
    class Program
    {
        // Class member variables.
        private static RelationshipGraph rg;

        /// <summary>
        ///  
        /// Method parses an input file and constructs a RelationshipGraph.
        /// 
        /// Asks the user to enter the name of a file containing relationship information.
        /// Parses the input file containing the relationship information.
        /// Constructs a new RelationshipGraph object based on the relationship information.
        /// 
        /// </summary>
        /// <param name="filename">name of the input file</param>
        private static void ReadRelationshipGraph(string filename)
        {
            // Create a new RelationshpGraph object.
            rg = new RelationshipGraph();

            // String to store the name of the person currently being parsed.
            string name = "";

            // Counter to keep track of the # of people in the RelationshipGraph.
            int numPeople = 0;

            // Array stores the components of each parsed relationship.
            string[] values;

            Console.Write("Reading file " + filename + "\n");
            try
            {
                // Command to read in the entire input file.
                string input = System.IO.File.ReadAllText(filename);

                // Replace "\r" or carriage returns
                input = input.Replace("\r", ";");
                // Replace "\n" or end line characters.
                input = input.Replace("\n", ";");

                // Parse the relationships separated by a ";" delimiter
                string[] inputItems = Regex.Split(input, @";\s*");

                // Create new node for each relationship or create new edge for each relationship.
                foreach (string item in inputItems)
                {
                    // Empty relationship if string in each element of the array is less than 2 characters. (ignore)
                    if (item.Length > 2)
                    {
                        // Split the string into its relationship components.

                        // Parse out relationship:name.
                        values = Regex.Split(item, @"\s*:\s*");

                        // Name:[personname] indicates start of new person.
                        if (values[0] == "name")
                        {
                            // Remember name for future relationships.
                            name = values[1];

                            // Create new node to represent the new person.
                            rg.AddNode(name);

                            // Increment the # of people counter.
                            numPeople++;
                        }
                        else
                        {
                            // Add relationship (name1, name2, relationship)
                            rg.AddEdge(name, values[1], values[0]);

                            // Handle symmetric relationships -- add the other way.
                            if (values[0] == "hasSpouse" || values[0] == "hasFriend")
                                rg.AddEdge(values[1], name, values[0]);

                            // For parent relationships add child as well.
                            else if (values[0] == "hasParent")
                                rg.AddEdge(values[1], name, "hasChild");
                            else if (values[0] == "hasChild")
                                rg.AddEdge(values[1], name, "hasParent");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.Write("Unable to read file {0}: {1}\n", filename, e.ToString());
            }
            Console.WriteLine(numPeople + " people read");
        }

        /// <summary>
        /// 
        /// Method displays to console all the relationships a specific individual
        /// has.
        /// 
        /// </summary>
        /// 
        /// <param name="name">name of the person to search for</param>
        private static void ShowPerson(string name)
        {
            // Obtain the node corresponding to the person.
            GraphNode n = rg.GetNode(name);

            if (n != null)
                Console.Write(n.ToString());
            else
                Console.WriteLine("{0} not found", name);
        }

        /// <summary>
        /// 
        /// Method displays to console all the relationships of type "friend" that
        /// a specific individual has.
        /// 
        /// </summary>
        /// <param name="name">name of the person to search for</param>
        private static void ShowFriends(string name)
        {
            // Obtain the node corresponding to the person.
            GraphNode n = rg.GetNode(name);

            if (n != null)
            {
                Console.Write("{0}'s friends: ", name);

                // Get the list of edges of type/label "hasFriend"
                List<GraphEdge> friendEdges = n.GetEdges("hasFriend");

                // Write each of the "hasFriend" edges to the console.
                foreach (GraphEdge e in friendEdges)
                {
                    Console.Write("{0} ", e.To());
                }
                Console.WriteLine();
            }
            else
                Console.WriteLine("{0} not found", name);
        }

        /// <summary>
        /// 
        /// Method displays a program menu by which the user can interact with the program.
        /// (accepts, parses, and executes user commands)
        /// 
        /// </summary>
        private static void CommandLoop()
        {
            // Stores the user commmand.
            string command = "";

            // Stores the user input as an array of words.
            string[] commandWords;

            Console.Write("Welcome to the Dutch Bingo Parlor!\n");

            // Continue program execution so long as user does not wish to exit.
            while (command != "exit")
            {
                Console.Write("\nEnter a command: ");
                command = Console.ReadLine();

                // Regex to split user input into an array of words.
                commandWords = Regex.Split(command, @"\s+");       
                command = commandWords[0];

                // Commmand to terminate the program.
                if (command == "exit")
                    return;                                               

                // Command to read a relationship graph from a file and construct a RelationshipGraph.
                else if (command == "read" && commandWords.Length > 1)
                    ReadRelationshipGraph(commandWords[1]);

                // Command to show all the relationships a specific individual has.
                else if (command == "show" && commandWords.Length > 1)
                    ShowPerson(commandWords[1]);

                // Command to show all the friends an specific individual has.
                else if (command == "friends" && commandWords.Length > 1)
                    ShowFriends(commandWords[1]);

                // Command to print the text representation of the entire RelationshipGraph to console.
                else if (command == "dump")
                    rg.Dump();

                // User entered an invalid command.
                else
                    Console.Write("\nLegal commands: read [filename], dump, show [personname],\n  friends [personname], exit\n");
            }
        }

        /// <summary>
        /// 
        /// Method initiates the Dutch Bingo application.
        /// 
        /// </summary>
        /// 
        /// <param name="args">command-line arguments the user inputs</param>
        static void Main(string[] args)
        {
            CommandLoop();
        }
    }
}
