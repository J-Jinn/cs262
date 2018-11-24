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

/**************************************************************************
 * Board.cs: a boardPositions for the game of Mankalah. 
 *
 * A boardPositions looks like this :
 *
 *        boardPositions[12] boardPositions[11] boardPositions[10] boardPositions[9] boardPositions[8] boardPositions[7]
 *boardPositions[13]                                                     [board6]
 *        boardPositions[0]  boardPositions[1]  boardPositions[2]  boardPositions[3] boardPositions[4] boardPositions[5]
 *
 * TOP player moves from locations 7..12 toward location 13. 
 * BOTTOM moves from 0..5 toward 6.
 *
 **************************************************************************/

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
    // Type of position.
    public enum Position : byte { Top, Bottom, Invalid }

    /// <summary>
    /// Class Board models a Mankalah game boardPositions.
    /// </summary>
    public class Board
    {
        // Store the position associated with player move.
        private Position _playerToMove;

        // Model all positions on the game boardPositions. (public for performance reasons)
        public int[] BoardPositions = new int[14];

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Default constructor that simply creates a new game board.
        /// Note: Added for testing purposes for Class jj47Player.
        /// </summary>
        public Board()
        {
            // These positions represent both player's pockets, holes, or pits.
            for (int i = 0; i < 13; i++)
            {
                BoardPositions[i] = 4;
            }

            // These two positions represents the player's scoring cache.
            BoardPositions[6] = 0;
            BoardPositions[13] = 0;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Constructor that creates a brand new fresh game boardPositions and specifies the starting player.
        /// </summary>
        /// <param name="toMove">position to initiate a move</param>
        public Board(Position toMove)
        {
            // These positions represent both player's pockets, holes, or pits.
            for (int i = 0; i < 13; i++)
            {
                BoardPositions[i] = 4;
            }

            // These two positions represents the player's scoring cache.
            BoardPositions[6] = 0;
            BoardPositions[13] = 0;

            _playerToMove = toMove;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Copy Constructor that updates the current state of the game boardPositions each move.
        /// </summary>
        /// <param name="b">Game Board object.</param>
        public Board(Board b)
        {
            for (int i = 0; i < 14; i++)
            {
                BoardPositions[i] = b.BoardPositions[i];
            }

            _playerToMove = b._playerToMove;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to copy the current state of the game boardPositions each move.
        /// </summary>
        /// <param name="b">Game Board Object</param>
        public void Copy(Board b)
        {
            for (int i = 0; i < 14; i++)
            {
                BoardPositions[i] = b.BoardPositions[i];
            }

            _playerToMove = b._playerToMove;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to retrieve the # of stones at specified boardPositions position.
        /// </summary>
        /// <param name="position">the position on the game boardPositions</param>
        /// <returns># of stones at specified position</returns>
        public int StonesAt(int position)
        {
            return BoardPositions[position];
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to set the # of stones at specified boardPositions position.
        /// Note: Added for testing purposes.
        ///
        /// TODO: Should not be public outside of testing purposes.
        /// </summary>
        /// <param name="position">the position on the game boardPositions</param>
        /// <param name="value">the number of stones to set to</param>
        public void SetStonesAt(int position, int value)
        {
            BoardPositions[position] = value;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to retrieve whose turn it is.
        /// </summary>
        /// <returns>playerToMove - the current player whose turn it is</returns>
        public Position WhoseMove()
        {
            return _playerToMove;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to determine if the player made a legitimate move.
        /// </summary>
        /// <param name="move">the position on the boardPositions the player wishes to initiate a move</param>
        /// <returns>true = legal move, false = illegal move</returns>
        public bool LegalMove(int move)
        {
            if (_playerToMove == Position.Top && move >= 7 && move <= 12 &&
                BoardPositions[move] != 0)
            {
                return true;
            }
            if (_playerToMove == Position.Bottom && move >= 0 && move <= 5 &&
                BoardPositions[move] != 0)
            {
                return true;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to determine if the game is over.
        /// </summary>
        /// <returns>true = game over, false = game not over</returns>
        public bool GameOver()
        {
            if (BoardPositions[0] == 0 && BoardPositions[1] == 0 && BoardPositions[2] == 0 &&
                BoardPositions[3] == 0 && BoardPositions[4] == 0 && BoardPositions[5] == 0)
            {
                return true;
            }
            if (BoardPositions[7] == 0 && BoardPositions[8] == 0 && BoardPositions[9] == 0 &&
                BoardPositions[10] == 0 && BoardPositions[11] == 0 && BoardPositions[12] == 0)
            {
                return true;
            }
            return false;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method determines which player is currently winning, given the current state of the game board.
        /// </summary>
        /// <returns>enumerated representation of the currently winning player</returns>
        public Position Winner()
        {
            // Determine the # of stones owned by Player 1.
            int player1Count = 0;

            for (int i = 7; i <= 13; i++)
            {
                player1Count += BoardPositions[i];
            }

            // Determine the # of stones owned by Player 2.
            int player2Count = 0;

            for (int i = 0; i <= 6; i++)
            {
                player2Count += BoardPositions[i];
            }

            // Determine who is currently winning.
            if (player1Count > player2Count)
            {
                return Position.Top;
            }
            if (player2Count > player1Count)
            {
                return Position.Bottom;
            }

            // Otherwise, error.
            return Position.Invalid;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method returns the current score for the TOP player.
        /// </summary>
        /// <returns>score of the TOP player</returns>
        public int ScoreTopPlayer()
        {
            int score = 0;

            for (int i = 7; i <= 13; i++)
            {
                score += BoardPositions[i];
            }
            return score;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method returns the current score for the BOTTOM player.
        /// </summary>
        /// <returns>score of the BOTTOM player</returns>
        public int ScoreBottomPlayer()
        {
            int score = 0;

            for (int i = 0; i <= 6; i++)
            {
                score += BoardPositions[i];
            }
            return score;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method modifies boardPositions by making a move and updating playerToMove.
        /// Returns the # of stones captures.
        /// If chatter (verbosity) is true, prints informational messages.
        /// 
        /// Note: Exits with error message if illegal move is attempted.
        /// </summary>
        /// <param name="move">the move to be made</param>
        /// <param name="chatter">do we want to be verbose?</param>
        /// <returns>the # of stones captures</returns>
        public int MakeMove(int move, bool chatter)
        {
            // Check for illegal moves.
            if (!LegalMove(move))
            {
                string err = String.Format("Player {0} cheated! (Attempted illegal move {1})",
                                           _playerToMove, move);
                Console.WriteLine(err);
                Console.Read();
                //Environment.Exit(1);
                throw new ArgumentException(err);
            }

            // Pick up the stones.
            int stones = BoardPositions[move];
            BoardPositions[move] = 0;

            // Integer to store the # of stones captures.
            int stonesCaptured = 0;

            // Distribute the stones.
            int position;

            for (position = move + 1; stones > 0; position++)
            {
                // Don't add stone to opposing player's scoring cup.
                if (_playerToMove == Position.Top && position == 6)
                {
                    position++;
                }

                // Don't add stone to opposing player's scoring cup.
                if (_playerToMove == Position.Bottom && position == 13)
                {
                    position++;
                }

                // Don't go out of bounds of the game board.
                if (position == 14)
                {
                    position = 0;
                }

                // Add stones to the appropriate positions on the game board.
                BoardPositions[position]++;

                // Add stones to each position till we run out.
                stones--;
            }

            // What does this do?
            position--;

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Determine if there was a capture by TOP player. 
            if (_playerToMove == Position.Top && position > 6 && position < 13 && BoardPositions[position] == 1 && BoardPositions[12 - position] > 0)
            {
                stonesCaptured = BoardPositions[12 - position] + 1;
                BoardPositions[13] += BoardPositions[12 - position];
                BoardPositions[12 - position] = 0;
                BoardPositions[13]++;
                BoardPositions[position] = 0;

                if (chatter)
                {
                    Console.WriteLine("TOP captured {0} stones!", stonesCaptured);
                }
            }

            // Determine if there was a capture by BOTTOM player. 
            if (_playerToMove == Position.Bottom && position >= 0 && position < 6 && BoardPositions[position] == 1 && BoardPositions[12 - position] > 0)
            {
                stonesCaptured = BoardPositions[12 - position] + 1;
                BoardPositions[6] += BoardPositions[12 - position];
                BoardPositions[12 - position] = 0;
                BoardPositions[6]++;
                BoardPositions[position] = 0;

                if (chatter)
                {
                    Console.WriteLine("BOTTOM captured {0} stones!", stonesCaptured);
                }
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            // Determine who gets the next move.
            if (_playerToMove == Position.Top)
            {
                if (position != 13)
                {
                    _playerToMove = Position.Bottom;
                }
                else
                {
                    if (chatter)
                    {
                        Console.WriteLine("TOP goes again.");
                    }
                }
            }
            else
            {
                if (position != 6)
                {
                    _playerToMove = Position.Top;
                }
                else
                {
                    if (chatter)
                    {
                        Console.WriteLine("BOTTOM goes again.");
                    }
                }
            }

            return stonesCaptured;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method displays a text representation of the current state of the game board.
        /// 
        /// TODO: Create a graphical representation of the game board by refactoring to WPF, etc.
        /// </summary>
        public void Display()
        {
            Console.Write("\n    ");

            for (int i = 12; i >= 7; i--)
            {
                Console.Write(BoardPositions[i] + "  ");
            }

            Console.WriteLine("");
            Console.WriteLine(BoardPositions[13] + "                     " + BoardPositions[6]);
            Console.Write("    ");

            for (int i = 0; i <= 5; i++)
            {
                Console.Write(BoardPositions[i] + "  ");
            }

            Console.WriteLine("");
        }
    }
}
