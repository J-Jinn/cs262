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
    /// Class models and represents a node in a RelationshipGraph.
    /// </summary>
    class GraphNode
    {
        /// <summary>
        /// 
        /// Method to get the name of the node.
        /// 
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 
        /// Method to get a list of edges incident to the node.
        /// (edges connected and sharing the same vertex)
        /// 
        /// </summary>
        public List<GraphEdge> incidentEdges { get; private set; }

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
        public GraphNode(string v)
        {
            Name = v;
            incidentEdges = new List<GraphEdge>();
        }

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
    }
}
