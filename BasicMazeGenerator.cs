using UnityEngine;
using System.Collections;

public abstract class BasicMazeGenerator {
	public int RowCount => _rows;
	public int ColumnCount => _columns;

	private int _rows;
	private int _columns;
	private Cell[,] _maze;

	public BasicMazeGenerator(int rows, int columns){

		_rows = Mathf.Abs(rows);
		_columns = Mathf.Abs(columns);

		if (_rows == 0)
			_rows = 1;
		if (_columns == 0)
			_columns = 1;
		
		_maze = new Cell[rows,columns];

		for (int row = 0; row < rows; row++) 
		{
			for(int column = 0; column < columns; column++)
			{
				_maze[row,column] = new Cell();
			}
		}
	}

	public abstract void GenerateMaze();

	public Cell GetMazeCell(int row, int column)
	{
		if (row >= 0 && column >= 0 && row < _rows && column < _columns) 
		{
			return _maze[row,column];
		}
		else
		{
			Debug.Log(row+" "+column);
			throw new System.ArgumentOutOfRangeException();
		}
	}

	protected void SetMazeCell(int row, int column, Cell cell)
	{
		if (row >= 0 && column >= 0 && row < _rows && column < _columns) 
		{
			_maze[row,column] = cell;
		}
		else
		{
			throw new System.ArgumentOutOfRangeException();
		}
	}
}
