﻿// ReSharper disable InvalidXmlDocComment
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
using System.Diagnostics;
using System.Linq;

/// <summary>
/// Namespace this class belongs to.
/// </summary>
namespace Mankalah
{
    /// <summary>
    /// Class jj47Player defines a hopefully less psychologically deficient Mankalah player then BonzoPlayer.
    /// This hopefully less sorry excuse for a player implements a heuristic evaluation function and the mini-max
    /// algorithm to choose the most optimal move to make. (will hopefully make Skynet proud)
    /// TODO: make AI at least as intelligent as a 10 year old... (gets rekted by Bonzo B and Bonzo C in tournament player)
    /// TODO: remember to rename functions/methods to default template names when attempting to create .dll file.
    /// </summary>
    public class jj47Player : Player
    {
        // Class fields.
        private static int turnTimeLimit;

        // Store each position and their opposing position on the board (for captures).
        private static SortedDictionary<int, int> _opposingPositionsTop;
        private static SortedDictionary<int, int> _opposingPositionsBottom;

        // Give each position on the board a string label and store their associated integer value.
        private static SortedDictionary<string, int> _positionLabelsTop;
        private static SortedDictionary<string, int> _positionLabelsBottom;

        // Populate dictionary containing current # of stones at each TOP and BOTTOM position.
        private SortedDictionary<int, int> topPositionsCurrentNumStones;
        private SortedDictionary<int, int> bottomPositionsCurrentNumStones;

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Constructor that sets the AI Player Position and Name.
        /// Note: Calls parent class Player's constructor.
        /// </summary>
        /// <param name="pos">Position of PLayer - TOP or BOTTOM</param>
        /// <param name="timeLimit">max time per turn or move</param>
        public jj47Player(Position pos, int timeLimit) : base(pos, "jj47", timeLimit)
        {
            // Set the turn time limit.
            turnTimeLimit = timeLimit;

            // Control debugging.
            bool debug = false;

            // Determine positions opposed to TOP positions.
            _opposingPositionsTop = new SortedDictionary<int, int>();
            // Determine positions opposed to BOTTOM positions.
            _opposingPositionsBottom = new SortedDictionary<int, int>();

            int topCounter = 12;
            int bottomCounter = 0;

            // For bottom positions, record their opposing positions on top.
            for (int bottom = 0; bottom < 6; bottom++)
            {
                _opposingPositionsBottom.Add(bottom, topCounter);
                topCounter--;

            }

            // For top positions, record their opposing positions on bottom.
            for (int top = 12; top > 6; top--)
            {
                _opposingPositionsTop.Add(top, bottomCounter);
                bottomCounter++;
            }

            // Label each position by a string and its associated integer value.
            _positionLabelsTop = new SortedDictionary<string, int>();
            _positionLabelsBottom = new SortedDictionary<string, int>();

            // Label the bottom positions.
            for (int position = 0; position < 6; position++)
            {
                string label = "A" + position;

                _positionLabelsBottom.Add(label, position);
            }

            //positionLabelsBOTTOM.Add("Player A Mankalah", 6);

            // Label the top positions.
            for (int position = 12; position > 6; position--)
            {
                string label = "B" + position;

                _positionLabelsTop.Add(label, position);
            }

            //positionLabelsTOP.Add("Player B Mankalah", 13);

            // Debugging to check data structures are storing the correct values.
            if (debug == true)
            {
                Console.WriteLine("\n\nOpposing position for TOP position:");
                foreach (KeyValuePair<int, int> entry in _opposingPositionsTop)
                {
                    Console.WriteLine("TOP Position: {0}, BOTTOM Opposing Position: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nOpposing position for BOTTOM position:");
                foreach (KeyValuePair<int, int> entry in _opposingPositionsBottom)
                {
                    Console.WriteLine("BOTTOM Position: {0}, TOP Opposing Position: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nLabels for TOP position:");
                foreach (KeyValuePair<string, int> entry in _positionLabelsTop)
                {
                    Console.WriteLine("TOP Position Label: {0}, TOP Position Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nLabels for BOTTOM position:");
                foreach (KeyValuePair<string, int> entry in _positionLabelsBottom)
                {
                    Console.WriteLine("BOTTOM Position Label: {0}, BOTTOM Position Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\n");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method calculates the move the AI Player should choose.
        /// </summary>
        /// 
        /// <param name="b">Game Board object</param>
        /// <returns>the move the AI chose to make</returns>
        public override int ChooseMove(Board b)
        {
            // Stopwatch to keep track of elapsed time.
            Stopwatch elapsedTime = new Stopwatch();
            elapsedTime.Start();

            // Set depth we can search to.
            int initialDepth = int.MaxValue;

            // Call mini-max algorithm.
            //int optimalMove = MiniMaxAlgorithm(b, initialDepth, elapsedTime).GetOptimalMove();

            // Initial alpha and beta values.
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            // Call mini-max algorithm with Alpha-Beta Pruning support.
            int optimalMovePruned = MiniMaxAlgorithmWithAlphaBetaPruning(b, initialDepth, elapsedTime, alpha, beta).GetOptimalMove();

            return optimalMovePruned;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method implements the mini-max algorithm via a staged (limited) depth-first search.
        /// Supports alpha-beta pruning.
        /// TODO - confirm it is actually pruning...
        /// </summary>
        /// 
        /// <param name="board">Game Board object</param>
        /// <param name="depth">current depth of the recursive search</param>
        /// <param name="time">keep track of the execution time of mini-max algorithm</param>
        /// <param name="alpha">TOP Player - maximizing value</param>
        /// <param name="beta">BOTTOM Player - minimizing value</param>
        /// <returns>the optimal position and optimal weight value of the game board</returns>
        private MovePredictionResults MiniMaxAlgorithmWithAlphaBetaPruning(Board board, int depth, Stopwatch time,
            int alpha, int beta)
        {
            int optimalMoveValue;
            int optimalMove = -1;

            /**
             * Base Case - ends recursion.
             * 1. We have reach the depth limit.
             * 2. The game is over.
             */
            if (depth == 0 || board.GameOver())
            {
                return new MovePredictionResults(-1, HeuristicEvaluation(board));
            }

            // Determine optimal move for TOP (MAX) Player.
            if (board.WhoseMove() == Position.Top)
            {
                optimalMoveValue = int.MinValue;

                // Loop through all possible choice of moves per turn.
                foreach (KeyValuePair<string, int> entry in _positionLabelsTop)
                {
                    if (board.LegalMove(entry.Value) && time.ElapsedMilliseconds < turnTimeLimit)
                    {
                        // Duplicate game board so we don't overwrite current game state.
                        Board predictionBoard = new Board(board);

                        // Initiate the move.
                        predictionBoard.MakeMove(entry.Value, false);

                        // Determine the optimality of that move.
                        int moveValue = MiniMaxAlgorithmWithAlphaBetaPruning(predictionBoard, depth - 1, time, alpha, beta).GetOptimalMoveScore();

                        alpha = Math.Max(alpha, optimalMoveValue);

                        // Set the best possible predicted move.
                        if (moveValue > optimalMoveValue)
                        {
                            optimalMoveValue = moveValue;
                            optimalMove = entry.Value;
                        }

                        // Prune.
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }
            // Determine optimal move for BOTTOM (MIN) Player.
            else if (board.WhoseMove() == Position.Bottom)
            {
                optimalMoveValue = int.MaxValue;

                // Loop through all possible choice of moves per turn.
                foreach (KeyValuePair<string, int> entry in _positionLabelsBottom)
                {
                    if (board.LegalMove(entry.Value) && time.ElapsedMilliseconds < turnTimeLimit)
                    {
                        // Duplicate game board so we don't overwrite current game state.
                        Board predictionBoard = new Board(board);

                        // Initiate the move.
                        predictionBoard.MakeMove(entry.Value, false);

                        // Determine the optimality of that move.
                        int moveValue = MiniMaxAlgorithmWithAlphaBetaPruning(predictionBoard, depth - 1, time, alpha, beta).GetOptimalMoveScore();

                        beta = Math.Min(beta, optimalMoveValue);

                        // Set the best possible predicted move.
                        if (moveValue < optimalMoveValue)
                        {
                            optimalMoveValue = moveValue;
                            optimalMove = entry.Value;
                        }

                        //Prune.
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }
            // Not a valid Player.
            else
            {
                return new MovePredictionResults(-1, 0);
            }
            return new MovePredictionResults(optimalMove, optimalMoveValue);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method implements the mini-max algorithm via a staged (limited) depth-first search.
        /// Does not support alpha-beta pruning.
        /// </summary>
        /// 
        /// <param name="board">Game Board object</param>
        /// <param name="depth">current depth of the recursive search</param>
        /// <param name="time">keep track of the execution time of mini-max algorithm</param>
        /// <returns>the optimal position and optimal weight value of the game board</returns>
        private MovePredictionResults MiniMaxAlgorithm(Board board, int depth, Stopwatch time)
        {
            int optimalMoveValue;
            int optimalMove = -1;

            /**
             * Base Case - ends recursion.
             * 1. We have reach the depth limit.
             * 2. The game is over.
             */
            if (depth == 0 || board.GameOver())
            {
                return new MovePredictionResults(-1, HeuristicEvaluation(board));
            }

            // Determine optimal move for TOP (MAX) Player.
            if (board.WhoseMove() == Position.Top)
            {
                optimalMoveValue = int.MinValue;

                // Loop through all possible choice of moves per turn.
                foreach (KeyValuePair<string, int> entry in _positionLabelsTop)
                {
                    if (board.LegalMove(entry.Value) && time.ElapsedMilliseconds < turnTimeLimit)
                    {
                        // Duplicate game board so we don't overwrite current game state.
                        Board predictionBoard = new Board(board);

                        // Initiate the move.
                        predictionBoard.MakeMove(entry.Value, false);

                        // Determine the optimality of that move.
                        int moveValue = MiniMaxAlgorithm(predictionBoard, depth - 1, time).GetOptimalMoveScore();

                        // Set the best possible predicted move.
                        if (moveValue > optimalMoveValue)
                        {
                            optimalMoveValue = moveValue;
                            optimalMove = entry.Value;
                        }
                    }
                }
            }
            // Determine optimal move for BOTTOM (MIN) Player.
            else if (board.WhoseMove() == Position.Bottom)
            {
                optimalMoveValue = int.MaxValue;

                // Loop through all possible choice of moves per turn.
                foreach (KeyValuePair<string, int> entry in _positionLabelsBottom)
                {
                    if (board.LegalMove(entry.Value) && time.ElapsedMilliseconds < turnTimeLimit)
                    {
                        // Duplicate game board so we don't overwrite current game state.
                        Board predictionBoard = new Board(board);

                        // Initiate the move.
                        predictionBoard.MakeMove(entry.Value, false);

                        // Determine the optimality of that move.
                        int moveValue = MiniMaxAlgorithm(predictionBoard, depth - 1, time).GetOptimalMoveScore();

                        // Set the best possible predicted move.
                        if (moveValue < optimalMoveValue)
                        {
                            optimalMoveValue = moveValue;
                            optimalMove = entry.Value;
                        }
                    }
                }
            }
            // Not a valid Player.
            else
            {
                return new MovePredictionResults(-1, 0);
            }
            return new MovePredictionResults(optimalMove, optimalMoveValue);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method to determine the optimality of the state of the game board.
        /// 
        /// Note: TOP player = MAX --> most positive scores are better.
        /// Note: BOTTOM player = MIN --> most negative scores are better.
        ///
        /// Changelog: Modified to return the optimality of the board instead of the best move possible based on the optimality of
        /// all positions in comparison to each other. (I think I misunderstood how the heuristic function was supposed to work)
        /// </summary>
        /// 
        /// <param name="b">Game Board object</param>
        /// <returns>how optimal the state of the game board is</returns>
        public override int HeuristicEvaluation(Board b)
        {
            // Store and update the optimality weight value of each position during heuristic evaluation for TOP player.
            SortedDictionary<int, int> positionOptimalValueMAX = new SortedDictionary<int, int>();

            foreach (KeyValuePair<string, int> entry in _positionLabelsTop)
            {
                //positionOptimalValueMAX.Add(entry.Value, int.MinValue);
                positionOptimalValueMAX.Add(entry.Value, 0);
            }
            // Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player.
            SortedDictionary<int, int> positionOptimalValueMIN = new SortedDictionary<int, int>();

            foreach (KeyValuePair<string, int> entry in _positionLabelsBottom)
            {
                //positionOptimalValueMIN.Add(entry.Value, int.MaxValue);
                positionOptimalValueMIN.Add(entry.Value, 0);
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - check we have set default optimal values for each position to Int.Max or Int.Min.
            bool debugDefaultOptimalValue = false;

            if (debugDefaultOptimalValue == true)
            {
                Console.WriteLine("\n\nDefault optimal value for each position for TOP player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nDefault optimal value for each position for BOTTOM player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\n");
            }

            // Heuristic Condition 0.
            IncreaseValueOfNonEmptyPositions(b, positionOptimalValueMAX, positionOptimalValueMIN);

            // Heuristic Condition 1.
            IncreaseValueOfPositionsWithMostStones(b, positionOptimalValueMAX, positionOptimalValueMIN);

            // Heuristic Condition 2.
            IncreaseValueOfPositionsWithGoAgains(b, positionOptimalValueMAX, positionOptimalValueMIN);

            // Heuristic Condition 3.
            IncreaseValueOfPositionsWithCaptures(b, positionOptimalValueMAX, positionOptimalValueMIN);

            // Heuristic Condition 4. (deprecated)
            //return SelectOptimalMove(b, positionOptimalValueMAX, positionOptimalValueMIN);

            // Heuristic Results.
            return CalculateBoardOptimalityValue(b, positionOptimalValueMAX, positionOptimalValueMIN);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// HEURISTIC CONDITION FIVE - Increase or Decrease value of board if own more stones than opponent.
        /// TODO - keep track of the # of stones owned from previous move compared to # owned for current move.
        /// </summary>
        /// <param name="b">Game Board object</param>
        private static int IncreaseValueOfBoardBasedOnNumStonedOwnedVersusOpponent(Board b, int boardValue)
        {
            // Get the # of stones in each player's Kalah.
            int stonesOwnedTop = b.StonesAt(13);
            int stonesOwnedBottom = b.StonesAt(6);

            if (b.WhoseMove() == Position.Top)
            {
                // If the move keeps us ahead of our opponent in # of stones owned.
                if (stonesOwnedTop > stonesOwnedBottom)
                {
                    boardValue += 100000000;
                }
            }
            if (b.WhoseMove() == Position.Bottom)
            {
                // If the move keeps us ahead of our opponent in # of stones owned.
                if (stonesOwnedBottom > stonesOwnedTop)
                {
                    boardValue -= 100000000;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            bool debugCheckBoardOptimalityValue = false;

            // Debug - check that our summed values make sense.
            if (debugCheckBoardOptimalityValue == true)
            {
                Console.WriteLine("\n\nNumber of stones in TOP player's Kalah:");
                Console.WriteLine("MAX - Position 13: # of stones: {0}", b.StonesAt(13));

                Console.WriteLine("\n\nNumber of stones in BOTTOM player's Kalah:");
                Console.WriteLine("MIN - Position 6: # of stones: {0}", b.StonesAt(6));

                Console.WriteLine("\n\n");
                Console.WriteLine("The board optimality value is: {0}", boardValue);
                Console.WriteLine("\n\n");
            }
            return boardValue;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// HEURISTIC RESULTS - Calculate the optimality of the current state of the game board.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// <param name="positionOptimalValueMAX">Store and update the optimality weight value of each position during heuristic evaluation for TOP player</param>
        /// <param name="positionOptimalValueMIN">Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player</param>
        /// <returns>the value of the optimal move selected</returns>
        private static int CalculateBoardOptimalityValue(Board b, SortedDictionary<int, int> positionOptimalValueMAX,
            SortedDictionary<int, int> positionOptimalValueMIN)
        {
            // Store the optimality for the game board.
            int boardOptimalityValue = 0;
            int boardOptimalityValueTop = 0;
            int boardOptimalityValueBottom = 0;

            // If we are evaluating the optimal move for the TOP player.
            if (b.WhoseMove() == Position.Top)
            {
                // Sum the optimality values for all TOP board positions.
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    boardOptimalityValueTop += entry.Value;
                }
                boardOptimalityValue = boardOptimalityValueTop;
            }

            // If we are evaluating the optimal move for the BOTTOM player.
            if (b.WhoseMove() == Position.Bottom)
            {
                // Sum the optimality values for all BOTTOM board positions.
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    boardOptimalityValueBottom += entry.Value;
                }
                boardOptimalityValue = boardOptimalityValueBottom;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            bool debugCheckOptimalityValues = false;

            // Debug - check that our summed values make sense.
            if (debugCheckOptimalityValues == true)
            {
                Console.WriteLine("\n\nUpdated optimal position weight values for TOP player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\nUpdated optimal position weight values for BOTTOM player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\n");

                // Display current state of the game board.
                b.Display();

                Console.WriteLine("\n\n");
                Console.WriteLine("The board optimality value is: {0}", boardOptimalityValue);
                Console.WriteLine("\n\n");
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Heuristic Condition 5.
            boardOptimalityValue = IncreaseValueOfBoardBasedOnNumStonedOwnedVersusOpponent(b, boardOptimalityValue);

            return boardOptimalityValue;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// HEURISTIC CONDITION FOUR - If positions have the same optimal weight value, randomly select one as the optimal move.
        /// TODO - confirm that this is working properly.
        /// Note: deprecated; using function to calculate the optimality of the entire game board and returning that value instead
        /// of the optimality value of just a single move, which is probably what the heuristic was supposed to do in the first place.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// <param name="positionOptimalValueMAX">Store and update the optimality weight value of each position during heuristic evaluation for TOP player</param>
        /// <param name="positionOptimalValueMIN">Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player</param>
        /// <returns>the value of the optimal move selected</returns>
        private static int SelectOptimalMove(Board b, SortedDictionary<int, int> positionOptimalValueMAX,
            SortedDictionary<int, int> positionOptimalValueMIN)
        {
            // Determine which move(s) are optimal, and randomly select one, if there are multiple choices.
            SortedDictionary<int, int> optimalMovesTop = new SortedDictionary<int, int>();
            SortedDictionary<int, int> optimalMovesBottom = new SortedDictionary<int, int>();

            // Store the optimal position to move.
            int optimalMove = 0;
            // Store the value for the optimal position to move.
            int optimalMoveValue = 0;

            // Random number generator.
            Random random = new Random();

            // Determine the move with the highest or lowest associated value.
            int currentMaxValue = int.MinValue;
            int currentMinValue = int.MaxValue;

            // If we are evaluating the optimal move for the TOP player.
            if (b.WhoseMove() == Position.Top)
            {
                // Set to highest weight value found.
                currentMaxValue = positionOptimalValueMAX.Values.Max();

                // Search for multiple instances of the highest weight value.
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    if (entry.Value == currentMaxValue)
                    {
                        optimalMovesTop.Add(entry.Key, entry.Value);
                    }
                }

                List<int> keys = optimalMovesTop.Keys.ToList();

                // Randomly select one of the moves with same highest weight value.
                optimalMove = keys[random.Next(keys.Count)];
                optimalMoveValue = optimalMovesTop[optimalMove];
            }

            // If we are evaluating the optimal move for the BOTTOM player.
            if (b.WhoseMove() == Position.Bottom)
            {
                // Set to lowest weight value found.
                currentMinValue = positionOptimalValueMIN.Values.Min();

                // Search for multiple isntances of the lowest weight value.
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    if (entry.Value == currentMinValue)
                    {
                        optimalMovesBottom.Add(entry.Key, entry.Value);
                    }
                }

                List<int> keys = optimalMovesBottom.Keys.ToList();

                // Randomly select one of the moves with the same lowest weight value.
                optimalMove = keys[random.Next(keys.Count)];
                optimalMoveValue = optimalMovesBottom[optimalMove];
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            bool debugCheckOptimalMoveAndOptimalMoveValue = false;

            // Debug - check that our optimal move is what we expect it to be.
            if (debugCheckOptimalMoveAndOptimalMoveValue == true)
            {
                Console.WriteLine("\n\nUpdated optimal position weight values for TOP player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\nUpdated optimal position weight values for BOTTOM player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\n");

                Console.WriteLine("\n\nOptimal move positions for TOP player:");
                foreach (KeyValuePair<int, int> entry in optimalMovesTop)
                {
                    Console.WriteLine("MAX - Optimal Position: {0}", entry);
                }

                Console.WriteLine("\n\nOptimal move positions for BOTTOM player:");
                foreach (KeyValuePair<int, int> entry in optimalMovesBottom)
                {
                    Console.WriteLine("MIN - Optimal Position: {0}", entry);
                }

                Console.WriteLine("\n\n");

                // Confirm we have stored all optimal positions.
                b.Display();

                Console.WriteLine("\n\n");
                Console.WriteLine("The optimal move is: {0}", optimalMove);
                Console.WriteLine("The value for the optimal move is: {0}", optimalMoveValue);
                Console.WriteLine("\n\n");
            }
            return optimalMoveValue;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// HEURISTIC CONDITION THREE - Increase value of position(s) that permit captures.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// <param name="positionOptimalValueMAX">Store and update the optimality weight value of each position during heuristic evaluation for TOP player</param>
        /// <param name="positionOptimalValueMIN">Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player</param>
        private void IncreaseValueOfPositionsWithCaptures(Board b, SortedDictionary<int, int> positionOptimalValueMAX,
            SortedDictionary<int, int> positionOptimalValueMIN)
        {
            // Determine if the hole is empty. If yes, add to list of empty positions and add the number of stones in 
            // its opposing position.
            SortedDictionary<int, int> topEmptyPositionsAndNumStonesOpposingPosition = new SortedDictionary<int, int>();
            SortedDictionary<int, int> bottomEmptyPositionsAndNumStonesOpposingPosition = new SortedDictionary<int, int>();

            // Determine empty positions and opposing positions' stones for TOP player.
            foreach (KeyValuePair<int, int> entry in _opposingPositionsTop)
            {
                if (b.StonesAt(entry.Key) == 0)
                {
                    topEmptyPositionsAndNumStonesOpposingPosition.Add(entry.Key, b.StonesAt(entry.Value));
                }
            }

            // Determine empty positions and opposing positions' stones for TOP player.
            foreach (KeyValuePair<int, int> entry in _opposingPositionsBottom)
            {
                if (b.StonesAt(entry.Key) == 0)
                {
                    bottomEmptyPositionsAndNumStonesOpposingPosition.Add(entry.Key, b.StonesAt(entry.Value));
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - check that we have properly added empty positions and stones in their opposing positions.
            bool debugEmptyPositions = true;

            if (debugEmptyPositions == false)
            {
                Console.WriteLine(("\n\nTOP Empty positions and number of stones in their opposing position:"));
                foreach (KeyValuePair<int, int> entry in topEmptyPositionsAndNumStonesOpposingPosition)
                {
                    Console.WriteLine("TOP Empty position: {0}, Stones at opposing position: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine(("\n\nBOTTOM Empty positions and number of stones in their opposing position:"));
                foreach (KeyValuePair<int, int> entry in bottomEmptyPositionsAndNumStonesOpposingPosition)
                {
                    Console.WriteLine("BOTTOM Empty position: {0}, Stones at opposing position: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\n");

                // Display the state of the board to confirm.
                b.Display();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            List<int> topEmptyPositionWithMostStones = new List<int>();
            List<int> bottomEmptyPositionWithMostStones = new List<int>();

            // Determine the empty position(s) with the most stones in its opposing position for TOP player.
            if (topEmptyPositionsAndNumStonesOpposingPosition.Count != 0)
            {
                int topEmptyPositionWithMostStonesCounter = topEmptyPositionsAndNumStonesOpposingPosition.Values.Max();

                foreach (KeyValuePair<int, int> entry in topEmptyPositionsAndNumStonesOpposingPosition)
                {
                    if (entry.Value == topEmptyPositionWithMostStonesCounter)
                    {
                        topEmptyPositionWithMostStones.Add(entry.Key);
                    }
                }
            }

            // Determine the empty position(s) with the most stones in its opposing position for BOTTOM player.
            if (bottomEmptyPositionsAndNumStonesOpposingPosition.Count != 0)
            {
                int bottomEmptyPositionWithMostStonesCounter = bottomEmptyPositionsAndNumStonesOpposingPosition.Values.Max();

                foreach (KeyValuePair<int, int> entry in bottomEmptyPositionsAndNumStonesOpposingPosition)
                {
                    if (entry.Value == bottomEmptyPositionWithMostStonesCounter)
                    {
                        bottomEmptyPositionWithMostStones.Add(entry.Key);
                    }
                }
            }

            // Increase or decrease optimality value of empty position(s) with the most stones in opposing position.
            foreach (int entry in topEmptyPositionWithMostStones)
            {
                positionOptimalValueMAX[entry] += 100;
            }

            foreach (int entry in bottomEmptyPositionWithMostStones)
            {
                positionOptimalValueMIN[entry] -= 100;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - check that we have updated the optimal weight value for empty position(s) with the most stones in its opposing position.
            bool debugUpdatedOptimalValuesEmptyPositionsWithMostStones = false;

            if (debugUpdatedOptimalValuesEmptyPositionsWithMostStones == true)
            {
                Console.WriteLine(
                    "\n\nUpdated optimal position weight values based on empty position with most stones for TOP player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine(
                    "\n\nUpdated optimal position weight values based on empty position with most stones for BOTTOM player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\n");
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            List<int> topCanCapturePositions = new List<int>();
            List<int> bottomCanCapturePositions = new List<int>();

            // TODO: does this properly take into account skipping past a opponent's cache?
            // TODO: does this properly account for potential multiple skips of opponent's cache, if # of stones high enough?
            // TODO: this part is O(n^2), reduce to O(n) if possible.

            // Determine if we can reach the empty position(s) for TOP player.
            foreach (KeyValuePair<int, int> entry in topPositionsCurrentNumStones)
            {
                int skipOpponentMankalah = 0;

                // Check if we need to skip opponent's cache.
                for (int i = 0; i <= entry.Value; i++)
                {
                    if (entry.Key + i % 13 == 6)
                    {
                        skipOpponentMankalah++;
                    }
                }

                // If we skip opponent's cache, we move one additional position to drop a stone.
                int total = entry.Key + entry.Value + skipOpponentMankalah % 13;

                if (total < 13 && total > 6 && entry.Value != 0 && total != 13 && total != 6)
                {
                    if (topPositionsCurrentNumStones[total] == 0)
                    {
                        topCanCapturePositions.Add(entry.Key);
                    }
                }
            }

            // Determine if we can reach the empty position(s) for BOTTOM player.
            foreach (KeyValuePair<int, int> entry in bottomPositionsCurrentNumStones)
            {
                int skipOpponentMankalah = 0;

                // Check if we need to skip opponent's cache.
                for (int i = 0; i <= entry.Value; i++)
                {
                    if (entry.Key + i % 13 == 13)
                    {
                        skipOpponentMankalah++;
                    }
                }

                // If we skip opponent's cache, we move one additional position to drop a stone.
                int total = entry.Key + entry.Value + skipOpponentMankalah % 13;

                if (total < 6 && total >= 0 && entry.Value != 0 && total != 13 && total != 6)
                {
                    if (bottomPositionsCurrentNumStones[total] == 0)
                    {
                        bottomCanCapturePositions.Add(entry.Key);
                    }
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - check that we have properly determined the position(s) that can be used for captures.
            bool debugShowPotentialCapturePositions = false;

            if (debugShowPotentialCapturePositions == true)
            {
                Console.WriteLine("\n\nPositions that can be used to capture for TOP player:");
                foreach (int entry in topCanCapturePositions)
                {
                    Console.WriteLine("MAX - Position: {0}", entry);
                }

                Console.WriteLine("\n\nPositions that can be used to capture for BOTTOM player");
                foreach (int entry in bottomCanCapturePositions)
                {
                    Console.WriteLine("MIN - Position: {0}", entry);
                }

                Console.WriteLine("\n\n");

                // Confirm we have stored all possible capture positions.
                b.Display();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Increase or decrease optimality value of position(s) that can be used to capture by ending in an empty position.
            foreach (int entry in topCanCapturePositions)
            {
                positionOptimalValueMAX[entry] += 100000;
            }

            foreach (int entry in bottomCanCapturePositions)
            {
                positionOptimalValueMIN[entry] -= 100000;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// HEURISTIC CONDITION TWO - Increase value of position(s) that permit go-agains.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// <param name="positionOptimalValueMAX">Store and update the optimality weight value of each position during heuristic evaluation for TOP player</param>
        /// <param name="positionOptimalValueMIN">Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player</param>
        private static void IncreaseValueOfPositionsWithGoAgains(Board b, SortedDictionary<int, int> positionOptimalValueMAX,
            SortedDictionary<int, int> positionOptimalValueMIN)
        {
            // Determine if we can go again from making a move at this position.
            List<int> goAgainPositionsTop = new List<int>();
            List<int> goAgainPositionsBottom = new List<int>();

            // Determine go-again moves for TOP player.
            for (int i = 12; i > 6; i--)
            {
                if (b.StonesAt(i) == 13 - i)
                {
                    goAgainPositionsTop.Add(i);
                    positionOptimalValueMAX[i] += 10000;
                }
            }

            // Determine go-again moves for BOTTOM player.
            for (int i = 5; i >= 0; i--)
            {
                if (b.StonesAt(i) == 6 - i)
                {
                    goAgainPositionsBottom.Add(i);
                    positionOptimalValueMIN[i] -= 10000;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - check that we have properly updated the optimal position weight values for all go-again positions.
            bool debugUpdatedOptimalValuesGoAgainPositions = false;

            if (debugUpdatedOptimalValuesGoAgainPositions == true)
            {
                Console.WriteLine("\n\nAvailable go-again positions for TOP player:\n");
                foreach (int i in goAgainPositionsTop)
                {
                    Console.WriteLine("TOP: Go-again position: {0}", i);
                }

                Console.WriteLine("\n\nAvailable go-again positions for BOTTOM player:\n");
                foreach (int i in goAgainPositionsBottom)
                {
                    Console.WriteLine("BOTTOM: Go-again position: {0}", i);
                }

                Console.WriteLine("\n\nUpdated optimal position weight values based on go-again positions for TOP player:\n\n");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine(
                    "\n\nUpdated optimal position weight values based on go-again positions for BOTTOM player:\n\n");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\n");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// HEURISTIC CONDITION ONE - Increase value of position(s) with the most stones.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// <param name="positionOptimalValueMAX">Store and update the optimality weight value of each position during heuristic evaluation for TOP player</param>
        /// <param name="positionOptimalValueMIN">Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player</param>
        private void IncreaseValueOfPositionsWithMostStones(Board b, SortedDictionary<int, int> positionOptimalValueMAX,
            SortedDictionary<int, int> positionOptimalValueMIN)
        {
            // Instantiate necessary data structures.
            topPositionsCurrentNumStones = new SortedDictionary<int, int>();
            bottomPositionsCurrentNumStones = new SortedDictionary<int, int>();

            List<int> topPositionWithMostStones = new List<int>();
            List<int> bottomPositionWithMostStones = new List<int>();

            int topPositionWithMostStonesCounter = 0;
            int bottomPositionWithMostStonesCounter = 0;

            // Populate dictionary containing current # of stones at each TOP and BOTTOM position.
            foreach (KeyValuePair<string, int> entry in _positionLabelsTop)
            {
                topPositionsCurrentNumStones.Add(entry.Value, b.StonesAt(entry.Value));
            }

            foreach (KeyValuePair<string, int> entry in _positionLabelsBottom)
            {
                bottomPositionsCurrentNumStones.Add(entry.Value, b.StonesAt(entry.Value));
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - display to console the number of stones at each position.
            bool currentNumStonesEveryPosition = false;

            if (currentNumStonesEveryPosition == true)
            {
                Console.WriteLine("\n\nCurrent # of stones at each position for TOP Player:");
                foreach (KeyValuePair<int, int> entry in topPositionsCurrentNumStones)
                {
                    Console.WriteLine("TOP - Position: {0}, # of stones: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\nCurrent # of stones at each position for BOTTOM Player:");
                foreach (KeyValuePair<int, int> entry in bottomPositionsCurrentNumStones)
                {
                    Console.WriteLine("BOTTOM - Position: {0}, # of stones: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\n");
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Determine the position(s) with the most stones for the TOP player.
            topPositionWithMostStonesCounter = topPositionsCurrentNumStones.Values.Max();

            foreach (KeyValuePair<int, int> entry in topPositionsCurrentNumStones)
            {
                if (entry.Value == topPositionWithMostStonesCounter)
                {
                    topPositionWithMostStones.Add(entry.Key);
                }
            }

            // Determine the position(s) with the most stones for the BOTTOM player.
            bottomPositionWithMostStonesCounter = bottomPositionsCurrentNumStones.Values.Max();

            foreach (KeyValuePair<int, int> entry in bottomPositionsCurrentNumStones)
            {
                if (entry.Value == bottomPositionWithMostStonesCounter)
                {
                    bottomPositionWithMostStones.Add(entry.Key);
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - display to console position(s) with the most stones.

            bool debugPositionsMostStones = false;

            if (debugPositionsMostStones == true)
            {
                Console.WriteLine("\n\nPositions with the most stones for TOP Player:");
                foreach (int entry in topPositionWithMostStones)
                {
                    Console.WriteLine("TOP - Position: {0}", entry);
                }

                Console.WriteLine("\n\nPositions with the most stones for BOTTOM Player:");
                foreach (int entry in bottomPositionWithMostStones)
                {
                    Console.WriteLine("BOTTOM - Position: {0}", entry);
                }

                Console.WriteLine("\n\n");

                // Display the state of the board to confirm.
                b.Display();
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            //Increase or decrease optimality value of position(s) with the most stones.
            foreach (int entry in topPositionWithMostStones)
            {
                positionOptimalValueMAX[entry] += 1000;
            }

            foreach (int entry in bottomPositionWithMostStones)
            {
                positionOptimalValueMIN[entry] -= 1000;
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - check that we have updated the optimal value for the position(s) with the most stones.
            bool debugUpdatedOptimalValuesPositionsMostStones = false;

            if (debugUpdatedOptimalValuesPositionsMostStones == true)
            {
                Console.WriteLine("\n\nUpdated optimal weight values for position(s) with the most stones for TOP Player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\nUpdated optimal weight values for position(s) with the most stones for BOTTOM Player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\n");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// HEURISTIC CONDITION ZERO - Increase value of position(s) that have x > 0 stones.
        /// </summary>
        /// <param name="b">Game Board object</param>
        /// <param name="positionOptimalValueMAX">Store and update the optimality weight value of each position during heuristic evaluation for TOP player</param>
        /// <param name="positionOptimalValueMIN">Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player</param>
        private static void IncreaseValueOfNonEmptyPositions(Board b, SortedDictionary<int, int> positionOptimalValueMAX,
            SortedDictionary<int, int> positionOptimalValueMIN)
        {
            // Increase optimal position weight values for all positions that are not empty of stones.
            // (this will ensure that we never try to choose a move that has no stones in the hole)
            foreach (KeyValuePair<string, int> entry in _positionLabelsTop)
            {
                if (b.StonesAt(entry.Value) != 0)
                {
                    positionOptimalValueMAX[entry.Value] += 10000000;
                }
            }

            foreach (KeyValuePair<string, int> entry in _positionLabelsBottom)
            {
                if (b.StonesAt(entry.Value) != 0)
                {
                    positionOptimalValueMIN[entry.Value] -= 10000000;
                }
            }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////

            // Debug - check that we have updated the optimal value for all non-empty positions.
            bool debugUpdatedOptimalValuesNonEmptyPositions = false;

            if (debugUpdatedOptimalValuesNonEmptyPositions == true)
            {
                Console.WriteLine("\n\nUpdated optimal weight values for non-empty positions for TOP Player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\nUpdated optimal weight values for non-empty positions for BOTTOM Player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }

                Console.WriteLine("\n\n");
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method obtains the image to use as the avatar.
        /// Note: Image stored in Mankalah\bin\Debug
        /// </summary>
        /// <returns>URL or relative file path of the image</returns>
        public override string GetImage()
        {
            // Orc peons generally aren't very smart but are still probably smarter than Bonzo.
            return "OrcPeonPortrait.jpg";
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR METHOD SEPARATOR
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Method permits the user to specify a personalized emotive message.
        /// </summary>
        /// <returns>emotive message</returns>
        public override string Gloat()
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
        private int _optimalMove;
        private int _optimalMoveScore;

        /// <summary>
        /// Constructor initializes the optimal move and its associated score.
        /// </summary>
        /// <param name="move">integer value of position on game board</param>
        /// <param name="score">integer value of score for a particular move</param>
        public MovePredictionResults(int move, int score)
        {
            _optimalMove = move;
            _optimalMoveScore = score;
        }

        /// <summary>
        /// Getter for class field optimalMove.
        /// </summary>
        /// <returns>integer value representing the optimal move</returns>
        public int GetOptimalMove()
        {
            return _optimalMove;
        }

        /// <summary>
        /// Setter for class field optimalMove.
        /// </summary>
        /// <param name="move">integer value representing the optimal move</param>
        public void SetOptimalMove(int move)
        {
            _optimalMove = move;
        }

        /// <summary>
        /// Getter for class field optimalMoveScore.
        /// </summary>
        /// <returns>integer value representing the score for the optimal move</returns>
        public int GetOptimalMoveScore()
        {
            return _optimalMoveScore;
        }

        /// <summary>
        /// Setter for class field optimalMoveScore.
        /// </summary>
        /// <param name="score">integer value representing the score for the optimal move</param>
        public void SetOptimalMoveScore(int score)
        {
            _optimalMoveScore = score;
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR CLASS SEPARATOR
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
}
