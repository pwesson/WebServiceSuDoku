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
	/// Summary description for class DALSuDokuPuzzles.
	/// </summary>
	public class DALSuDokuPuzzles
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
		/// lNumber
		///</summary>
		public long alNumber(int index)
		{
			if (index >= 0 && index < aNumArray)
			{
				return System.Convert.ToInt64(this.dsData.Tables["Table"].Rows[index]["Number"]);
			}
			else
			{
				return 0;
			}
		}

		///<summary>
		/// sType
		///</summary>
		public string asType(int index)
		{
			if (index >= 0 && index < aNumArray)
			{
				return System.Convert.ToString(this.dsData.Tables["Table"].Rows[index]["Type"]);
			}
			else
			{
				return "";
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
		/// SuDokuPuzzles
		///</summary>
		public DALSuDokuPuzzles()
		{
			//Initialisation of variables

			dsData =  new DataSet();
		}

		//----------------------------------------
		///<summary>
		/// MethodSELECTByAllKeys
		///</summary>
		public void MethodSELECTByAllKeys(long varNumber,string varType)
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

				dbCommand = new SqlCommand("spSuDokuPuzzlesSELECTBYNumberBYType", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				dbParam1 = new SqlParameter("@varNumber",SqlDbType.BigInt,8);
				dbParam1.Direction = ParameterDirection.Input;
				dbParam1.Value = varNumber;
				dbCommand.Parameters.Add(dbParam1);

				dbParam2 = new SqlParameter("@varType",SqlDbType.VarChar,50);
				dbParam2.Direction = ParameterDirection.Input;
				dbParam2.Value = varType;
				dbCommand.Parameters.Add(dbParam2);

				SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

				dsData.Clear();
				sAdapter.Fill(dsData,"Table");
				
				//Close connection to database

				dbConnection.Close();
			}
			catch (Exception e)
			{
				LogException (e);
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

				dbCommand = new SqlCommand("spSuDokuPuzzlesSELECTALL", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

				dsData.Clear();
				sAdapter.Fill(dsData,"Table");
				
				//Close connection to database

				dbConnection.Close();
			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodSELECTAll2File
		///</summary>
		public void MethodSELECTAll2File()
		{
			try
			{
				//Declare memory used

				string sLine;
				SqlConnection dbConnection = null;
				SqlCommand dbCommand;
				DataSet ds = new DataSet();

				//Instantiate and open the connection

				dbConnection = GetConnection();

				//Instantiate and initialise command

				dbCommand = new SqlCommand("spSuDokuPuzzlesSELECTALL", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

				dsData.Clear();
				sAdapter.Fill(dsData,"Table");
				DataTable dbTable = dsData.Tables["Table"];
	
				FileStream filStream = new FileStream("g:\\Shared\\Alternative Investments\\Users\\p\\SuDokuPuzzles.txt", FileMode.CreateNew, FileAccess.Write);
				BufferedStream bufStream = new BufferedStream(filStream);
				StreamWriter sWriter = new StreamWriter(bufStream);
	
	
				foreach (DataRow dbDataRow in dbTable.Rows)
				{
					sLine = System.Convert.ToString(dbDataRow["Number"]);
					sLine = sLine + "," + System.Convert.ToString(dbDataRow["Type"]);
					sLine = sLine + "," + System.Convert.ToString(dbDataRow["Puzzle"]);


					sWriter.WriteLine(sLine);
					sWriter.Flush();
				}
				sWriter.Flush();
				sWriter.Close();
				
				//Close connection to database

				dbConnection.Close();
			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodSELECTByNumber
		///</summary>
		public void MethodSELECTByNumber(long varNumber)
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

				dbCommand = new SqlCommand("spSuDokuPuzzlesSELECTBYNumber", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				dbParam1 = new SqlParameter("@varNumber",SqlDbType.BigInt,8);
				dbParam1.Direction = ParameterDirection.Input;
				dbParam1.Value = varNumber;
				dbCommand.Parameters.Add(dbParam1);

				SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

				dsData.Clear();
				sAdapter.Fill(dsData,"Table");
				
				//Close connection to database

				dbConnection.Close();
			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodSELECTDistinctNumber
		///</summary>
		public void MethodSELECTDistinctNumber()
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

				dbCommand = new SqlCommand("spSuDokuPuzzlesSELECTDistinctNumber", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command


				SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

				dsData.Clear();
				sAdapter.Fill(dsData,"Table");
				
				//Close connection to database

				dbConnection.Close();
			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodSELECTByType
		///</summary>
		public void MethodSELECTByType(string varType)
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

				dbCommand = new SqlCommand("spSuDokuPuzzlesSELECTBYType", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				dbParam1 = new SqlParameter("@varType",SqlDbType.VarChar,50);
				dbParam1.Direction = ParameterDirection.Input;
				dbParam1.Value = varType;
				dbCommand.Parameters.Add(dbParam1);

				SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

				dsData.Clear();
				sAdapter.Fill(dsData,"Table");
				
				//Close connection to database

				dbConnection.Close();
			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodSELECTDistinctType
		///</summary>
		public void MethodSELECTDistinctType()
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

				dbCommand = new SqlCommand("spSuDokuPuzzlesSELECTDistinctType", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command


				SqlDataAdapter sAdapter = new SqlDataAdapter(dbCommand);

				dsData.Clear();
				sAdapter.Fill(dsData,"Table");
				
				//Close connection to database

				dbConnection.Close();
			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodDelete
		///</summary>
		public void MethodDelete(long varNumber,string varType)
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

				dbCommand = new SqlCommand("spSuDokuPuzzlesDELETEBYNumberBYType", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				dbParam1 = new SqlParameter("@varNumber",SqlDbType.BigInt,8);
				dbParam1.Direction = ParameterDirection.Input;
				dbParam1.Value = varNumber;
				dbCommand.Parameters.Add(dbParam1);

				dbParam2 = new SqlParameter("@varType",SqlDbType.VarChar,50);
				dbParam2.Direction = ParameterDirection.Input;
				dbParam2.Value = varType;
				dbCommand.Parameters.Add(dbParam2);

				//Execute SqlCommand

				try{ dbCommand.ExecuteNonQuery(); }catch (Exception e){LogException (e);}
				dbConnection.Close();

			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodUpdate
		///</summary>
		public void MethodUpdate(long varNumber, string varType, string varPuzzle)
		{
			try
			{
				//Declare memory used

				SqlConnection dbConnection = null;
				SqlCommand dbCommand;

				SqlParameter dbParam1;
				SqlParameter dbParam2;
				SqlParameter dbParam3;

				//Instantiate and open the connection

				dbConnection = GetConnection();

				//Instantiate and initialise command

				dbCommand = new SqlCommand("spSuDokuPuzzlesUPDATE", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				dbParam1 = new SqlParameter("@varNumber",SqlDbType.BigInt,8);
				dbParam1.Direction = ParameterDirection.Input;
				dbParam1.Value = varNumber;
				dbCommand.Parameters.Add(dbParam1);

				dbParam2 = new SqlParameter("@varType",SqlDbType.VarChar,50);
				dbParam2.Direction = ParameterDirection.Input;
				dbParam2.Value = varType;
				dbCommand.Parameters.Add(dbParam2);

				dbParam3 = new SqlParameter("@varPuzzle",SqlDbType.VarChar,2000);
				dbParam3.Direction = ParameterDirection.Input;
				dbParam3.Value = varPuzzle;
				dbCommand.Parameters.Add(dbParam3);

				//Execute SqlCommand

				try{ dbCommand.ExecuteNonQuery(); }catch (Exception e){LogException (e);}
				dbConnection.Close();

			}
			catch (Exception e)
			{
				LogException (e);
			}
		}
	
		//----------------------------------------
		///<summary>
		/// MethodInsert
		///</summary>
		public void MethodInsert(long varNumber, string varType, string varPuzzle)	
		{
			try
			{
				//Declare memory used

				SqlConnection dbConnection = null;
				SqlCommand dbCommand;
				SqlParameter dbParam1;
				SqlParameter dbParam2;
				SqlParameter dbParam3;
	
				//Instantiate and open the connection

				dbConnection = GetConnection();

				//Instantiate and initialise command

				dbCommand = new SqlCommand("spSuDokuPuzzlesINSERT", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				dbParam1 = new SqlParameter("@varNumber",SqlDbType.BigInt,8);
				dbParam1.Direction = ParameterDirection.Input;
				dbParam1.Value = varNumber;
				dbCommand.Parameters.Add(dbParam1);

				dbParam2 = new SqlParameter("@varType",SqlDbType.VarChar,50);
				dbParam2.Direction = ParameterDirection.Input;
				dbParam2.Value = varType;
				dbCommand.Parameters.Add(dbParam2);

				dbParam3 = new SqlParameter("@varPuzzle",SqlDbType.VarChar,2000);
				dbParam3.Direction = ParameterDirection.Input;
				dbParam3.Value = varPuzzle;
				dbCommand.Parameters.Add(dbParam3);

				//Execute SqlCommand

				try{ dbCommand.ExecuteNonQuery(); }catch (Exception e){LogException (e);}
				dbConnection.Close();

			}
			catch (Exception e)
			{
				LogException (e);
			}
		}

		//----------------------------------------
		///<summary>
		/// MethodInsertUpdate
		///</summary>
		public void MethodInsertUpdate(long varNumber, string varType, string varPuzzle)
		{
			try
			{
				//Declare memory used

				SqlConnection dbConnection = null;
				SqlCommand dbCommand;
				SqlParameter dbParam1;
				SqlParameter dbParam2;
				SqlParameter dbParam3;

				//Instantiate and open the connection

				dbConnection = GetConnection();

				//Instantiate and initialise command

				dbCommand = new SqlCommand("spSuDokuPuzzlesINSERTUPDATE", dbConnection);
				dbCommand.CommandType = CommandType.StoredProcedure;

				//Instantiate, initialize and add parameter to command

				dbParam1 = new SqlParameter("@varNumber",SqlDbType.BigInt,8);
				dbParam1.Direction = ParameterDirection.Input;
				dbParam1.Value = varNumber;
				dbCommand.Parameters.Add(dbParam1);

				dbParam2 = new SqlParameter("@varType",SqlDbType.VarChar,50);
				dbParam2.Direction = ParameterDirection.Input;
				dbParam2.Value = varType;
				dbCommand.Parameters.Add(dbParam2);

				dbParam3 = new SqlParameter("@varPuzzle",SqlDbType.VarChar,2000);
				dbParam3.Direction = ParameterDirection.Input;
				dbParam3.Value = varPuzzle;
				dbCommand.Parameters.Add(dbParam3);

				//Execute SqlCommand

				try{ dbCommand.ExecuteNonQuery(); }catch (Exception e){LogException (e);}
				dbConnection.Close();

			}
			catch (Exception e)
			{
				LogException (e);
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
                SqlConnection connection = new SqlConnection("uid=;pwd=; DATABASE='SuDoku'; SERVER=(local); Integrated Security=SSPI;");

				connection.Open();
				return connection;
			}
			catch (Exception e)
			{
				LogException (e);
				return null;
			}
		}
	
		///<summary>
		/// LogException
		///</summary>
		private void LogException (Exception ex)
		{
			string errMessage = "";
			errMessage = ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace + Environment.NewLine;
			EventLog.WriteEntry("DAL", errMessage, EventLogEntryType.Error);
		}
	}
}



