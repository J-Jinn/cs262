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
using System.Text.RegularExpressions;

/// <summary>
/// Namespace the class belongs to.
/// </summary>
namespace Dutch_Bingo
{
    /// <summary>
    /// 
    /// Class models and represents a Dutch Bingo relationship graph.
    /// 
    /// Contains various methods that retrieves different sorts of information from the RelationshipGraph.
    /// 
    /// </summary>
    class Program
    {
        // Class member variables.
        private static RelationshipGraph rg;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        ///  
        /// Method parses an input file and constructs a RelationshipGraph.
        /// 
        /// Asks the user to enter the name of a file containing relationship information.
        /// Parses the input file containing the relationship information.
        /// Constructs a new RelationshipGraph object based on the relationship information.
        /// 
        /// Note: I have not edited this in any way, shape, or form, besides adding/modifying some comments so
        /// I can retain a understanding of how it works.
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method displays to console all the relationships of type "friend" that
        /// a specific individual has.
        /// 
        /// Note: This was included with the template, unintentionally by Professor Plantinga, so I am just going to use his
        /// method without modification.  Free points ftw, I guess. ;D
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Utility method to detect whether a cycle exists within the RelationShipGraph from node currently set as the
        /// starting/root node for the retrieval of all descendants of a particular individual.
        /// 
        /// Note: Uses recursive DFS.
        /// 
        /// </summary>
        /// <param name="name">name of the starting/root node to search for cycles</param>
        /// <returns></returns>
        private static bool DetectCycles(string name)
        {
            // Copy of entire RelationshipGraph to use for cycle detection.
            RelationshipGraph rgCopy = rg;

            // Set all nodes in the graph that we may traverse to as unexplored.
            foreach (GraphNode n in rgCopy.nodes)
            {
                n.SetStatus("unexplored");

                foreach (GraphEdge e in n.incidentEdges)
                {
                    e.SetStatus("unexplored");
                }
            }

            // Recursive call.
            DFSCycles(name, rgCopy);

            // Check for any edges labeled as a "back" edge, indicating we have found a cycle.
            foreach (GraphNode n in rgCopy.nodes)
            {
                foreach (GraphEdge e in n.incidentEdges)
                {
                    if (e.GetStatus() == "back")
                    {
                        // Found a cycle.
                        return true;
                    }
                }
            }
            // Didn't find a cycle.
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Helper method for DetectCycles(string name) that implements the actual depth-first search (DPS)
        /// algorithm to detect any existing cycles based on traversal of "hasChild" from-to relationships,
        /// indicating descendancy.  A back-edge indicates we have found a cycle.
        /// 
        /// Note: Adapted from Chapter 6 PowerPoint Slides on DFS
        /// 
        /// Note: Took way too long to figure this one out because I was getting all edges at first, which
        /// meant it was checking for potential cycles for all types of relationships, not just the ones
        /// that involve descendants.
        /// 
        /// </summary>
        /// <param name="name">name of the node to label</param>
        /// <param name="graph">copy of the RelationshipGraph read in by user</param>
        private static void DFSCycles(string name, RelationshipGraph graph)
        {
            // Get the node associated with the name of the person.
            GraphNode n = graph.GetNode(name);

            // Indicate we have visited this node.
            n.SetStatus("visited");

            // Obtain all edges of the current node that deals with descendants (a.k.a "hasChild")
            List<GraphEdge> nodeEdges = n.GetEdges("hasChild");

            // Iterate through all connected nodes.
            foreach (GraphEdge e in nodeEdges)
            {
                // If we have not yet visited this particular edge.
                if (e.GetStatus() == "unexplored")
                {
                    // If the node connected to the current node has not been visited yet.
                    if (e.ToNode().GetStatus() == "unexplored")
                    {
                        e.SetStatus("discovery");
                        DFSCycles(e.ToNodeName(), graph);
                    }
                    // If the node connected to the current node has been visited already.
                    else
                    {
                        e.SetStatus("back");
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method displays to console all of the descendants that a specific
        /// individiual has.
        /// 
        /// Note: Uses recursive BFS via recursion.
        /// 
        /// TODO: Display to console all descendants and generation labels in sorted order.
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
            // Check for cycles in RelationshipGraph from root/starting node.
            if (DetectCycles(name))
            {
                Console.WriteLine("Cycle detected from root/starting node!  Aborting operation!");
                return;
            }
            Console.WriteLine("{0}'s descendants:\n\n", name);

            // Keep track of current generation of descendants.
            Stack<int> depthStack = new Stack<int>();

            // Hash table to store/track visited nodes.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // Store each descendant and their associated depth value from root.
            Dictionary<GraphNode, int> trackGen = new Dictionary<GraphNode, int>();

            // Set the initial depth of the root.
            depthStack.Push(0);

            // Call recursive BFS algorithm.
            BFS(name, depthStack, visited, trackGen);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Utility method that implements the actual recursive breadth-first search to find
        /// the descendants of the specified individual.
        /// 
        /// </summary>
        /// <param name="name">Name of the current node we're iterating on</param>
        /// <param name="visited">HashTable of nodes already visited</param>
        /// <param name="currentGen">How far down the generations we are from original person</param>
        private static void BFS(string name, Stack<int> depthStack, HashSet<GraphNode> visited, Dictionary<GraphNode, int> trackGen)
        {
            // Get the node associated with the name of the person.
            GraphNode n = rg.GetNode(name);

            // Add start/root node as visited.
            visited.Add(n);

            // Current depth from parent.
            int depth = depthStack.Peek();

            // Obtain all edges of the current node labeled "hasChild"
            List<GraphEdge> edges = n.GetEdges("hasChild");

            // Iterate through all connected "hasChild" nodes.
            foreach (GraphEdge e in edges)
            {
                // Iterate only through unvisited nodes.
                if (!visited.Contains(e.ToNode()))
                {
                    // Add node as visited.
                    visited.Add(e.ToNode());

                    // Increase the depth of next generation of descendants.
                    depthStack.Push(depth + 1);

                    // Add node and its generation depth to dictionary.
                    if (!trackGen.ContainsKey(e.ToNode()))
                    {
                        trackGen.Add(e.ToNode(), depthStack.Peek());
                    }
                    // Construct labels according to generation.
                    if (depthStack.Peek() == 1)
                    {
                        Console.WriteLine("Children: {0}", e.ToNodeName());
                    }
                    if (depthStack.Peek() == 2)
                    {
                        Console.WriteLine("Grand-Children: {0}", e.ToNodeName());
                    }
                    // Dynamically add prefix "Great" appropriate number of times.
                    if (depthStack.Peek() > 2)
                    {
                        // Counter variable.
                        int counter = depthStack.Peek();

                        while (counter > 2)
                        {
                            Console.Write("Great-");
                            counter--;
                        }
                        Console.WriteLine("Grand-Children: {0}", e.ToNodeName());
                    }
                    // Recursive call.
                    BFS(e.ToNodeName(), depthStack, visited, trackGen);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method displays to console all of the descendants that a specific
        /// individual has.
        /// 
        /// FIXME: Not properly tracking and labeling each descendent as their correct
        /// generation from the original person we are tracking descendants for.
        /// 
        /// Note: This implementation of generation tracking and labeling DOES work for my
        /// recursive breadth-first search algorithm above though. /shrug
        /// 
        /// Note: Uses non-recursive BFS via a queue.
        /// 
        /// </summary>
        /// 
        /// <param name="name">name of the person whose descendants to search for</param>
        private static void ShowDescendantsNonRecursive(string name)
        {
            // Check that the individual exists.
            if (rg.GetNode(name) == null)
            {
                Console.WriteLine("Individual does not exist!");
                return;
            }
            Console.WriteLine("{0}'s descendants:\n\n", name);

            // Keep track of current generation of descendants.
            Stack<int> depthStack = new Stack<int>();

            // Hash table to store/track visited nodes.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // Store each descendant and their associated depth value from root.
            Dictionary<GraphNode, int> trackGen = new Dictionary<GraphNode, int>();

            // Queue for non-recursive BFS implementation.
            Queue<GraphNode> nodeQueue = new Queue<GraphNode>();

            // Get the node associated with the name of the person.
            GraphNode n = rg.GetNode(name);

            // Add start/root node as visited.
            visited.Add(n);

            // Add start/root node to the queue.
            nodeQueue.Enqueue(n);

            // Set the initial depth of the root.
            depthStack.Push(0);

            // Iterate while queue is non-empty.
            while (nodeQueue.Count > 0)
            {
                // Current node we are on.
                GraphNode currentNode = nodeQueue.Dequeue();

                // Current depth from parent.
                int depth = depthStack.Peek();

                // Obtain all edges of the current node labeled "hasChild"
                List<GraphEdge> edges = currentNode.GetEdges("hasChild");

                // Iterate through all connected "hasChild" nodes.
                foreach (GraphEdge e in edges)
                {
                    // Iterate only through unvisited nodes.
                    if (!visited.Contains(e.ToNode()))
                    {
                        // Add node as visited.
                        visited.Add(e.ToNode());
                        // Add node to queue.
                        nodeQueue.Enqueue(e.ToNode());
                        // Increase the depth of next generation of descendants.
                        depthStack.Push(depth + 1);

                        // Add node and its depth to dictionary.
                        if (!trackGen.ContainsKey(e.ToNode()))
                        {
                            trackGen.Add(e.ToNode(), depthStack.Peek());
                        }
                    }
                }
            }
            // Print all descendants to console.
            printDescendants(trackGen);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Utility method that prints all of a person's descendants to console with 
        /// appropriate labels.
        /// 
        /// </summary>
        /// 
        /// <param name="trackGen">dictionary containing all nodes and their associated depth value from root</param>
        private static void printDescendants(Dictionary<GraphNode, int> trackGen)
        {
            foreach (KeyValuePair<GraphNode, int> p in trackGen)
            {
                // Construct labels according to generation.
                if (p.Value == 1)
                {
                    Console.WriteLine("Children: {0}", p.Key.Name);
                }
                if (p.Value == 2)
                {
                    Console.WriteLine("Grand-Children: {0}", p.Key.Name);
                }
                // Dynamically add prefix "Great" appropriate number of times.
                if (p.Value > 2)
                {
                    int counter = p.Value;

                    while (counter > 2)
                    {
                        Console.Write("Great-");
                        counter--;
                    }
                    Console.WriteLine("Grand-Children: {0}", p.Key.Name);
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method displays to console the shortest path of relationship between two people
        /// or says they are unrelated.  Also eports on the # of edges searched overall and
        /// if no relationship was found, displays a message saying so.
        /// 
        /// Note: Uses a variation of recursive DFS.
        /// 
        /// Note: Something I tried but didn't work out.  Might go back to it in the future on my own time.
        /// 
        /// </summary>
        /// <param name="source_name">name of the person to search a relationship from</param>
        /// <param name="destination_name">name of the person to search a relationship to</param>
        /// 
        private static void BingoGameFailed(string source_name, string destination_name)
        {
            // Trivial (stupid) case where we're looking for the same person
            if (source_name == destination_name)
            {
                Console.WriteLine("You have found yourself!");
                return;
            }
            // If we're looking for relationships among non-existent individual(s)
            if (rg.GetNode(source_name) == null || rg.GetNode(destination_name) == null)
            {
                Console.WriteLine("One or both individuals do not exist!");
                Console.WriteLine("Please try again!");
                return;
            }

            // Get the node associated with the name of the person.
            GraphNode source_node = rg.GetNode(source_name);
            GraphNode destination_node = rg.GetNode(destination_name);

            // List to retrace path.
            List<List<GraphNode>> successfulPaths = new List<List<GraphNode>>();
            List<GraphNode> currentPath = new List<GraphNode>();
            List<GraphNode> shortestPath = new List<GraphNode>();

            // Hash table to store/track visited nodes.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // Recursive call.
            DFSSearch(source_node, destination_node, visited, successfulPaths, currentPath);

            int shortestLength = int.MaxValue;

            foreach (List<GraphNode> list in successfulPaths)
            {
                if (list.Count < shortestLength)
                {
                    shortestPath = list;
                    shortestLength = list.Count;
                }
            }

            // Print the shortest path to console.
            foreach (GraphNode n in shortestPath)
            {
                Console.WriteLine("Node: {0}", n.Name);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Utility method that actually implements the recursive depth-first search algorithm in order to find all
        /// possible paths of relationship between the two specified individuals.
        /// 
        /// </summary>
        /// 
        /// <param name="source">the person we are starting the search from</param>
        /// <param name="destination">the person we are attempting to reach</param>
        /// <param name="visited">stores all the nodes we have already visited</param>
        /// <param name="successfulPaths">stores all the paths by which we can connect the two people</param>
        /// <param name="currentPath">store the current path we are traversing in order ot find a relationship</param>
        private static void DFSSearch(GraphNode source, GraphNode destination, HashSet<GraphNode> visited, List<List<GraphNode>> successfulPaths, List<GraphNode> currentPath)
        {
            // Indicate we have visited this node.
            visited.Add(source);
            // Add source node to path.
            currentPath.Add(source);

            // Obtain all edges of the current node that deals with descendants (a.k.a "hasChild")
            List<GraphEdge> nodeEdges = source.GetEdges();

            // Iterate through all connected nodes.
            foreach (GraphEdge e in nodeEdges)
            {
                if (e.ToNode() == destination)
                {
                    currentPath.Add(e.ToNode());
                    successfulPaths.Add(currentPath);
                    currentPath.Remove(e.ToNode());
                }
                else
                {
                    if (!visited.Contains(e.ToNode()))
                    {
                        GraphNode next = e.ToNode();
                        DFSSearch(next, destination, visited, successfulPaths, currentPath);
                    }
                }
            }
            currentPath.Remove(source);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method displays to console the shortest path of relationship between two people
        /// or says they are unrelated.  Also eports on the # of edges searched overall and
        /// if no relationship was found, displays a message saying so.
        /// 
        /// Note: Uses a variation of non-recursive BFS.
        /// 
        /// Note: Currently finds a path if one exists, but still need to figure out a way to parse the actual path from the
        /// nodes it visits that are stored in List<GraphNode> path.
        /// 
        /// </summary>
        /// <param name="source_name">name of the person to search a relationship from</param>
        /// <param name="destination_name">name of the person to search a relationship to</param>
        /// 
        private static void BingoGameOriginal(string source_name, string destination_name)
        {
            // Trivial (stupid) case where we're looking for the same person
            if (source_name == destination_name)
            {
                Console.WriteLine("You have found yourself!");
                return;
            }
            // If we're looking for relationships among non-existent individual(s)
            if (rg.GetNode(source_name) == null || rg.GetNode(destination_name) == null)
            {
                Console.WriteLine("One or both individuals do not exist!");
                Console.WriteLine("Please try again!");
                return;
            }

            // Get the node associated with the name of the person.
            GraphNode source_node = rg.GetNode(source_name);

            // Stack to record the path.
            Stack<GraphNode> stackPath = new Stack<GraphNode>();

            // List to record the path.
            List<GraphNode> listPath = new List<GraphNode>();

            // Hash table to store/track visited nodes.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // Create queue for breadth-first search algorithm.
            Queue<GraphNode> queue = new Queue<GraphNode>();

            // Add node as visited.
            visited.Add(source_node);

            // Add node to queue.
            queue.Enqueue(source_node);

            // Counts # of edges traversed.
            int path_counter = 0;

            while (queue.Count > 0)
            {
                // Retrieve the node we should traverse with.
                GraphNode current_node = queue.Dequeue();

                // Add current node as part of the path.
                stackPath.Push(current_node);
                listPath.Add(current_node);

                // Debug statement.
                //Console.WriteLine("Current node is: {0}", current_node.Name);

                // Check if we have found a path.
                if (current_node.Name == destination_name)
                {
                    Console.WriteLine("Path of relationship found!");

                    // Not functional.
                    traceShortestPath(stackPath, source_name, destination_name);

                    // Display to console all the nodes we have traversed.
                    for (int i = 0; i < listPath.Count; i++)
                    {
                        Console.WriteLine("Path Node: {0}", listPath[i].Name);
                    }

                    Console.WriteLine("Edges searched: {0}", path_counter);
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

                        // Debug statement.
                        //Console.WriteLine("{0} : {1} : {2}", e.FromNodeName(), e.Label, e.ToNodeName());
                    }
                }
                path_counter++;
            }
            Console.WriteLine("No relationship path was found between these two individuals!");
            Console.WriteLine("Edges searched: {0}", path_counter);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method parses the nodes visited during the BFS search in order to find the actual shortest path between the
        /// two people.
        /// 
        /// Note: currently not functioning.
        /// 
        /// </summary>
        /// 
        /// <param name="path"></param>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        private static void traceShortestPath(Stack<GraphNode> path, string source, string destination)
        {
            List<GraphNode> shortestPath = new List<GraphNode>();

            GraphNode endNode = rg.GetNode("destination");
            GraphNode startNode = rg.GetNode("source");

            GraphNode currentNode = path.Pop();

            while (currentNode != startNode && path.Count > 0)
            {
                List<GraphEdge> nodeEdges = currentNode.GetEdges();

                foreach (GraphEdge e in nodeEdges)
                {
                    if (e.FromNode() == path.Peek())
                    {
                        shortestPath.Add(path.Peek());
                    }
                }
                currentNode = path.Pop();
            }

            shortestPath.Reverse();

            Console.WriteLine("Path is: ");

            for (int i = 0; i < shortestPath.Count; i++)
            {
                Console.WriteLine("Node: {0}", shortestPath[i].Name);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method displays a program menu by which the user can interact with the program.
        /// (accepts, parses, and executes user commands)
        /// 
        /// Modified from original template code provided for this assignment.
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
                    // Use recursive DFS (properly labels each generation of descendants)
                    ShowDescendantsRecursive(commandWords[1]);

                    // Use non-recursive DFS (doesn't properly label each generation of descendants)
                    //ShowDescendantsNonRecursive(commandWords[1]);
                }
                else if (command == "bingo" && commandWords.Length > 2)
                {
                    BingoGameOriginal(commandWords[1], commandWords[2]);
                    //BingoGameWorkInProgress(commandWords[1], commandWords[2]);
                    //BingoGameFailed(commandWords[1], commandWords[2]);
                }

                // User entered an invalid command.
                else
                {
                    Console.Write("\nLegal commands:\n read [filename]\n dump\n show [personname]\n friends [personname]\n exit\n");
                    Console.Write(" orphans\n descendants [personname]\n \bingo\n");
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

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
