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
using MySuDokuSolver;

namespace MySuDokuSolver
{
    public class Guess
    {
        public struct QA
        {
            public string question;
            public string answer;
            public string unsolved;
        }

        public Guess()
        {

        }

        /// <summary>
        /// Method
        /// </summary>
        /// <param name="varPuzzleDescription"></param>
        /// <returns></returns>
        public DataSet Method(string varPuzzleDescription)
        {
            //Want to guess solutions 

            DataSet dsData = new DataSet();

            //Declare memory
            int puzzleptr;
            ArrayList puzzles = new ArrayList();

            //Initialise variables
            puzzleptr = 0;
            puzzles.Clear();

            QA tmp = new QA();
            QA newtmp = new QA();
            tmp.question = varPuzzleDescription;
            tmp.answer = "";
            puzzles.Add(tmp);

            //Loop over the puzzles array until the end

            while (puzzleptr < puzzles.Count)
            {
                //Get the next problem

                tmp = (QA)puzzles[puzzleptr];

                //Find the solution

                Solver obj = new Solver();
                obj.dsData.Clear();
                obj.WebSolve(tmp.question.ToString());

                dsData = obj.dsData;
                tmp.answer = dsData.Tables["TableGrids"].Rows[1]["Grid"].ToString();
                tmp.unsolved = dsData.Tables["TableGrids"].Rows[3]["Grid"].ToString();
                puzzles[puzzleptr] = tmp;

                //Add to the ArrayList if "Too Hard"

                if (dsData.Tables["TableGrids"].Rows[2]["Grid"].ToString() == "Too Hard")
                {
                    //Find the first cell to guess its entries.
                    string cell = "";

                    for (int i = 1; i <= 81; i++)
                    {
                        cell = tmp.unsolved.Substring(3 * i - 3, 3);
                        if (cell.ToString() != "[-]")
                        {
                            int[] possible = Possibility(cell);
                            for (int j = 1; j <= 9; j++)
                            {
                                if (possible[j] > 0)
                                {
                                    newtmp.question = Inject(tmp.answer, i, j);
                                    newtmp.answer = "";
                                    newtmp.unsolved = "";
                                    puzzles.Add(newtmp);
                                }
                            }
                            break;
                        }
                    }
                }//Too Hard
                else
                {
                    //Have found a solution!
                    break;
                }
                puzzleptr = puzzleptr + 1;

                if (puzzleptr > 500) break;
            }

            return dsData;
       }

        /// <summary>
        /// Inject
        /// </summary>
        /// <param name="question"></param>
        /// <param name="cell"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        private string Inject(string question, int cell, int num)
        {
            string newquestion = "";

            for (int i = 1; i <= 81; i++)
            {
                if (cell == i)
                {
                    newquestion = newquestion + num.ToString();
                }
                else
                {
                    newquestion = newquestion + question.Substring(i - 1, 1);
                }
            }

            return newquestion;
        }

        /// <summary>
        /// Possibility
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private int[] Possibility(string snumber)
        {
            int[] nums = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if (snumber == "[-]") return nums;

            int number = Convert.ToInt32(snumber);

            if (number >= 256) { nums[9] = 1; number = number - 256; }
            if (number >= 128) { nums[8] = 1; number = number - 128; }
            if (number >= 64) { nums[7] = 1; number = number - 64; }
            if (number >= 32) { nums[6] = 1; number = number - 32; }
            if (number >= 16) { nums[5] = 1; number = number - 16; }
            if (number >= 8) { nums[4] = 1; number = number - 8; }
            if (number >= 4) { nums[3] = 1; number = number - 4; }
            if (number >= 2) { nums[2] = 1; number = number - 2; }
            if (number >= 1) { nums[1] = 1; number = number - 1; }

            return nums;
        }


    }
}
