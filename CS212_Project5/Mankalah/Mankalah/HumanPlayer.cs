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
    /// Class HumanPlayer defines a default human player.  The user is prompted to input a move.
    /// </summary>
    public class HumanPlayer : Player
    {
        /// <summary>
        /// Constructor that sets the Player Position and Name.
        /// </summary>
        /// 
        /// <param name="pos">Position of PLayer - TOP or BOTTOM</param>
        /// <param name="timeLimit">max time per turn or move</param>
        public HumanPlayer(Position pos, int timeLimit) : base(pos, "Human", timeLimit) { }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method permits the user to choose his or her move.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// 
        /// <returns>the move the user chose to make</returns>
        public override int chooseMove(Board b)
        {
            int move = -1;
            string moveString;

            // Continue asking for a legal move, if illegal.
            while (!b.legalMove(move))
            {
                Console.Write("Your move: ");
                moveString = Console.ReadLine();

                // Obtain integer representation of move and store in variable.
                if (!int.TryParse(moveString, out move) || !b.legalMove(move))
                    Console.WriteLine("Illegal move. Try again.");
            }
            return move;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method permits the user to specify a personalized emotive message.
        /// </summary>
        /// <returns>emotive message</returns>
        public override string gloat()
        {
            return "I WIN! Humans still rule.";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
