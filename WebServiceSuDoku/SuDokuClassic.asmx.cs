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
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

using MySuDokuSolver;
using System.Data;

namespace WebServiceSuDoku
{
    /// <summary>
    /// Summary description for SuDokuClassic
    /// </summary>
    [WebService(Namespace = "http://pwesson.github.io/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SuDokuClassic : System.Web.Services.WebService
    {
        [WebMethod]
        public DataSet MethodSELECTSolve(string varPuzzleDescription)
        {
            Solver obj = new Solver();
            obj.dsData.Clear();
            obj.WebSolve(varPuzzleDescription);
            return obj.dsData;
        }

        [WebMethod]
        public DataSet MethodSELECTByNumber(long varNumber)
        {

            DALClassic obj = new DALClassic();
            obj.MethodSELECTByPuzzleNumber(varNumber);

            return obj.dsData;
        }

        [WebMethod]
        public string MethodSELECTQuestionByNumberInMemory(long varNumber)
        {
            StoredPuzzles obj = new StoredPuzzles();
            return obj.PuzzleQuestion(varNumber);
        }

        [WebMethod]
        public string MethodSELECTLevelByNumberInMemory(long varNumber)
        {
            StoredPuzzles obj = new StoredPuzzles();
            return obj.PuzzleLevel(varNumber);
        }

        [WebMethod]
        public string MethodSELECTSortOrderByNumberInMemory(long varNumber)
        {
            StoredPuzzles obj = new StoredPuzzles();
            return obj.PuzzleSolveOrder(varNumber);
        }

        [WebMethod]
        public void MethodInsertUpdate(long PuzzleNumber, string Puzzle, string Solution, string Level, DateTime dt, string IPaddress, int Visible, string Comments, string SolveOrder, string varPassword)
        {
            if (varPassword.ToString() == "Wesson")
            {
                DALClassic obj = new DALClassic();
                obj.MethodInsertUpdate(PuzzleNumber, Puzzle, Solution, Level, dt, IPaddress, Visible, Comments, SolveOrder);
            }
        }

        [WebMethod]
        public DataSet MethodSELECTGuess(string varPuzzleDescription)
        {
            //Solve the original question

            DataSet dsData = new DataSet();
            Guess obj = new Guess();

            dsData = obj.Method(varPuzzleDescription);

            return dsData;
        }
    }
}
