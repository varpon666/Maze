using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeGenerator : BasicMazeGenerator {

	private struct CellToVisit
	{
		public int Row;
		public int Column;
		public Direction MoveMade;

		public CellToVisit(int row, int column, Direction move)
		{
			Row = row;
			Column = column;
			MoveMade = move;
		}

		public override string ToString ()
		{
			return string.Format ("[MazeCell {0} {1}]",Row,Column);
		}
	}

	List<CellToVisit> _cellsToVisit = new List<CellToVisit>();

	[SerializeField] private ObstacleSpawn _obstacleSpawn;

	public MazeGenerator(int row, int column):base(row,column)
	{

	}

	public override void GenerateMaze ()
	{
		Direction[] movesAvailable = new Direction[4];

		int movesAvailableCount = 0;

		_cellsToVisit.Add (new CellToVisit (Random.Range (0, RowCount), Random.Range (0, ColumnCount),Direction.Start));
		
		while (_cellsToVisit.Count > 0) 
		{
			movesAvailableCount = 0;

			CellToVisit cellToVisit = _cellsToVisit[GetCellInRange(_cellsToVisit.Count-1)];
			
			//check move right
			if(cellToVisit.Column+1 < ColumnCount && !GetMazeCell(cellToVisit.Row,cellToVisit.Column+1).IsVisited && !IsCellInList(cellToVisit.Row,cellToVisit.Column+1))
			{
				movesAvailable[movesAvailableCount] = Direction.Right;
				movesAvailableCount++;
			}
			else if(!GetMazeCell(cellToVisit.Row,cellToVisit.Column).IsVisited && cellToVisit.MoveMade != Direction.Left){
				
				GetMazeCell(cellToVisit.Row,cellToVisit.Column).WallRight = true;
				
				if(cellToVisit.Column+1 < ColumnCount)
				{
					GetMazeCell(cellToVisit.Row,cellToVisit.Column+1).WallLeft = true;
				}
			}
			//check move forward
			if(cellToVisit.Row+1 < RowCount && !GetMazeCell(cellToVisit.Row+1,cellToVisit.Column).IsVisited && !IsCellInList(cellToVisit.Row+1,cellToVisit.Column))
			{
				movesAvailable[movesAvailableCount] = Direction.Front;
				movesAvailableCount++;
			}
			else if(!GetMazeCell(cellToVisit.Row,cellToVisit.Column).IsVisited && cellToVisit.MoveMade != Direction.Back)
			{
				GetMazeCell(cellToVisit.Row,cellToVisit.Column).WallFront = true;
				
				if(cellToVisit.Row+1 < RowCount)
				{
					GetMazeCell(cellToVisit.Row+1,cellToVisit.Column).WallBack = true;
				}
			}
			//check move left
			if(cellToVisit.Column > 0 && cellToVisit.Column-1 >= 0 && !GetMazeCell(cellToVisit.Row,cellToVisit.Column-1).IsVisited && !IsCellInList(cellToVisit.Row,cellToVisit.Column-1))
			{
				movesAvailable[movesAvailableCount] = Direction.Left;
				movesAvailableCount++;
			}
			else if(!GetMazeCell(cellToVisit.Row,cellToVisit.Column).IsVisited && cellToVisit.MoveMade != Direction.Right)
			{
				GetMazeCell(cellToVisit.Row,cellToVisit.Column).WallLeft = true;
				
				if(cellToVisit.Column > 0 && cellToVisit.Column-1 >= 0){
					GetMazeCell(cellToVisit.Row,cellToVisit.Column-1).WallRight = true;
				}
			}
			//check move backward
			if(cellToVisit.Row > 0 && cellToVisit.Row-1 >= 0 && !GetMazeCell(cellToVisit.Row-1,cellToVisit.Column).IsVisited && !IsCellInList(cellToVisit.Row-1,cellToVisit.Column))
			{
				movesAvailable[movesAvailableCount] = Direction.Back;
				movesAvailableCount++;
			}
			else if(!GetMazeCell(cellToVisit.Row,cellToVisit.Column).IsVisited && cellToVisit.MoveMade != Direction.Front)
			{
				GetMazeCell(cellToVisit.Row,cellToVisit.Column).WallBack = true;

				if(cellToVisit.Row > 0 && cellToVisit.Row-1 >= 0){
					GetMazeCell(cellToVisit.Row-1,cellToVisit.Column).WallFront = true;
				}
			}

			GetMazeCell(cellToVisit.Row,cellToVisit.Column).IsVisited = true;
			
			if(movesAvailableCount > 0){

				switch(movesAvailable[Random.Range(0,movesAvailableCount)])
				{
				case Direction.Start:
					break;
				case Direction.Right:
					_cellsToVisit.Add(new CellToVisit(cellToVisit.Row,cellToVisit.Column+1,Direction.Right));
					break;
				case Direction.Front:
					_cellsToVisit.Add(new CellToVisit(cellToVisit.Row+1,cellToVisit.Column,Direction.Front));
					break;
				case Direction.Left:
					_cellsToVisit.Add(new CellToVisit(cellToVisit.Row,cellToVisit.Column-1,Direction.Left));
					break;
				case Direction.Back:
					_cellsToVisit.Add(new CellToVisit(cellToVisit.Row-1,cellToVisit.Column,Direction.Back));
					break;
				}
			}
			else
			{
				_cellsToVisit.Remove(cellToVisit);
			}
		}
	}

	private bool IsCellInList(int row, int column)
	{
		return _cellsToVisit.FindIndex ((other) => other.Row == row && other.Column == column) >= 0;
	}

	protected int GetCellInRange(int max)
	{
		return Random.Range(0, max + 1);
	}
}
