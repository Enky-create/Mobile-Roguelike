using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestGrid : MonoBehaviour
{
    private GenericGrid<MapObject> grid;
    void Start()
    {
        grid = new GenericGrid<MapObject>(4, 4, 10, new Vector3(-10, -20),(grid,x,y)=>new MapObject(grid, x, y));
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            MapObject gridObject =  grid.GetObject(UtilsClass.GetMouseWorldPosition());
            gridObject?.AddValue(10);
            Debug.Log(gridObject?.ToString());
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetObject(UtilsClass.GetMouseWorldPosition()));
        }
    }
}
public class MapObject
{
    private GenericGrid<MapObject> grid;
    private int x;
    private int y;
    private int value;
    public MapObject(GenericGrid<MapObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }
    public void AddValue(int value)
    {
        this.value += value;
        grid.TriggerGridObjectChange(x, y);
    }
    public override string ToString()
    {
        return value.ToString();
    }
}