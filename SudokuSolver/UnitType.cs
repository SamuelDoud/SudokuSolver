using System;
using System.Collections.Generic;

namespace SudokuSolver
{
    //While units are rows, columns, or blocks, unit types are the n*n collection of these units. It delegates squares to a unit and returns them.

    class UnitType
    {
        public bool isComplete;
        public const int ROW = 0;
        public const int COLUMN = ROW + 1;
        public const int BLOCK = COLUMN + 1;
        private Square[] allSquares;
        private Square[][] groupedSquares;
        private int n, n2, n4, myType;
        public UnitType(Square[] squares, int n, int type)
        {
            this.n = n;
            n2 = n * n;
            n4 = n2 * n2;
            allSquares = squares;
            myType = type;
            groupedSquares = new Square[n2][];
            isComplete = checkCompletion();
            groupSquares();
        }
        public bool checkCompletion()
        {
            bool tempCompletion = true;
            foreach (Square s in allSquares)
            {
                if (s.getValue() == Square.NULL_VALUE)
                {
                    tempCompletion = false;
                    break;
                }
            }
            isComplete = tempCompletion;//using a temp so threading wont mess this up
            return isComplete;
        }
        private void groupSquares()
        {
            int[] currentPositionInGroupArray = new int[n2];
            int tempGrouping;
            for (int i = 0; i < n2; i++)
            {
                groupedSquares[i] = new Square[n2];
            }
            for (int i = 0; i < n4; i++)
            {
                tempGrouping = allSquares[i].getGroupingValue(myType);

                groupedSquares[tempGrouping][currentPositionInGroupArray[tempGrouping]] = allSquares[i];
                currentPositionInGroupArray[tempGrouping]++;
            }
        }
        public Square[] operate()
        {
            //create a unit for each group value, thread this
            Unit[] unitsOfAType = new Unit[n2];
            int index = 0;
            foreach (Square[] sArr in groupedSquares)
            {
                unitsOfAType[index] = new Unit(sArr);
                index++;
            }
            for (int i = 0; i < n2; i++)
            {
                groupedSquares[i] = unitsOfAType[i].operate();
            }
            //wait for all eliminations to complete

            //stitch the all squares back together and return that
            allSquares = stitchJaggedArray(groupedSquares);
            //check completion status
            XWing();
            checkCompletion();//I need to make sure this is properly ordered
            return allSquares;
        }
        /**
            An advanced strategy looking for numbers in a square area
            Done in the unit type class because its search spans multiple types
            must have either exclusitivity in the row or the column... may have to write seperate loops for that
        */
        private void XWing()
        {
            SortSquares();
            //get a count of rows and column with only two occurances of a certain value
            int[,] valuesOnRows = new int[n2, n2];
            int[,] valuesOnColumns = new int[n2, n2];
            int row, column;
            bool[] possibleValues;

            foreach (Square s in allSquares)
            {
                column = s.getColumn();
                row = s.getRow();
                if (s.getValue() == Square.NULL_VALUE)
                {
                    possibleValues = s.getPossibleValues();
                    for (int value = 0; value < n2; value++)
                    {
                        if (possibleValues[value])
                        {
                            valuesOnColumns[column, value]++;
                            valuesOnRows[row, value]++;
                        }
                    }
                }
                else
                {
                    valuesOnColumns[column, s.getValue()] = -1 * n2;
                    valuesOnRows[row, s.getValue()] = -1 * n2;
                }
            }
            //now look for candidates
            XWingRow(valuesOnRows);
            XWingColumn(valuesOnColumns);
        }
        private void XWingRow(int[,] valuesOnRows)
        {
            int firstUnit, lastUnit;
            List<int> possibleUnits;
            List<int> nums;
            for (int value = 0; value < n2; value++)
            {
                possibleUnits = new List<int>();
                firstUnit = 0;
                lastUnit = 0;
                for (int unit = 0; unit < n2; unit++)
                {
                    if (valuesOnRows[unit, value] == 2)
                    {
                        possibleUnits.Add(unit);

                    }
                }
                for (int firstPossibleIndex = 0; firstPossibleIndex < possibleUnits.Count - 1; firstPossibleIndex++)
                {
                    for (int secondPossibleIndex = firstPossibleIndex + 1; secondPossibleIndex < possibleUnits.Count; secondPossibleIndex++)
                    {
                        firstUnit = possibleUnits[firstPossibleIndex];
                        lastUnit = possibleUnits[secondPossibleIndex];

                        //Console.WriteLine("Row " + firstUnit + " and " + lastUnit + " with value " + (value + 1));
                        //first unit and last unit could possibly form an xWing
                        //check if they do!
                        nums = new List<int>();
                        for (int otherColumn = 0; otherColumn < n2; otherColumn++)
                        {
                            if (allSquares[convertRowColumnToPosition(firstUnit, otherColumn)].getValue() == Square.NULL_VALUE && allSquares[convertRowColumnToPosition(lastUnit, otherColumn)].getValue() == Square.NULL_VALUE)
                            {
                                if (allSquares[convertRowColumnToPosition(firstUnit, otherColumn)].getPossibleValues()[value] && allSquares[convertRowColumnToPosition(lastUnit, otherColumn)].getPossibleValues()[value])
                                {
                                    nums.Add(otherColumn);
                                }
                            }
                        }
                        if (nums.Count == 2)
                        {
                            //Console.WriteLine("(" + firstUnit + ", " + nums[0] + ") and (" + lastUnit + ", " + nums[1] + ") value: " + (value + 1));
                            for (int otherRows = 0; otherRows < n2; otherRows++)
                            {
                                if (otherRows != firstUnit && otherRows != lastUnit)
                                {
                                    foreach (int columnFound in nums)
                                    {
                                        allSquares[convertRowColumnToPosition(otherRows, columnFound)].impossibleValue(value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void XWingColumn(int[,] valuesOnColumn)
        {
            int firstUnit, lastUnit;
            List<int> possibleUnits;
            List<int> nums;
            for (int value = 0; value < n2; value++)
            {
                possibleUnits = new List<int>();
                firstUnit = 0;
                lastUnit = 0;
                for (int firstPossibleIndex = 0; firstPossibleIndex < possibleUnits.Count - 1; firstPossibleIndex++)
                {
                    for (int secondPossibleIndex = firstPossibleIndex + 1; secondPossibleIndex < possibleUnits.Count; secondPossibleIndex++)
                    {
                        firstUnit = possibleUnits[firstPossibleIndex];
                        lastUnit = possibleUnits[secondPossibleIndex];

                        //Console.WriteLine("Column " + firstUnit + " and " + lastUnit + " with value " + (value + 1));
                        //first unit and last unit could possibly form an xWing
                        //check if they do!
                        nums = new List<int>();
                        for (int otherRow = 0; otherRow < n2; otherRow++)
                        {
                            if (allSquares[convertRowColumnToPosition(otherRow, firstUnit)].getValue() == Square.NULL_VALUE && allSquares[convertRowColumnToPosition(otherRow, lastUnit)].getValue() == Square.NULL_VALUE)
                            {
                                if (allSquares[convertRowColumnToPosition(otherRow, firstUnit)].getPossibleValues()[value] && allSquares[convertRowColumnToPosition(otherRow, lastUnit)].getPossibleValues()[value])
                                {
                                    nums.Add(otherRow);
                                }
                            }
                        }
                        if (nums.Count == 2)
                        {
                            //Console.WriteLine("(" + firstUnit + ", " + nums[0] + ") and (" + lastUnit + ", " + nums[1] + ") value: " + (value + 1));
                            for (int otherColumns = 0; otherColumns < n2; otherColumns++)
                            {
                                if (otherColumns != firstUnit && otherColumns != lastUnit)
                                {
                                    foreach (int rowFound in nums)
                                    {
                                        allSquares[convertRowColumnToPosition(rowFound, otherColumns)].impossibleValue(value);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Swordfish()
        {
            SortSquares();
            //get a count of rows and column with only two occurances of a certain value
            int[,] valuesOnRows = new int[n2, n2];
            int[,] valuesOnColumns = new int[n2, n2];
            int row, column;
            bool[] possibleValues;

            foreach (Square s in allSquares)
            {
                column = s.getColumn();
                row = s.getRow();
                if (s.getValue() == Square.NULL_VALUE)
                {
                    possibleValues = s.getPossibleValues();
                    for (int value = 0; value < n2; value++)
                    {
                        if (possibleValues[value])
                        {
                            valuesOnColumns[column, value]++;
                            valuesOnRows[row, value]++;
                        }
                    }
                }
                else
                {
                    valuesOnColumns[column, s.getValue()] = -1 * n2;
                    valuesOnRows[row, s.getValue()] = -1 * n2;
                }
            }
            SwordfishRow(valuesOnRows);
        }
        private void SwordfishRow(int [,] valuesOnRows)
        {
            //find a value which appears twice on a row, see if it appears on two other rows
            List<int> candidates;
            int[,] positionArray;
            int eitherOneORZero;
            for (int value = 0; value < n2; value++)
            {//go through all the values
                candidates = new List<int>();
                for (int row = 0; row < n2; row++)
                {
                    if (valuesOnRows[row, value] == 2)
                    {
                        candidates.Add(row);
                    }
                }
                if (candidates.Count > 0)
                {
                    positionArray = new int[n2, 2];
                    for (int row = 0; row < n2; row++)
                    {
                        if (candidates.Contains(row))//has the row been determined to be a candidate?
                        {
                            for (int column = 0; column < n2; column++)//go through each column to find the values on each row
                            {
                                if (allSquares[convertRowColumnToPosition(row, column)].getPossibleValues()[value])
                                {
                                    positionArray[row, 0] = (positionArray[row, 0] == 0) ? eitherOneORZero = 0 : eitherOneORZero = 1;//get the first null element
                                    positionArray[row, eitherOneORZero] = column;//fill the array at the row with the column
                                }
                            }
                        }
                    }

                    //Now search for swordfishes!
                    int remCol1, remCol2;
                    int[,] positionsOfSwordFish;
                    for (int row1 = 0; row1 < n2 - 2; row1++)
                    {

                        if (candidates.Contains(row1))
                        {
                            for (int row2 = row1; row2 < n2 - 1; row2++)
                            {
                                positionsOfSwordFish = new int[3, 2];
                                remCol1 = -1;
                                remCol2 = -1;
                                if (candidates.Contains(row2))
                                {
                                    //check if they have a column that is similar
                                    if (positionArray[row1, 0] == positionArray[row2, 0])
                                    {
                                        positionsOfSwordFish[0, 0] = positionArray[row1, 0];
                                        remCol1 = positionArray[row1, 1];
                                        positionsOfSwordFish[1, 0] = positionArray[row2, 0];
                                        remCol2 = positionArray[row2, 1];
                                    }
                                    if (positionArray[row1, 1] == positionArray[row2, 0])
                                    {
                                        positionsOfSwordFish[0, 0] = positionArray[row1, 1];
                                        remCol1 = positionArray[row1, 0];
                                        positionsOfSwordFish[1, 0] = positionArray[row2, 0];
                                        remCol2 = positionArray[row2, 1];
                                    }
                                    if (positionArray[row1, 0] == positionArray[row2, 1])
                                    {
                                        positionsOfSwordFish[0, 0] = positionArray[row1, 0];
                                        remCol1 = positionArray[row1, 1];
                                        positionsOfSwordFish[1, 0] = positionArray[row2, 1];
                                        remCol2 = positionArray[row2, 0];
                                    }
                                    if (positionArray[row1, 1] == positionArray[row2, 1])
                                    {
                                        positionsOfSwordFish[0, 0] = positionArray[row1, 1];
                                        remCol1 = positionArray[row1, 0];
                                        positionsOfSwordFish[1, 0] = positionArray[row2, 1];
                                        remCol2 = positionArray[row2, 0];
                                    }
                                    if (remCol1 > 0)
                                    {
                                        for (int row3 = row2; row3 < n2; row3++)
                                        {
                                            if (candidates.Contains(row3))
                                            {
                                                if ((positionArray[row3, 0] == remCol1 || positionArray[row3, 1] == remCol1) && (positionArray[row3, 0] == remCol2 || positionArray[row3, 1] == remCol2))
                                                {
                                                    Console.WriteLine("Swordfish on rows" + row1 + ", " + row2 + ", and " + row3);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public int convertRowColumnToPosition(int row, int column)
        {
            return row * n2 + column;
        }
        private Square[] stitchJaggedArray(Square[][] sJaggedArray)
        {
            List<Square> sList = new List<Square>();
            foreach (Square[] sArr in sJaggedArray)
            {
                sList.AddRange(sArr);
            }
            sList.Sort();
            return sList.ToArray();
        }
        private void SortSquares()
        {
            List<Square> squares = new List<Square>();
            foreach (Square s in allSquares)
            {
                squares.Add(s);
            }
            squares.Sort();
        }
    }
}
