// Windows ASP.NET webservice - SuDoku
// Copyright (C) 2008
// 1e67427a-51a3-404b-b13c-a36e43b9890a
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
// These Terms shall be governed and construed in accordance with the laws of 
// England and Wales, without regard to its conflict of law provisions.
//


using System;
using System.Data;
using System.Collections;

namespace MySuDokuSolver
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class EmptyRectangle
    {
        public EmptyRectangle()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Method
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dsTableSteps"></param>
        public void Method(int[, ,] grid, DataTable dsTableSteps)
        {
            int toprow =0;
            int rightcolumn =0;
            int leftcolumn = 0;
            int bottomrow = 0;

            //Want to search for Empty Rectangles

            //Loop over the Numbers
            for (int num = 1; num <= 9; num++)
            {
                //Loop over the squares
                for (int square = 1; square <= 9; square++)
                {
                    if (OnlyTwoLinesInSquare(num, square, grid, ref toprow, ref rightcolumn))
                    {
                        //Found a corner
                        if (OnlyTwoInColumn(num, grid, toprow, rightcolumn, ref leftcolumn, ref bottomrow))
                        {
                            //If all this is true then. Then 

                            if (grid[bottomrow, rightcolumn, num] > 0)
                            {

                                grid[bottomrow, rightcolumn, num] = 0;
                                string Message = "Eliminate this number. Empty Rectangle.";
                                Message = Message + "top row=" + toprow + ". Right col=" + rightcolumn + ". Left col=" + leftcolumn + ". bottomrow=" + bottomrow;
                                UpdateDataTableRow(1, bottomrow, rightcolumn, num, Message, dsTableSteps);
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// OnlyTwoLinesInSquare
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="Square"></param>
        /// <param name="grid"></param>
        /// <param name="foundrow"></param>
        /// <param name="foundcol"></param>
        /// <returns></returns>
        public bool OnlyTwoLinesInSquare(int Number, int Square, int[, ,] grid, ref int foundrow, ref int foundcol)
        {
            bool rtn = false;
            int found;
            int NumStillVisible;
            int[,] squaregrid = new int[4, 4];

            //Update the square grid from the large grid

            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (Square == 1) { squaregrid[i, j] = grid[i, j, Number]; }
                    if (Square == 2) { squaregrid[i, j] = grid[i, j + 3, Number]; }
                    if (Square == 3) { squaregrid[i, j] = grid[i, j + 6, Number]; }
                    if (Square == 4) { squaregrid[i, j] = grid[i + 3, j, Number]; }
                    if (Square == 5) { squaregrid[i, j] = grid[i + 3, j + 3, Number]; }
                    if (Square == 6) { squaregrid[i, j] = grid[i + 3, j + 6, Number]; }
                    if (Square == 7) { squaregrid[i, j] = grid[i + 6, j, Number]; }
                    if (Square == 8) { squaregrid[i, j] = grid[i + 6, j + 3, Number]; }
                    if (Square == 9) { squaregrid[i, j] = grid[i + 6, j + 6, Number]; }
                }
            }

            //Now have the information in a 3x3 square
            //Does the Number only appear in one row and one column
            //
            //    x x x
            //    x 4 4 
            //    x x 4

            found = 0;

            for (int BlankRow = 1; BlankRow <= 3; BlankRow++)
            {
                for (int BlankCol = 1; BlankCol <= 3; BlankCol++)
                {
                    //Number must be on pivotal cell in square
                    if (squaregrid[BlankRow, BlankCol] == Number)
                    {
                        NumStillVisible = 0;
                        //So if we do not count the above blankrow and blankcol entries
                        //Loop over all cells in square
                        for (int i = 1; i <= 3; i++)
                        {
                            for (int j = 1; j <= 3; j++)
                            {
                                if (i!=BlankRow && j!=BlankCol && squaregrid[i,j]==Number)
                                {
                                    NumStillVisible = NumStillVisible + 1;
                                }
                            }
                        }//i

                        if (NumStillVisible == 0 && found==0)
                        {
                            foundrow = BlankRow;
                            foundcol = BlankCol;
                            rtn = true;
                        }
                    }
                }
            }//BlankRow

            if (Square == 1) { foundrow = foundrow + 0; foundcol = foundcol + 0; }
            if (Square == 2) { foundrow = foundrow + 0; foundcol = foundcol + 3; }
            if (Square == 3) { foundrow = foundrow + 0; foundcol = foundcol + 6; }
            if (Square == 4) { foundrow = foundrow + 3; foundcol = foundcol + 0; }
            if (Square == 5) { foundrow = foundrow + 3; foundcol = foundcol + 3; }
            if (Square == 6) { foundrow = foundrow + 3; foundcol = foundcol + 6; }
            if (Square == 7) { foundrow = foundrow + 6; foundcol = foundcol + 0; }
            if (Square == 8) { foundrow = foundrow + 6; foundcol = foundcol + 3; }
            if (Square == 9) { foundrow = foundrow + 6; foundcol = foundcol + 6; }

            return rtn;
        }


        /// <summary>
        /// OnlyTwoInColumn
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="grid"></param>
        /// <param name="ThisRow"></param>
        /// <param name="NotThisCol"></param>
        /// <param name="foundcol"></param>
        /// <param name="bottomRow"></param>
        /// <returns></returns>
        public bool OnlyTwoInColumn(int Number, int[, ,] grid, int ThisRow, int NotThisCol, ref int foundcol, ref int bottomRow)
        {
            int tmp = 0;
            bool workcol = false;
            bool rtn = false;

            //Loop over the columns
            for (int col = 1; col <= 9; col++)
            {
                workcol = false;
                if (NotThisCol >= 1 && NotThisCol <= 3 && col >= 4) { workcol = true; }
                if (NotThisCol >= 4 && NotThisCol <= 6 && (col <= 3 || col>=7)) { workcol = true; }
                if (NotThisCol >= 7 && NotThisCol <= 9 && col <= 6) { workcol = true; }

                //Don't want to look at this column. Used elsewhere in logic 
                if (workcol)
                {
                    //Only interested in 'Number' entry is in ThisRow for this column
                    if (grid[ThisRow, col, Number] == Number)
                    {
                        int Count = 0;
                        for (int row = 1; row <= 9; row++)
                        {
                            if (grid[row, col, Number] > 0)
                            {
                                Count = Count + 1;
                                if (row != ThisRow) { tmp = row; }
                            }
                        }

                        if (Count == 2)
                        {
                            foundcol = col;
                            bottomRow = tmp;
                            rtn = true;

                            //Check the rows are in different squares
                            if (ThisRow >= 1 && ThisRow <= 3 && bottomRow >= 1 && bottomRow <= 3) { rtn = false; }
                            if (ThisRow >= 4 && ThisRow <= 6 && bottomRow >= 4 && bottomRow <= 6) { rtn = false; }
                            if (ThisRow >= 7 && ThisRow <= 9 && bottomRow >= 7 && bottomRow <= 9) { rtn = false; }

                        }
                    }
                }
            }

            return rtn;
        }


        /// <summary>
        /// UpdateDataTableRow
        /// </summary>
        /// <param name="Step"></param>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <param name="Number"></param>
        /// <param name="Description"></param>
        /// <param name="dsTableSteps"></param>
        private void UpdateDataTableRow(int Step, int Row, int Col, int Number, string Description, DataTable dsTableSteps)
        {
            DataRow myRow = dsTableSteps.NewRow();
            myRow["Step"] = Step;
            myRow["Row"] = Row;
            myRow["Col"] = Col;
            myRow["Number"] = Number;
            myRow["Note"] = Description;
            dsTableSteps.Rows.Add(myRow);
        }

    }
}
