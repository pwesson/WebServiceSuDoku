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
    public class BugTechnique
    {
        public BugTechnique()
        {
        }

        public void Method(int[, ,] grid, DataTable dsTableSteps)
        {
            ArrayList work = new ArrayList();
            int[] ListRows = new int[] { 1, 1, 1, 4, 4, 4, 6, 6, 6 };
            int[] ListCols = new int[] { 1, 4, 7, 1, 4, 7, 1, 4, 7 };
            int[] NumCount = new int[10];

            //Look at the squares. If one contains 3 choices and the others 2 
            //Then pair off the numbers and the one what isn't paired in the number in the cell

            int foundrow = 0;
            int foundcol = 0;
            int maybe = 1;
            int InvestigateNumber = 0;
            int InvestigateSquare = 0;
            int pair = 0;

            //Loop over the squares
            for (int square = 1; square <= 9; square++)
            {
                //Loop over the numbers
                for (int nNum = 1; nNum <= 9; nNum++)
                {
                    pair = 0;
                    for (int row = ListRows[square]; row < ListRows[square] + 3; row++)
                    {
                        for (int col = ListCols[square]; col < ListCols[square] + 3; col++)
                        {
                            if (grid[row, col, nNum] > 0)
                            {
                                pair = pair + 1;
                            }
                            if (pair >= 3 && InvestigateNumber > 0)
                            {
                                maybe = 0;
                            }
                            if (pair == 3)
                            {
                                InvestigateNumber = nNum;
                                InvestigateSquare = square;
                                foundrow = row;
                                foundcol = col;
                            }
                        }
                    }
                    //If pair = 2 then okay.

                    if (pair == 1) //Must check its the only one in the cell
                    {
                        for (int row = ListRows[square]; row < ListRows[square] + 3; row++)
                        {
                            for (int col = ListCols[square]; col < ListCols[square] + 3; col++)
                            {
                                if (grid[row, col, nNum] > 0)
                                {
                                    int count = 0;
                                    for (int nNum2 = 1; nNum2 <= 9; nNum2++)
                                    {
                                        if (grid[row, col, nNum2] > 0)
                                        {
                                            count = count + 1;
                                        }
                                    }
                                    //Must not be greater than 1
                                    if (count > 1)
                                    {
                                        maybe = 0;
                                    }
                                }
                            }
                        }
                    }

                }//nNum

                if (maybe == 1 && InvestigateNumber > 0)
                {
                    for (int nNum = 1; nNum <= 9; nNum++)
                    {
                        grid[foundrow, foundcol, nNum] = 0;
                    }
                    grid[foundrow, foundcol, InvestigateNumber] = InvestigateNumber;
                    UpdateDataTableRow(1, foundrow, foundcol, InvestigateNumber, "Bug Technique",dsTableSteps);
                }

            }//square

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
