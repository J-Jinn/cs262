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
    /// Class BonzoPlayer defines a psychologically deficient Mankalah player for simple testing purposes.
    /// This sorry excuse for a player always takes the first available go-again, if there is one.  If not,
    /// this player takes the first available move. (Skynet would cry)
    /// </summary>
    public class BonzoPlayer : Player
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor that sets the AI Player Position and Name.
        /// </summary>
        /// <param name="pos">Position of PLayer - TOP or BOTTOM</param>
        /// <param name="timeLimit">max time per turn or move</param>
        public BonzoPlayer(Position pos, int timeLimit) : base(pos, "Bonzo", timeLimit) { }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method permits the user to specify a personalized emotive message.
        /// </summary>
        /// <returns>emotive message</returns>
        public override string Gloat()
        {
            return "I WIN! YOU'RE DUMBER THAN BONZO!!";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method calculates the move the AI Player should choose.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// 
        /// <returns>the move the AI chose to make</returns>
        public override int ChooseMove(Board b)
        {
            // If AI Player is Position TOP
            if (b.WhoseMove() == Position.Top)
            {
                // Try first go-again.
                for (int i = 12; i >= 7; i--)
                {
                    if (b.StonesAt(i) == 13 - i) return i;
                }
                // Otherwise, choose first available move.
                for (int i = 12; i >= 7; i--)
                {
                    if (b.StonesAt(i) > 0) return i;
                }
            }
            // If AI Player is Position  BOTTOM
            else
            {
                // Try first go-again.
                for (int i = 5; i >= 0; i--)
                {
                    if (b.StonesAt(i) == 6 - i) return i;
                }
                // Otherwise, choose first available move.
                for (int i = 5; i >= 0; i--)
                {
                    if (b.StonesAt(i) > 0) return i;
                }
            }
            // Return illegal move, if no legal moves are possible. (only if game is over)
            return -1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method obtains the image to use as the avatar.
        /// </summary>
        /// <returns>URL or relative file path of the image</returns>
        public String getImage()
        {
            return "Bonzo.png";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
