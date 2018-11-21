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
    /// Class jj47Player defines a hopefully less psychologically deficient Mankalah player then BonzoPlayer.
    /// This hopefully less sorry excuse for a player implements a heuristic evaluation function and the mini-max
    /// algorithm to choose the most optimal move to make. (will hopefully make Skynet proud)
    /// </summary>
    public class jj47Player : Player
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor that sets the AI Player Position and Name.
        /// </summary>
        /// <param name="pos">Position of PLayer - TOP or BOTTOM</param>
        /// <param name="timeLimit">max time per turn or move</param>
        public jj47Player(Position pos, int timeLimit) : base(pos, "jj47", timeLimit) { }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method calculates the move the AI Player should choose.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// 
        /// <returns>the move the AI chose to make</returns>
        public override int chooseMove(Board b)
        {
            // If AI Player is Position TOP
            if (b.whoseMove() == Position.Top)
            {
                // Try first go-again.
                for (int i = 12; i >= 7; i--)
                {
                    if (b.stonesAt(i) == 13 - i) return i;
                }
                // Otherwise, choose first available move.
                for (int i = 12; i >= 7; i--)
                {
                    if (b.stonesAt(i) > 0) return i;
                }
            }

            // If AI Player is Position  BOTTOM
            if(b.whoseMove() == Position.Bottom)
            {
                // Try first go-again.
                for (int i = 5; i >= 0; i--)
                {
                    if (b.stonesAt(i) == 6 - i) return i;
                }
                // Otherwise, choose first available move.
                for (int i = 5; i >= 0; i--)
                {
                    if (b.stonesAt(i) > 0) return i;
                }
            }
            // Return illegal move, if no legal moves are possible. (only if game is over)
            return -1;
        }

        /// <summary>
        /// Method to determine the optimality of the state of the game board.
        /// 
        /// Note: TOP player = MAX --> most positive scores are better.
        /// Note: BOTTOM player = MIN --> most negative scores are better
        /// </summary>
        /// 
        /// <param name="b">Game Board object</param>
        /// <returns>how optimal the state of the game board is</returns>
        public override int heuristicEvaluation(Board b)
        {
            int optimalMove = 0;

            for(int i = 0; i < 14; i++)
            {
                List<int> stones = new List<int>();

                stones.Add(b.stonesAt(i));

                
            }
          
            if (b.legalMove(optimalMove))
            {
                return optimalMove;
            }
            return 1;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method obtains the image to use as the avatar.
        /// Note: Image stored in Mankalah\bin\Debug
        /// </summary>
        /// <returns>URL or relative file path of the image</returns>
        public String getImage()
        {
            return "OrcPeonPortrait.png";
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
            return "I WIN! Buy me a Nvidia Geforce RTX 2080 Ti!!!";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Utility class MovePredictionResults assists the mini-max algorithm in returning an object containing the optimal move and
    /// its associated score (MAX or MIN) for a player.
    /// </summary>
    public class MovePredictionResults
    {
        // Class fields.
        private int optimalMove;
        private int optimalMoveScore;

        /// <summary>
        /// Constructor initializes the optimal move and its associated score.
        /// </summary>
        /// <param name="move">integer value of position on game board</param>
        /// <param name="score">integer value of score for a particular move</param>
        MovePredictionResults(int move, int score)
        {
            optimalMove = move;
            optimalMoveScore = score;
        }

        /// <summary>
        /// Getter for class field optimalMove.
        /// </summary>
        /// <returns>integer value representing the optimal move</returns>
        public int getOptimalMove()
        {
            return optimalMove;
        }

        /// <summary>
        /// Setter for class field optimalMove.
        /// </summary>
        /// <param name="move">integer value representing the optimal move</param>
        private void setOptimalMove(int move)
        {
            optimalMove = move;
        }

        /// <summary>
        /// Getter for class field optimalMoveScore.
        /// </summary>
        /// <returns>integer value representing the score for the optimal move</returns>
        public int getOptimalMoveScore()
        {
            return optimalMoveScore;
        }

        /// <summary>
        /// Setter for class field optimalMoveScore.
        /// </summary>
        /// <param name="score">integer value representing the score for the optimal move</param>
        private void setOptimalMoveScore(int score)
        {
            optimalMoveScore = score;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
