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
    public class HiddenTriple
    {
        public HiddenTriple()
        {
        }

        public void Method(int[, ,] grid, DataTable dsTableSteps)
        {
            int[] aWork = new int[10];

            //Check first the columns ----------------------

            for (int i = 1; i <= 7; i++)  //i,j,k are the numbers to find
            {
                for (int j = i + 1; j <= 8; j++)
                {
                    for (int k = j + 1; k <= 9; k++)
                    {
                        //have i,j,k #'s to investigate, say 1,8,9

                        int nMatch = 0;

                        for (int nCol = 1; nCol <= 9; nCol++)
                        {
                            //Interested in Cells just holdings #'s 1,8,9 max

                            nMatch = 0;

                            for (int nRow = 1; nRow <= 9; nRow++)
                            {
                                aWork[nRow] = 0;
                                if (grid[nRow, nCol, i] > 0) { nMatch++; aWork[nRow] = 1; }
                                if (grid[nRow, nCol, j] > 0) { nMatch++; aWork[nRow] = 1; }
                                if (grid[nRow, nCol, k] > 0) { nMatch++; aWork[nRow] = 1; }
                            }

                            if (nMatch == 3)
                            {
                                // aWork[nCell] == 1 can eliminate #'s i,j,k
                                for (int nRow = 1; nRow <= 9; nRow++)
                                {
                                    if (aWork[nRow] == 1)
                                    {
                                        for (int nNum = 1; nNum <= 9; nNum++)
                                        {
                                            if (nNum != i && nNum != j && nNum != k)
                                            {
                                                //Can eliminate
                                                if (grid[nRow, nCol, nNum] > 0)
                                                {
                                                    grid[nRow, nCol, nNum] = 0;
                                                    UpdateDataTableRow(1, nRow, nCol, nNum, "Eliminate this number: Hidden Triple", dsTableSteps);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }//nCol
                    }//k
                }//j
            }//i

            //Check second the rows ----------------------

            for (int i = 1; i <= 7; i++)  //i,j,k are the numbers to find
            {
                for (int j = i + 1; j <= 8; j++)
                {
                    for (int k = j + 1; k <= 9; k++)
                    {
                        //have i,j,k #'s to investigate, say 1,8,9

                        int nMatch = 0;

                        for (int nRow = 1; nRow <= 9; nRow++)
                        {
                            nMatch = 0;

                            for (int nCol = 1; nCol <= 9; nCol++)
                            {
                                aWork[nCol] = 0;
                                if (grid[nRow, nCol, i] > 0) { nMatch++; aWork[nCol] = 1; }
                                if (grid[nRow, nCol, j] > 0) { nMatch++; aWork[nCol] = 1; }
                                if (grid[nRow, nCol, k] > 0) { nMatch++; aWork[nCol] = 1; }
                            }

                            if (nMatch == 3)
                            {
                                // aWork[nCell] == 1 can eliminate #'s i,j,k
                                for (int nCol = 1; nCol <= 9; nCol++)
                                {
                                    if (aWork[nCol] == 1)
                                    {
                                        for (int nNum = 1; nNum <= 9; nNum++)
                                        {
                                            if (nNum != i && nNum != j && nNum != k)
                                            {
                                                //Can eliminate
                                                if (grid[nRow, nCol, nNum] > 0)
                                                {
                                                    grid[nRow, nCol, nNum] = 0;
                                                    UpdateDataTableRow(1, nRow, nCol, nNum, "Eliminate this number: Hidden Triple", dsTableSteps);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }//nRow
                    }//k
                }//j
            }//i

            //---squares

            for (int i = 1; i <= 7; i++)
            {
                for (int j = i + 1; j <= 8; j++)
                {
                    for (int k = j + 1; k <= 9; k++)
                    {
                        int nMatch = 0;

                        for (int square = 0; square < 9; square++)
                        {
                            int ptrRow = 3 * ((int)(square / 3) + 1) - 2;
                            int ptrCol = 3 * (square % 3) + 1;
                            int nCell = 0;

                            nMatch = 0;

                            for (int nRow = ptrRow; nRow < ptrRow + 3; nRow++)
                            {
                                for (int nCol = ptrCol; nCol < ptrCol + 3; nCol++)
                                {
                                    nCell++;
                                    aWork[nCell] = 0;
                                    if (grid[nRow, nCol, i] > 0) { nMatch++; aWork[nCell] = 1; }
                                    if (grid[nRow, nCol, j] > 0) { nMatch++; aWork[nCell] = 1; }
                                    if (grid[nRow, nCol, k] > 0) { nMatch++; aWork[nCell] = 1; }
                                }
                            }
                            if (nMatch == 3)
                            {
                                nCell = 0;
                                // aWork[nCell] == 1 can eliminate #'s i,j,k
                                for (int nRow = ptrRow; nRow < ptrRow + 3; nRow++)
                                {
                                    for (int nCol = ptrCol; nCol < ptrCol + 3; nCol++)
                                    {
                                        nCell++;
                                        if (aWork[nCell] == 1)
                                        {
                                            for (int nNum = 1; nNum <= 9; nNum++)
                                            {
                                                if (nNum != i && nNum != j && nNum != k)
                                                {
                                                    //Can eliminate
                                                    if (grid[nRow, nCol, nNum] > 0)
                                                    {
                                                        grid[nRow, nCol, nNum] = 0;
                                                        UpdateDataTableRow(1, nRow, nCol, nNum, "Eliminate this number: Hidden Triple", dsTableSteps);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }//square
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
