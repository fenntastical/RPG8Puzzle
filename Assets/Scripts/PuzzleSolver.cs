using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleSolver : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}

class PuzzleState
{
    public string Board;  // Board stored as a string (e.g., "123456780")
    public int X, Y;  // Position of '0'
    public int Depth; // BFS depth level

    public PuzzleState(string board, int x, int y, int depth)
    {
        Board = board;
        X = x;
        Y = y;
        Depth = depth;
    }
}

class EightPuzzleBFS
{
    const int N = 3;

    // Possible moves: Left, Right, Up, Down
    static int[] row = { 0, 0, -1, 1 };
    static int[] col = { -1, 1, 0, 0 };

    static readonly string GOAL_STATE = "123456780"; // Goal state stored as string

    // Function to convert 2D board to a string representation
    static string BoardToString(int[,] board)
    {
        char[] sb = new char[9];
        int index = 0;
        for (int i = 0; i < N; i++)
            for (int j = 0; j < N; j++)
                sb[index++] = (char)(board[i, j] + '0');
        return new string(sb);
    }

    // Function to print board from string representation
    // static void PrintBoard(string board)
    // {
    //     for (int i = 0; i < 9; i++)
    //     {
    //         Console.Write(board[i] + " ");
    //         if ((i + 1) % 3 == 0)
    //             Console.WriteLine();
    //     }
    //     Console.WriteLine("--------");
    // }

    // BFS function to solve the 8-puzzle problem
    static void SolvePuzzleBFS(int[,] start, int x, int y)
    {
        Queue<PuzzleState> queue = new Queue<PuzzleState>();
        HashSet<string> visited = new HashSet<string>();

        string startBoard = BoardToString(start);
        queue.Enqueue(new PuzzleState(startBoard, x, y, 0));
        visited.Add(startBoard);

        while (queue.Count > 0)
        {
            PuzzleState curr = queue.Dequeue();

            // Print the current board state
            // Console.WriteLine("Depth: " + curr.Depth);
            // PrintBoard(curr.Board);

            // Check if goal state is reached
            if (curr.Board == GOAL_STATE)
            {
                // Console.WriteLine("Goal state reached at depth " + curr.Depth);
                return;
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

                    // If this state has not been visited before, add to queue
                    if (!visited.Contains(newBoard))
                    {
                        visited.Add(newBoard);
                        queue.Enqueue(new PuzzleState(newBoard, newX, newY, curr.Depth + 1));
                    }

                    // Swap back to restore original board for next iteration
                    temp = boardArray[zeroPos];
                    boardArray[zeroPos] = boardArray[swapPos];
                    boardArray[swapPos] = temp;
                }
            }
        }

        // Console.WriteLine("No solution found (BFS Brute Force reached depth limit)");
    }

    void PuzzleRandomizer()
    {
        
    }

    // Driver Code
    static void Main()
    {
        int[,] start = {
            { 1, 2, 3 },
            { 4, 0, 5 },
            { 6, 7, 8 }
        }; // Initial state
        int x = 1, y = 1; // Position of the empty tile (0)

        // Console.WriteLine("Initial State:");
        // PrintBoard(BoardToString(start));

        SolvePuzzleBFS(start, x, y);
    }
}