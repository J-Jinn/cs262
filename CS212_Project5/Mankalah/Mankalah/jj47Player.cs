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
        // Class fields.

        // Store each position and their opposing position on the board (for captures).
        private static SortedDictionary<int, int> opposingPositions;

        // Give each position on the board a string label and store their associated integer value.
        private static SortedDictionary<string, int> positionLabels;

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
            // Control debugging.
            bool debug = true;

            // Determine positions opposed to each other between TOP and BOTTOM
            opposingPositions = new SortedDictionary<int, int>();

            int topCounter = 12;
            int bottomCounter = 0;

            // For bottom positions, record their opposing positions on top.
            for (int bottom = 0; bottom < 6; bottom++)
            {
                opposingPositions.Add(bottom, topCounter);
                topCounter--;

            }

            // For top positions, record their opposing positions on bottom.
            for (int top = 12; top > 6; top--)
            {
                opposingPositions.Add(top, bottomCounter);
                bottomCounter++;
            }

            // Label each position by a string and its associated integer value.
            positionLabels = new SortedDictionary<string, int>();

            // Label the bottom positions.
            for (int position = 0; position < 6; position++)
            {
                string label = "A" + position;

                positionLabels.Add(label, position);
            }

            positionLabels.Add("A Mancala", 6);

            // Label the top positions.
            for (int position = 12; position > 6; position--)
            {
                string label = "B" + position;

                positionLabels.Add(label, position);
            }

            positionLabels.Add("B Mancala", 13);

            // Debugging to check data structures are storing the correct values.
            if (debug == true)
            {
                Console.WriteLine("\n\nOpposing position for each position:");
                foreach (KeyValuePair<int, int> entry in opposingPositions)
                {
                    Console.WriteLine("Position: {0}, Opposing Position: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nLabels for each position:");
                foreach (KeyValuePair<string, int> entry in positionLabels)
                {
                    Console.WriteLine("Position Label: {0}, Position Value: {1}", entry.Key, entry.Value);
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
        /// <param name="b">Game Board object</param>
        /// 
        /// <returns>the move the AI chose to make</returns>
        public override int chooseMove(Board b)
        {

            // Return illegal move, if no legal moves are possible. (only if game is over)
            return -1;
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
        /// TODO: re-factor code into separate functions.
        /// </summary>
        /// 
        /// <param name="b">Game Board object</param>
        /// <returns>how optimal the state of the game board is</returns>
        public override int heuristicEvaluation(Board b)
        {
            bool debugSetup = true;
            bool debugPositionMostStones = true;

            // Store the optimal position to move.
            int optimalMove = 0;

            // Store and update the optimality weight value of each position during heuristic evaluation for TOP player.
            SortedDictionary<int, int> positionOptimalValueMAX = new SortedDictionary<int, int>();

            foreach (KeyValuePair<string, int> entry in positionLabels)
            {
                positionOptimalValueMAX.Add(entry.Value, int.MinValue);
            }
            // Store and update the optimality weight value of each position during heuristic evaluation for BOTTOM player.
            SortedDictionary<int, int> positionOptimalValueMIN = new SortedDictionary<int, int>();

            foreach (KeyValuePair<string, int> entry in positionLabels)
            {
                positionOptimalValueMIN.Add(entry.Value, int.MaxValue);
            }

            // Debug - check we have set default optimal values for each position to Int.Max or Int.Min
            if (debugSetup == true)
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


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// HEURISTIC CONDITION ZERO

            // TODO: Implement a way to increase optimal position weight values for all positions that are not empty of stones
            // (this will ensure that we never try to choose a move that has no stones in the hole)

            //// If AI Player is Position TOP
            //if (b.whoseMove() == Position.Top)
            //{
            //    // Try first go-again.
            //    for (int i = 12; i >= 7; i--)
            //    {
            //        if (b.stonesAt(i) == 13 - i) return i;
            //    }
            //    // Otherwise, choose first available move.
            //    for (int i = 12; i >= 7; i--)
            //    {
            //        if (b.stonesAt(i) > 0) return i;
            //    }
            //}

            //// If AI Player is Position  BOTTOM
            //if (b.whoseMove() == Position.Bottom)
            //{
            //    // Try first go-again.
            //    for (int i = 5; i >= 0; i--)
            //    {
            //        if (b.stonesAt(i) == 6 - i) return i;
            //    }
            //    // Otherwise, choose first available move.
            //    for (int i = 5; i >= 0; i--)
            //    {
            //        if (b.stonesAt(i) > 0) return i;
            //    }
            //}

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// HEURISTIC CONDITION ONE

            // Determine the position with the most stones.
            int positionWithMostStones = 0;
            int positionWithMostStonesCounter = 0;

            foreach (KeyValuePair<string, int> entry in positionLabels)
            {
                if (b.stonesAt(entry.Value) > positionWithMostStonesCounter)
                {
                    positionWithMostStonesCounter = b.stonesAt(entry.Value);
                    positionWithMostStones = entry.Value;
                }
            }

            // Increase or decrease optimality value of position with the most stones.
            positionOptimalValueMAX[positionWithMostStones] += 1000;
            positionOptimalValueMIN[positionWithMostStones] -= 1000;

            // Debug - check that we have updated the optimal value for the position with most stones.
            if (debugSetup == true)
            {
                Console.WriteLine("\n\nPosition with the most stones for TOP Player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nPosition with the most stones for BOTTOM Player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\n");
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// HEURISTIC CONDITION TWO

            // Determine if we can go again from making a move at this position.
            List<int> goAgainPositionsTOP = new List<int>();
            List<int> goAgainPositionsBOTTOM = new List<int>();

            // Determine go-again moves for TOP player.
            for (int i = 12; i > 6; i--)
            {
                if (b.stonesAt(i) == 13 - i)
                {
                    goAgainPositionsTOP.Add(i);
                    positionOptimalValueMAX[i] += 10000;
                }
            }
            // Determine go-again moves for BOTTOM player.
            for (int i = 5; i >= 0; i--)
            {
                if (b.stonesAt(i) == 6 - i)
                {
                    goAgainPositionsBOTTOM.Add(i);
                    positionOptimalValueMIN[i] -= 10000;
                }
            }

            // Debug - check that we have properly updated the optimal position weight values for all go-again positions.
            if (debugSetup == true)
            {
                Console.WriteLine("\n\nAvailable go-again positions for TOP player:\n");
                foreach (int i in goAgainPositionsTOP)
                {
                    Console.WriteLine("TOP: Go-again position: {0}", i);
                }
                Console.WriteLine("\n\nAvailable go-again positions for BOTTOM player:\n");
                foreach (int i in goAgainPositionsBOTTOM)
                {
                    Console.WriteLine("BOTTOM: Go-again position: {0}", i);
                }

                Console.WriteLine("\n\nUpdated optimal position weight values based on go-again positions for TOP player:\n\n");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nUpdated optimal position weight values based on go-again positions for BOTTOM player:\n\n");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\n");
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// HEURISTIC CONDITION THREE

            // TODO: Implement way to check to see if we can reach the empty position based on # of stones in other positions.

            // Determine if the hole is empty. If yes, add to list of empty positions and add the number of stones in 
            // its opposing position.
            SortedDictionary<int, int> emptyPositionsAndNumStonesOpposingPosition = new SortedDictionary<int, int>();

            foreach (KeyValuePair<int, int> entry in opposingPositions)
            {
                if (b.stonesAt(entry.Key) == 0)
                {
                    emptyPositionsAndNumStonesOpposingPosition.Add(entry.Key, b.stonesAt(entry.Value));
                }
            }

            // Determine the empty position with the most stones in its opposing position.
            int emptyPositionWithMostStones = 0;
            int emptyPositionWithMostStonesCounter = 0;

            foreach (KeyValuePair<int, int> entry in emptyPositionsAndNumStonesOpposingPosition)
            {
                if (b.stonesAt(entry.Value) > emptyPositionWithMostStonesCounter)
                {
                    emptyPositionWithMostStonesCounter = b.stonesAt(entry.Value);
                    emptyPositionWithMostStones = entry.Key;
                }
            }

            // Debug - check that we have properly added empty positions and stones in their opposition positions.
            if (debugSetup == true)
            {
                Console.WriteLine(("\n\nEmpty positions and number of stones in their opposing position:"));
                foreach (KeyValuePair<int, int> entry in emptyPositionsAndNumStonesOpposingPosition)
                {
                    Console.WriteLine("Empty position: {0}, Stones at opposing position: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\n");
            }

            // Increase or decrease optimality value of empty position with the most stones in opposing position.
            positionOptimalValueMAX[emptyPositionWithMostStones] += 100;
            positionOptimalValueMIN[emptyPositionWithMostStones] -= 100;

            // Debug - check that we have updated the optimal value for the empty position with the most stones in its opposing position.
            if (debugSetup == true)
            {
                Console.WriteLine("\n\nUpdated optimal position weight values based on empty position with most stones for TOP player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    Console.WriteLine("MAX - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\nUpdated optimal position weight values based on empty position with most stones for BOTTOM player:");
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    Console.WriteLine("MIN - Position: {0}, Optimal Value: {1}", entry.Key, entry.Value);
                }
                Console.WriteLine("\n\n");
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// DETERMINE OPTIMAL MOVE AND RETURN IT

            // TODO - restrict positionOptimalValueMax and positionOptimalValueMin dictionaries to positions available to each player.

            // Determine the move with the highest or lowest associated value.
            int currentMaxValue = int.MinValue;
            int currentMinValue = int.MaxValue;

            // If we are evaluating the optimal move for the TOP player.
            if (b.whoseMove() == Position.Top)
            {
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMAX)
                {
                    if (entry.Value > currentMaxValue)
                    {
                        currentMaxValue = entry.Value;
                        optimalMove = entry.Key;
                    }
                }
            }

            // If we are evaluating the optimal move for the BOTTOM player.
            if (b.whoseMove() == Position.Bottom)
            {
                foreach (KeyValuePair<int, int> entry in positionOptimalValueMIN)
                {
                    if (entry.Value < currentMinValue)
                    {
                        currentMinValue = entry.Value;
                        optimalMove = entry.Key;
                    }
                }
            }

            // Debug - check that our optimal move is what we expect it to be.
            if (debugSetup == true)
            {
                Console.WriteLine("\n\n");
                Console.WriteLine("The optimal move is: {0}", optimalMove);
                Console.WriteLine("\n\n");
            }
            return optimalMove;
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
            // Orc peons generally aren't very smart but are still probably smarter than Bonzo.
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
