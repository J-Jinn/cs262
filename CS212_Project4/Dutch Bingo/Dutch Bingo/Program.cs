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
    /// Note: I have collapsed the methods that can be ignored for grading.  Not sure if this is portable though.
    /// 
    /// Note: Keeping deprecated methods and code in, in case I want to go back in the future and see if I can implement them
    /// properly.
    /// 
    /// </summary>
    class Program
    {
        // Class member variables.
        private static RelationshipGraph rg = new RelationshipGraph();

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
        /// Note: For use with "ShowDescendantsRecursive(string name)" below.
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
            DetectCyclesDFS(name, rgCopy);

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
        /// indicating it is a descendant.  A back-edge indicates we have found a cycle.
        /// 
        /// Note: Adapted from Chapter 6 PowerPoint Slides on DFS
        /// 
        /// Note: For use with "DetectCycles(string name)" above.
        /// 
        /// Note: Took way too long to figure this one out because I was getting all edges at first, which
        /// meant it was checking for potential cycles for all types of relationships, not just the ones
        /// that involve descendants.
        /// 
        /// </summary>
        /// <param name="name">name of the node to label</param>
        /// <param name="graph">copy of the RelationshipGraph read in by user</param>
        private static void DetectCyclesDFS(string name, RelationshipGraph graph)
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
                        DetectCyclesDFS(e.ToNodeName(), graph);
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
        /// individual has.
        /// 
        /// Note: Uses recursive BFS.
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
            ShowDescendantsRecursiveBFS(name, depthStack, visited, trackGen);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Utility method that implements the actual recursive breadth-first search to find
        /// the descendants of the specified individual.
        /// 
        /// Note: For use with "ShowDescendantsRecursive(string name)" above.
        /// 
        /// </summary>
        /// <param name="name">Name of the current node we're iterating on</param>
        /// <param name="visited">HashTable of nodes already visited</param>
        /// <param name="currentGen">How far down the generations we are from original person</param>
        private static void ShowDescendantsRecursiveBFS(string name, Stack<int> depthStack, HashSet<GraphNode> visited, Dictionary<GraphNode, int> trackGen)
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
                    ShowDescendantsRecursiveBFS(e.ToNodeName(), depthStack, visited, trackGen);
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
        /// FIXME: Not properly tracking and labeling each descendant as their correct
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
        /// 

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void ShowDescendantsNonRecursive(string name)
        {
            // Check that the individual exists.
            if (rg.GetNode(name) == null)
            {
                Console.WriteLine("\nIndividual does not exist!\n");
                return;
            }
            Console.WriteLine("\n{0}'s descendants:\n\n", name);

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
            printDescendantsNonRecursive(trackGen);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Utility method that prints all of a person's descendants to console with 
        /// appropriate labels.
        /// 
        /// Note: For use with "ShowDescendantsNonRecursive(string name)" above.
        /// 
        /// </summary>
        /// 
        /// <param name="trackGen">dictionary containing all nodes and their associated depth value from root</param>
        /// 

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void printDescendantsNonRecursive(Dictionary<GraphNode, int> trackGen)
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
        /// or says they are unrelated.  Also reports on the # of edges searched overall and
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

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void BingoGameFailed(string source_name, string destination_name)
        {
            // Trivial (stupid) case where we're looking for the same person
            if (source_name == destination_name)
            {
                Console.WriteLine("\nYou have found yourself!\n");
                return;
            }
            // If we're looking for relationships among non-existent individual(s)
            if (rg.GetNode(source_name) == null || rg.GetNode(destination_name) == null)
            {
                Console.WriteLine("\nOne or both individuals do not exist!");
                Console.WriteLine("Please try again!\n");
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
            BingoGameFailedDFSSearch(source_node, destination_node, visited, successfulPaths, currentPath);

            // Find the shortest path in all the viable paths.
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
        /// Note: For use with "BingoGameFailed(string source_name, string destination_name)" above.
        /// 
        /// </summary>
        /// 
        /// <param name="source">the person we are starting the search from</param>
        /// <param name="destination">the person we are attempting to reach</param>
        /// <param name="visited">stores all the nodes we have already visited</param>
        /// <param name="successfulPaths">stores all the paths by which we can connect the two people</param>
        /// <param name="currentPath">store the current path we are traversing in order to find a relationship</param>
        /// 

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD IGNORE THIS METHOD
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private static void BingoGameFailedDFSSearch(GraphNode source, GraphNode destination, HashSet<GraphNode> visited, List<List<GraphNode>> successfulPaths, List<GraphNode> currentPath)
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
                // If we have reached our destination.
                if (e.ToNode() == destination)
                {
                    currentPath.Add(e.ToNode());
                    successfulPaths.Add(currentPath);
                    currentPath.Remove(e.ToNode());
                }
                else
                {
                    // If we haven't visited this node yet.
                    if (!visited.Contains(e.ToNode()))
                    {
                        GraphNode next = e.ToNode();

                        // Recursive call.
                        BingoGameFailedDFSSearch(next, destination, visited, successfulPaths, currentPath);
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
        /// or says they are unrelated.  Also reports on the # of edges searched overall and
        /// if no relationship was found, displays a message saying so.
        /// 
        /// Note: Uses a variation of non-recursive BFS via a queue.
        /// 
        /// Note: Currently finds a path if one exists, but still need to figure out a way to parse the actual path from the
        /// nodes it visits that are stored in the associated data structure(s)
        /// 
        /// Update: I'm sure my BFS search is working properly, it's just parsing the shortest path that is presenting difficulties.
        /// 
        /// </summary>
        /// <param name="source_name">name of the person to search a relationship from</param>
        /// <param name="destination_name">name of the person to search a relationship to</param>
        private static void BingoGameOriginal(string source_name, string destination_name)
        {
            // Trivial (stupid) case where we're looking for the same person
            if (source_name == destination_name)
            {
                Console.WriteLine("\nYou have found yourself!\n");
                return;
            }
            // If we're looking for relationships among non-existent individual(s)
            if (rg.GetNode(source_name) == null || rg.GetNode(destination_name) == null)
            {
                Console.WriteLine("\nOne or both individuals do not exist!");
                Console.WriteLine("Please try again!\n");
                return;
            }

            // Dictionary to record the node and its parent.
            Dictionary<GraphNode, GraphNode> nodeAndParent = new Dictionary<GraphNode, GraphNode>();

            // Stack to record the nodes traversed to find the path.
            Stack<GraphNode> stackPathNode = new Stack<GraphNode>();

            // Stack to record the edges traversed to find the path.
            Stack<GraphEdge> stackPathEdge = new Stack<GraphEdge>();

            // List to record the nodes traversed to find the path.
            List<GraphNode> listPathNodes = new List<GraphNode>();

            // Hash table to indicate and store the nodes already visited.
            HashSet<GraphNode> visited = new HashSet<GraphNode>();

            // Create queue for non-recursive breadth-first search algorithm.
            Queue<GraphNode> queue = new Queue<GraphNode>();

            // Get the node associated with the names of the people we are finding a connection for.
            GraphNode sourceNode = rg.GetNode(source_name);
            GraphNode destinationNode = rg.GetNode(destination_name);

            // Add starting node as visited.
            visited.Add(sourceNode);

            // Add starting node to queue.
            queue.Enqueue(sourceNode);

            // Counts # of edges traversed during the BFS search.
            int pathCounter = 0;

            while (queue.Count > 0)
            {
                // Retrieve the node we should traverse with.
                GraphNode current_node = queue.Dequeue();

                // Add current node as part of the path, if not already part of the path.
                if (!stackPathNode.Contains(current_node))
                {
                    listPathNodes.Add(current_node);
                    stackPathNode.Push(current_node);
                }

                // Check if we have found a path.
                if (current_node == destinationNode)
                {
                    Console.WriteLine("Relationship path found!");

                    // Debug Method - View contents of data structures to check it is storing the proper nodes/edges.
                    DebugDataStructures(nodeAndParent, stackPathNode, stackPathEdge, listPathNodes);

                    // Parses the actual shortest path - Semi-functional (ugly as heck, but it sort of works)
                    traceShortestPath(nodeAndParent, stackPathNode, stackPathEdge, listPathNodes, source_name, destination_name);

                    Console.WriteLine("\nTotal edges searched to find a path: {0}\n", pathCounter);
                    return;
                }

                // Obtain all edges of the current node.
                List<GraphEdge> edges = current_node.GetEdges();

                // Iterate through all connected nodes.
                foreach (GraphEdge e in edges)
                {
                    // Iterate through unvisited nodes only.
                    if (!visited.Contains(e.ToNode()))
                    {
                        // Add node as visited.
                        visited.Add(e.ToNode());

                        // Add node to queue.
                        queue.Enqueue(e.ToNode());

                        // Store the edges we traversed.
                        stackPathEdge.Push(e);

                        // Store the current node and its parent.
                        nodeAndParent.Add(e.ToNode(), e.FromNode());
                    }
                }
                pathCounter++;
            }
            Console.WriteLine("\nNo relationship path was found between these two individuals!");
            Console.WriteLine("Total edges searched: {0}\n", pathCounter);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method is purely for debugging the contents of the data structures that store the nodes/edges traversed during the BFS traversal.
        /// 
        /// Note: Can be ignored for grading purposes.
        /// 
        /// </summary>
        /// 
        /// <param name="nodeAndParent">dictionary that stores a node and its immediate parent node</param>
        /// <param name="stackPathNode">stack that stores the nodes we traversed during the search</param>
        /// <param name="stackPathEdge">stack that stores the edges we traversed during the search</param>
        /// <param name="listPathNodes">list that stores the nodes we traversed during the search</param>
        private static void DebugDataStructures(Dictionary<GraphNode, GraphNode> nodeAndParent, Stack<GraphNode> stackPathNode, Stack<GraphEdge> stackPathEdge, List<GraphNode> listPathNodes)
        {
            // Booleans to turn on debugging-related code sections.
            bool debug1 = false;
            bool debug2 = false;
            bool debug3 = false;
            bool debug4 = false;

            Console.WriteLine("\n\n\n");

            if (debug1 == true)
            {
                // Make a copy for debug purposes.
                Stack<GraphEdge> stackPathEdgeCopy = new Stack<GraphEdge>(stackPathEdge);

                Console.WriteLine("\nStack Path Edges:\n");

                while (stackPathEdgeCopy.Count > 0)
                {
                    Console.WriteLine("From Node: {0}", stackPathEdgeCopy.Peek().FromNodeName());
                    Console.WriteLine("Edge Label: {0}", stackPathEdgeCopy.Peek().Label);
                    Console.WriteLine("To Node: {0}", stackPathEdgeCopy.Peek().ToNodeName());
                    Console.WriteLine("Stack Path Edges Remaining: {0}", stackPathEdgeCopy.Count);
                    stackPathEdgeCopy.Pop();
                }
            }
            if (debug2 == true)
            {
                // Make a copy for debug purposes.
                Stack<GraphNode> stackPathCopy = new Stack<GraphNode>(stackPathNode);

                Console.WriteLine("\nStack Path Nodes:\n");

                while (stackPathCopy.Count > 0)
                {
                    Console.WriteLine("Stack: Node Name: {0}", stackPathCopy.Peek().Name);
                    Console.WriteLine("Stack Path Nodes Remaining: {0}", stackPathCopy.Count);
                    stackPathCopy.Pop();
                }
            }
            if (debug3 == true)
            {
                // Display to console all the nodes we have traversed in the List.
                Console.WriteLine("\nList Path Nodes:\n");

                int listCounter = listPathNodes.Count;

                foreach (GraphNode node in listPathNodes)
                {
                    Console.WriteLine("List: Node Name: {0}", node.Name);
                    Console.WriteLine("List Path Nodes Remaining: {0}", listCounter);
                    listCounter--;
                }
            }
            if (debug4 == true)
            {
                // Display to console all the nodes we have traversed in the Dictionary.
                Console.WriteLine("\nDictionary Path Nodes:\n");

                int dictCounter = nodeAndParent.Count;

                foreach (KeyValuePair<GraphNode, GraphNode> entry in nodeAndParent)
                {
                    Console.WriteLine("Dictionary Node's Name is: {0}", entry.Key.Name);
                    Console.WriteLine("Dictionary Node's Parent Node's Name is: {0}", entry.Value.Name);
                    Console.WriteLine("Dictionary Path Nodes Remaining: {0}", dictCounter);
                    dictCounter--;
                }
            }
            Console.WriteLine("\n\n\n");
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method parses the nodes visited during the BFS search in order to find the actual shortest path between the
        /// two people.
        /// 
        /// Note: Not functioning as intended thus far.
        /// 
        /// </summary>
        /// 
        /// <param name="nodePlusParent">dictionary containing a node and it's parent during BFS traversal</param>
        /// <param name="pathNodeStack">stack storing all the nodes traversed during BFS</param>
        /// <param name="pathEdgeStack">stack storing all the edges traversed during BFS</param>
        /// <param name="pathNodeList">list storing all the nodes traversed during BFS</param>
        /// <param name="source">the person who we are trying to establish a relationship for</param>
        /// <param name="destination">the person we are trying to establish a relationship with</param>
        private static void traceShortestPath(Dictionary<GraphNode, GraphNode> nodePlusParent, Stack<GraphNode> pathNodeStack, Stack<GraphEdge> pathEdgeStack,
            List<GraphNode> pathNodeList, string source, string destination)
        {
            // Booleans to turn on debugging-related code sections.
            bool debug1 = false;
            bool debug2 = false;
            bool debug3 = false;

            // Booleans to turn on/off different methods of parsing the actual shortest path from all nodes traversed to find the path.
            bool searchUsingStackNodes = true;
            bool searchUsingStackEdges = false;
            bool searchUsingStackNodesAndDictionaryNodes = false;
            bool searchUsingDictionaryNodes = false;

            // The two people we want to find a relationship for.
            GraphNode startNode = rg.GetNode("source");
            GraphNode endNode = rg.GetNode("destination");

            // Parse the sequence of nodes for the shortest path.
            if (searchUsingStackNodes == true)
            {

                List<GraphNode> shortestPath = new List<GraphNode>();

                GraphNode currentNode = pathNodeStack.Pop();

                while (currentNode != startNode && pathNodeStack.Count > 0)
                {
                    if (debug1 == true)
                        Console.WriteLine("Current node is: {0}", currentNode.Name);

                    List<GraphEdge> nodeEdges = currentNode.GetEdges();

                    if (debug1 == true)
                        Console.WriteLine("Path.peek value is: {0}", pathNodeStack.Peek());

                    foreach (GraphEdge e in nodeEdges)
                    {
                        if (e.FromNode() == pathNodeStack.Peek())
                        {
                            shortestPath.Add(pathNodeStack.Peek());
                            break;
                        }
                    }
                    currentNode = pathNodeStack.Pop();
                }

                // Reverse the list.
                shortestPath.Reverse();

                Console.WriteLine("Node Path is: ");

                for (int i = 0; i < shortestPath.Count; i++)
                {
                    Console.WriteLine("Node: {0}", shortestPath[i].Name);
                }
            }

            // Parse the sequence of edges for the shortest path.
            if (searchUsingStackEdges == true)
            {
                List<GraphEdge> shortestPathEdges = new List<GraphEdge>();

                GraphEdge currentEdge = pathEdgeStack.Pop();

                while (currentEdge.FromNode() != startNode && pathEdgeStack.Count > 0)
                {
                    if (debug2 == true)
                        Console.WriteLine("Current edge is: {0}", currentEdge.Label);

                    if (debug2 == true)
                        Console.WriteLine("Path.peek value is: {0}", pathEdgeStack.Peek());

                    if (currentEdge.ToNodeName() == destination)
                    {
                        shortestPathEdges.Add(currentEdge);
                    }
                    if (currentEdge.FromNodeName() == pathEdgeStack.Peek().ToNodeName())
                    {
                        shortestPathEdges.Add(currentEdge);
                    }
                    currentEdge = pathEdgeStack.Pop();
                }

                // Reverse the list.
                shortestPathEdges.Reverse();

                Console.WriteLine("Shortest Edge Path is: ");

                foreach (GraphEdge edge in shortestPathEdges)
                {
                    Console.WriteLine("Edge: {0}", edge);
                }
            }

            //Parse the sequence of nodes using dictionary with node and parent node information.
            if (searchUsingStackNodesAndDictionaryNodes == true)
            {
                List<GraphNode> shortestPathTraversal = new List<GraphNode>();

                shortestPathTraversal.Add(pathNodeStack.Peek());
                GraphNode currentPathNode = pathNodeStack.Pop();

                while (currentPathNode != startNode && pathNodeStack.Count > 0)
                {
                    if (nodePlusParent.ContainsKey(currentPathNode))
                    {
                        shortestPathTraversal.Add(nodePlusParent[currentPathNode]);
                    }

                    currentPathNode = pathNodeStack.Pop();
                }

                // Reverse the list so we start from the source and head towards the destination.
                shortestPathTraversal.Reverse();

                // This section removes duplicates in the shortest path.
                List<GraphNode> shortestPathWithoutDuplicates = new List<GraphNode>();

                foreach (GraphNode n in shortestPathTraversal)
                {
                    if (!shortestPathWithoutDuplicates.Contains(n))
                    {
                        shortestPathWithoutDuplicates.Add(n);
                    }
                }

                if (debug3 == true)
                {
                    Console.WriteLine("Node Traversal Path with duplicates is: ");

                    Console.WriteLine("The shortest path of connections between {0} and {1} is: ", startNode.Name, endNode.Name);

                    foreach (GraphNode entry in shortestPathTraversal)
                    {
                        {
                            Console.WriteLine("Person: {0}", entry.Name);
                        }
                    }
                }

                Console.WriteLine("Node Traversal Path is: ");

                for (int i = 0; i < shortestPathWithoutDuplicates.Count; i++)
                {
                    Console.WriteLine("Node: {0}", shortestPathWithoutDuplicates[i].Name);
                }


                // Parse only the dictionary to retrieve the shortest path (non-functional)
                if (searchUsingDictionaryNodes == true)
                {
                    List<GraphNode> shortestPathOfRelationship = new List<GraphNode>();

                    foreach (KeyValuePair<GraphNode, GraphNode> entry in nodePlusParent)
                    {
                        if (!shortestPathOfRelationship.Contains(entry.Value))
                        {
                            shortestPathOfRelationship.Add(entry.Value);
                        }
                    }

                    Console.WriteLine("The shortest path of connections between {0} and {1} is: ", startNode.Name, endNode.Name);

                    foreach (GraphNode entry in shortestPathOfRelationship)
                    {
                        Console.WriteLine("person: {0}", entry.Name);
                    }
                }
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
            // Stores the user command.
            string command = "";

            // Stores the user input as an array of words.
            string[] commandWords;

            Console.Write("Welcome to the Dutch Bingo Parlor!\n");
            Console.WriteLine("Kind suggestion: Read in a RelationshipGraph before attempting these commands! ;D");
            Console.WriteLine("If you don't it won't crash, but it'll be a very boring default RelationshipGraph ^_^\n");

            // Continue program execution so long as user does not wish to exit.
            while (command != "exit")
            {
                Console.WriteLine("\nEnter \"help\" for a list of valid commands");

                Console.Write("\nEnter a command: ");
                command = Console.ReadLine();
                Console.WriteLine();

                // Regex to split user input into an array of words.
                commandWords = Regex.Split(command, @"\s+");
                command = commandWords[0];

                // Command to terminate the program.
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
                // Command to show all descendants in the RelationshipGraph.
                else if (command == "descendants" && commandWords.Length > 1)
                {
                    // Use recursive DFS (properly labels each generation of descendants)
                    ShowDescendantsRecursive(commandWords[1]);

                    // Use non-recursive DFS (doesn't properly label each generation of descendants)
                    //ShowDescendantsNonRecursive(commandWords[1]);
                }
                // Command to find shortest relationship between two people.
                else if (command == "bingo" && commandWords.Length > 2)
                {
                    // Currently finds a path and # of edges traversed to find it, but doesn't parse
                    // properly for the actual # of nodes/edges in the path.
                    BingoGameOriginal(commandWords[1], commandWords[2]);

                    // Failed attempt at using a inefficient recursive DFS search to find shortest path.
                    //BingoGameFailed(commandWords[1], commandWords[2]);
                }
                // Command to display description of all commands.
                else if (command == "help")
                {
                    Console.WriteLine("read [filename]");
                    Console.WriteLine("This command reads in the specified input file as a RelationshipGraph");
                    Console.WriteLine();

                    Console.WriteLine("dump");
                    Console.WriteLine("This command prints to console the entire contents of the RelationshipGraph");
                    Console.WriteLine("Example: \"name:Zebina; hasSpouse:Minneiah; hasFriend:Jekamiah\"");
                    Console.WriteLine("Example: \"name:Akkub; hasSpouse:Jedidiah; hasChild:Miamin; hasPastor:Apocalypse\"");
                    Console.WriteLine();

                    Console.WriteLine("show [personname]");
                    Console.WriteLine("This command will display all the relationships the specified person is involved in");
                    Console.WriteLine("Similar to \"dump\" command but only for a specific individual");
                    Console.WriteLine();

                    Console.WriteLine("friends [personname]");
                    Console.WriteLine("This command will display all the \"hasFriend\" relationships the specified person has");
                    Console.WriteLine();

                    Console.WriteLine("exit");
                    Console.WriteLine("This command will terminate the Dutch Bingo program");
                    Console.WriteLine();

                    Console.WriteLine("orphans");
                    Console.WriteLine("This command will display all individuals without \"hasParent\" relationships");
                    Console.WriteLine();

                    Console.WriteLine("descendants [personname]");
                    Console.WriteLine("This command will display all the \"hasChild\" relationships (descendants) of the specified person");
                    Console.WriteLine();

                    Console.WriteLine("bingo");
                    Console.WriteLine("This command will find the shortest path of relationship (connection) between the two specified people");
                    Console.WriteLine();
                }
                // User entered an invalid command.
                else
                {
                    Console.WriteLine("Oops! You entered a non-existent command!  Enter \"help\" for a helpful description of all valid commands");
                    Console.Write("\nLegal commands:\n read [filename]\n dump\n show [personname]\n friends [personname]\n exit\n");
                    Console.Write(" orphans\n descendants [personname]\n \bingo\n");
                    Console.WriteLine();
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
            // Default node and edges to prevent program from crashing if user didn't enter a input file before attempting commands.
            // (aka user stupidity insurance)
            rg.AddNode("DefaultNode1");
            rg.AddNode("DefaultNode2");
            rg.AddEdge("DefaultNode1", "DefaultNode2", "hasFriend");
            rg.AddEdge("DefaultNode2", "DefaultNode1", "hasFriend");

            // Execute the Dutch Bingo program.
            CommandLoop();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
