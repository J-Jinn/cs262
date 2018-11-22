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

/// <summary>
/// Namespace this class belongs to.
/// </summary>
namespace Mankalah
{
    /// <summary>
    /// Class KalahMatch creates two different Players and runs a pair of Mankalah games, one with
    /// each player starting.  The match results are reported.
    /// </summary>
    public class KalahMatch
    {
        // Turn time limit calculated in milliseconds
        private static int turnTimeLimit = 1000;

        // The player on the TOP (MAX)
        private static Player playerTop = new BonzoPlayer(Position.Top, turnTimeLimit);
        // The player on the BOTTOM (MIN)
        private static Player playerBottom = new HumanPlayer(Position.Bottom, turnTimeLimit);

        // The game board.
        private static Board board;
        // The current move position or value.
        private static int move;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Play a Mankalah game with the two given players, with the firstPlayer starting.
        /// Returns TOP's score.
        /// </summary>
        /// <param name="playerTop">the player on the top</param>
        /// <param name="playerBottom">the player on the bottom</param>
        /// <param name="firstPlayer">the player who is starting the game</param>
        /// <returns>the score of the TOP player</returns>
        public static int playGame(Player playerTop, Player playerBottom, Position firstPlayer)
        {
            // Create a new game board.
            board = new Board(firstPlayer);

            // Determine the player who starts.
            if (firstPlayer == Position.Top)
            {
                Console.WriteLine("Player " + playerTop.getName() + " starts.");
            }
            else
            {
                Console.WriteLine("Player " + playerBottom.getName() + " starts.");
            }

            // Display the current state of the game board.
            board.display();

            // Continue rotating turns till the game is over.
            while (!board.gameOver())
            {
                Console.WriteLine();

                // Get the player's move and output what move the player made.
                if (board.whoseMove() == Position.Top)
                {
                    move = playerTop.chooseMove(board);
                    Console.WriteLine(playerTop.getName() + " chooses move " + move);
                }

                else
                {
                    move = playerBottom.chooseMove(board);
                    Console.WriteLine(playerBottom.getName() + " chooses move " + move);
                }

                // Commit the move to the game state. (true = verbose, false = non-verbose)
                board.makeMove(move, true);

                // Display the new state of the game board.
                board.display();

                // Game is over, determine final results.
                if (board.gameOver())
                {
                    if (board.winner() == Position.Top)
                    {
                        Console.WriteLine("Player " + playerTop.getName() +
                        " (TOP) wins " + board.scoreTopPlayer() + " to " + board.scoreBottomPlayer());
                    }
                    else if (board.winner() == Position.Bottom)
                    {
                        Console.WriteLine("Player " + playerBottom.getName() +
                        " (BOTTOM) wins " + board.scoreBottomPlayer() + " to " + board.scoreTopPlayer());
                    }
                    else
                    {
                        Console.WriteLine("A tie!");
                    }
                }
                // Game is not over, ask player to make their move.
                else
                    if (board.whoseMove() == Position.Top)
                    Console.WriteLine(playerTop.getName() + " to move.");
                else
                    Console.WriteLine(playerBottom.getName() + " to move.");
            }

            // Return the final score for the match.
            return board.scoreTopPlayer();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Main method outputs the results of each match to the console.
        /// </summary>
        /// <param name="arguments">optional command-line arguments</param>
        public static void Main(String[] arguments)
        {
            // Boolean variables for debugging purposes.
            bool debuggingDisabled = false;
            bool debugHeuristicEvaluationFunction = true;
            bool debugChooseMoveMiniMaxAlgorithmFunction = false;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Play the game.

            if (debuggingDisabled == true)
            {
                // Obtain final score for the TOP and BOTTOM player.
                int finalScore;

                Console.WriteLine("\n================ Game 1 ================");
                finalScore = playGame(playerTop, playerBottom, Position.Bottom);

                Console.WriteLine("\n================ Game 2 ================");
                finalScore += playGame(playerTop, playerBottom, Position.Top);

                Console.WriteLine("\n========================================");
                Console.Write("Match result: ");

                // Determine the winner and loser.
                int botScore = 96 - finalScore;

                if (finalScore > 48)
                {
                    Console.WriteLine(playerTop.getName() + " wins " + finalScore + " to " + botScore);
                    playerTop.gloat();
                }
                else if (botScore > 48)
                {
                    Console.WriteLine(playerBottom.getName() + " wins " + botScore + " to " + finalScore);
                    playerBottom.gloat();
                }
                else
                    Console.WriteLine("Match was a tie, 48-48!");

                Console.Read();
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// TEST EACH METHOD OR FUNCTION IN jj47Player works as intended

            if (debugHeuristicEvaluationFunction == true)
            {

                // Create new game board.
                Board testBoard = new Board();

                testBoard.display();
                Console.WriteLine("\n\n");

                // Debug - test setter for number of stones at specified position.
                testBoard.setStonesAt(2, 0);

                testBoard.display();
                Console.WriteLine("\n\n");

                if (testBoard.stonesAt(2) != 0)
                {
                    Console.WriteLine("Method setStonesAt(position) not functioning properly");
                }

                // Store the move the AI determined it should make.
                int moveTOP = -1;
                int moveBOTTOM = -1;

                // Test as TOP Player.
                jj47Player testTOPPlayer = new jj47Player(Position.Top, 100000);

                moveTOP = testTOPPlayer.heuristicEvaluation(testBoard);

                Console.WriteLine("TOP Player AI determined optimal move was: {0}", moveTOP);

                // Test as BOTTOM Player.
                jj47Player testBOTTOMPlayer = new jj47Player(Position.Bottom, 100000);

                moveBOTTOM = testBOTTOMPlayer.heuristicEvaluation(testBoard);

                Console.WriteLine("BOTTOM Player AI determined optimal move was: {0}", moveBOTTOM);

            }

            while (true)
            {
                //Do nothing.  Keep terminal open.
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
