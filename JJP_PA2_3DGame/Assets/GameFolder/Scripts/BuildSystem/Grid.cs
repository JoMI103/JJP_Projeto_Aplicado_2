using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jomi.Utils;
using Unity.VisualScripting;
using System;

public class Grid<TGridObject>{

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x; 
        public int y;
    }


    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    //func -->   () => new TGridObject()
    public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> createGridFunction)
    {
        this.width = width; 
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];


        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridFunction();
            }


        if (false) //Debug
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    debugTextArray[x, y] = UtilsCode.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.blue, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.blue, 100f);
                }
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }

        
    }

    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    private void GetXY(Vector3 WorldPosition,out int x,out int y)
    {
        x = Mathf.FloorToInt((WorldPosition - originPosition).x/cellSize);
        y = Mathf.FloorToInt((WorldPosition - originPosition).y/cellSize);
    }


    public void SetGridObject(int x, int y, TGridObject value)
    {
        if (x < 0 || y < 0 || x >= width || y >= height) return;
        gridArray[x,y] = value;
        if(OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x=x,y=y });
    }

    public void TriggerGridObjectChanged(int x, int y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }
    
    public void SetGridObject(Vector3 WorldPosition, TGridObject value)
    {
        int x,y;
        GetXY(WorldPosition,out x,out y);
        SetGridObject(x,y,value);
    }

    public TGridObject GetGridObject(int x,int y) {
        if (x < 0 || y < 0 || x >= width || y >= height) return default(TGridObject);
        return gridArray[x,y];
    }

    public TGridObject GetGridObject(Vector3 WorldPosition)
    {
        int x, y;
        GetXY(WorldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}
