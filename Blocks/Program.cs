using System;
using System.Threading;

class Tetris
{
    static int[,] grid = new int[20, 10];
    static int[,] piece;
    static int pieceRow = 0;
    static int pieceCol = 3;
    static int score = 0;
    static int highScore = 0;

    static Random random = new Random();

    static void Main()
    {
        Console.CursorVisible = false;

        while (true)
        {
            piece = GetRandomPiece();
            pieceRow = 0;
            pieceCol = 3;

            if (!CanPlacePiece(piece, pieceRow, pieceCol))
            {
                break; // Game over
            }

            while (true)
            {
                DrawGrid();
                DrawPiece(piece, pieceRow, pieceCol);
                Console.SetCursorPosition(0, 22);
                Console.WriteLine($"Score: {score}  High Score: {highScore}");

                Thread.Sleep(500);

                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;

                    if (key == ConsoleKey.LeftArrow && CanPlacePiece(piece, pieceRow, pieceCol - 1))
                        pieceCol--;
                    else if (key == ConsoleKey.RightArrow && CanPlacePiece(piece, pieceRow, pieceCol + 1))
                        pieceCol++;
                    else if (key == ConsoleKey.DownArrow && CanPlacePiece(piece, pieceRow + 1, pieceCol))
                        pieceRow++;
                    else if (key == ConsoleKey.UpArrow)
                        RotatePiece(ref piece);
                }

                if (CanPlacePiece(piece, pieceRow + 1, pieceCol))
                {
                    pieceRow++;
                }
                else
                {
                    PlacePiece(piece, pieceRow, pieceCol);
                    ClearFullRows();
                    break;
                }
            }
        }

        Console.Clear();
        Console.WriteLine("Game Over!");
        Console.WriteLine($"Your Score: {score}");
        if (score > highScore)
        {
            highScore = score;
            Console.WriteLine("New High Score!");
        }
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static int[,] GetRandomPiece()
    {
        switch (random.Next(7))
        {
            case 0: return new int[,] { { 1, 1, 1, 1 } }; // I
            case 1: return new int[,] { { 1, 1 }, { 1, 1 } }; // O
            case 2: return new int[,] { { 0, 1, 1 }, { 1, 1, 0 } }; // S
            case 3: return new int[,] { { 1, 1, 0 }, { 0, 1, 1 } }; // Z
            case 4: return new int[,] { { 1, 1, 1 }, { 0, 1, 0 } }; // T
            case 5: return new int[,] { { 1, 1, 1 }, { 1, 0, 0 } }; // L
            case 6: return new int[,] { { 1, 1, 1 }, { 0, 0, 1 } }; // J
            default: return new int[,] { { 1, 1, 1, 1 } }; // Fallback I piece
        }
    }

    static void RotatePiece(ref int[,] piece)
    {
        int size = piece.GetLength(0);
        int[,] rotated = new int[size, size];

        for (int r = 0; r < size; r++)
        {
            for (int c = 0; c < size; c++)
            {
                rotated[c, size - r - 1] = piece[r, c];
            }
        }

        if (CanPlacePiece(rotated, pieceRow, pieceCol))
        {
            piece = rotated;
        }
    }

    static bool CanPlacePiece(int[,] piece, int row, int col)
    {
        for (int r = 0; r < piece.GetLength(0); r++)
        {
            for (int c = 0; c < piece.GetLength(1); c++)
            {
                if (piece[r, c] == 1)
                {
                    int newRow = row + r;
                    int newCol = col + c;

                    if (newRow < 0 || newRow >= grid.GetLength(0) ||
                        newCol < 0 || newCol >= grid.GetLength(1) ||
                        grid[newRow, newCol] == 1)
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    static void PlacePiece(int[,] piece, int row, int col)
    {
        for (int r = 0; r < piece.GetLength(0); r++)
        {
            for (int c = 0; c < piece.GetLength(1); c++)
            {
                if (piece[r, c] == 1)
                {
                    grid[row + r, col + c] = 1;
                }
            }
        }
    }

    static void ClearFullRows()
    {
        for (int r = grid.GetLength(0) - 1; r >= 0; r--)
        {
            bool fullRow = true;
            for (int c = 0; c < grid.GetLength(1); c++)
            {
                if (grid[r, c] == 0)
                {
                    fullRow = false;
                    break;
                }
            }

            if (fullRow)
            {
                for (int rr = r; rr > 0; rr--)
                {
                    for (int cc = 0; cc < grid.GetLength(1); cc++)
                    {
                        grid[rr, cc] = grid[rr - 1, cc];
                    }
                }

                for (int cc = 0; cc < grid.GetLength(1); cc++)
                {
                    grid[0, cc] = 0;
                }

                score += 100;
                r++;
            }
        }
    }

    static void DrawGrid()
    {
        Console.Clear();
        for (int r = 0; r < grid.GetLength(0); r++)
        {
            for (int c = 0; c < grid.GetLength(1); c++)
            {
                Console.SetCursorPosition(c * 2, r);
                Console.Write(grid[r, c] == 1 ? "██" : "  ");
            }
        }
    }

    static void DrawPiece(int[,] piece, int row, int col)
    {
        for (int r = 0; r < piece.GetLength(0); r++)
        {
            for (int c = 0; c < piece.GetLength(1); c++)
            {
                if (piece[r, c] == 1)
                {
                    Console.SetCursorPosition((col + c) * 2, row + r);
                    Console.Write("██");
                }
            }
        }
    }
}
