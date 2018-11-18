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

/// <summary>
/// Namespace the class belongs to.
/// </summary>
namespace Dutch_Bingo
{
    /// <summary>
    /// Class models and represents a directed labeled graph with a string Name at each node
    /// and a string Label for each edge.
    /// </summary>
    class RelationshipGraph
    {
        /*
         *  This data structure contains a list of nodes (each of which has
         *  an adjacency list) and a dictionary (hash table) for efficiently 
         *  finding nodes by name
         */
        public List<GraphNode> nodes { get; private set; }
        private Dictionary<String, GraphNode> nodeDict;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Public constructor.
        /// 
        /// Creates a list to contain nodes.
        /// 
        /// Creates a dictionary to provide search functionality.
        /// (stores the string format of a node and its incident edges)
        /// 
        /// </summary>
        public RelationshipGraph()
        {
            nodes = new List<GraphNode>();
            nodeDict = new Dictionary<String, GraphNode>();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method creates and adds a new node if one with that name doesn't already exist.
        /// 
        /// Checks if a node by that name exists already.
        /// Creates a new node by that name.
        /// 
        /// Adds node to the list of nodes.
        /// Adds node to the dictionary with key = name, value = node.
        /// 
        /// </summary>
        /// <param name="name">name to give the node</param>
        public void AddNode(string name)
        {
            if (!nodeDict.ContainsKey(name))
            {
                GraphNode n = new GraphNode(name, "unexplored", int.MaxValue);
                nodes.Add(n);
                nodeDict.Add(name, n);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method adds a new directed edge, creating endpoint nodes if necessary.
        /// (edge is added to adjacency list of from-to (origin-destination) edges)
        /// 
        /// 1.  Create from/origin node if it doesn't exist.
        /// 1.a. Get node reference from dictionary.
        /// 2.  Create to/destination node if it doesn't exist.
        /// 2.a. Get node reference from the dictionary.
        /// 
        /// 3.  Create the new edge using the values obtained from the dictionary
        /// and the type of relationship specified.
        /// 
        /// 4.  Add the new edge to the list of edges incident to the origin/to node.
        /// 
        /// </summary>
        /// 
        /// <param name="name1">name to give the from/origin node</param>
        /// <param name="name2">name to give the to/destination node</param>
        /// <param name="relationship">type of relationship the from/to nodes share</param>
        public void AddEdge(string name1, string name2, string relationship)
        {
            AddNode(name1);                     // create the node if it doesn't already exist
            GraphNode n1 = nodeDict[name1];     // now fetch a reference to the node

            AddNode(name2);
            GraphNode n2 = nodeDict[name2];

            GraphEdge e = new GraphEdge(n1, n2, relationship, "unexplored", 1);
            n1.AddIncidentEdge(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method searches the dictionary for a node by its name.
        /// 
        /// If found, returns the node.
        /// If not found, returns null.
        /// 
        /// </summary>
        /// <param name="name">specify the name of the node to search for</param>
        /// <returns>node or null</returns>
        public GraphNode GetNode(string name)
        {
            if (nodeDict.ContainsKey(name))
                return nodeDict[name];
            else
                return null;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method prints to console terminal the entire text representation
        /// of the graph. (all its nodes/vertices and edges)
        /// 
        /// </summary>
        public void Dump()
        {
            foreach (GraphNode n in nodes)
            {
                Console.Write(n.ToString());
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
