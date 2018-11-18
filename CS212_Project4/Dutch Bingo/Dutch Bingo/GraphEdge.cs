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
    /// Class models and represents a labeled, directed edge in a RelationshipGraph.
    /// </summary>
    class GraphEdge
    {
        // Class member variables.
        private string Status;
        private GraphNode fromNode, toNode;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Class member variable and attached methods to get/set the name of the node.
        /// 
        /// </summary>
        /// 
        public string Label { get; private set; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Public constructor.
        /// 
        /// Sets the origin node of the directed edge.
        /// Sets the destination node of the directed edge.
        /// Sets the name of the directed edge.
        /// 
        /// </summary>
        /// 
        /// <param name="from">origin node</param>
        /// <param name="to">destination node</param>
        /// <param name="myLabel">name of the directed edge</param>
        public GraphEdge(GraphNode from, GraphNode to, string myLabel, string status)
        {
            fromNode = from;
            toNode = to;
            Label = myLabel;
            Status = status;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Get the status of the edge (unexplored, discovery, or back)
        /// 
        /// </summary>
        /// 
        /// <returns>status of the specified edge</returns>
        public string GetStatus()
        {
            return Status;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Set the status of the edge (unexplored, discovery, or back)
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
        /// Method gets the destination node name of the directed edge.
        /// (return the name of the "to" person in the relationship)
        /// 
        /// </summary>
        /// 
        /// <returns>name of the destination node 
        /// of the directed edge</returns>
        public string ToNodeName()
        {
            return toNode.Name;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method gets the source node name of the directed edge.
        /// (return the name of the "from" person in the relationship)
        /// 
        /// </summary>
        /// 
        /// <returns>name of the source node
        /// of the directed edge</returns>
        public string FromNodeName()
        {
            return fromNode.Name;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method gets the destination node of the directed edge.
        /// (return the node of the "to" person in the relationship)
        /// 
        /// </summary>
        /// <returns>destination node</returns>
        public GraphNode ToNode()
        {
            return toNode;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method gets the source node of the directed edge.
        /// (return the node of the "from" person in the relationship)
        /// 
        /// </summary>
        /// <returns>source node</returns>
        public GraphNode FromNode()
        {
            return fromNode;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 
        /// Method formats and returns the names of the components of a directed
        /// edge as a string.
        /// 
        /// </summary>
        /// 
        /// <returns>string containing the names of the origin node, destination node,
        ///  and directed edge</returns>
        public override string ToString()
        {
            string result = fromNode.Name + " --(" + Label + ")--> " + toNode.Name;
            return result;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}

