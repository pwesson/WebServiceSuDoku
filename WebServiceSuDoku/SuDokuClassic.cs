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
using System.Collections;  //for the ArrayList
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.IO;  //for file input-output

namespace MySuDokuSolver
{
    /// <summary>
    /// Summary description for class DALClassic.
    /// </summary>
    public class DALClassic
    {
        //Declare memory used

        ///<summary>
        ///dsData
        ///</summary>
        public DataSet dsData;

        ///<summary>
        ///aNumArray
        ///</summary>
        public long aNumArray
        {
            get
            {
                return dsData.Tables["Table"].Rows.Count;
            }
        }

        ///<summary>
        /// PuzzleNumber
        ///</summary>
        public long aPuzzleNumber(int index)
		{
			if (index >= 0 && index < aNumArray)
			{
				return System.Convert.ToInt64(this.dsData.Tables["Table"].Rows[index]["PuzzleNumber"]);
			}
			else
			{
                return 0;
			}
		}

        ///<summary>
        /// sPuzzle
        ///</summary>
        public string asPuzzle(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToString(this.dsData.Tables["Table"].Rows[index]["Puzzle"]);
            }
            else
            {
                return "";
            }
        }

        ///<summary>
        /// sSolution
        ///</summary>
        public string asSolution(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToString(this.dsData.Tables["Table"].Rows[index]["Solution"]);
            }
            else
            {
                return "";
            }
        }

        ///<summary>
        /// sLevel
        ///</summary>
        public string asLevel(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToString(this.dsData.Tables["Table"].Rows[index]["Level"]);
            }
            else
            {
                return "";
            }
        }

        ///<summary>
        /// dtCreated
        ///</summary>
        public DateTime adtCreated(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToDateTime(this.dsData.Tables["Table"].Rows[index]["Created"]);
            }
            else
            {
                return new DateTime(1900, 1, 1);
            }
        }

        ///<summary>
        /// sIPaddress
        ///</summary>
        public string asIPaddress(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToString(this.dsData.Tables["Table"].Rows[index]["IPaddress"]);
            }
            else
            {
                return "";
            }
        }

        ///<summary>
        /// nVisible
        ///</summary>
        public int anVisible(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToInt32(this.dsData.Tables["Table"].Rows[index]["Visible"]);
            }
            else
            {
                return 0;
            }
        }

        ///<summary>
        /// sComments
        ///</summary>
        public string asComments(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToString(this.dsData.Tables["Table"].Rows[index]["Comments"]);
            }
            else
            {
                return "";
            }
        }

        ///<summary>
        /// sSolveOrder
        ///</summary>
        public string asSolveOrder(int index)
        {
            if (index >= 0 && index < aNumArray)
            {
                return System.Convert.ToString(this.dsData.Tables["Table"].Rows[index]["SolveOrder"]);
            }
            else
            {
                return "";
            }
        }

        ///<summary>
        /// Classic
        ///</summary>
        public DALClassic()
        {
            //Initialisation of variables

            dsData = new DataSet();
        }

        //----------------------------------------
        ///<summary>
        /// MethodSELECTByAllKeys
        ///</summary>
        public void MethodSELECTByAllKeys(string varPuzzleNumber, string varPuzzle)
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                SqlParameter dbParam1;
                SqlParameter dbParam2;
                DataSet ds = new DataSet();

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicSELECTBYPuzzleNumberBYPuzzle", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                dbParam1 = new SqlParameter("@varPuzzleNumber", SqlDbType.BigInt, 8);
                dbParam1.Direction = ParameterDirection.Input;
                dbParam1.Value = varPuzzleNumber;
                dbCommand.Parameters.Add(dbParam1);

                dbParam2 = new SqlParameter("@varPuzzle", SqlDbType.VarChar, 100);
                dbParam2.Direction = ParameterDirection.Input;
                dbParam2.Value = varPuzzle;
                dbCommand.Parameters.Add(dbParam2);

                SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

                dsData.Clear();
                sAdapter.Fill(dsData, "Table");

                //Close connection to database

                dbConnection.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodSELECTAll
        ///</summary>
        public void MethodSELECTAll()
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                DataSet ds = new DataSet();

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicSELECTALL", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

                dsData.Clear();
                sAdapter.Fill(dsData, "Table");

                //Close connection to database

                dbConnection.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }


        //----------------------------------------
        ///<summary>
        /// MethodSELECTByPuzzleNumber
        ///</summary>
        public void MethodSELECTByPuzzleNumber(long varPuzzleNumber)
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                SqlParameter dbParam1;
                DataSet ds = new DataSet();

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicSELECTBYPuzzleNumber", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                dbParam1 = new SqlParameter("@varPuzzleNumber", SqlDbType.BigInt, 8);
                dbParam1.Direction = ParameterDirection.Input;
                dbParam1.Value = varPuzzleNumber;
                dbCommand.Parameters.Add(dbParam1);

                SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

                dsData.Clear();
                sAdapter.Fill(dsData, "Table");

                //Close connection to database

                dbConnection.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodSELECTDistinctPuzzleNumber
        ///</summary>
        public void MethodSELECTDistinctPuzzleNumber()
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                DataSet ds = new DataSet();

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicSELECTDistinctPuzzleNumber", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command


                SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

                dsData.Clear();
                sAdapter.Fill(dsData, "Table");

                //Close connection to database

                dbConnection.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodSELECTByPuzzle
        ///</summary>
        public void MethodSELECTByPuzzle(string varPuzzle)
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                SqlParameter dbParam1;
                DataSet ds = new DataSet();

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicSELECTBYPuzzle", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                dbParam1 = new SqlParameter("@varPuzzle", SqlDbType.VarChar, 100);
                dbParam1.Direction = ParameterDirection.Input;
                dbParam1.Value = varPuzzle;
                dbCommand.Parameters.Add(dbParam1);

                SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

                dsData.Clear();
                sAdapter.Fill(dsData, "Table");

                //Close connection to database

                dbConnection.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodSELECTDistinctPuzzle
        ///</summary>
        public void MethodSELECTDistinctPuzzle()
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                DataSet ds = new DataSet();

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicSELECTDistinctPuzzle", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command


                SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

                dsData.Clear();
                sAdapter.Fill(dsData, "Table");

                //Close connection to database

                dbConnection.Close();
            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodDelete
        ///</summary>
        public void MethodDelete(string varPuzzleNumber, string varPuzzle)
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                SqlParameter dbParam1;
                SqlParameter dbParam2;

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicDELETEBYPuzzleNumberBYPuzzle", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                dbParam1 = new SqlParameter("@varPuzzleNumber", SqlDbType.BigInt, 8);
                dbParam1.Direction = ParameterDirection.Input;
                dbParam1.Value = varPuzzleNumber;
                dbCommand.Parameters.Add(dbParam1);

                dbParam2 = new SqlParameter("@varPuzzle", SqlDbType.VarChar, 100);
                dbParam2.Direction = ParameterDirection.Input;
                dbParam2.Value = varPuzzle;
                dbCommand.Parameters.Add(dbParam2);

                //Execute SqlCommand

                try { dbCommand.ExecuteNonQuery(); }
                catch (Exception e) { LogException(e); }
                dbConnection.Close();

            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodUpdate
        ///</summary>
        public void MethodUpdate(string varPuzzleNumber, string varPuzzle, string varSolution, string varLevel, DateTime varCreated, string varIPaddress, int varVisible, string varComments, string varSolveOrder)
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;

                SqlParameter dbParam1;
                SqlParameter dbParam2;
                SqlParameter dbParam3;
                SqlParameter dbParam4;
                SqlParameter dbParam5;
                SqlParameter dbParam6;
                SqlParameter dbParam7;
                SqlParameter dbParam8;
                SqlParameter dbParam9;

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicUPDATE", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                dbParam1 = new SqlParameter("@varPuzzleNumber", SqlDbType.BigInt, 8);
                dbParam1.Direction = ParameterDirection.Input;
                dbParam1.Value = varPuzzleNumber;
                dbCommand.Parameters.Add(dbParam1);

                dbParam2 = new SqlParameter("@varPuzzle", SqlDbType.VarChar, 100);
                dbParam2.Direction = ParameterDirection.Input;
                dbParam2.Value = varPuzzle;
                dbCommand.Parameters.Add(dbParam2);

                dbParam3 = new SqlParameter("@varSolution", SqlDbType.VarChar, 100);
                dbParam3.Direction = ParameterDirection.Input;
                dbParam3.Value = varSolution;
                dbCommand.Parameters.Add(dbParam3);

                dbParam4 = new SqlParameter("@varLevel", SqlDbType.VarChar, 100);
                dbParam4.Direction = ParameterDirection.Input;
                dbParam4.Value = varLevel;
                dbCommand.Parameters.Add(dbParam4);

                dbParam5 = new SqlParameter("@varCreated", SqlDbType.DateTime, 8);
                dbParam5.Direction = ParameterDirection.Input;
                dbParam5.Value = varCreated;
                dbCommand.Parameters.Add(dbParam5);

                dbParam6 = new SqlParameter("@varIPaddress", SqlDbType.VarChar, 100);
                dbParam6.Direction = ParameterDirection.Input;
                dbParam6.Value = varIPaddress;
                dbCommand.Parameters.Add(dbParam6);

                dbParam7 = new SqlParameter("@varVisible", SqlDbType.Int, 4);
                dbParam7.Direction = ParameterDirection.Input;
                dbParam7.Value = varVisible;
                dbCommand.Parameters.Add(dbParam7);

                dbParam8 = new SqlParameter("@varComments", SqlDbType.VarChar, 5000);
                dbParam8.Direction = ParameterDirection.Input;
                dbParam8.Value = varComments;
                dbCommand.Parameters.Add(dbParam8);

                dbParam9 = new SqlParameter("@varSolveOrder", SqlDbType.VarChar, 200);
                dbParam9.Direction = ParameterDirection.Input;
                dbParam9.Value = varSolveOrder;
                dbCommand.Parameters.Add(dbParam9);

                //Execute SqlCommand

                try { dbCommand.ExecuteNonQuery(); }
                catch (Exception e) { LogException(e); }
                dbConnection.Close();

            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodInsert
        ///</summary>
        public void MethodInsert(string varPuzzleNumber, string varPuzzle, string varSolution, string varLevel, DateTime varCreated, string varIPaddress, int varVisible, string varComments, string varSolveOrder)
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                SqlParameter dbParam1;
                SqlParameter dbParam2;
                SqlParameter dbParam3;
                SqlParameter dbParam4;
                SqlParameter dbParam5;
                SqlParameter dbParam6;
                SqlParameter dbParam7;
                SqlParameter dbParam8;
                SqlParameter dbParam9;

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicINSERT", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                dbParam1 = new SqlParameter("@varPuzzleNumber", SqlDbType.BigInt, 8);
                dbParam1.Direction = ParameterDirection.Input;
                dbParam1.Value = varPuzzleNumber;
                dbCommand.Parameters.Add(dbParam1);

                dbParam2 = new SqlParameter("@varPuzzle", SqlDbType.VarChar, 100);
                dbParam2.Direction = ParameterDirection.Input;
                dbParam2.Value = varPuzzle;
                dbCommand.Parameters.Add(dbParam2);

                dbParam3 = new SqlParameter("@varSolution", SqlDbType.VarChar, 100);
                dbParam3.Direction = ParameterDirection.Input;
                dbParam3.Value = varSolution;
                dbCommand.Parameters.Add(dbParam3);

                dbParam4 = new SqlParameter("@varLevel", SqlDbType.VarChar, 100);
                dbParam4.Direction = ParameterDirection.Input;
                dbParam4.Value = varLevel;
                dbCommand.Parameters.Add(dbParam4);

                dbParam5 = new SqlParameter("@varCreated", SqlDbType.DateTime, 8);
                dbParam5.Direction = ParameterDirection.Input;
                dbParam5.Value = varCreated;
                dbCommand.Parameters.Add(dbParam5);

                dbParam6 = new SqlParameter("@varIPaddress", SqlDbType.VarChar, 100);
                dbParam6.Direction = ParameterDirection.Input;
                dbParam6.Value = varIPaddress;
                dbCommand.Parameters.Add(dbParam6);

                dbParam7 = new SqlParameter("@varVisible", SqlDbType.Int, 4);
                dbParam7.Direction = ParameterDirection.Input;
                dbParam7.Value = varVisible;
                dbCommand.Parameters.Add(dbParam7);

                dbParam8 = new SqlParameter("@varComments", SqlDbType.VarChar, 5000);
                dbParam8.Direction = ParameterDirection.Input;
                dbParam8.Value = varComments;
                dbCommand.Parameters.Add(dbParam8);

                dbParam9 = new SqlParameter("@varSolveOrder", SqlDbType.VarChar, 200);
                dbParam9.Direction = ParameterDirection.Input;
                dbParam9.Value = varSolveOrder;
                dbCommand.Parameters.Add(dbParam9);

                //Execute SqlCommand

                try { dbCommand.ExecuteNonQuery(); }
                catch (Exception e) { LogException(e); }
                dbConnection.Close();

            }
            catch (Exception e)
            {
                LogException(e);
            }
        }

        //----------------------------------------
        ///<summary>
        /// MethodInsertUpdate
        ///</summary>
        public void MethodInsertUpdate(long varPuzzleNumber, string varPuzzle, string varSolution, string varLevel, DateTime varCreated, string varIPaddress, int varVisible, string varComments, string varSolveOrder)
        {
            try
            {
                //Declare memory used

                SqlConnection dbConnection = null;
                SqlCommand dbCommand;
                SqlParameter dbParam1;
                SqlParameter dbParam2;
                SqlParameter dbParam3;
                SqlParameter dbParam4;
                SqlParameter dbParam5;
                SqlParameter dbParam6;
                SqlParameter dbParam7;
                SqlParameter dbParam8;
                SqlParameter dbParam9;

                //Instantiate and open the connection

                dbConnection = GetConnection();

                //Instantiate and initialise command

                dbCommand = new SqlCommand("spClassicINSERTUPDATE", dbConnection);
                dbCommand.CommandType = CommandType.StoredProcedure;
                dbCommand.CommandTimeout = 600;

                //Instantiate, initialize and add parameter to command

                dbParam1 = new SqlParameter("@varPuzzleNumber", SqlDbType.BigInt, 8);
                dbParam1.Direction = ParameterDirection.Input;
                dbParam1.Value = varPuzzleNumber;
                dbCommand.Parameters.Add(dbParam1);

                dbParam2 = new SqlParameter("@varPuzzle", SqlDbType.VarChar, 100);
                dbParam2.Direction = ParameterDirection.Input;
                dbParam2.Value = varPuzzle;
                dbCommand.Parameters.Add(dbParam2);

                dbParam3 = new SqlParameter("@varSolution", SqlDbType.VarChar, 100);
                dbParam3.Direction = ParameterDirection.Input;
                dbParam3.Value = varSolution;
                dbCommand.Parameters.Add(dbParam3);

                dbParam4 = new SqlParameter("@varLevel", SqlDbType.VarChar, 100);
                dbParam4.Direction = ParameterDirection.Input;
                dbParam4.Value = varLevel;
                dbCommand.Parameters.Add(dbParam4);

                dbParam5 = new SqlParameter("@varCreated", SqlDbType.DateTime, 8);
                dbParam5.Direction = ParameterDirection.Input;
                dbParam5.Value = varCreated;
                dbCommand.Parameters.Add(dbParam5);

                dbParam6 = new SqlParameter("@varIPaddress", SqlDbType.VarChar, 100);
                dbParam6.Direction = ParameterDirection.Input;
                dbParam6.Value = varIPaddress;
                dbCommand.Parameters.Add(dbParam6);

                dbParam7 = new SqlParameter("@varVisible", SqlDbType.Int, 4);
                dbParam7.Direction = ParameterDirection.Input;
                dbParam7.Value = varVisible;
                dbCommand.Parameters.Add(dbParam7);

                dbParam8 = new SqlParameter("@varComments", SqlDbType.VarChar, 5000);
                dbParam8.Direction = ParameterDirection.Input;
                dbParam8.Value = varComments;
                dbCommand.Parameters.Add(dbParam8);

                dbParam9 = new SqlParameter("@varSolveOrder", SqlDbType.VarChar, 200);
                dbParam9.Direction = ParameterDirection.Input;
                dbParam9.Value = varSolveOrder;
                dbCommand.Parameters.Add(dbParam9);

                //Execute SqlCommand

                try { dbCommand.ExecuteNonQuery(); }
                catch (Exception e) { LogException(e); }
                dbConnection.Close();

            }
            catch (Exception e)
            {
                LogException(e);
            }
        }
        //----------------------------------------
        ///<summary>
        /// GetConnection
        ///</summary>
        private SqlConnection GetConnection()
        {
            try
            {
                //SqlConnection connection = new SqlConnection("server=72.232.82.146; database=Sudoku2; uid=worker; pwd=worker100");
                SqlConnection connection = new SqlConnection("uid='TestOneUser';pwd='Password10'; DATABASE='SuDoku'; SERVER='174.36.218.228';");
                //SqlConnection connection = new SqlConnection("uid=;pwd=; DATABASE='SuDoku'; SERVER=(local); Integrated Security=SSPI;");
                connection.Open();
                return connection;
            }
            catch (Exception e)
            {
                LogException(e);
                return null;
            }
        }

        ///<summary>
        /// LogException
        ///</summary>
        private void LogException(Exception ex)
        {
            string errMessage = "";
            errMessage = ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace + Environment.NewLine;
            EventLog.WriteEntry("DAL", errMessage, EventLogEntryType.Error);
        }
    }
}


