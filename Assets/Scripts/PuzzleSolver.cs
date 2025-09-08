using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class PuzzleSolver : MonoBehaviour
{
    const int N = 3;

    // Possible moves: Left, Right, Up, Down
    static int[] row = { 0, 0, -1, 1 };
    static int[] col = { -1, 1, 0, 0 };

    static readonly string GOAL_STATE = "123456780"; // Goal state stored as string

    public class PuzzleState
    {
        public string Board;  // Board stored as a string (e.g., "123456780")
        public int X, Y;      // Position of '0'
        public int Depth;     // BFS depth level
        public PuzzleState Parent;

        public PuzzleState(string board, int x, int y, int depth, PuzzleState parent = null)
        {
            Board = board;
            X = x;
            Y = y;
            Depth = depth;
            Parent = parent;
        }
    }

    // Called when the scene starts
    void Start()
    {
        
    }

    // Convert 2D board to string representation
    static string BoardToString(int[,] board)
    {
        char[] sb = new char[9];
        int index = 0;
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                sb[index++] = (char)(board[i, j] + '0');
        return new string(sb);
    }

    // Print board from string representation
    static void PrintBoard(string board)
    {
        // string rowStr = "";
        // for (int i = 0; i < 9; i++)
        // {
        //     rowStr += board[i] + " ";
        //     if ((i + 1) % 3 == 0)
        //     {
        //         Debug.Log(rowStr);
        //         rowStr = "";
        //     }
        // }

        Debug.Log(board);
        // Debug.Log("--------");
    }

    // Reconstruct path from goal back to start
    static List<string> ReconstructPath(PuzzleState goalState)
    {
        List<string> path = new List<string>();
        PuzzleState curr = goalState;

        while (curr != null)
        {
            path.Add(curr.Board);
            curr = curr.Parent;
        }

        path.Reverse(); // Start → Goal
        return path;
    }

    // BFS function to solve the puzzle
    public List<string> SolvePuzzleBFS(int[,] start, int x, int y)
    {
        Queue<PuzzleState> queue = new Queue<PuzzleState>();
        HashSet<string> visited = new HashSet<string>();

        string startBoard = BoardToString(start);
        PuzzleState startState = new PuzzleState(startBoard, x, y, 0);
        queue.Enqueue(startState);
        visited.Add(startBoard);

        while (queue.Count > 0)
        {
            PuzzleState curr = queue.Dequeue();

            // Check if goal state is reached
            if (curr.Board == GOAL_STATE)
            {
                return ReconstructPath(curr);
            }

            char[] boardArray = curr.Board.ToCharArray();
            for (int i = 0; i < 4; i++)
            {
                int newX = curr.X + row[i];
                int newY = curr.Y + col[i];

                if (newX >= 0 && newX < N && newY >= 0 && newY < N)
                {
                    int zeroPos = curr.X * N + curr.Y;
                    int swapPos = newX * N + newY;

                    // Swap 0 with the new position
                    char temp = boardArray[zeroPos];
                    boardArray[zeroPos] = boardArray[swapPos];
                    boardArray[swapPos] = temp;

                    string newBoard = new string(boardArray);

                    if (!visited.Contains(newBoard))
                    {
                        visited.Add(newBoard);
                        queue.Enqueue(new PuzzleState(newBoard, newX, newY, curr.Depth + 1, curr));
                    }

                    // Swap back
                    boardArray[swapPos] = boardArray[zeroPos];
                    boardArray[zeroPos] = temp;
                }
            }
        }

        return null; // No solution found
    }

    void PuzzleRandomizer()
    {
        
    }
}