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
/// 
/// Namespace the class belongs to.
/// 
/// </summary>
namespace Dutch_Bingo
{
    /// <summary>
    /// 
    /// Class models and represents a node in a RelationshipGraph.
    /// 
    /// Added Status and WeightValue fields.
    /// 
    /// </summary>
    class GraphNode
    {
        // Class member variables.
        private string Status;
        private int WeightValue;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Class member variable and attached methods to get/set the name of the node.
        /// 
        /// </summary>
        public string Name { get; private set; }     

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method to get a list of edges incident to the node.
        /// (edges connected and sharing the same vertex)
        /// 
        /// </summary>
        public List<GraphEdge> incidentEdges { get; private set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Public constructor.
        /// 
        /// Sets the name of the node.
        /// Creates a new List to contain edges incident to the node.
        /// 
        /// </summary>
        /// 
        /// <param name="v">name of the node</param>
        /// <param name="status">unexplored, discovery, or back</param>
        /// <param name="weight">associated weight value of this node</param>
        public GraphNode(string v, string status, int weight)
        {
            Name = v;
            incidentEdges = new List<GraphEdge>();
            Status = status;
            WeightValue = weight;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Get the weight value of the edge.
        /// 
        /// </summary>
        /// 
        /// <returns>status of the specified node</returns>
        public int GetWeight()
        {
            return WeightValue;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Set the weight value of the edge.
        /// 
        /// </summary>
        /// 
        /// <param name="weight">weight value</param>
        public void SetWeight(int weight)
        {
            WeightValue = weight;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Get the status of the node (unexplored, discovery, or back)
        /// 
        /// </summary>
        /// 
        /// <returns>status of the specified node</returns>
        public string GetStatus()
        {
            return Status;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Set the status of the node (unexplored, discovery, or back)
        /// 
        /// </summary>
        /// 
        /// <param name="status">status name</param>
        public void SetStatus(string status)
        {
            Status = status;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method adds a new edge to the list of edges incident to the vertex,
        /// if it does not exist already in the list.
        /// 
        /// </summary>
        /// 
        /// <param name="e">an edge</param>
        public void AddIncidentEdge(GraphEdge e)
        {
            foreach (GraphEdge edge in incidentEdges)
            {
                if (edge.ToString() == e.ToString())
                    return;
            }

            incidentEdges.Add(e);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method that returns a list of all edges incident to the vertex.
        /// 
        /// </summary>
        /// 
        /// <returns>edges incident to the vertex</returns>
        public List<GraphEdge> GetEdges()
        {
            return incidentEdges;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method searches for and returns a list of edges corresponding to
        /// a search query string.
        /// 
        /// </summary>
        /// 
        /// <param name="label">edge name to search for</param>
        /// <returns>a list of matching edges</returns>
        public List<GraphEdge> GetEdges(string label)
        {
            List<GraphEdge> list = new List<GraphEdge>();

            foreach (GraphEdge e in incidentEdges)
                if (e.Label == label)
                    list.Add(e);
            return list;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method returns the name of the node and the names of all edges
        /// connected to that node.
        /// 
        /// </summary>
        /// 
        /// <returns>string containing the text form of a node and its
        /// incident edges</returns>
        public override string ToString()
        {
            string result = Name + "\n";

            foreach (GraphEdge e in incidentEdges)
            {
                result = result + "  " + e.ToString() + "\n";
            }
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
