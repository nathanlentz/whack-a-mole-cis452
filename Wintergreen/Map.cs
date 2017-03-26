using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wintergreen
{
    public enum CellType : byte
    {
        Passable = 0,
        Impassable = 1
    }

    public class Map
    {
        protected int _rowCount;
        /// <summary>
        /// The number of rows in this <see cref="Map"/>'s Grid.
        /// </summary>
        public int RowCount
        {
            get { return _rowCount; }
        }

        protected int _columnCount;
        /// <summary>
        /// The number of columns in this <see cref="Map"/>'s Grid.
        /// </summary>
        public int ColumnCount
        {
            get { return _columnCount; }
        }

        protected byte[,] _grid;
        /// <summary>
        /// A 2D <see cref="Array"/> of <see cref="byte"/>s representing cells in a space.
        /// The first dimension indexes columns (x) and the second dimension indexes rows (y).
        /// <c>0</c> represents passable space. <c>1</c> represents impassable space. 
        /// Other values may be used as seen fit.
        /// </summary>
        public byte[,] Grid
        {
            get { return _grid; }
        }

        /// <summary>
        /// Creates a <see cref="Map"/> instance with the specified number of columns and rows.
        /// </summary>
        /// <param name="width">The number of columns in the <see cref="Map"/>'s grid.</param>
        /// <param name="height">The number of rows in the <see cref="Map"/>'s grid.</param>
        public Map(int width, int height)
        {
            _grid = new byte[width, height];
            _columnCount = width;
            _rowCount = height;
        }

        /// <summary>
        /// Sets all of the border cells within this <see cref="Map"/> to the given <see cref="CellType"/>.
        /// </summary>
        /// <param name="borderWidth"></param>
        /// <param name="type"></param>
        public void SetBorders(int borderWidth, CellType type)
        {
            SetBorders(borderWidth, (byte)type);
        }

        /// <summary>
        /// Sets all of the border cells within this <see cref="Map"/> to the given value.
        /// </summary>
        /// <param name="borderWidth"></param>
        /// <param name="value"></param>
        public void SetBorders(int borderWidth, byte value)
        {
            if (borderWidth == 0)
                return;

            if (borderWidth > ColumnCount || borderWidth > RowCount)
                throw new Exception("Border width can not be wider than the map.");

            // Set first row.
            for (int j = 0; j < borderWidth; j++)
            {
                for (int i = 0; i < _columnCount; i++)
                    _grid[i, j] = value;
            }

            // Set the last row.
            int lastRow = _rowCount - 1;
            for (int j = lastRow; j > lastRow - borderWidth; j--)
            {
                for (int i = 0; i < _columnCount; i++)
                    _grid[i, lastRow] = value;
            }

            // No need to continue if this grid has no middle rows.
            if (_rowCount < 3)
                return;

            // Set the first and last columns of the middle rows
            int lastCol = _columnCount - 1;
            for (int j = borderWidth; j < _rowCount - borderWidth; j++)
            {
                for (int i = 0; i < borderWidth; i++)
                {
                    _grid[i, j] = value;
                }
                for (int i = lastCol; i > lastCol - borderWidth; i--)
                {
                    _grid[i, j] = value;
                }
            }
        }
    }
}
