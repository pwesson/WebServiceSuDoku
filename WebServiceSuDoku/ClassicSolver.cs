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
using System.Collections;

namespace MySuDokuSolver
{
	/// <summary>
	/// Summary description for Solver.
	/// </summary>
	public class Solver
	{
		bool bContinue;

		int bFound;
		int nRow;
		int nKeepRow;
		int nCol;
		int nKeepCol;
		int nNum;
		int nUpto;
		int nWork;
		int nTmp;
		int nx;
		int ny;
		int nCount;
		public string DifficultyText;
		
		int [,] answer = new int[10,10];
		int [] aWork = new int[10];

		public int[,,] grid = new int[10,10,10];
		public int[,] FoundA = new int[10,10];
		public int [,] question = new int[10,10];
		public DateTime [,] answerTime = new DateTime[10,10];

		public DataSet dsData;
		public DataTable dsTableSteps;
		public DataTable dsTableGrids;
		
		/// <summary>
		/// Solver
		/// </summary>
		public Solver()
		{	
			dsData = new DataSet();

			//Work on the First table to store Step information

			dsTableSteps = new DataTable("TableSteps");
			
			DataColumn idColumn1 = new DataColumn();
			idColumn1.DataType = System.Type.GetType("System.Int32");
			idColumn1.ColumnName = "Step";
			dsTableSteps.Columns.Add(idColumn1);

			DataColumn idColumn2 = new DataColumn();
			idColumn2.DataType = System.Type.GetType("System.Int32");
			idColumn2.ColumnName = "Row";
			dsTableSteps.Columns.Add(idColumn2);

			DataColumn idColumn3 = new DataColumn();
			idColumn3.DataType = System.Type.GetType("System.Int32");
			idColumn3.ColumnName = "Col";
			dsTableSteps.Columns.Add(idColumn3);

			DataColumn idColumn4 = new DataColumn();
			idColumn4.DataType = System.Type.GetType("System.Int32");
			idColumn4.ColumnName = "Number";
			dsTableSteps.Columns.Add(idColumn4);

			DataColumn idColumn5 = new DataColumn();
			idColumn5.DataType = System.Type.GetType("System.String");
			idColumn5.ColumnName = "Note";
			dsTableSteps.Columns.Add(idColumn5);

			//Work on a second table

			dsTableGrids = new DataTable("TableGrids");

			DataColumn idColumn6 = new DataColumn();
			idColumn6.DataType = System.Type.GetType("System.String");
			idColumn6.ColumnName = "Note";
			dsTableGrids.Columns.Add(idColumn6);

			DataColumn idColumn7 = new DataColumn();
			idColumn7.DataType = System.Type.GetType("System.String");
			idColumn7.ColumnName = "Grid";
			dsTableGrids.Columns.Add(idColumn7);

		}

		public DataSet WebSolve(string PuzzleDescription)
		{
			//Return to User if not a whole question

			if (PuzzleDescription.Length == 0)
			{
                PuzzleDescription = ".6.8...5..87.5......4.93....289..73.4.5...2.8.79..264....23.5......7.46..9...6.8.";
			}
			if (PuzzleDescription.Length == 1)
			{
				PuzzleDescription = "8.64...9..1.83.5.44......7......97......4......12......9......77.8.52.1.3....76.5";
			}
            if (PuzzleDescription.Length == 2)
            {
                PuzzleDescription = "9.........4...3..8.1.67....6.....3...8...5..9.9.83....85....72..6..9...47..1...6.";
            }

			DataRow myRow1 = dsTableGrids.NewRow();
			myRow1["Note"] = "Original";
			myRow1["Grid"] = PuzzleDescription;
			dsTableGrids.Rows.Add(myRow1);

            DifficultyText = "Basic Techniques";

			if (PuzzleDescription.Length != 81)
			{
				DataRow myRow = dsTableSteps.NewRow();
				myRow["Note"] = "Not complete problem [" + PuzzleDescription + "]";
				dsTableSteps.Rows.Add(myRow);
				dsData.Tables.Add(dsTableSteps);
				dsData.Tables.Add(dsTableGrids);
				return dsData;
			}

			//Set up the grid from the question 4..78...2..1.7.23.7.32.214. etc

			question = Setup(PuzzleDescription);

			//Initialise the grid - starting for the point that all numbers are possible
			//grid[ x,y, number = number i.e. it might exist in this cell

			for (int i=1; i <=9; i++)
			{
				for (int j=1; j <=9; j++)
				{
					for (int nNum = 1; nNum <=9; nNum++)
					{
						grid[i,j,nNum] = nNum;
						FoundA[i, j] = 0;
					}
				}
			}

			//Need to pass in the question

			for (int i=1; i<=9; i++)
			{
				for (int j=1; j<=9; j++)
				{
					if (question[i,j] > 0) 
					{
						//We already know a Cell, so must eliminate other possiblilities
						//Eliminate the number in the row

						for (int Col=1; Col<=9; Col++)
						{
							grid[i,Col,question[i,j]] = 0;
						}
						//Eliminate the number in the column

						for (int Row=1; Row<=9; Row++)
						{
							grid[Row,j,question[i,j]] = 0;
						}
						//Eliminate all other numbers in cell

						for (int nNum=1; nNum<=9; nNum++)
						{
							grid[i,j, nNum] = 0;
						}
						//Set the number to the workgrid

						grid[i,j,question[i,j]] = question[i,j];
                        FoundA[i, j] = question[i, j];
					}
				}
			}

			TryToSolve(7);

			string Unsolved = "";
			for (int i=1; i<=9; i++)
			{
				for (int j=1; j<=9; j++)
				{
					int count = 0;
					long val = 0;
					for (int nNum=1; nNum<=9; nNum++)
					{
						if (grid[i,j,nNum] > 0)
						{
							count++;
							val = val + Convert.ToInt64(System.Math.Pow(2,nNum-1));
						}
					}
					if (count >1)
					{
						Unsolved = Unsolved + val.ToString("000");
					}
					else
					{
						Unsolved = Unsolved + "[-]";
					}
				}
			}

			myRow1 = dsTableGrids.NewRow();
			myRow1["Note"] = "UnSolved";
			myRow1["Grid"] = Unsolved;
			dsTableGrids.Rows.Add(myRow1);

			//Save the results

			dsData.Tables.Add(dsTableSteps);
			dsData.Tables.Add(dsTableGrids);
			return dsData;
		}

		//====================================================================
		private int[,] Setup(string PuzzleDescription)
		{
			int k=0;
			int [,] square = new int[10,10];
			for (int i=1; i<=9; i++)
			{
				for (int j=1; j<=9; j++)
				{
					square[i,j] = 0;
					if (PuzzleDescription[k].ToString() == "1") square[i,j] = 1;
					if (PuzzleDescription[k].ToString() == "2") square[i,j] = 2;
					if (PuzzleDescription[k].ToString() == "3") square[i,j] = 3;
					if (PuzzleDescription[k].ToString() == "4") square[i,j] = 4;
					if (PuzzleDescription[k].ToString() == "5") square[i,j] = 5;
					if (PuzzleDescription[k].ToString() == "6") square[i,j] = 6;
					if (PuzzleDescription[k].ToString() == "7") square[i,j] = 7;
					if (PuzzleDescription[k].ToString() == "8") square[i,j] = 8;
					if (PuzzleDescription[k].ToString() == "9") square[i,j] = 9;
					k = k+1;
				}
			}
			return square;
		}


		/// <summary>
		/// UpdateDataTableRow
		/// </summary>
		/// <param name="Step"></param>
		/// <param name="Row"></param>
		/// <param name="Col"></param>
		/// <param name="Number"></param>
		/// <param name="Description"></param>
		public void UpdateDataTableRow(int Step, int Row, int Col, int Number, string Description)
		{
			DataRow myRow = dsTableSteps.NewRow();
			myRow["Step"] = Step;
			myRow["Row"] = Row;
			myRow["Col"] = Col;
			myRow["Number"] = Number;
			myRow["Note"] = Description;
			dsTableSteps.Rows.Add(myRow);
		}


		/// <summary>
		/// TryToSolve
		/// </summary>
		/// <param name="Level"></param>
		private void TryToSolve(int Level)
		{
			//Blank out the answer

			for (nRow = 1; nRow<=9; nRow++)
			{
				for (nCol = 1; nCol<=9; nCol++)
				{
					answer[nRow,nCol] = question[nRow,nCol];
				}
			}

			//Load up the question

			nTmp = 1;
			for (nRow = 1; nRow<=9; nRow++)
			{
				for (nCol = 1; nCol<=9; nCol++)
				{
					if (question[nRow,nCol] > 0)
					{ 
						nTmp = nTmp +1;
					}
				}
			}
			
			//The main loop that searches for the SuDoku solution ////////////////////////////////

            int NewGridCount;
            
            for (int power = 0; power <= Level; power++)
            {
                //Initialise variables

                nUpto = nTmp;

                bContinue = true;

                nWork = 1;

                while (bContinue)
                {
                    // STRATEGY: SINGLE IN BOX //////////////////////

                    for (nx = 1; nx <= 7; nx = nx + 3)
                    {
                        for (ny = 1; ny <= 7; ny = ny + 3)
                        {
                            for (nNum = 1; nNum <= 9; nNum++)
                            {
                                bFound = 0;

                                if (grid[nx, ny, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx, ny + 1, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx, ny + 2, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx + 1, ny, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx + 1, ny + 1, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx + 1, ny + 2, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx + 2, ny, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx + 2, ny + 1, nNum] == nNum) bFound = bFound + 1;
                                if (grid[nx + 2, ny + 2, nNum] == nNum) bFound = bFound + 1;

                                if (bFound == 1)
                                {
                                    if (grid[nx, ny, nNum] == nNum)
                                    {
                                        if (answer[nx, ny] == 0)
                                        {
                                            answer[nx, ny] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx, ny, nNum, "Single in Box");
                                        }
                                    }
                                    if (grid[nx, ny + 1, nNum] == nNum)
                                    {
                                        if (answer[nx, ny + 1] == 0)
                                        {
                                            answer[nx, ny + 1] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx, ny + 1, nNum, "Single in Box");
                                        }
                                    }
                                    if (grid[nx, ny + 2, nNum] == nNum)
                                    {
                                        if (answer[nx, ny + 2] == 0)
                                        {
                                            answer[nx, ny + 2] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx, ny + 2, nNum, "Single in Box");
                                        }
                                    }

                                    //Second row of squares

                                    if (grid[nx + 1, ny, nNum] == nNum)
                                    {
                                        if (answer[nx + 1, ny] == 0)
                                        {
                                            answer[nx + 1, ny] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx + 1, ny, nNum, "Single in Box");
                                        }
                                    }
                                    if (grid[nx + 1, ny + 1, nNum] == nNum)
                                    {
                                        if (answer[nx + 1, ny + 1] == 0)
                                        {
                                            answer[nx + 1, ny + 1] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx + 1, ny + 1, nNum, "Single in Box");
                                        }
                                    }
                                    if (grid[nx + 1, ny + 2, nNum] == nNum)
                                    {
                                        if (answer[nx + 1, ny + 2] == 0)
                                        {
                                            answer[nx + 1, ny + 2] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx + 1, ny + 2, nNum, "Single in Box");
                                        }
                                    }

                                    //Third row of squares

                                    if (grid[nx + 2, ny, nNum] == nNum)
                                    {
                                        if (answer[nx + 2, ny] == 0)
                                        {
                                            answer[nx + 2, ny] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx + 2, ny, nNum, "Single in Box");
                                        }
                                    }
                                    if (grid[nx + 2, ny + 1, nNum] == nNum)
                                    {
                                        if (answer[nx + 2, ny + 1] == 0)
                                        {
                                            answer[nx + 2, ny + 1] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx + 2, ny + 1, nNum, "Single in Box");
                                        }
                                    }
                                    if (grid[nx + 2, ny + 2, nNum] == nNum)
                                    {
                                        if (answer[nx + 2, ny + 2] == 0)
                                        {
                                            answer[nx + 2, ny + 2] = nNum;
                                            nUpto = nUpto + 1;
                                            UpdateDataTableRow(nUpto, nx + 2, ny + 2, nNum, "Single in Box");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // STRATEGY: SINGLE IN ROW //////////////////////////////////////////

                    for (nRow = 1; nRow <= 9; nRow++)
                    {
                        for (nNum = 1; nNum <= 9; nNum++)
                        {
                            nCount = 0;

                            for (nCol = 1; nCol <= 9; nCol++)
                            {
                                if (grid[nRow, nCol, nNum] > 0)
                                {
                                    nCount = nCount + 1;
                                    nKeepCol = nCol;
                                }
                            }

                            if (nCount == 1 && FoundA[nRow, nKeepCol] == 0)
                            {
                                for (nTmp = 1; nTmp <= 9; nTmp++)
                                {
                                    grid[nTmp, nKeepCol, nNum] = 0;
                                }
                                for (nTmp = 1; nTmp <= 9; nTmp++)
                                {
                                    grid[nRow, nTmp, nNum] = 0;
                                }
                                for (nTmp = 1; nTmp <= 9; nTmp++)
                                {
                                    grid[nRow, nKeepCol, nTmp] = 0;
                                }
                                grid[nRow, nKeepCol, nNum] = nNum;
                                FoundA[nRow, nKeepCol] = nNum;
                                if (answer[nRow, nKeepCol] == 0)
                                {
                                    answer[nRow, nKeepCol] = nNum;
                                    UpdateDataTableRow(1, nRow, nKeepCol, nNum, "Single in Row");
                                }
                            }
                        }
                    }

                    //STRATEGY: SINGLE IN COLUMN //////////////////////////////////////////

                    for (nCol = 1; nCol <= 9; nCol++)
                    {
                        for (nNum = 1; nNum <= 9; nNum++)
                        {
                            nCount = 0;

                            for (nRow = 1; nRow <= 9; nRow++)
                            {
                                if (grid[nRow, nCol, nNum] > 0)
                                {
                                    nCount = nCount + 1;
                                    nKeepRow = nRow;
                                }
                            }

                            if (nCount == 1 && FoundA[nKeepRow, nCol] == 0)
                            {
                                for (nTmp = 1; nTmp <= 9; nTmp++)
                                {
                                    grid[nKeepRow, nTmp, nNum] = 0;
                                }
                                for (nTmp = 1; nTmp <= 9; nTmp++)
                                {
                                    grid[nTmp, nCol, nNum] = 0;
                                }
                                for (nTmp = 1; nTmp <= 9; nTmp++)
                                {
                                    grid[nKeepRow, nCol, nTmp] = 0;
                                }
                                grid[nKeepRow, nCol, nNum] = nNum;
                                FoundA[nKeepRow, nCol] = nNum;
                                if (answer[nKeepRow, nCol] == 0)
                                {
                                    answer[nKeepRow, nCol] = nNum;
                                    UpdateDataTableRow(1, nKeepRow, nCol, nNum, "Single in Column");
                                }
                            }
                        }
                    }


                    if (power >= 1)
                    {
                        DifficultyText = "Moderate Techniques";

                        NakedSingle(grid, dsTableSteps, FoundA, answer);
                        PointingPairAndBox(grid, dsTableSteps);
                    }

                    if (power >= 2)
                    {
                        DifficultyText = "Advanced Techniques ";

                        NakedPair(grid, dsTableSteps);
                        NakedTriple(grid, dsTableSteps);
                        NakedQuad(grid, dsTableSteps);
                        HiddenPair(grid, dsTableSteps);
                        HiddenTriple(grid, dsTableSteps);
                    }

                    if (power >= 3)
                    {
                        DifficultyText = "Very Advanced Techniques";

                        UniqueRectangesType1b(grid);
                        XWing2(grid);
                        XWing3(grid);
                        EmptyRectanges(grid, dsTableSteps);
                        Color(grid, dsTableSteps);
                    }

                    if (power >= 4)
                    {
                        //DifficultyText = "Hard";
                        RemotePair(grid);
                        //XWingJellyFish(grid);
                        //XYWing(grid, dsTableSteps);
                        //BugTechnique(grid);
                    }

                    if (power >= 5)
                    {
                        DifficultyText = "Too Hard";
                    }

                    //Check to terminate the While loop

                    nWork = nWork + 1;
                    if (nWork > nUpto) bContinue = false;

                } //End of the While loop

                NewGridCount = GridCounter(grid);
                if (NewGridCount == 81) break;

                //Else increase 'power' of the Solver

            }//power



			for (nx=1; nx<=9; nx++)
			{
				for (ny=1; ny<=9; ny++)
				{
					question[nx,ny] = FoundA[nx, ny];
				}
			}

			string sAnswer = "";
			for (int i=1; i<=9; i++)
			{
				for (int j=1; j<=9; j++)
				{
					if (FoundA[i,j] == 0) sAnswer = sAnswer + ".";
					if (FoundA[i,j] == 1) sAnswer = sAnswer + "1";
					if (FoundA[i,j] == 2) sAnswer = sAnswer + "2";
					if (FoundA[i,j] == 3) sAnswer = sAnswer + "3";
					if (FoundA[i,j] == 4) sAnswer = sAnswer + "4";
					if (FoundA[i,j] == 5) sAnswer = sAnswer + "5";
					if (FoundA[i,j] == 6) sAnswer = sAnswer + "6";
					if (FoundA[i,j] == 7) sAnswer = sAnswer + "7";
					if (FoundA[i,j] == 8) sAnswer = sAnswer + "8";
					if (FoundA[i,j] == 9) sAnswer = sAnswer + "9";
				}
			}

			//The last thing to do is write out the final answer

			DataRow myRow1 = dsTableGrids.NewRow();
			myRow1["Note"] = "Final";
			myRow1["Grid"] = sAnswer;
			dsTableGrids.Rows.Add(myRow1);

            DataRow myRow2 = dsTableGrids.NewRow();
			myRow2["Note"] = "Level";
			myRow2["Grid"] = DifficultyText;
			dsTableGrids.Rows.Add(myRow2);

		}


        /// <summary>
        /// GridCounter
        /// </summary>
        /// <param name="grid"></param>
        /// <returns></returns>
        public int GridCounter(int[, ,] grid)
        {
            int sum = 0;
            for (int i = 1; i <= 9; i++)
            {
                for (int j = 1; j <= 9; j++)
                {
                    for (int num = 1; num <= 9; num++)
                    {
                        if (grid[i, j, num] > 0)
                        {
                            sum = sum + 1;
                        }
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// TheRow
        /// </summary>
        /// <param name="Cell"></param>
        /// <returns></returns>
        public int TheRow(string Cell)
		{
			return System.Convert.ToInt32(Cell.Substring(0,1));
		}

		/// <summary>
		/// TheCol
		/// </summary>
		/// <param name="Cell"></param>
		/// <returns></returns>
		public int TheCol(string Cell)
		{
			return System.Convert.ToInt32(Cell.Substring(1,1));
		}



		/// <summary>
		/// XWing
		/// </summary>
		/// <param name="grid"></param>
		public void XWing2(int[,,] grid)
		{
			int[,] NumberCountPerRow = new int [10,10]; 
			int[,] UniqueSumCountPerRow = new int [10,10];

			int[,] NumberCountPerCol = new int [10,10]; 
			int[,] UniqueSumCountPerCol = new int [10,10];

			//Create lookup table

			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int nRow=1; nRow<=9; nRow++)
				{
					NumberCountPerRow[nRow, nNum] = 0;
					UniqueSumCountPerRow[nRow, nNum] = 0;

					for (int nCol=1; nCol<=9; nCol++)
					{
						if (grid[nRow, nCol, nNum] >0)
						{
							NumberCountPerRow[nRow, nNum] += 1;
							UniqueSumCountPerRow[nRow, nNum] += (int)System.Math.Pow(2,nCol);
						}
					}
				}

				for (int nCol=1; nCol<=9; nCol++)
				{
					NumberCountPerCol[nCol, nNum] = 0;
					UniqueSumCountPerCol[nCol, nNum] = 0;

					for (int nRow=1; nRow<=9; nRow++)
					{
						if (grid[nRow, nCol, nNum] >0)
						{
							NumberCountPerCol[nCol, nNum] += 1;
							UniqueSumCountPerCol[nCol, nNum] += (int)System.Math.Pow(2,nRow);
						}
					}
				}

			}

			//Search for pair occurances
			
			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int i=1; i<=8; i++)
				{
					if (NumberCountPerRow[i,nNum] == 2)
					{	
						for (int j=i+1; j<=9; j++)
						{
							if (NumberCountPerRow[j,nNum] == 2)
							{
								if (UniqueSumCountPerRow[i,nNum] == UniqueSumCountPerRow[j,nNum])
								{
									//Can now eliminate nNum for the columns that nNum appears in the row
									//i and j are the rows in question.
									//Need to find the column the nNum is in

									for (int nCol=1; nCol<=9; nCol++)
									{
										if (grid[i,nCol,nNum]>0)
										{
											//Have found the column of interest
											//Want to eliminate the nNum in this column
											
											for (int nRow=1; nRow<=9; nRow++)
											{
												grid[nRow,nCol,nNum] = 0;
											}
											grid[i, nCol, nNum] = nNum;
											grid[j, nCol, nNum] = nNum;
										}
									}
								}
							}
						}//j
					}
				}//i
			}//nNum


			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int i=1; i<=8; i++)
				{
					if (NumberCountPerCol[i,nNum] == 2)
					{	
						for (int j=i+1; j<=9; j++)
						{
							if (NumberCountPerCol[j,nNum] == 2)
							{
								if (UniqueSumCountPerCol[i,nNum] == UniqueSumCountPerCol[j,nNum])
								{
									//Can now eliminate nNum for the columns that nNum appears in the row
									//i and j are the columns in question.
									//Need to find the Row the nNum is in

									for (int nRow=1; nRow<=9; nRow++)
									{
										if (grid[nRow, i, nNum]>0)
										{
											//Have found the column of interest
											//Want to eliminate the nNum in this column
											
											for (int nCol=1; nCol<=9; nCol++)
											{
												grid[nRow,nCol,nNum] = 0;
											}
											grid[nRow, i, nNum] = nNum;
											grid[nRow, j, nNum] = nNum;
										}
									}
								}
							}
						}//j
					}
				}//i
			}//nNum
		}


		/// <summary>
		/// XWing3
		/// </summary>
		/// <param name="grid"></param>
		public void XWing3(int[,,] grid)
		{
			int[,] NumberCountPerRow = new int [10,10]; 
			int[,] UniqueSumCountPerRow = new int [10,10];

			int[,] NumberCountPerCol = new int [10,10]; 
			int[,] UniqueSumCountPerCol = new int [10,10];

			//Create lookup table

			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int nRow=1; nRow<=9; nRow++)
				{
					NumberCountPerRow[nRow, nNum] = 0;
					UniqueSumCountPerRow[nRow, nNum] = 0;

					for (int nCol=1; nCol<=9; nCol++)
					{
						if (grid[nRow, nCol, nNum] >0)
						{
							NumberCountPerRow[nRow, nNum] += 1;
							UniqueSumCountPerRow[nRow, nNum] += (int)System.Math.Pow(2,nCol);
						}
					}
				}

				for (int nCol=1; nCol<=9; nCol++)
				{
					NumberCountPerCol[nCol, nNum] = 0;
					UniqueSumCountPerCol[nCol, nNum] = 0;

					for (int nRow=1; nRow<=9; nRow++)
					{
						if (grid[nRow, nCol, nNum] >0)
						{
							NumberCountPerCol[nCol, nNum] += 1;
							UniqueSumCountPerCol[nCol, nNum] += (int)System.Math.Pow(2,nRow);
						}
					}
				}

			}

			//Search for pair occurances
			
			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int i=1; i<=7; i++)
				{
					if (NumberCountPerRow[i,nNum] == 3)
					{	
						for (int j=i+1; j<=8; j++)
						{
							if (NumberCountPerRow[j,nNum] == 3)
							{
								for (int k=j+1; k<=9; k++)
								{
									if (NumberCountPerRow[k,nNum] == 3)
									{
										if (UniqueSumCountPerRow[i,nNum] == UniqueSumCountPerRow[j,nNum] &&
											UniqueSumCountPerRow[i,nNum] == UniqueSumCountPerRow[k,nNum])
										{
											//Can now eliminate nNum for the columns that nNum appears in the row
											//i,j and k are the rows in question.
											//Need to find the column the nNum is in

											for (int nCol=1; nCol<=9; nCol++)
											{
												if (grid[i,nCol,nNum]>0)
												{
													//Have found the column of interest
													//Want to eliminate the nNum in this column
											
													for (int nRow=1; nRow<=9; nRow++)
													{
														grid[nRow,nCol,nNum] = 0;
													}
													grid[i, nCol, nNum] = nNum;
													grid[j, nCol, nNum] = nNum;
													grid[k, nCol, nNum] = nNum;
												}
											}
										}
									}
								}
							}
						}//j
					}
				}//i
			}//nNum


			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int i=1; i<=7; i++)
				{
					if (NumberCountPerCol[i,nNum] == 3)
					{	
						for (int j=i+1; j<=8; j++)
						{
							if (NumberCountPerCol[j,nNum] == 3)
							{
								for (int k=j+1; k<=9; k++)
								{
									if (NumberCountPerCol[k, nNum] == 3)
									{

										if (UniqueSumCountPerCol[i,nNum] == UniqueSumCountPerCol[j,nNum] &&
											UniqueSumCountPerCol[i,nNum] == UniqueSumCountPerCol[k,nNum])
										{
											//Can now eliminate nNum for the columns that nNum appears in the row
											//i and j are the columns in question.
											//Need to find the Row the nNum is in

											for (int nRow=1; nRow<=9; nRow++)
											{
												if (grid[nRow, i, nNum]>0)
												{
													//Have found the column of interest
													//Want to eliminate the nNum in this column
											
													for (int nCol=1; nCol<=9; nCol++)
													{
														grid[nRow,nCol,nNum] = 0;
													}
													grid[nRow, i, nNum] = nNum;
													grid[nRow, j, nNum] = nNum;
													grid[nRow, k, nNum] = nNum;
												}
											}
										}
									}
								}
							}
						}//j
					}
				}//i
			}//nNum


		}


		/// <summary>
		/// XWingJellyFish
		/// </summary>
		/// <param name="grid"></param>
		public void XWingJellyFish(int[,,] grid)
		{
			int[,] NumberCountPerRow = new int [10,10]; 
			int[,] UniqueSumCountPerRow = new int [10,10];

			int[,] NumberCountPerCol = new int [10,10]; 
			int[,] UniqueSumCountPerCol = new int [10,10];

			//Create lookup table

			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int nRow=1; nRow<=9; nRow++)
				{
					NumberCountPerRow[nRow, nNum] = 0;
					UniqueSumCountPerRow[nRow, nNum] = 0;

					for (int nCol=1; nCol<=9; nCol++)
					{
						if (grid[nRow, nCol, nNum] >0)
						{
							NumberCountPerRow[nRow, nNum] += 1;
							UniqueSumCountPerRow[nRow, nNum] += (int)System.Math.Pow(2,nCol);
						}
					}
				}

				for (int nCol=1; nCol<=9; nCol++)
				{
					NumberCountPerCol[nCol, nNum] = 0;
					UniqueSumCountPerCol[nCol, nNum] = 0;

					for (int nRow=1; nRow<=9; nRow++)
					{
						if (grid[nRow, nCol, nNum] >0)
						{
							NumberCountPerCol[nCol, nNum] += 1;
							UniqueSumCountPerCol[nCol, nNum] += (int)System.Math.Pow(2,nRow);
						}
					}
				}

			}

			//Search for pair occurances
			
			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int i=1; i<=6; i++)
				{
					if (NumberCountPerRow[i,nNum] == 4)
					{	
						for (int j=i+1; j<=7; j++)
						{
							if (NumberCountPerRow[j,nNum] == 4)
							{
								for (int k=j+1; k<=8; k++)
								{
									if (NumberCountPerRow[k,nNum] == 4)
									{
										for (int m=k+1; m<=9; m++)
										{
											if (NumberCountPerRow[m,nNum] == 4)
											{

												if (UniqueSumCountPerRow[i,nNum] == UniqueSumCountPerRow[j,nNum] &&
													UniqueSumCountPerRow[i,nNum] == UniqueSumCountPerRow[k,nNum] &&
													UniqueSumCountPerRow[i,nNum] == UniqueSumCountPerRow[m,nNum])
												{
													//Can now eliminate nNum for the columns that nNum appears in the row
													//i,j,k and m are the rows in question.
													//Need to find the column the nNum is in

													for (int nCol=1; nCol<=9; nCol++)
													{
														if (grid[i,nCol,nNum]>0)
														{
															//Have found the column of interest
															//Want to eliminate the nNum in this column
											
															for (int nRow=1; nRow<=9; nRow++)
															{
																grid[nRow,nCol,nNum] = 0;
															}
															grid[i, nCol, nNum] = nNum;
															grid[j, nCol, nNum] = nNum;
															grid[k, nCol, nNum] = nNum;
															grid[m, nCol, nNum] = nNum;
														}
													}
												}
											}
										}
									}
								}
							}
						}//j
					}
				}//i
			}//nNum


			for (int nNum=1; nNum<=9; nNum++)
			{
				for (int i=1; i<=6; i++)
				{
					if (NumberCountPerCol[i,nNum] == 4)
					{	
						for (int j=i+1; j<=7; j++)
						{
							if (NumberCountPerCol[j,nNum] == 4)
							{
								for (int k=j+1; k<=8; k++)
								{
									if (NumberCountPerCol[k, nNum] == 4)
									{
										for (int m=k+1; m<=9; m++)
										{
											if (NumberCountPerCol[m, nNum] == 4)
											{

												if (UniqueSumCountPerCol[i,nNum] == UniqueSumCountPerCol[j,nNum] &&
													UniqueSumCountPerCol[i,nNum] == UniqueSumCountPerCol[k,nNum] &&
													UniqueSumCountPerCol[i,nNum] == UniqueSumCountPerCol[m,nNum])
												{
													//Can now eliminate nNum for the columns that nNum appears in the row
													//i and j are the columns in question.
													//Need to find the Row the nNum is in

													for (int nRow=1; nRow<=9; nRow++)
													{
														if (grid[nRow, i, nNum]>0)
														{
															//Have found the column of interest
															//Want to eliminate the nNum in this column
											
															for (int nCol=1; nCol<=9; nCol++)
															{
																grid[nRow,nCol,nNum] = 0;
															}
															grid[nRow, i, nNum] = nNum;
															grid[nRow, j, nNum] = nNum;
															grid[nRow, k, nNum] = nNum;
															grid[nRow, m, nNum] = nNum;
														}
													}
												}
											}
										}
									}
								}
							}
						}//j
					}
				}//i
			}//nNum
		}

		public struct RemotePairInfo
		{
			public int square;
			public int row;
			public int col;
			public int NumberA;
			public int NumberB;
		}

		/// <summary>
		/// RemotePair
		/// </summary>
		/// <param name="grid"></param>
		public void RemotePair(int[,,] grid)
		{
			ArrayList work = new ArrayList();

			for (int nRow=1; nRow<=9; nRow++)
			{
				for (int nCol=1; nCol<=9; nCol++)
				{
					//Working on cell [nRow,nCol]

					int number = 0;
					for (int nNum=1; nNum<=9; nNum++)
					{
						if (grid[nRow, nCol, nNum]>0)
						{
							number++;
						}
					}

					//We have a Naked Pair?
					if (number == 2)
					{
						int square = 0;

						if (nCol<=3            && nRow<=3)            square=1;
						if (nCol>=4 && nCol<=6 && nRow<=3)            square=2;
						if (nCol>=7            && nRow<=3)            square=3;
						if (nCol<=3            && nRow>=4 && nRow<=6) square=4;
						if (nCol>=4 && nCol<=6 && nRow>=4 && nRow<=6) square=5;
						if (nCol>=7            && nRow>=4 && nRow<=6) square=6;
						if (nCol<=3            && nRow>=7)            square=7;
						if (nCol>=4 && nCol<=6 && nRow>=7)            square=8;
						if (nCol>=7            && nRow>=7)            square=9;

						//Add to the arraylist

						RemotePairInfo NewEntry = new RemotePairInfo();
						NewEntry.col = nCol;
						NewEntry.row = nRow;
						NewEntry.square = square;
						NewEntry.NumberA = 0;
						NewEntry.NumberB = 0;

						for (nNum=1; nNum<=9; nNum++)
						{
							if (grid[nRow,nCol,nNum]>0)
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

			if (work.Count ==0) return;

			//Process the work ArrayList

			for (int i=0; i<work.Count-1; i++)
			{
				for (int j=i+1; j<work.Count; j++)
				{
					//Does the i and j share the same square and not same row/column?

					RemotePairInfo EntryI = (RemotePairInfo)work[i];
					RemotePairInfo EntryJ = (RemotePairInfo)work[j];
					
					if (EntryI.square == EntryJ.square && EntryI.row != EntryJ.row && EntryI.col != EntryJ.col 
										&& EntryI.NumberA == EntryJ.NumberA 
										&& EntryI.NumberB == EntryJ.NumberB) 
					{
						int EliminateCellRowA = 0;
						int EliminateCellColA = 0;
						int EliminateCellRowB = 0;
						int EliminateCellColB = 0;

						//Do we have another entry that shares the same row as EntryI?
						for (int k=0; k<work.Count; k++)
						{
							if (k != i && k != j)
							{
								RemotePairInfo EntryK = (RemotePairInfo)work[k];

								if (EntryI.NumberA == EntryK.NumberA && EntryI.NumberB == EntryK.NumberB)
								{
									if (EntryK.row == EntryI.row) 
									{
										EliminateCellColA = EntryK.col;
									}
									if (EntryK.col == EntryI.col)
									{
										EliminateCellRowB = EntryK.row;
									}

									if (EntryK.row == EntryJ.row)
									{
										EliminateCellColB = EntryK.col;
									}
									if (EntryK.col == EntryJ.col)
									{
										EliminateCellRowA = EntryK.row;
									}
								}
							}
						}//k

						//Can we eliminate something?

						if (EliminateCellRowA >0 && EliminateCellColA >0)
						{
							grid[EliminateCellRowA,EliminateCellColA,EntryI.NumberA] = 0;
							grid[EliminateCellRowA,EliminateCellColA,EntryI.NumberB] = 0;
						}

						if (EliminateCellRowB >0 && EliminateCellColB >0)
						{
							grid[EliminateCellRowB, EliminateCellColB,EntryI.NumberA] = 0;
							grid[EliminateCellRowB, EliminateCellColB,EntryI.NumberB] = 0;
						}

					}
				}//j
			}//i
		}


		/// <summary>
		/// UniqueRectangesType1
		/// </summary>
		/// <param name="grid"></param>
		public void UniqueRectangesType1(int[,,] grid)
		{
			int[,] CountNumsInCell = new int [10,10];
			int[,] UniqueSumForPair = new int [10,10];

			//Create a look up table

			for (int nRow=1; nRow<=9; nRow++)
			{
				for (int nCol=1; nCol<=9; nCol++)
				{
					CountNumsInCell[nRow, nCol] = 0;
					UniqueSumForPair[nRow, nCol] = 0;

					for (int nNum=1; nNum<=9; nNum++)
					{
						if (grid[nRow,nCol,nNum] >0)
						{
							CountNumsInCell[nRow,nCol]++;
							UniqueSumForPair[nRow, nCol] += (int)System.Math.Pow(2,nNum);
						}
					}
				}
			}

			//Now want to process the pairs
			//CountNumsInCell[nRow, nCol] == 2 means a pair exists
			//Same pairs exist if share the same UniqueSum value

			//Loop over the different numbers pairs.

			for (int nNum1=1; nNum1<=8; nNum1++)
			{
				for (int nNum2=nNum1+1; nNum2<=9; nNum2++)
				{
					//We have two numbers to consider nNum1 and nNum2
					
					int UniqueSum = (int)System.Math.Pow(2,nNum1) + (int)System.Math.Pow(2,nNum2);
					int FirstCol = 0;
					int SecondCol = 0;

					//Do if only two "pairs" of these numbers exist in a row?

					for (nRow=1; nRow<=9; nRow++)
					{
						int nRowPairs = 0;

						for (nCol=1; nCol<=9; nCol++)
						{
							if (CountNumsInCell[nRow,nCol] == 2 && 
								UniqueSumForPair[nRow, nCol] == UniqueSum)
							{
								nRowPairs++;
								if (FirstCol == 0)
								{
									FirstCol = nCol;
								}
								else
								{
									SecondCol = nCol;
								}
							}
						}//nCol

						if (nRowPairs == 2)
						{
							//So we have found only two Pairs (Num1 & Num2) in the nRow
							//and these pairs appear in the columns FirstCol & SecondCol
							//need to search these columns for two pairs as well

							//Consider FirstCol 

							int nColPairs = 0;
							int FirstRow = 0;
							int SecondRow = 0;

							for (int nRowb=1; nRowb<=9; nRowb++)
							{
								if (CountNumsInCell[nRowb,FirstCol] == 2 && 
									UniqueSumForPair[nRowb, FirstCol] == UniqueSum)
								{
									nColPairs++;
									if (FirstRow == 0)
									{
										FirstRow = nRowb;
									}
									else
									{
										SecondRow = nRowb;
									}
								}
							}//nRowb

							if (nColPairs == 2)
							{
								//So we have also found only two Pairs (Num1 & Num2) in the FirstCol
								//So..... we can eliminate the numbers Num1 & Num2 from
								//the cell the does not equal the UniqueSum

								if (UniqueSumForPair[FirstRow, SecondCol] != UniqueSum)
								{
									//grid[FirstRow, SecondCol, nNum1] = 0;
									//grid[FirstRow, SecondCol, nNum2] = 0;
								}
								if (UniqueSumForPair[SecondRow, SecondCol] != UniqueSum)
								{
									grid[SecondRow, SecondCol, nNum1] = 0;
									grid[SecondRow, SecondCol, nNum2] = 0;
								}
							}

							//Consider SecondCol 

							nColPairs = 0;
							FirstRow = 0;
							SecondRow = 0;

							for (int nRowb=1; nRowb<=9; nRowb++)
							{
								if (CountNumsInCell[nRowb,SecondCol] == 2 && 
									UniqueSumForPair[nRowb, SecondCol] == UniqueSum)
								{
									nColPairs++;
									if (FirstRow == 0)
									{
										FirstRow = nRowb;
									}
									else
									{
										SecondRow = nRowb;
									}
								}
							}//nRowb

							if (nColPairs == 2)
							{
								//So we have also found only two Pairs (Num1 & Num2) in the SecondCol
								//So..... we can eliminate the numbers Num1 & Num2 from
								//the cell in the column FirstCol in both FirstRow & SecondRow

								if (UniqueSumForPair[FirstRow, FirstCol] != UniqueSum)
								{
									//grid[FirstRow, FirstCol, nNum1] = 0;
									//grid[FirstRow, FirstCol, nNum2] = 0;
								}
								if (UniqueSumForPair[SecondRow, FirstCol] != UniqueSum)
								{
									grid[SecondRow, FirstCol, nNum1] = 0;
									grid[SecondRow, FirstCol, nNum2] = 0;
								}
							}


						}

					}//nRow

				}//nNum2

			}//nNum1

		}


		/// <summary>
		/// UniqueRectangesType1b
		/// </summary>
		/// <param name="grid"></param>
		public void UniqueRectangesType1b(int[,,] grid)
		{
			int xNum1;
			int xNum2;
			int nMaxLookup;
			
			int[,] CountNumsInCell = new int [10,10];
			int[,] UniqueSumForPair = new int [10,10];
			int[,] Lookup = new int [100,6];
			
			//Create a look up table
			
			nMaxLookup = 0;
			for (int nRow=1; nRow<=9; nRow++)
			{
				for (int nCol=1; nCol<=9; nCol++)
				{
					xNum1 = xNum2 = 0;

					CountNumsInCell[nRow, nCol] = 0;
					UniqueSumForPair[nRow, nCol] = 0;

					for (int nNum=1; nNum<=9; nNum++)
					{
						if (grid[nRow,nCol,nNum] >0)
						{
							CountNumsInCell[nRow,nCol]++;
							UniqueSumForPair[nRow, nCol] += (int)System.Math.Pow(2,nNum);
							if (xNum1 == 0)
							{
								xNum1 = nNum;
							}
							else
							{
								xNum2 = nNum;
							}
						}
					}
					//If we have two numbers in a Cell - remember them

					if (CountNumsInCell[nRow,nCol] == 2)
					{
						nMaxLookup++;
						Lookup[nMaxLookup,1] = nRow;
						Lookup[nMaxLookup,2] = nCol;
						Lookup[nMaxLookup,3] = UniqueSumForPair[nRow,nCol];
						Lookup[nMaxLookup,4] = xNum1;
						Lookup[nMaxLookup,5] = xNum2;
					}
				}
			}

			//Now want to process the pairs
			//Loop over the pairs. Are there only two "pairs" in a row

			for (int Look1=1; Look1<=nMaxLookup-2; Look1++)
			{
				for (int Look2=Look1+1; Look2<=nMaxLookup-1; Look2++)
				{
					for (int Look3=Look2+1; Look3<=nMaxLookup; Look3++)
					{
						if (Lookup[Look1,1] == Lookup[Look2,1] && Lookup[Look1,3] == Lookup[Look2,3]) //The rows match
						{
							if (Lookup[Look1,2] == Lookup[Look3,2] && Lookup[Look1,3] == Lookup[Look3,3]) //The columns match
							{
								//We have two pairs in a row
								//We have two pairs in a col
								int n=0;
								for (int nNum=1; nNum<=9; nNum++)
								{
									if (grid[Lookup[Look3,1], Lookup[Look2,2], nNum] > 0)
									{
										n++;
									}
								}
								if (n>2)
								{
									grid[Lookup[Look3,1], Lookup[Look2,2], Lookup[Look1,4]] = 0;
									grid[Lookup[Look3,1], Lookup[Look2,2], Lookup[Look1,5]] = 0;
								}
							}
						}

						if (Lookup[Look1,1] == Lookup[Look3,1] && Lookup[Look1,3] == Lookup[Look3,3]) // The rows match
						{
							if (Lookup[Look1,2] == Lookup[Look2,2] && Lookup[Look1,3] == Lookup[Look2,3]) //The columns match
							{
								//We have two pairs in a row
								//We have two pairs in a col
								int n=0;
								for (int nNum=1; nNum<=9; nNum++)
								{
									if (grid[Lookup[Look3,1], Lookup[Look2,2], nNum] > 0)
									{
										n++;
									}
								}
								if (n>2)
								{
									grid[Lookup[Look2,1], Lookup[Look3,2], Lookup[Look1,4]] = 0;
									grid[Lookup[Look2,1], Lookup[Look3,2], Lookup[Look1,5]] = 0;
								}
							}
						}
					}
				}
			}
		}




        public void NakedSingle(int[, ,] grid, DataTable dsTableSteps, int[,] FoundA, int[,] answer)
        {
            MySuDokuSolver.NakedSingle obj = new MySuDokuSolver.NakedSingle();
            obj.Method(grid, dsTableSteps, FoundA, answer);
        }

        public void PointingPairAndBox(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.PointingPairAndBox obj = new MySuDokuSolver.PointingPairAndBox();
            obj.Method(grid, dsTableSteps);
        }

        public void NakedPair(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.NakedPair obj = new MySuDokuSolver.NakedPair();
            obj.Method(grid, dsTableSteps);
        }

        public void HiddenPair(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.HiddenPair obj = new MySuDokuSolver.HiddenPair();
            obj.Method(grid, dsTableSteps);
        }

        public void NakedTriple(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.NakedTriple obj = new MySuDokuSolver.NakedTriple();
            obj.Method(grid, dsTableSteps);
        }

        public void HiddenTriple(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.HiddenTriple obj = new MySuDokuSolver.HiddenTriple();
            obj.Method(grid, dsTableSteps);
        }

        public void NakedQuad(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.NakedQuad obj = new MySuDokuSolver.NakedQuad();
            obj.Method(grid, dsTableSteps);
        }

        public void XYWing(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.XYWing obj = new MySuDokuSolver.XYWing();
            obj.Method(grid, dsTableSteps);
        }

        public void EmptyRectanges(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.EmptyRectangle obj = new MySuDokuSolver.EmptyRectangle();
            obj.Method(grid, dsTableSteps);
        }

        public void Color(int[, ,] grid, DataTable dsTableSteps)
        {
            MySuDokuSolver.Colors obj = new MySuDokuSolver.Colors();
            obj.Method(grid, dsTableSteps);
        }


	}
}
