// ReSharper disable InvalidXmlDocComment
/// <summary>
/// Project 5: Mankalah
/// CS-212 Data Structures and Algorithms
/// Section: B
/// Instructor: Professor Plantinga
/// Date: 11-20-18
/// 
/// Mankalah Game Framework.
/// Modified from the original template provided for this assignment.
/// </summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Namespace this class belongs to.
/// </summary>
namespace Mankalah
{
    /// <summary>
    /// Abstract class Player defines the interface for modeling a Mankalah player.
    /// Base or parent class for all players.
    /// All players are derived and inherit from this base-parent class.
    /// Ensure that any player works properly both as TOP and BOTTOM player.
    /// </summary>
    public abstract class Player
    {
        // Store the name of the player.
        private readonly string _myName;

        // Determine if the player is TOP, BOTTOM, or INVALID.
        private readonly Position _myPosition;

        // Time limit per move calculated in milliseconds.
        private readonly int _timePerMove;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Default constructor.
        /// Note: Can override constructor to perform any necessary tasks before game-play begins.
        /// 
        /// TODO: implement enforcement of time limit to prevent disqualification.
        /// </summary>
        /// 
        /// <param name="position">Position (TOP or BOTTOM) the player is to play</param>
        /// <param name="name">name of the player</param>
        /// <param name="maxTimePerMove">time limit per turn in milliseconds</param>
        protected Player(Position position, String name, int maxTimePerMove)
        {
            _myName = name;
            _myPosition = position;
            _timePerMove = maxTimePerMove;

            Console.Write("Player " + name + " playing on ");

            if (position == Position.Top)
            {
                Console.WriteLine("top.");
            }
            if (position == Position.Bottom)
            {
                Console.WriteLine("bottom.");
            }
            if (position != Position.Top && _myPosition != Position.Bottom)
            {
                Console.Write("...an illegal side of the board.");
                Environment.Exit(1);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to determine the optimality of the state of the game board.
        /// 
        /// Note: TOP player = MAX --> most positive scores are better.
        /// Note: BOTTOM player = MIN --> most negative scores are better
        /// 
        /// Override and implement heuristic evaluation algorithm to determine optimal move.
        /// (by default, just counts the score).
        /// </summary>
        /// 
        /// <param name="b">Game Board object</param>
        /// <returns>how optimal the state of the game board is</returns>
        public virtual int HeuristicEvaluation(Board b)
        {
            return b.StonesAt(13) - b.StonesAt(6);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to get the name of the player.
        /// </summary>
        /// <returns>name of the player</returns>
        public string GetName()
        {
            return _myName;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to get the max time allowed per turn or move.
        /// </summary>
        /// <returns>time limit per turn or move</returns>
        public int GetTimePerMove()
        {
            return _timePerMove;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Virtual method to display a avatar for the player.
        /// Use URL or relative file path.
        /// Override with personalized avatar.
        /// </summary>
        /// <returns>string representation of URL or relative file path</returns>
        public virtual string GetImage()
        {
            return string.Empty;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Virtual method to determine the best optimal move based on heuristic evaluation and subsequent mini-max algorithm.
        /// Override with customized mini-max algorithm depth-first staged search.
        /// </summary>
        /// <param name="b">Game Board object.</param>
        /// <returns>the optimal move to make</returns>
        public abstract int ChooseMove(Board b);

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Virtual method for constructing emotive messages.
        /// Override with personalized message.
        /// </summary>
        /// <returns>message in string format</returns>
        public virtual string Gloat()
        {
            return "I win.";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
