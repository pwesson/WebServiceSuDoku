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
    public class PointingPairAndBox
    {
        public PointingPairAndBox()
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
            PointingPairAndBoxLineReduction(1, 1, 2, 1, 3, 1, 4, 1, 5, 1, 6, 1, 7, 2, 8, 2, 9, 2, 7, 3, 8, 3, 9, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 2, 2, 2, 3, 2, 4, 2, 5, 2, 6, 2, 7, 1, 8, 1, 9, 1, 7, 3, 8, 3, 9, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 3, 2, 3, 3, 3, 4, 3, 5, 3, 6, 3, 7, 1, 8, 1, 9, 1, 7, 2, 8, 2, 9, 2, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 4, 2, 4, 3, 4, 4, 4, 5, 4, 6, 4, 7, 5, 8, 5, 9, 5, 7, 6, 8, 6, 9, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 5, 2, 5, 3, 5, 4, 5, 5, 5, 6, 5, 7, 4, 8, 4, 9, 4, 7, 6, 8, 6, 9, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 6, 2, 6, 3, 6, 4, 6, 5, 6, 6, 6, 7, 4, 8, 4, 9, 4, 7, 5, 8, 5, 9, 5, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 7, 2, 7, 3, 7, 4, 7, 5, 7, 6, 7, 7, 8, 8, 8, 9, 8, 7, 9, 8, 9, 9, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 8, 2, 8, 3, 8, 4, 8, 5, 8, 6, 8, 7, 7, 8, 7, 9, 7, 7, 9, 8, 9, 9, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 9, 2, 9, 3, 9, 4, 9, 5, 9, 6, 9, 7, 7, 8, 7, 9, 7, 7, 8, 8, 8, 9, 8, grid, dsTableSteps);

            PointingPairAndBoxLineReduction(1, 1, 2, 1, 3, 1, 7, 1, 8, 1, 9, 1, 4, 2, 5, 2, 6, 2, 4, 3, 5, 3, 6, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 2, 2, 2, 3, 2, 7, 2, 8, 2, 9, 2, 4, 1, 5, 1, 6, 1, 4, 3, 5, 3, 6, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 3, 2, 3, 3, 3, 7, 3, 8, 3, 9, 3, 4, 1, 5, 1, 6, 1, 4, 2, 5, 2, 6, 2, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 4, 2, 4, 3, 4, 7, 4, 8, 4, 9, 4, 4, 5, 5, 5, 6, 5, 4, 6, 5, 6, 6, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 5, 2, 5, 3, 5, 7, 5, 8, 5, 9, 5, 4, 4, 5, 4, 6, 4, 4, 6, 5, 6, 6, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 6, 2, 6, 3, 6, 7, 6, 8, 6, 9, 6, 4, 4, 5, 4, 6, 4, 4, 5, 5, 5, 6, 5, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 7, 2, 7, 3, 7, 7, 7, 8, 7, 9, 7, 4, 8, 5, 8, 6, 8, 4, 9, 5, 9, 6, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 8, 2, 8, 3, 8, 7, 8, 8, 8, 9, 8, 4, 7, 5, 7, 6, 7, 4, 9, 5, 9, 6, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(1, 9, 2, 9, 3, 9, 7, 9, 8, 9, 9, 9, 4, 7, 5, 7, 6, 7, 4, 8, 5, 8, 6, 8, grid, dsTableSteps);

            PointingPairAndBoxLineReduction(4, 1, 5, 1, 6, 1, 7, 1, 8, 1, 9, 1, 1, 2, 2, 2, 3, 2, 1, 3, 2, 3, 3, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 2, 5, 2, 6, 2, 7, 2, 8, 2, 9, 2, 1, 1, 2, 1, 3, 1, 1, 3, 2, 3, 3, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 3, 5, 3, 6, 3, 7, 3, 8, 3, 9, 3, 1, 1, 2, 1, 3, 1, 1, 2, 2, 2, 3, 2, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 4, 5, 4, 6, 4, 7, 4, 8, 4, 9, 4, 1, 5, 2, 5, 3, 5, 1, 6, 2, 6, 3, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 5, 5, 5, 6, 5, 7, 5, 8, 5, 9, 5, 1, 4, 2, 4, 3, 4, 1, 6, 2, 6, 3, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 6, 5, 6, 6, 6, 7, 6, 8, 6, 9, 6, 1, 4, 2, 4, 3, 4, 1, 5, 2, 5, 3, 5, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 7, 5, 7, 6, 7, 7, 7, 8, 7, 9, 7, 1, 8, 2, 8, 3, 8, 1, 9, 2, 9, 3, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 8, 5, 8, 6, 8, 7, 8, 8, 8, 9, 8, 1, 7, 2, 7, 3, 7, 1, 9, 2, 9, 3, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 9, 5, 9, 6, 9, 7, 9, 8, 9, 9, 9, 1, 7, 2, 7, 3, 7, 1, 8, 2, 8, 3, 8, grid, dsTableSteps);

            PointingPairAndBoxLineReduction(1, 1, 1, 2, 1, 3, 1, 4, 1, 5, 1, 6, 2, 7, 2, 8, 2, 9, 3, 7, 3, 8, 3, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(2, 1, 2, 2, 2, 3, 2, 4, 2, 5, 2, 6, 1, 7, 1, 8, 1, 9, 3, 7, 3, 8, 3, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(3, 1, 3, 2, 3, 3, 3, 4, 3, 5, 3, 6, 1, 7, 1, 8, 1, 9, 2, 7, 2, 8, 2, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 1, 4, 2, 4, 3, 4, 4, 4, 5, 4, 6, 5, 7, 5, 8, 5, 9, 6, 7, 6, 8, 6, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(5, 1, 5, 2, 5, 3, 5, 4, 5, 5, 5, 6, 4, 7, 4, 8, 4, 9, 6, 7, 6, 8, 6, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(6, 1, 6, 2, 6, 3, 6, 4, 6, 5, 6, 6, 4, 7, 4, 8, 4, 9, 5, 7, 5, 8, 5, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(7, 1, 7, 2, 7, 3, 7, 4, 7, 5, 7, 6, 8, 7, 8, 8, 8, 9, 9, 7, 9, 8, 9, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(8, 1, 8, 2, 8, 3, 8, 4, 8, 5, 8, 6, 7, 7, 7, 8, 7, 9, 9, 7, 9, 8, 9, 9, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(9, 1, 9, 2, 9, 3, 9, 4, 9, 5, 9, 6, 7, 7, 7, 8, 7, 9, 8, 7, 8, 8, 8, 9, grid, dsTableSteps);

            PointingPairAndBoxLineReduction(1, 1, 1, 2, 1, 3, 1, 7, 1, 8, 1, 9, 2, 4, 2, 5, 2, 6, 3, 4, 3, 5, 3, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(2, 1, 2, 2, 2, 3, 2, 7, 2, 8, 2, 9, 1, 4, 1, 5, 1, 6, 3, 4, 3, 5, 3, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(3, 1, 3, 2, 3, 3, 3, 7, 3, 8, 3, 9, 1, 4, 1, 5, 1, 6, 2, 4, 2, 5, 2, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 1, 4, 2, 4, 3, 4, 7, 4, 8, 4, 9, 5, 4, 5, 5, 5, 6, 6, 4, 6, 5, 6, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(5, 1, 5, 2, 5, 3, 5, 7, 5, 8, 5, 9, 4, 4, 4, 5, 4, 6, 6, 4, 6, 5, 6, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(6, 1, 6, 2, 6, 3, 6, 7, 6, 8, 6, 9, 4, 4, 4, 5, 4, 6, 5, 4, 5, 5, 5, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(7, 1, 7, 2, 7, 3, 7, 7, 7, 8, 7, 9, 8, 4, 8, 5, 8, 6, 9, 4, 9, 5, 9, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(8, 1, 8, 2, 8, 3, 8, 7, 8, 8, 8, 9, 7, 4, 7, 5, 7, 6, 9, 4, 9, 5, 9, 6, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(9, 1, 9, 2, 9, 3, 9, 7, 9, 8, 9, 9, 7, 4, 7, 5, 7, 6, 8, 4, 8, 5, 8, 6, grid, dsTableSteps);

            PointingPairAndBoxLineReduction(1, 4, 1, 5, 1, 6, 1, 7, 1, 8, 1, 9, 2, 1, 2, 2, 2, 3, 3, 1, 3, 2, 3, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(2, 4, 2, 5, 2, 6, 2, 7, 2, 8, 2, 9, 1, 1, 1, 2, 1, 3, 3, 1, 3, 2, 3, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(3, 4, 3, 5, 3, 6, 3, 7, 3, 8, 3, 9, 1, 1, 1, 2, 1, 3, 2, 1, 2, 2, 2, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(4, 4, 4, 5, 4, 6, 4, 7, 4, 8, 4, 9, 5, 1, 5, 2, 5, 3, 6, 1, 6, 2, 6, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(5, 4, 5, 5, 5, 6, 5, 7, 5, 8, 5, 9, 4, 1, 4, 2, 4, 3, 6, 1, 6, 2, 6, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(6, 4, 6, 5, 6, 6, 6, 7, 6, 8, 6, 9, 4, 1, 4, 2, 4, 3, 5, 1, 5, 2, 5, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(7, 4, 7, 5, 7, 6, 7, 7, 7, 8, 7, 9, 8, 1, 8, 2, 8, 3, 9, 1, 9, 2, 9, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(8, 4, 8, 5, 8, 6, 8, 7, 8, 8, 8, 9, 7, 1, 7, 2, 7, 3, 9, 1, 9, 2, 9, 3, grid, dsTableSteps);
            PointingPairAndBoxLineReduction(9, 4, 9, 5, 9, 6, 9, 7, 9, 8, 9, 9, 7, 1, 7, 2, 7, 3, 8, 1, 8, 2, 8, 3, grid, dsTableSteps);	

        }

        /// <summary>
        /// PointingPairAndBoxLineReduction
        /// </summary>
        /// <param name="NotHere1x"></param>
        /// <param name="NotHere1y"></param>
        /// <param name="NotHere2x"></param>
        /// <param name="NotHere2y"></param>
        /// <param name="NotHere3x"></param>
        /// <param name="NotHere3y"></param>
        /// <param name="NotHere4x"></param>
        /// <param name="NotHere4y"></param>
        /// <param name="NotHere5x"></param>
        /// <param name="NotHere5y"></param>
        /// <param name="NotHere6x"></param>
        /// <param name="NotHere6y"></param>
        /// <param name="Eliminate1x"></param>
        /// <param name="Eliminate1y"></param>
        /// <param name="Eliminate2x"></param>
        /// <param name="Eliminate2y"></param>
        /// <param name="Eliminate3x"></param>
        /// <param name="Eliminate3y"></param>
        /// <param name="Eliminate4x"></param>
        /// <param name="Eliminate4y"></param>
        /// <param name="Eliminate5x"></param>
        /// <param name="Eliminate5y"></param>
        /// <param name="Eliminate6x"></param>
        /// <param name="Eliminate6y"></param>
        /// <param name="grid"></param>
        public void PointingPairAndBoxLineReduction(int NotHere1x, int NotHere1y,
                        int NotHere2x, int NotHere2y,
                        int NotHere3x, int NotHere3y,
                        int NotHere4x, int NotHere4y,
                        int NotHere5x, int NotHere5y,
                        int NotHere6x, int NotHere6y,
                        int Eliminate1x, int Eliminate1y,
                        int Eliminate2x, int Eliminate2y,
                        int Eliminate3x, int Eliminate3y,
                        int Eliminate4x, int Eliminate4y,
                        int Eliminate5x, int Eliminate5y,
                        int Eliminate6x, int Eliminate6y,
                        int[, ,] grid, DataTable dsTableSteps)
        {
            //If a number can not appear in cells a1,b1,c1, d1,e1, f1 then it must 
            //exist in the cells g1, h1, i1. Thus can eliminate g2,h2,i2 and g3,h3,i3
            //Now the caller of the function must figure out the combinations.
            //This subroutine loops over each number 1 to 9
            //If the number can not appear in NotHere1,....,NotHere6, then eliminate in Eliminate1...,6.

            int CanExist = 0;

            //Loop over the numbers

            for (int nNum = 1; nNum <= 9; nNum++)
            {
                CanExist = 0;

                if (grid[NotHere1x, NotHere1y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[NotHere2x, NotHere2y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[NotHere3x, NotHere3y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[NotHere4x, NotHere4y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[NotHere5x, NotHere5y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[NotHere6x, NotHere6y, nNum] > 0) { CanExist = 1; }  //The number might exist here

                if (CanExist == 0)
                {
                    //The number nNum can not exist in the cells a1,b1,c1, d1,e1,f1
                    //Thus can eliminate in the other squares

                    if (grid[Eliminate1x, Eliminate1y, nNum] > 0) UpdateDataTableRow(1, Eliminate1x, Eliminate1y, nNum, "Eliminate this number: Pointing Pair", dsTableSteps);
                    if (grid[Eliminate2x, Eliminate2y, nNum] > 0) UpdateDataTableRow(1, Eliminate2x, Eliminate2y, nNum, "Eliminate this number: Pointing Pair", dsTableSteps);
                    if (grid[Eliminate3x, Eliminate3y, nNum] > 0) UpdateDataTableRow(1, Eliminate3x, Eliminate3y, nNum, "Eliminate this number: Pointing Pair", dsTableSteps);
                    if (grid[Eliminate4x, Eliminate4y, nNum] > 0) UpdateDataTableRow(1, Eliminate4x, Eliminate4y, nNum, "Eliminate this number: Pointing Pair", dsTableSteps);
                    if (grid[Eliminate5x, Eliminate5y, nNum] > 0) UpdateDataTableRow(1, Eliminate5x, Eliminate5y, nNum, "Eliminate this number: Pointing Pair", dsTableSteps);
                    if (grid[Eliminate6x, Eliminate6y, nNum] > 0) UpdateDataTableRow(1, Eliminate6x, Eliminate6y, nNum, "Eliminate this number: Pointing Pair", dsTableSteps);

                    grid[Eliminate1x, Eliminate1y, nNum] = 0;
                    grid[Eliminate2x, Eliminate2y, nNum] = 0;
                    grid[Eliminate3x, Eliminate3y, nNum] = 0;
                    grid[Eliminate4x, Eliminate4y, nNum] = 0;
                    grid[Eliminate5x, Eliminate5y, nNum] = 0;
                    grid[Eliminate6x, Eliminate6y, nNum] = 0;
                }

            } // for loop for nNum

            //Vice versa also works - the complete opposite

            for (int nNum = 1; nNum <= 9; nNum++)
            {
                CanExist = 0;

                if (grid[Eliminate1x, Eliminate1y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[Eliminate2x, Eliminate2y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[Eliminate3x, Eliminate3y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[Eliminate4x, Eliminate4y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[Eliminate5x, Eliminate5y, nNum] > 0) { CanExist = 1; }  //The number might exist here
                if (grid[Eliminate6x, Eliminate6y, nNum] > 0) { CanExist = 1; }  //The number might exist here

                if (CanExist == 0)
                {
                    //The number nNum can not exist in the cells a1,b1,c1, d1,e1,f1
                    //Thus can eliminate in the other squares

                    if (grid[NotHere1x, NotHere1y, nNum] > 0) UpdateDataTableRow(1, NotHere1x, NotHere1y, nNum, "Eliminate this number: Box Line Reduction", dsTableSteps);
                    if (grid[NotHere2x, NotHere2y, nNum] > 0) UpdateDataTableRow(1, NotHere2x, NotHere2y, nNum, "Eliminate this number: Box Line Reduction", dsTableSteps);
                    if (grid[NotHere3x, NotHere3y, nNum] > 0) UpdateDataTableRow(1, NotHere3x, NotHere3y, nNum, "Eliminate this number: Box Line Reduction", dsTableSteps);
                    if (grid[NotHere4x, NotHere4y, nNum] > 0) UpdateDataTableRow(1, NotHere4x, NotHere4y, nNum, "Eliminate this number: Box Line Reduction", dsTableSteps);
                    if (grid[NotHere5x, NotHere5y, nNum] > 0) UpdateDataTableRow(1, NotHere5x, NotHere5y, nNum, "Eliminate this number: Box Line Reduction", dsTableSteps);
                    if (grid[NotHere6x, NotHere6y, nNum] > 0) UpdateDataTableRow(1, NotHere6x, NotHere6y, nNum, "Eliminate this number: Box Line Reduction", dsTableSteps);

                    grid[NotHere1x, NotHere1y, nNum] = 0;
                    grid[NotHere2x, NotHere2y, nNum] = 0;
                    grid[NotHere3x, NotHere3y, nNum] = 0;
                    grid[NotHere4x, NotHere4y, nNum] = 0;
                    grid[NotHere5x, NotHere5y, nNum] = 0;
                    grid[NotHere6x, NotHere6y, nNum] = 0;
                }

            } // for loop for nNum
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
