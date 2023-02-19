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
    public class Colors
    {
        public Colors()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public struct PairInfo
        {
            public int row1;
            public int column1;
            public int square1;
            public int colour1;
            public int row2;
            public int column2;
            public int square2;
            public int colour2;
        }

        /// <summary>
        /// Method
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dsTableSteps"></param>
        public void Method(int[, ,] grid, DataTable dsTableSteps)
        {
            ArrayList work = new ArrayList();

            int row1 = 0;
            int row2 = 0;
            int col1 = 0;
            int col2 = 0;
            int square1 = 0;
            int square2 = 0;
            string[,] color = new string[10, 10];

            //Loop over the Numbers
            for (int num = 1; num <= 9; num++)
            {
                //Reset the color map

                work.Clear();
                for (int i = 1; i <= 9; i++)
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        color[i, j] = "..";
                    }
                }

                //Loop over rows
                for (int row = 1; row <= 9; row++)
                {
                    if (PairInRow(row, num, grid, ref col1, ref col2, ref square1, ref square2))
                    {
                        PairInfo NewEntry = new PairInfo();
                        NewEntry.row1 = row;
                        NewEntry.column1 = col1;
                        NewEntry.square1 = square1;
                        NewEntry.row2 = row;
                        NewEntry.column2 = col2;
                        NewEntry.square2 = square2;
                        work.Add(NewEntry);
                    }
                }

                //Loop over columns
                for (int col = 1; col <= 9; col++)
                {
                    if (PairInColumn(col, num, grid, ref row1, ref row2, ref square1, ref square2))
                    {
                        PairInfo NewEntry = new PairInfo();
                        NewEntry.row1 = row1;
                        NewEntry.column1 = col;
                        NewEntry.square1 = square1;
                        NewEntry.row2 = row2;
                        NewEntry.column2 = col;
                        NewEntry.square2 = square2;
                        work.Add(NewEntry);
                    }
                }

                //Loop over squares
                for (int square = 1; square <= 9; square++)
                {
                    if (PairInSquare(square, num, grid, ref row1, ref col1, ref row2, ref col2))
                    {
                        PairInfo NewEntry = new PairInfo();
                        NewEntry.row1 = row1;
                        NewEntry.column1 = col1;
                        NewEntry.square1 = square;
                        NewEntry.row2 = row2;
                        NewEntry.column2 = col2;
                        NewEntry.square2 = square;
                        work.Add(NewEntry);
                    }
                }

                //Have the pairs in a 'work' array

                //Loop over 'work' list until all the cells are coloured in

                int LookForNewPair = 1;
                int Letter = 64;
                for (int pair = 0; pair < work.Count; pair++)
                {
                    PairInfo Entry = (PairInfo)work[pair];

                    //If pair are not coloured in then set = Pink("1") & Purple("2")
                    if (color[Entry.row1, Entry.column1] == ".." &&
                        color[Entry.row2, Entry.column2] == ".." && LookForNewPair == 1)
                    {
                        Letter++;
                        color[Entry.row1, Entry.column1] = (char)Letter + "1";
                        color[Entry.row2, Entry.column2] = (char)Letter + "2";
                        LookForNewPair = 0;
                    }

                    int ColouredIn = 0;
                    int Safe = 0;

                    //Loop if still finding pairs to fill-in
                    do
                    {
                        ColouredIn = 0;
                        Safe = Safe + 1;

                        for (int i = 0; i < work.Count; i++)
                        {
                            PairInfo FillPair = (PairInfo)work[i];

                            //If part of pair is filled in the complete

                            if (color[FillPair.row1, FillPair.column1] == (char)Letter + "1" && color[FillPair.row2, FillPair.column2] == "..") { color[FillPair.row2, FillPair.column2] = (char)Letter + "2"; ColouredIn = 1; }
                            if (color[FillPair.row1, FillPair.column1] == (char)Letter + "2" && color[FillPair.row2, FillPair.column2] == "..") { color[FillPair.row2, FillPair.column2] = (char)Letter + "1"; ColouredIn = 1; }

                            if (color[FillPair.row1, FillPair.column1] == ".." && color[FillPair.row2, FillPair.column2] == (char)Letter + "1") { color[FillPair.row1, FillPair.column1] = (char)Letter + "2"; ColouredIn = 1; }
                            if (color[FillPair.row1, FillPair.column1] == ".." && color[FillPair.row2, FillPair.column2] == (char)Letter + "2") { color[FillPair.row1, FillPair.column1] = (char)Letter + "1"; ColouredIn = 1; }

                        }

                    } while (ColouredIn == 1 || Safe > 20);

                    LookForNewPair = 1;

                }//pair

                //Have now coloured in the grid (for a specific number)
                //can we elimiate some cells?

                //(Type "1")Loop over cells

                for (int row = 1; row <= 9; row++)
                {
                    for (int col = 1; col <= 9; col++)
                    {
                        //Not in a colour-chain
                        if (color[row, col] == "..")
                        {
                            //Row & Column

                            if (WhatColorRow(row, color).Substring(0, 1) == WhatColorCol(col, color).Substring(0, 1) &&
                                WhatColorRow(row, color).Substring(1, 1) != WhatColorCol(col, color).Substring(1, 1) &&
                                    WhatColorRow(row, color) != ".." && WhatColorCol(col, color) != "..")
                            {
                                //Can eliminate
                                if (grid[row, col, num] > 0)
                                {
                                    grid[row, col, num] = 0;
                                    UpdateDataTableRow(1, row, col, num, "Eliminate this number: Color technique", dsTableSteps);
                                }
                            }

                            //Row & Square




                            //Col & Square



                        }
                    }//col
                }//row

                //(Type "2") Loop over groups. Does a group contain 2 of the same colour?
                //(rows)

                for (int l = 65; l <= Letter; l++)
                {
                    for (int row = 1; row <= 9; row++)
                    {
                        int count1 = 0;
                        int count2 = 0;

                        for (int col = 1; col <= 9; col++)
                        {
                            if (color[row, col] == (char)l + "1") { count1++; }
                            if (color[row, col] == (char)l + "2") { count2++; }
                        }

                        if (count1 > 1)
                        {
                            for (int col = 1; col <= 9; col++)
                            {
                                //Can eliminate the two cells
                                if (grid[row, col, num] > 0 && color[row, col] == (char)l + "1")
                                {
                                    grid[row, col, num] = 0;
                                    UpdateDataTableRow(1, row, col, num, "Eliminate this number: Color technique", dsTableSteps);
                                }
                            }
                        }

                        if (count2 > 1)
                        {
                            for (int col = 1; col <= 9; col++)
                            {
                                //Can eliminate the two cells
                                if (grid[row, col, num] > 0 && color[row, col] == (char)l + "2")
                                {
                                    grid[row, col, num] = 0;
                                    UpdateDataTableRow(1, row, col, num, "Eliminate this number: Color technique", dsTableSteps);
                                }
                            }
                        }
                    }//row

                    //(cols)

                    for (int col = 1; col <= 9; col++)
                    {
                        int count1 = 0;
                        int count2 = 0;

                        for (int row = 1; row <= 9; row++)
                        {
                            if (color[row, col] == (char)l + "1") { count1++; }
                            if (color[row, col] == (char)l + "2") { count2++; }
                        }

                        if (count1 > 1)
                        {
                            for (int row = 1; row <= 9; row++)
                            {
                                //Can eliminate the two cells
                                if (grid[row, col, num] > 0 && color[row, col] == (char)l + "1")
                                {
                                    grid[row, col, num] = 0;
                                    UpdateDataTableRow(1, row, col, num, "Eliminate this number: Color technique", dsTableSteps);
                                }
                            }
                        }

                        if (count2 > 1)
                        {
                            for (int row = 1; row <= 9; row++)
                            {
                                //Can eliminate the two cells
                                if (grid[row, col, num] > 0 && color[row, col] == (char)l + "2")
                                {
                                    grid[row, col, num] = 0;
                                    UpdateDataTableRow(1, row, col, num, "Eliminate this number: Color technique", dsTableSteps);
                                }
                            }
                        }
                    }//col

                    //(squares)

                    for (int ThisSquare = 1; ThisSquare <= 9; ThisSquare++)
                    {
                        if (ThisSquare == 1)
                        {
                            int count1 = 0;
                            int count2 = 0;

                            for (int row = 1; row <= 3; row++)
                            {
                                for (int col = 1; col <= 3; col++)
                                {
                                    if (color[row, col] == (char)l + "1") { count1++; }
                                    if (color[row, col] == (char)l + "2") { count2++; }
                                }//col
                            }//row

                            if (count1 > 1)
                            {
                                for (int row = 1; row <= 3; row++)
                                {
                                    for (int col = 1; col <= 3; col++)
                                    {
                                        //Can eliminate the two cells
                                        if (grid[row, col, num] > 0 && color[row, col] == (char)l + "1")
                                        {
                                            grid[row, col, num] = 0;
                                            UpdateDataTableRow(1, row, col, num, "Eliminate this number: Color technique", dsTableSteps);
                                        }
                                    }
                                }
                            }

                            if (count2 > 1)
                            {
                                for (int row = 1; row <= 3; row++)
                                {
                                    for (int col = 1; col <= 3; col++)
                                    {
                                        //Can eliminate the two cells
                                        if (grid[row, col, num] > 0 && color[row, col] == (char)l + "2")
                                        {
                                            grid[row, col, num] = 0;
                                            UpdateDataTableRow(1, row, col, num, "Eliminate this number: Color technique", dsTableSteps);
                                        }
                                    }
                                }
                            }
                        }//ThisSquare
                    }

                }//l       

            }//num

        }



        /// <summary>
        /// WhatColorRow
        /// </summary>
        /// <param name="ThisRow"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public string WhatColorRow(int ThisRow, string[,] color)
        {
            string rtn = "..";
            for (int col = 1; col <= 9; col++)
            {
                if (color[ThisRow, col] != "..") { rtn = color[ThisRow, col]; }
            }

            return rtn;
        }

        /// <summary>
        /// WhatColorCol
        /// </summary>
        /// <param name="ThisColumn"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public string WhatColorCol(int ThisColumn, string[,] color)
        {
            string rtn = "..";
            for (int row = 1; row <= 9; row++)
            {
                if (color[row, ThisColumn] != "..") { rtn = color[row, ThisColumn]; }
            }
            return rtn;
        }

        /// <summary>
        /// WhatColorSquare
        /// </summary>
        /// <param name="ThisSquare"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public string WhatColorSquare(int ThisSquare, string[,] color)
        {
            string rtn = "..";

            if (ThisSquare == 1)
            {
                for (int row = 1; row <= 3; row++)
                {
                    for (int col = 1; col <= 3; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 2)
            {
                for (int row = 1; row <= 3; row++)
                {
                    for (int col = 4; col <= 6; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 3)
            {
                for (int row = 1; row <= 3; row++)
                {
                    for (int col = 7; col <= 9; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 4)
            {
                for (int row = 4; row <= 6; row++)
                {
                    for (int col = 1; col <= 3; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 5)
            {
                for (int row = 4; row <= 6; row++)
                {
                    for (int col = 4; col <= 6; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 6)
            {
                for (int row = 4; row <= 6; row++)
                {
                    for (int col = 7; col <= 9; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 5)
            {
                for (int row = 7; row <= 9; row++)
                {
                    for (int col = 1; col <= 3; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 8)
            {
                for (int row = 7; row <= 9; row++)
                {
                    for (int col = 4; col <= 6; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }

            if (ThisSquare == 9)
            {
                for (int row = 7; row <= 9; row++)
                {
                    for (int col = 7; col <= 9; col++)
                    {
                        if (color[row, col] != "..") { rtn = color[row, col]; }
                    }
                }
            }
            return rtn;
        }

        /// <summary>
        /// PairInRow
        /// </summary>
        /// <param name="ThisRow"></param>
        /// <param name="Number"></param>
        /// <param name="grid"></param>
        /// <param name="column1"></param>
        /// <param name="column2"></param>
        /// <param name="square1"></param>
        /// <param name="square2"></param>
        /// <returns></returns>
        public bool PairInRow(int ThisRow, int Number, int[, ,] grid, ref int column1, ref int column2, ref int square1, ref int square2)
        {
            bool rtn = false;
            int tmp = 0;
            int count = 0;

            for (int col = 1; col <= 9; col++)
            {
                if (grid[ThisRow, col, Number] > 0)
                {
                    if (tmp == 1) { column2 = col; tmp = 2; }
                    if (tmp == 0) { column1 = col; tmp = 1; }
                    count = count + 1;
                }
            }

            if (count == 2) rtn = true;

            if (ThisRow >= 1 && ThisRow <= 3 && column1 >= 1 && column1 <= 3) { square1 = 1; }
            if (ThisRow >= 1 && ThisRow <= 3 && column1 >= 4 && column1 <= 6) { square1 = 2; }
            if (ThisRow >= 1 && ThisRow <= 3 && column1 >= 7 && column1 <= 9) { square1 = 3; }
            if (ThisRow >= 4 && ThisRow <= 6 && column1 >= 1 && column1 <= 3) { square1 = 4; }
            if (ThisRow >= 4 && ThisRow <= 6 && column1 >= 4 && column1 <= 6) { square1 = 5; }
            if (ThisRow >= 4 && ThisRow <= 6 && column1 >= 7 && column1 <= 9) { square1 = 6; }
            if (ThisRow >= 7 && ThisRow <= 9 && column1 >= 1 && column1 <= 3) { square1 = 7; }
            if (ThisRow >= 7 && ThisRow <= 9 && column1 >= 4 && column1 <= 6) { square1 = 8; }
            if (ThisRow >= 7 && ThisRow <= 9 && column1 >= 7 && column1 <= 9) { square1 = 9; }

            if (ThisRow >= 1 && ThisRow <= 3 && column2 >= 1 && column2 <= 3) { square2 = 1; }
            if (ThisRow >= 1 && ThisRow <= 3 && column2 >= 4 && column2 <= 6) { square2 = 2; }
            if (ThisRow >= 1 && ThisRow <= 3 && column2 >= 7 && column2 <= 9) { square2 = 3; }
            if (ThisRow >= 4 && ThisRow <= 6 && column2 >= 1 && column2 <= 3) { square2 = 4; }
            if (ThisRow >= 4 && ThisRow <= 6 && column2 >= 4 && column2 <= 6) { square2 = 5; }
            if (ThisRow >= 4 && ThisRow <= 6 && column2 >= 7 && column2 <= 9) { square2 = 6; }
            if (ThisRow >= 7 && ThisRow <= 9 && column2 >= 1 && column2 <= 3) { square2 = 7; }
            if (ThisRow >= 7 && ThisRow <= 9 && column2 >= 4 && column2 <= 6) { square2 = 8; }
            if (ThisRow >= 7 && ThisRow <= 9 && column2 >= 7 && column2 <= 9) { square2 = 9; }

            return rtn;
        }


        /// <summary>
        /// PairInColumn
        /// </summary>
        /// <param name="ThisColumn"></param>
        /// <param name="Number"></param>
        /// <param name="grid"></param>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="square1"></param>
        /// <param name="square2"></param>
        /// <returns></returns>
        public bool PairInColumn(int ThisColumn, int Number, int[, ,] grid, ref int row1, ref int row2, ref int square1, ref int square2)
        {
            bool rtn = false;
            int tmp = 0;
            int count = 0;

            for (int row = 1; row <= 9; row++)
            {
                if (grid[row, ThisColumn, Number] > 0)
                {
                    if (tmp == 1) { row2 = row; tmp = 2; }
                    if (tmp == 0) { row1 = row; tmp = 1; }
                    count = count + 1;
                }
            }

            if (count == 2) rtn = true;

            if (row1 >= 1 && row1 <= 3 && ThisColumn >= 1 && ThisColumn <= 3) { square1 = 1; }
            if (row1 >= 1 && row1 <= 3 && ThisColumn >= 4 && ThisColumn <= 6) { square1 = 2; }
            if (row1 >= 1 && row1 <= 3 && ThisColumn >= 7 && ThisColumn <= 9) { square1 = 3; }
            if (row1 >= 4 && row1 <= 6 && ThisColumn >= 1 && ThisColumn <= 3) { square1 = 4; }
            if (row1 >= 4 && row1 <= 6 && ThisColumn >= 4 && ThisColumn <= 6) { square1 = 5; }
            if (row1 >= 4 && row1 <= 6 && ThisColumn >= 7 && ThisColumn <= 9) { square1 = 6; }
            if (row1 >= 7 && row1 <= 9 && ThisColumn >= 1 && ThisColumn <= 3) { square1 = 7; }
            if (row1 >= 7 && row1 <= 9 && ThisColumn >= 4 && ThisColumn <= 6) { square1 = 8; }
            if (row1 >= 7 && row1 <= 9 && ThisColumn >= 7 && ThisColumn <= 9) { square1 = 9; }

            if (row2 >= 1 && row2 <= 3 && ThisColumn >= 1 && ThisColumn <= 3) { square1 = 1; }
            if (row2 >= 1 && row2 <= 3 && ThisColumn >= 4 && ThisColumn <= 6) { square1 = 2; }
            if (row2 >= 1 && row2 <= 3 && ThisColumn >= 7 && ThisColumn <= 9) { square1 = 3; }
            if (row2 >= 4 && row2 <= 6 && ThisColumn >= 1 && ThisColumn <= 3) { square1 = 4; }
            if (row2 >= 4 && row2 <= 6 && ThisColumn >= 4 && ThisColumn <= 6) { square1 = 5; }
            if (row2 >= 4 && row2 <= 6 && ThisColumn >= 7 && ThisColumn <= 9) { square1 = 6; }
            if (row2 >= 7 && row2 <= 9 && ThisColumn >= 1 && ThisColumn <= 3) { square1 = 7; }
            if (row2 >= 7 && row2 <= 9 && ThisColumn >= 4 && ThisColumn <= 6) { square1 = 8; }
            if (row2 >= 7 && row2 <= 9 && ThisColumn >= 7 && ThisColumn <= 9) { square1 = 9; }

            return rtn;
        }

        /// <summary>
        /// PairInSquare
        /// </summary>
        /// <param name="ThisSquare"></param>
        /// <param name="Number"></param>
        /// <param name="grid"></param>
        /// <param name="row1"></param>
        /// <param name="col1"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        public bool PairInSquare(int ThisSquare, int Number, int[, ,] grid, ref int row1, ref int col1, ref int row2, ref int col2)
        {
            bool rtn = false;
            int tmp = 0;
            int count = 0;

            if (ThisSquare == 1)
            {
                for (int row = 1; row <= 3; row++)
                {
                    for (int col = 1; col <= 3; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (ThisSquare == 2)
            {
                for (int row = 1; row <= 3; row++)
                {
                    for (int col = 4; col <= 6; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (ThisSquare == 3)
            {
                for (int row = 1; row <= 3; row++)
                {
                    for (int col = 7; col <= 9; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (ThisSquare == 4)
            {
                for (int row = 4; row <= 6; row++)
                {
                    for (int col = 1; col <= 3; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (ThisSquare == 5)
            {
                for (int row = 4; row <= 6; row++)
                {
                    for (int col = 4; col <= 6; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (ThisSquare == 6)
            {
                for (int row = 4; row <= 6; row++)
                {
                    for (int col = 7; col <= 9; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (ThisSquare == 7)
            {
                for (int row = 7; row <= 9; row++)
                {
                    for (int col = 1; col <= 3; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (ThisSquare == 8)
            {
                for (int row = 7; row <= 9; row++)
                {
                    for (int col = 4; col <= 6; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }


            if (ThisSquare == 9)
            {
                for (int row = 7; row <= 9; row++)
                {
                    for (int col = 7; col <= 9; col++)
                    {
                        if (grid[row, col, Number] > 0)
                        {
                            if (tmp == 1) { row2 = row; col2 = col; tmp = 2; }
                            if (tmp == 0) { row1 = row; col1 = col; tmp = 1; }
                            count = count + 1;
                        }
                    }
                }
            }

            if (row1 == row2 || col1 == col2) return false;

            if (count == 2) rtn = true;

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
