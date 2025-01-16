using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris.Models
{
    public class GameGrid(int rows, int columns)
    {
        private readonly int[,] grid = new int[rows, columns];
        public int Rows { get; } = rows;
        public int Columns { get; } = columns;
        public int this[int r, int c]
        {
            get => grid[r, c];
            set => grid[r, c] = value;
        }

        public bool IsInside(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        public bool IsEmpty(int r, int c)
        {
            return IsInside(r, c) && grid[r, c] == 0;
        }

        public bool IsRowFull(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] == 0)
                {
                    return false;
                }

            }
            return true;
        }

        public bool IsRowEmpty(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (grid[r, c] != 0)
                {
                    return false;
                }

            }
            return true;
        }

        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r, c] = 0;
            }
        }

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                grid[r + numRows, c] = grid[r, c];
                grid[r, c] = 0;
            }
        }
        public int ClearFullRows()
        {
            int cleread = 0;

            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsRowFull(r))
                {
                    ClearRow(r);
                    cleread++;
                }
                else if (cleread > 0)
                {
                    MoveRowDown(r, cleread);
                }
            }
            return cleread;
        }
    }
}
