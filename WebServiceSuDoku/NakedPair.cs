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

namespace MySuDokuSolver
{
    public class NakedPair
    {
        public NakedPair()
        {

        }

        /// <summary>
        /// Method
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="dsTableSteps"></param>
        public void Method(int[, ,] grid, DataTable dsTableSteps)
        {
            int[] aWork = new int[10];

            //Check first the columns ----------------------

            for (int i = 1; i <= 8; i++)  //i,j are the numbers to find
            {
                for (int j = i + 1; j <= 9; j++)
                {
                    //have i,j,k #'s to investigate, say 1,8

                    int nMatch = 0;

                    for (int nCol = 1; nCol <= 9; nCol++)
                    {
                        //Interested in Cells just holdings #'s 1,8

                        int nProcess = 1;
                        nMatch = 0;

                        for (int nRow = 1; nRow <= 9; nRow++)
                        {
                            aWork[nRow] = 0;
                            nProcess = 1;

                            //Does it meet the condition
                            //Loop over the numbers in the Cell
                            for (int nNum = 1; nNum <= 9; nNum++)
                            {
                                if (nNum != i && nNum != j)
                                {
                                    if (grid[nRow, nCol, nNum] > 0)
                                    {
                                        nProcess = 0;	//Can't process, so not interested
                                    }
                                }
                            }
                            if (nProcess == 1)
                            {
                                nMatch = nMatch + 1;
                                aWork[nRow] = 1;  //Could possibly elimate some numbers in this cell
                            }
                        } //Next nRow

                        if (nMatch == 2)  // exactly the same 3 numbers in exactly 3 cells
                        {
                            //Can elimate some numbers

                            for (int nRow = 1; nRow <= 9; nRow++)
                            {
                                if (aWork[nRow] == 0)  // the other cells with other numbers. Can eliminate 1,8,9
                                {
                                    //grid[nRow, nCol, i] = 0;
                                    //Can eliminate
                                    if (grid[nRow, nCol, i] > 0)
                                    {
                                        grid[nRow, nCol, i] = 0;
                                        UpdateDataTableRow(1, nRow, nCol, i, "Eliminate this number: Naked Pair", dsTableSteps);
                                    }

                                    grid[nRow, nCol, j] = 0;
                                    //Can eliminate
                                    if (grid[nRow, nCol, j] > 0)
                                    {
                                        grid[nRow, nCol, j] = 0;
                                        UpdateDataTableRow(1, nRow, nCol, j, "Eliminate this number: Naked Pair", dsTableSteps);
                                    }
                                }
                            }
                        }
                    }//nCol 
                } //j	
            } //i

            //Check second the rows -------------------------------

            for (int i = 1; i <= 8; i++)
            {
                for (int j = i + 1; j <= 9; j++)
                {
                    int nMatch = 0;

                    for (int nRow = 1; nRow <= 9; nRow++)
                    {
                        //Interested in Cells just holdings #'s 1,8,9 max

                        int nProcess = 1;
                        nMatch = 0;

                        for (int nCol = 1; nCol <= 9; nCol++)
                        {
                            aWork[nCol] = 0;
                            nProcess = 1;

                            //Does it meet the condition 
                            for (int nNum = 1; nNum <= 9; nNum++)
                            {
                                if (nNum != i && nNum != j)
                                {
                                    if (grid[nRow, nCol, nNum] > 0)
                                    {
                                        nProcess = 0;
                                    }
                                }
                            }
                            if (nProcess == 1)
                            {
                                nMatch = nMatch + 1;
                                aWork[nCol] = 1;  //Could possibly elimate some numbers in this cell
                            }
                        } //Next nRow

                        if (nMatch == 2)
                        {
                            //Can elimate some numbers

                            for (int nCol = 1; nCol <= 9; nCol++)
                            {
                                if (aWork[nCol] == 0)
                                {
                                    //grid[nRow, nCol, i] = 0;
                                    if (grid[nRow, nCol, i] > 0)
                                    {
                                        grid[nRow, nCol, i] = 0;
                                        UpdateDataTableRow(1, nRow, nCol, i, "Eliminate this number: Naked Pair", dsTableSteps);
                                    }

                                    //grid[nRow, nCol, j] = 0;
                                    if (grid[nRow, nCol, j] > 0)
                                    {
                                        grid[nRow, nCol, j] = 0;
                                        UpdateDataTableRow(1, nRow, nCol, j, "Eliminate this number: Naked Pair", dsTableSteps);
                                    }
                                }
                            }
                        }
                    }//nRow
                } //j	
            } //i

            //Check last the squares -------------------------------

            for (int i = 1; i <= 8; i++)
            {
                for (int j = i + 1; j <= 9; j++)
                {
                    int nMatch = 0;

                    for (int square = 0; square < 9; square++)
                    {
                        int ptrRow = 3 * ((int)(square / 3) + 1) - 2;
                        int ptrCol = 3 * (square % 3) + 1;

                        int nCell = 0;
                        int nProcess = 1;
                        nMatch = 0;

                        for (int nRow = ptrRow; nRow < ptrRow + 3; nRow++)
                        {
                            for (int nCol = ptrCol; nCol < ptrCol + 3; nCol++)
                            {
                                nCell++;
                                aWork[nCell] = 0;
                                nProcess = 1;

                                //Does it meet the condition 
                                for (int nNum = 1; nNum <= 9; nNum++)
                                {
                                    if (nNum != i && nNum != j)
                                    {
                                        if (grid[nRow, nCol, nNum] > 0)
                                        {
                                            nProcess = 0;
                                        }
                                    }
                                }
                                if (nProcess == 1)
                                {
                                    nMatch = nMatch + 1;
                                    aWork[nCell] = 1;  //Could possibly elimate some numbers in this cell
                                }
                            }//nCol
                        }//nRow

                        if (nMatch == 2)
                        {
                            //Can elimate some numbers

                            nCell = 0;
                            for (int nRow = ptrRow; nRow < ptrRow + 3; nRow++)
                            {
                                for (int nCol = ptrCol; nCol < ptrCol + 3; nCol++)
                                {
                                    nCell++;
                                    if (aWork[nCell] == 0)
                                    {
                                        //grid[nRow, nCol, i] = 0;
                                        if (grid[nRow, nCol, i] > 0)
                                        {
                                            grid[nRow, nCol, i] = 0;
                                            UpdateDataTableRow(1, nRow, nCol, i, "Eliminate this number: Naked Pair", dsTableSteps);
                                        }

                                        //grid[nRow, nCol, j] = 0;
                                        if (grid[nRow, nCol, j] > 0)
                                        {
                                            grid[nRow, nCol, j] = 0;
                                            UpdateDataTableRow(1, nRow, nCol, j, "Eliminate this number: Naked Pair", dsTableSteps);
                                        }
                                    }
                                }
                            }
                        }
                    }//square
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
