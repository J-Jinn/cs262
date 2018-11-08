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
        public string Label { get; private set; }
        private GraphNode fromNode, toNode;

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
        public GraphEdge(GraphNode from, GraphNode to, string myLabel)
        {
            fromNode = from;
            toNode = to;
            Label = myLabel;
        }

        /// <summary>
        /// 
        /// Method gets the destination node of the directed edge.
        /// (return the name of the "to" person in the relationship)
        /// 
        /// </summary>
        /// 
        /// <returns>name of the destination node 
        /// of the directed edge</returns>
        public string To()
        {
            return toNode.Name;
        }

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
    }
}

