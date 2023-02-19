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
    public class NakedSingle
    {
        public NakedSingle()
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
        /// <param name="FoundA"></param>
        /// <param name="answer"></param>
        public void Method(int[, ,] grid, DataTable dsTableSteps, int[,] FoundA, int[,] answer)
        {

            for (int nx = 1; nx <= 9; nx++)
            {
                for (int ny = 1; ny <= 9; ny++)
                {
                    int nCount = 0;

                    for (int nNum = 1; nNum <= 9; nNum++)
                    {
                        if (grid[nx, ny, nNum] > 0)
                        {
                            nCount = nCount + 1;
                        }
                    }

                    if (nCount == 1 && FoundA[nx, ny] == 0)
                    {
                        for (int nNum = 1; nNum <= 9; nNum++)
                        {
                            if (grid[nx, ny, nNum] > 0)
                            {
                                nCount = nNum;
                            }
                        }

                        //Found a new number

                        for (int nNum = 1; nNum <= 9; nNum++)
                        {
                            grid[nNum, ny, nCount] = 0;
                        }
                        for (int nNum = 1; nNum <= 9; nNum++)
                        {
                            grid[nx, nNum, nCount] = 0;
                        }
                        for (int nNum = 1; nNum <= 9; nNum++)
                        {
                            grid[nx, ny, nNum] = 0;
                        }
                        grid[nx, ny, nCount] = nCount;
                        FoundA[nx, ny] = nCount;
                        if (answer[nx, ny] == 0)
                        {
                            answer[nx, ny] = nCount;
                            UpdateDataTableRow(1, nx, ny, nCount, "Naked single", dsTableSteps);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// IsFound
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="Col"></param>
        /// <returns></returns>
        private bool IsFound(int Row, int Col)
        {


            return false;
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
