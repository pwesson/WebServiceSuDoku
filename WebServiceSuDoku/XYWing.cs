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
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace MySuDokuSolver
{
    public class XYWing
    {
        public struct RemotePairInfo
        {
            public int square;
            public int row;
            public int col;
            public int NumberA;
            public int NumberB;
        }

        public XYWing()
        {
        }

        public void Method(int[, ,] grid, DataTable dsTableSteps)
        {
            string Message;
            ArrayList work = new ArrayList();
            int[,] NumCount = new int[10, 10];

            for (int square = 1; square <= 9; square++)
            {
                for (int nNum = 1; nNum <= 9; nNum++)
                {
                    NumCount[square, nNum] = 0;
                }
            }

            for (int nRow = 1; nRow <= 9; nRow++)
            {
                for (int nCol = 1; nCol <= 9; nCol++)
                {
                    //Working on cell [nRow,nCol]

                    int square = 0;

                    if (nCol <= 3 && nRow <= 3) square = 1;
                    if (nCol >= 4 && nCol <= 6 && nRow <= 3) square = 2;
                    if (nCol >= 7 && nRow <= 3) square = 3;
                    if (nCol <= 3 && nRow >= 4 && nRow <= 6) square = 4;
                    if (nCol >= 4 && nCol <= 6 && nRow >= 4 && nRow <= 6) square = 5;
                    if (nCol >= 7 && nRow >= 4 && nRow <= 6) square = 6;
                    if (nCol <= 3 && nRow >= 7) square = 7;
                    if (nCol >= 4 && nCol <= 6 && nRow >= 7) square = 8;
                    if (nCol >= 7 && nRow >= 7) square = 9;

                    int number = 0;
                    for (int nNum = 1; nNum <= 9; nNum++)
                    {
                        if (grid[nRow, nCol, nNum] > 0)
                        {
                            NumCount[square, nNum] += 1;
                            number++;
                        }
                    }

                    //We have a Naked Pair?
                    if (number == 2)
                    {
                        //Add to the arraylist

                        RemotePairInfo NewEntry = new RemotePairInfo();
                        NewEntry.col = nCol;
                        NewEntry.row = nRow;
                        NewEntry.square = square;
                        NewEntry.NumberA = 0;
                        NewEntry.NumberB = 0;

                        for (int nNum = 1; nNum <= 9; nNum++)
                        {
                            if (grid[nRow, nCol, nNum] > 0)
                            {
                                if (NewEntry.NumberA == 0)
                                {
                                    NewEntry.NumberA = nNum;
                                }
                                else
                                {
                                    NewEntry.NumberB = nNum;
                                }
                            }
                        }
                        work.Add(NewEntry);
                    }//if number=2
                }//nCol
            }//nRow

            if (work.Count == 0) return;

            //Process the work ArrayList

            for (int i = 0; i < work.Count; i++)
            {
                for (int j = 0; j < work.Count; j++)
                {
                    for (int k = 0; k < work.Count; k++)
                    {
                        if (i != j && i != k && j != k)
                        {
                            //Does the i and j share the same square and not same row/column?

                            RemotePairInfo EntryI = (RemotePairInfo)work[i];
                            RemotePairInfo EntryJ = (RemotePairInfo)work[j];
                            RemotePairInfo EntryK = (RemotePairInfo)work[k];

                            Message = "Eliminate this number. XYWing [Row " + EntryI.row + ",Col " + EntryI.col + ",Square " + EntryI.square + ",#s " + EntryI.NumberA + "," + EntryI.NumberB + "]";
                            Message += "[Row " + EntryJ.row + ", Col " + EntryJ.col + ",Square " + EntryJ.square + ",#s" + EntryJ.NumberA + "," + EntryJ.NumberB + "]";
                            Message += "[Row " + EntryK.row + ", Col " + EntryK.col + ",Square " + EntryK.square + ",#s" + EntryK.NumberA + "," + EntryK.NumberB + "]";

                            if (EntryI.square == EntryJ.square &&
                                            EntryI.square != EntryK.square &&
                                            EntryI.row != EntryJ.row &&
                                            EntryI.col != EntryJ.col)
                            {

                                if (EntryI.NumberA == EntryJ.NumberA && NumCount[EntryJ.square, EntryJ.NumberA] == 2)
                                {
                                    //I & J share a common number I.NumberA

                                    if (EntryJ.NumberB == EntryK.NumberA && EntryI.NumberB == EntryK.NumberB)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberB, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberB, Message, dsTableSteps);
                                        }
                                    }

                                    if (EntryJ.NumberB == EntryK.NumberB && EntryI.NumberB == EntryK.NumberA)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberB, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberB, Message, dsTableSteps);
                                        }

                                    }
                                }


                                if (EntryI.NumberA == EntryJ.NumberB && NumCount[EntryJ.square, EntryJ.NumberB] == 2)
                                {
                                    //I & J share a common number I.NumberA

                                    if (EntryJ.NumberA == EntryK.NumberA && EntryI.NumberB == EntryK.NumberB)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberB, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberB, Message, dsTableSteps);
                                        }

                                    }

                                    if (EntryJ.NumberA == EntryK.NumberB && EntryI.NumberB == EntryK.NumberA)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberB, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberB] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberB, Message, dsTableSteps);
                                        }

                                    }
                                }



                                if (EntryI.NumberB == EntryJ.NumberA && NumCount[EntryJ.square, EntryJ.NumberA] == 2)
                                {
                                    //I & J share a common number I.NumberA

                                    if (EntryJ.NumberB == EntryK.NumberA && EntryI.NumberA == EntryK.NumberB)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberA, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberA, Message, dsTableSteps);
                                        }
                                    }

                                    if (EntryJ.NumberB == EntryK.NumberB && EntryI.NumberA == EntryK.NumberA)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberA, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberA, Message, dsTableSteps);
                                        }

                                    }
                                }


                                if (EntryI.NumberB == EntryJ.NumberB && NumCount[EntryJ.square, EntryJ.NumberB] == 2)
                                {
                                    //I & J share a common number I.NumberA

                                    if (EntryJ.NumberA == EntryK.NumberA && EntryI.NumberA == EntryK.NumberB)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberA, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberA, Message, dsTableSteps);
                                        }

                                    }

                                    if (EntryJ.NumberA == EntryK.NumberB && EntryI.NumberA == EntryK.NumberA)
                                    {
                                        //J & K share a common number J.NumberB

                                        if (EntryJ.col == EntryK.col)
                                        {
                                            grid[EntryK.row, EntryI.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryK.row, EntryI.col, EntryI.NumberA, Message, dsTableSteps);
                                        }
                                        if (EntryJ.row == EntryK.row)
                                        {
                                            grid[EntryI.row, EntryK.col, EntryI.NumberA] = 0;
                                            UpdateDataTableRow(1, EntryI.row, EntryK.col, EntryI.NumberA, Message, dsTableSteps);
                                        }

                                    }
                                }

                            }
                        }
                    }//k
                }//j
            }//i

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
