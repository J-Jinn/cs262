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
        private const int TurnTimeLimit = 1000;

        // The default AI player on TOP (MAX).
        private static readonly Player AIPlayerBonzoTop = new BonzoPlayer(Position.Top, TurnTimeLimit);

        // The custom AI player on TOP (MAX).
        private static readonly Player AIPlayerJJ47Top = new jj47Player(Position.Top, TurnTimeLimit);

        // The custom AI player on BOTTOM (MIN).
        private static readonly Player AIPlayerJJ47Bottom = new jj47Player(Position.Bottom, TurnTimeLimit);

        // The Human player on TOP (MAX).
        private static readonly Player HumanPlayerTop = new HumanPlayer(Position.Top, TurnTimeLimit);

        // The Human player on BOTTOM (MIN).
        private static readonly Player HumanPlayerBottom = new HumanPlayer(Position.Bottom, TurnTimeLimit);

        // The game board.
        private static Board _board;
        // The current move position or value.
        private static int _move;

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
        public static int PlayGame(Player playerTop, Player playerBottom, Position firstPlayer)
        {
            // Create a new game board.
            _board = new Board(firstPlayer);

            // Determine the player who starts.
            if (firstPlayer == Position.Top)
            {
                Console.WriteLine("Player " + playerTop.GetName() + " starts.");
            }
            else
            {
                Console.WriteLine("Player " + playerBottom.GetName() + " starts.");
            }

            // Display the current state of the game board.
            _board.Display();

            // Continue rotating turns till the game is over.
            while (!_board.GameOver())
            {
                Console.WriteLine();

                // Get the player's move and output what move the player made.
                if (_board.WhoseMove() == Position.Top)
                {
                    _move = playerTop.ChooseMove(_board);
                    Console.WriteLine(playerTop.GetName() + " chooses move " + _move);
                }

                else
                {
                    _move = playerBottom.ChooseMove(_board);
                    Console.WriteLine(playerBottom.GetName() + " chooses move " + _move);
                }

                // Commit the move to the game state. (true = verbose, false = non-verbose)
                _board.MakeMove(_move, true);

                // Display the new state of the game board.
                _board.Display();

                // Game is over, determine final results.
                if (_board.GameOver())
                {
                    if (_board.Winner() == Position.Top)
                    {
                        Console.WriteLine("Player " + playerTop.GetName() +
                        " (TOP) wins " + _board.ScoreTopPlayer() + " to " + _board.ScoreBottomPlayer());
                    }
                    else if (_board.Winner() == Position.Bottom)
                    {
                        Console.WriteLine("Player " + playerBottom.GetName() +
                        " (BOTTOM) wins " + _board.ScoreBottomPlayer() + " to " + _board.ScoreTopPlayer());
                    }
                    else
                    {
                        Console.WriteLine("A tie!");
                    }
                }
                // Game is not over, ask player to make their move.
                else
                    if (_board.WhoseMove() == Position.Top)
                    Console.WriteLine(playerTop.GetName() + " to move.");
                else
                    Console.WriteLine(playerBottom.GetName() + " to move.");
            }

            // Return the final score for the match.
            return _board.ScoreTopPlayer();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Main method outputs the results of each match to the console.
        /// </summary>
        /// <param name="arguments">optional command-line arguments</param>
        public static void Main(string[] arguments)
        {
            // Boolean variables for debugging purposes.
            bool playDefaultGame = false;
            bool playCustomGameHumanOnBottomGoesFirst = true;
            bool playCustomGameAIOnTopGoesFirst = false;
            bool playCustomGameHumanOnTopGoesFirst = false;
            bool playCustomGameAIOnBottomGoesFirst = false;
            bool debugHeuristicEvaluationFunctionTOP = false;
            bool debugHeuristicEvaluationFunctionBOTTOM = false;
            bool debugChooseMoveMiniMaxAlgorithmFunctionTop = false;
            bool debugChooseMoveMiniMaxAlgorithmFunctionBottom = false;

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Play the default game.

            if (playDefaultGame == true)
            {
                // Obtain final score for the TOP and BOTTOM player.
                int finalScore;

                Console.WriteLine("\n================ Game 1 ================");
                finalScore = PlayGame(AIPlayerBonzoTop, HumanPlayerBottom, Position.Bottom);

                Console.WriteLine("\n================ Game 2 ================");
                finalScore += PlayGame(AIPlayerBonzoTop, HumanPlayerBottom, Position.Top);

                Console.WriteLine("\n========================================");
                Console.Write("Match result: ");

                // Determine the winner and loser.
                int botScore = 96 - finalScore;

                if (finalScore > 48)
                {
                    Console.WriteLine(AIPlayerBonzoTop.GetName() + " wins " + finalScore + " to " + botScore);
                    AIPlayerBonzoTop.Gloat();
                }
                else if (botScore > 48)
                {
                    Console.WriteLine(HumanPlayerBottom.GetName() + " wins " + botScore + " to " + finalScore);
                    HumanPlayerBottom.Gloat();
                }
                else
                    Console.WriteLine("Match was a tie, 48-48!");

                Console.Read();
            }

            // For testing purposes.

            PlayCustomGameHumanOnBottomGoesFirst(playCustomGameHumanOnBottomGoesFirst);

            PlayCustomGameAiOnTopGoesFirst(playCustomGameAIOnTopGoesFirst);

            PlayCustomGameHumanOnTopGoesFirst(playCustomGameHumanOnTopGoesFirst);

            PlayCustomGameAiOnBottomGoesFirst(playCustomGameAIOnBottomGoesFirst);

            DebugHeuristicEvaluationFunction(debugHeuristicEvaluationFunctionTOP, debugHeuristicEvaluationFunctionBOTTOM);

            DebugChooseMoveMiniMaxAlgorithmFunction(debugChooseMoveMiniMaxAlgorithmFunctionTop, debugChooseMoveMiniMaxAlgorithmFunctionBottom);

            while (true)
            {
                //Do nothing.  Keep terminal open.
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Test to see that mini-max algorithm function In jj47Player works as intended.
        /// </summary>
        /// <param name="debugChooseMoveMiniMaxAlgorithmFunctionTop">enable or disable for TOP player</param>
        /// <param name="debugChooseMoveMiniMaxAlgorithmFunctionBottom">enable or disable for BOTTOM player</param>
        private static void DebugChooseMoveMiniMaxAlgorithmFunction(bool debugChooseMoveMiniMaxAlgorithmFunctionTop,
            bool debugChooseMoveMiniMaxAlgorithmFunctionBottom)
        {
            // Test for TOP player.
            if (debugChooseMoveMiniMaxAlgorithmFunctionTop == true)
            {
                // Create new game board.
                Board testBoardMiniMaxTOP = new Board(Position.Top);

                // Test as TOP Player.
                jj47Player testTopPlayer = new jj47Player(Position.Top, 3000);

                // Calls method that implement mini-max algorithm to predict the optimal move.
                int optimalMove = testTopPlayer.ChooseMove(testBoardMiniMaxTOP);

                Console.WriteLine("TOP Player AI determined optimal board move was: {0}", optimalMove);
            }

            // Test for BOTTOM player.
            if (debugChooseMoveMiniMaxAlgorithmFunctionBottom == true)
            {
                // Create new game board.
                Board testBoardMiniMaxBottom = new Board(Position.Bottom);

                // Test as TOP Player.
                jj47Player testBottomPlayer = new jj47Player(Position.Bottom, 3000);

                // Calls method that implement mini-max algorithm to predict the optimal move.
                int optimalMove = testBottomPlayer.ChooseMove(testBoardMiniMaxBottom);

                Console.WriteLine("BOTTOM Player AI determined optimal board move was: {0}", optimalMove);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Test to see that heuristic evaluation function In jj47Player works as intended.
        /// </summary>
        /// <param name="debugHeuristicEvaluationFunctionTOP">enable or disable for TOP player</param>
        /// <param name="debugHeuristicEvaluationFunctionBOTTOM">enable or disable for BOTTOM player</param>
        private static void DebugHeuristicEvaluationFunction(bool debugHeuristicEvaluationFunctionTOP,
            bool debugHeuristicEvaluationFunctionBOTTOM)
        {
            // Test for TOP player.
            if (debugHeuristicEvaluationFunctionTOP == true)
            {
                // Create new game board.
                Board testBoardHeuristicTop = new Board(Position.Top);

                testBoardHeuristicTop.Display();
                Console.WriteLine("\n\n");

                // Debug - test setter for number of stones at specified position.
                testBoardHeuristicTop.SetStonesAt(2, 0);

                testBoardHeuristicTop.Display();
                Console.WriteLine("\n\n");

                if (testBoardHeuristicTop.StonesAt(2) != 0)
                {
                    Console.WriteLine("Method setStonesAt(position) not functioning properly");
                }

                // Store the board value the AI determined.
                int moveTopValue = -1;

                //Test as TOP Player.
                jj47Player testTopPlayer = new jj47Player(Position.Top, 100000);

                moveTopValue = testTopPlayer.HeuristicEvaluation(testBoardHeuristicTop);

                Console.WriteLine("TOP Player AI determined optimal board value was: {0}", moveTopValue);
            }

            // Test for BOTTOM player.
            if (debugHeuristicEvaluationFunctionBOTTOM == true)
            {
                // Create new game board.
                Board testBoardHeuristicBottom = new Board(Position.Bottom);

                testBoardHeuristicBottom.Display();
                Console.WriteLine("\n\n");

                // Debug - test setter for number of stones at specified position.
                testBoardHeuristicBottom.SetStonesAt(10, 0);

                testBoardHeuristicBottom.Display();
                Console.WriteLine("\n\n");

                if (testBoardHeuristicBottom.StonesAt(10) != 0)
                {
                    Console.WriteLine("Method setStonesAt(position) not functioning properly");
                }

                // Store the board value the AI determined.
                int moveBottomValue = -1;

                // Test as BOTTOM Player.
                jj47Player testBottomPlayer = new jj47Player(Position.Bottom, 100000);

                moveBottomValue = testBottomPlayer.HeuristicEvaluation(testBoardHeuristicBottom);

                Console.WriteLine("BOTTOM Player AI determined optimal board value was: {0}", moveBottomValue);
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Play a human versus AI match where the human player is the TOP player and the AI goes first.
        /// </summary>
        /// <param name="playCustomGameAIOnBottomGoesFirst">turn method on or off</param>
        private static void PlayCustomGameAiOnBottomGoesFirst(bool playCustomGameAIOnBottomGoesFirst)
        {
            if (playCustomGameAIOnBottomGoesFirst == true)
            {
                // Obtain final score for the TOP and BOTTOM player.
                int finalScore;

                Console.WriteLine("\n================ Game 1 ================");
                finalScore = PlayGame(HumanPlayerTop, AIPlayerJJ47Bottom, Position.Bottom);

                Console.WriteLine("\n================ Game 2 ================");
                finalScore += PlayGame(HumanPlayerTop, AIPlayerJJ47Bottom, Position.Top);

                Console.WriteLine("\n========================================");
                Console.Write("Match result: ");

                // Determine the winner and loser.
                int botScore = 96 - finalScore;

                if (finalScore > 48)
                {
                    Console.WriteLine(AIPlayerJJ47Bottom.GetName() + " wins " + finalScore + " to " + botScore);
                    AIPlayerJJ47Bottom.Gloat();
                }
                else if (botScore > 48)
                {
                    Console.WriteLine(HumanPlayerTop.GetName() + " wins " + botScore + " to " + finalScore);
                    HumanPlayerTop.Gloat();
                }
                else
                    Console.WriteLine("Match was a tie, 48-48!");

                Console.Read();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Play a human versus AI match where the human player is the TOP player and the human goes first.
        /// </summary>
        /// <param name="playCustomGameHumanOnTopGoesFirst">turn method on or off</param>
        private static void PlayCustomGameHumanOnTopGoesFirst(bool playCustomGameHumanOnTopGoesFirst)
        {
            if (playCustomGameHumanOnTopGoesFirst == true)
            {
                // Obtain final score for the TOP and BOTTOM player.
                int finalScore;

                Console.WriteLine("\n================ Game 1 ================");
                finalScore = PlayGame(HumanPlayerTop, AIPlayerJJ47Bottom, Position.Top);

                Console.WriteLine("\n================ Game 2 ================");
                finalScore += PlayGame(HumanPlayerTop, AIPlayerJJ47Bottom, Position.Bottom);

                Console.WriteLine("\n========================================");
                Console.Write("Match result: ");

                // Determine the winner and loser.
                int botScore = 96 - finalScore;

                if (finalScore > 48)
                {
                    Console.WriteLine(AIPlayerJJ47Bottom.GetName() + " wins " + finalScore + " to " + botScore);
                    AIPlayerJJ47Bottom.Gloat();
                }
                else if (botScore > 48)
                {
                    Console.WriteLine(HumanPlayerTop.GetName() + " wins " + botScore + " to " + finalScore);
                    HumanPlayerTop.Gloat();
                }
                else
                    Console.WriteLine("Match was a tie, 48-48!");

                Console.Read();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Play a human versus AI match where the human player is the BOTTOM player and the AI goes first.
        /// </summary>
        /// <param name="playCustomGameAIOnTopGoesFirst">turn method on or off</param>
        private static void PlayCustomGameAiOnTopGoesFirst(bool playCustomGameAIOnTopGoesFirst)
        {
            if (playCustomGameAIOnTopGoesFirst == true)
            {
                // Obtain final score for the TOP and BOTTOM player.
                int finalScore;

                Console.WriteLine("\n================ Game 1 ================");
                finalScore = PlayGame(AIPlayerJJ47Top, HumanPlayerBottom, Position.Top);

                Console.WriteLine("\n================ Game 2 ================");
                finalScore += PlayGame(AIPlayerJJ47Top, HumanPlayerBottom, Position.Bottom);

                Console.WriteLine("\n========================================");
                Console.Write("Match result: ");

                // Determine the winner and loser.
                int botScore = 96 - finalScore;

                if (finalScore > 48)
                {
                    Console.WriteLine(AIPlayerJJ47Top.GetName() + " wins " + finalScore + " to " + botScore);
                    AIPlayerJJ47Top.Gloat();
                }
                else if (botScore > 48)
                {
                    Console.WriteLine(HumanPlayerBottom.GetName() + " wins " + botScore + " to " + finalScore);
                    HumanPlayerBottom.Gloat();
                }
                else
                    Console.WriteLine("Match was a tie, 48-48!");

                Console.Read();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Play a human versus AI match where the human player is the BOTTOM player and the human goes first.
        /// </summary>
        /// <param name="playCustomGameHumanOnBottomGoesFirst">turn method on or off</param>
        private static void PlayCustomGameHumanOnBottomGoesFirst(bool playCustomGameHumanOnBottomGoesFirst)
        {
            if (playCustomGameHumanOnBottomGoesFirst == true)
            {
                // Obtain final score for the TOP and BOTTOM player.
                int finalScore;

                Console.WriteLine("\n================ Game 1 ================");
                finalScore = PlayGame(AIPlayerJJ47Top, HumanPlayerBottom, Position.Bottom);

                Console.WriteLine("\n================ Game 2 ================");
                finalScore += PlayGame(AIPlayerJJ47Top, HumanPlayerBottom, Position.Top);

                Console.WriteLine("\n========================================");
                Console.Write("Match result: ");

                // Determine the winner and loser.
                int botScore = 96 - finalScore;

                if (finalScore > 48)
                {
                    Console.WriteLine(AIPlayerJJ47Top.GetName() + " wins " + finalScore + " to " + botScore);
                    AIPlayerJJ47Top.Gloat();
                }
                else if (botScore > 48)
                {
                    Console.WriteLine(HumanPlayerBottom.GetName() + " wins " + botScore + " to " + finalScore);
                    HumanPlayerBottom.Gloat();
                }
                else
                    Console.WriteLine("Match was a tie, 48-48!");

                Console.Read();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}
