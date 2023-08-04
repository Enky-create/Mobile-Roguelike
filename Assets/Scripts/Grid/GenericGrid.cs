using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class GenericGrid<T> 
{
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private T[,] gridCells;
    private bool isTesting = true;

    public event EventHandler<OnGridObjectChangeEventArgs> OnGridObjectChange;
    public class OnGridObjectChangeEventArgs:EventArgs
    {
        public int x;
        public int y;
    }
    //for testing
    private TextMesh[,] textMeshForTesting;

    public GenericGrid(int width, int height, float cellSize , Vector3 originPosition, Func<GenericGrid<T>, int, int, T> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        gridCells = new T[width, height];
        textMeshForTesting = new TextMesh[width, height];

        //initializing grid
        for (int x = 0; x < gridCells.GetLength(0); x++) {
            for (int y = 0; y < gridCells.GetLength(1); y++) {
                gridCells[x, y] = createGridObject(this, x, y);
                
            }
        }

        if (isTesting)
        {
            for (int x = 0; x < gridCells.GetLength(0); x++)
            {
                for (int y = 0; y < gridCells.GetLength(1); y++)
                {
                    textMeshForTesting[x, y] = UtilsClass.CreateWorldText(
                        gridCells[x, y]?.ToString(),
                        null,
                        GetWorldPoition(x, y) + new Vector3(cellSize, cellSize) * .5f,
                        5,
                        Color.white,
                        TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPoition(x, y), GetWorldPoition(x + 1, y), Color.white, 1000);
                    Debug.DrawLine(GetWorldPoition(x, y), GetWorldPoition(x, y + 1), Color.white, 1000);
                }
            }
            Debug.DrawLine(GetWorldPoition(0, height), GetWorldPoition(width, height), Color.white, 1000);
            Debug.DrawLine(GetWorldPoition(width, 0), GetWorldPoition(width, height), Color.white, 1000);
            OnGridObjectChange += (object sender, OnGridObjectChangeEventArgs eventArgs) =>
                {
                    textMeshForTesting[eventArgs.x, eventArgs.y].text = gridCells[eventArgs.x, eventArgs.y]?.ToString();
                };
        }
    }
    private Vector3 GetWorldPoition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }
    public void SetObject(int x, int y, T generic)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            gridCells[x, y] = generic;
            OnGridObjectChange?.Invoke(this, new OnGridObjectChangeEventArgs { x = x, y = y });
        }
    }
    public void SetObject(Vector3 worldPosition, T generic)
    {
        GetXY(worldPosition, out int x, out int y);
        SetObject(x, y, generic);
    }
    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition- originPosition).x /cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    public T GetObject(int x, int y)
    {
        return gridCells[x, y]; 
    }
    public T GetObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);
        if (x >= 0 && x <= width && y >= 0 && y <= height)
        {
            return GetObject(x, y);
        }
        else
        {
            return default(T);
        }
    }
    public void TriggerGridObjectChange(int x, int y)
    {
        OnGridObjectChange?.Invoke(this, new OnGridObjectChangeEventArgs {x=x, y=y });
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
}

