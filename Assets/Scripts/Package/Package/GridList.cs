using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridList 
{
    public int length;
    public int width;
    public int gridSize;
    public Vector3 origin;
    public Grid[,] gridList;

    public GridList(int length,int width,int gridSize,Vector2 origin)
    {
        this.length = length;
        this.width = width;
        this.gridSize = gridSize;
        this.origin = origin;
        gridList = new Grid[length,width];
        for (int i = 0; i < gridList.GetLength(0); i++)
        {
            for (int z = 0; z < gridList.GetLength(1); z++)
            {
                gridList[i, z] = new Grid(i, z);
                
                Debug.DrawLine(GetWorldPosition(i,z),GetWorldPosition(i,z+1),Color.red,100);
                Debug.DrawLine(GetWorldPosition(i,z),GetWorldPosition(i+1,z),Color.red,100);
            }
        }
        Debug.DrawLine(GetWorldPosition(length, 0), GetWorldPosition(length, width), Color.red, 100);
        Debug.DrawLine(GetWorldPosition(length, width), GetWorldPosition(0, width), Color.red, 100);
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * gridSize+origin;
    }

    public Vector2Int GetPosXY(Vector3 worldPos)
    {
        int  x = Mathf.FloorToInt((worldPos - origin).x / gridSize);
        int  y = Mathf.FloorToInt((worldPos - origin).y / gridSize);
        return new  Vector2Int(x, y);
    }



}
