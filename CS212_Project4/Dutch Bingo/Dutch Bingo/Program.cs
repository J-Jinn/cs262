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
        /// 
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
                    Console.Write("{0} ", e.ToNodeName());
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("{0} not found", name);
            }
        }

        /// <summary>
        /// 
        /// Method displays to console all the people with no parents.
        /// 
        /// </summary>
        private static void ShowOrphans()
        {
            foreach (GraphNode n in rg.nodes)
            {
                List<GraphEdge> edges = n.GetEdges("hasParent");

                if (edges.Count == 0)
                {
                    Console.WriteLine("{0} is an orphan", n.Name);
                }
            }
        }

        /// <summary>
        /// 
        /// Method displays to console all of the descendants that a specific
        /// individiual has.
        /// 
        /// Note: Uses recursive DFS.
        /// 
        /// </summary>
        /// 
        /// <param name="name">name of the person whose descendants to search for</param>
        private static void ShowDescendantsRecursive(string name)
        {
            // Check that the individual exists.
            if (rg.GetNode(name) == null)
            {
                Console.WriteLine("Individual does not exist!");
                return;
            }

            //Hash table to store/track visited nodes.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            Console.WriteLine("{0}'s descendants:\n\n", name);

            // Track descendent generational gap.
            int currentGen = 0;

            // Call DFS algorithm.
            DFS(name, visited, currentGen);
        }

        /// <summary>
        /// 
        /// Utility method that implements the actual recursive depth-first search to find
        /// the descendants of the specified individual.
        /// 
        /// TODO: Get descendant labeling working properly.
        /// 
        /// </summary>
        /// <param name="name">Name of the current node we're iterating on</param>
        /// <param name="visited">HashTable of nodes already visited</param>
        /// <param name="currentGen">How far down the generations we are from original person</param>
        private static void DFS(string name, HashSet<GraphNode> visited, int currentGen)
        {
            // Get the node associated with the name of the person.
            GraphNode n = rg.GetNode(name);

            // If the person exists, search for his/her descendants.
            if (n != null)
            {
                // Add node as visited.
                visited.Add(n);

                // Obtain all edges of the current node that is labeled "hasChild"
                List<GraphEdge> edges = n.GetEdges("hasChild");

                // Iterate through all connected nodes.
                foreach (GraphEdge e in edges)
                {
                    // Iterate through univisited nodes.
                    if (!visited.Contains(e.ToNode()))
                    {
                        // Add node as visited.
                        visited.Add(e.ToNode());

                        // Print to console the descendants.
                        if (currentGen == 0)
                        {
                            Console.WriteLine("Children: {0}", e.ToNode().Name);
                        }
                        if (currentGen == 1)
                        {
                            Console.WriteLine("Grand-Children: {0}", e.ToNode().Name);
                        }
                        //if (currentGen == 2)
                        //{
                        //    Console.WriteLine("Great-Grand-Children: {0}", e.ToNode().Name);
                        //}
                        //if (currentGen == 3)
                        //{
                        //    Console.WriteLine("Great-Great-Grand-Children: {0}", e.ToNode().Name);
                        //}
                        //if (currentGen == 4)
                        //{
                        //    Console.WriteLine("Great-Great-Great-Grand-Children: {0}", e.ToNode().Name);
                        //}
                        if (currentGen >= 2)
                        {
                            // Counter variable.
                            int counter = currentGen;

                            while (counter >= 2)
                            {
                                Console.Write("Great-");
                                counter--;
                            }
                            Console.WriteLine("Grand-Children: {0}", e.ToNode().Name);
                        }

                        // Increment the generation.
                        currentGen++;

                        // Recursive call.
                        DFS(e.ToNodeName(), visited, currentGen);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// Method displays to console all of the descendants that a specific
        /// individiual has.
        /// 
        /// TODO: Get descendant labeling working properly.
        /// 
        /// Note: Uses non-recursive DFS via a stack.
        /// 
        /// </summary>
        /// 
        /// <param name="name">name of the person whose descendants to search for</param>
        private static void ShowDescendantsNonRecursive(string name)
        {
            // Check that the individual exists.
            if(rg.GetNode(name) == null)
            {
                Console.WriteLine("Individual does not exist!");
                return;
            }

            // Track descendent generational gap.
            int currentGen = 0;

            //Hash table to store/track visited nodes.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // Stack for DFS implementation.
            Stack<GraphNode> nodeStack = new Stack<GraphNode>();

            // Get the node associated with the name of the person.
            GraphNode n = rg.GetNode(name);

            // If the person exists, search for his/her descendants.
            if (n != null)
            {
                Console.WriteLine("{0}'s descendants:\n\n", name);

                // Add start/root node as visited.
                visited.Add(n);

                // Add start/root node to the stack.
                nodeStack.Push(n);

                // Iterate while stack is non-empty.
                while (nodeStack.Count > 0)
                {
                    // Current node we are on.
                    GraphNode currentNode = nodeStack.Pop();

                    // If not visited yet, add to hash table.
                    if (!visited.Contains(currentNode))
                    {
                        // Add node as visited.
                        visited.Add(currentNode);                                       
                    }

                    // Obtain all edges of the current node labeled "hasChild"
                    List<GraphEdge> edges = currentNode.GetEdges("hasChild");

                    // Iterate through all connected nodes.
                    foreach (GraphEdge e in edges)
                    {
                        // Iterate through univisited nodes.
                        if (!visited.Contains(e.ToNode()))
                        {
                            // Add node as visited.
                            visited.Add(e.ToNode());
                            // Add node to stack.
                            nodeStack.Push(e.ToNode());

                            // Print to console the descendants.
                            if (currentGen == 0)
                            {
                                Console.WriteLine("Children: {0}", e.ToNode().Name);
                            }
                            if (currentGen == 1)
                            {
                                Console.WriteLine("Grand-Children: {0}", e.ToNode().Name);
                            }
                            //if (currentGen == 2)
                            //{
                            //    Console.WriteLine("Great-Grand-Children: {0}", e.ToNode().Name);
                            //}
                            //if (currentGen == 3)
                            //{
                            //    Console.WriteLine("Great-Great-Grand-Children: {0}", e.ToNode().Name);
                            //}
                            //if (currentGen == 4)
                            //{
                            //    Console.WriteLine("Great-Great-Great-Grand-Children: {0}", e.ToNode().Name);
                            //}
                            if (currentGen > 1)
                            {
                                // Counter variable.
                                int counter = currentGen;

                                while (counter > 1)
                                {
                                    Console.Write("Great-");
                                    counter--;
                                }
                                Console.WriteLine("Grand-Children: {0}", e.ToNode().Name);
                            }
                        }                                         
                    }
                    currentGen++;
                }
            }
        }

        /// <summary>
        /// 
        /// Method displays to console the shortest path of relationships between two people
        /// or says they are unrelated.
        /// 
        /// Note: Uses a variation of BFS
        /// 
        /// </summary>
        /// <param name="source_name">name of the person to search a relationship from</param>
        /// <param name="destination_name">name of the person to search a relationship to</param>
        private static void BingoGame(string source_name, string destination_name)
        {
            // Trivial (stupid) case where we're looking for the same person
            if(source_name == destination_name)
            {
                Console.WriteLine("You have found yourself!");
                return;
            }

            // If we're looking for relationships among non-existent individual(s)
            if(rg.GetNode(source_name) == null || rg.GetNode(destination_name) == null){
                Console.WriteLine("One or both individuals do not exist!");
                Console.WriteLine("Please try again!");
                return;
            }

            // Hash table to store/track visited nodes.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // Create queue for breadth-first search algorithm.
            Queue<GraphNode> queue = new Queue<GraphNode>();

            // Get the node associated with the name of the person.
            GraphNode source_node = rg.GetNode(source_name);

            // Add node as visited.
            visited.Add(source_node);

            // Add node to queue.
            queue.Enqueue(source_node);

            int path_counter = 0;

            while (queue.Count > 0)
            {
                GraphNode current_node = queue.Dequeue();

                // Debug statement.
                Console.WriteLine("Current node is: {0}", current_node.Name);

                if (current_node.Name == destination_name)
                {
                    Console.WriteLine("Path of relationship found!");
                    Console.WriteLine("Edges taken: {0}", path_counter);
                    return;
                }

                // Obtain all edges of the current node.
                List<GraphEdge> edges = current_node.GetEdges();

                // Iterate through all connected nodes.
                foreach (GraphEdge e in edges)
                {
                    // Iterate through univisited nodes.
                    if (!visited.Contains(e.ToNode()))
                    {
                        // Add node as visited.
                        visited.Add(e.ToNode());
                        // Add node to queue
                        queue.Enqueue(e.ToNode());

                        Console.WriteLine("{0} : {1} : {2}", e.FromNodeName(), e.Label, e.ToNodeName());
                    }
                }
                path_counter++;
            }
            Console.WriteLine("No relationship path was found between these two individuals!");
            Console.WriteLine("Edges searched: {0}", path_counter);
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
                {
                    return;
                }
                // Command to read a relationship graph from a file and construct a RelationshipGraph.
                else if (command == "read" && commandWords.Length > 1)
                {
                    ReadRelationshipGraph(commandWords[1]);
                }
                // Command to show all the relationships a specific individual has.
                else if (command == "show" && commandWords.Length > 1)
                {
                    ShowPerson(commandWords[1]);
                }
                // Command to show all the friends an specific individual has.
                else if (command == "friends" && commandWords.Length > 1)
                {
                    ShowFriends(commandWords[1]);
                }
                // Command to print the text representation of the entire RelationshipGraph to console.
                else if (command == "dump")
                {
                    rg.Dump();
                }
                // Command to show all orphans (no parents) in the RelationshipGraph.
                else if (command == "orphans")
                {
                    ShowOrphans();
                }
                else if (command == "descendants" && commandWords.Length > 1)
                {
                    // Use recursive DFS
                    //ShowDescendantsRecursive(commandWords[1]);

                    // Use non-recursive DFS
                    ShowDescendantsNonRecursive(commandWords[1]);
                }
                else if (command == "bingo" && commandWords.Length > 2)
                {
                    BingoGame(commandWords[1], commandWords[2]);
                }

                // User entered an invalid command.
                else
                {
                    Console.Write("\nLegal commands:\n read [filename]\n dump\n show [personname]\n friends [personname]\n exit\n");
                    Console.Write(" orphans\n descendants [personname]\n");
                }
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
