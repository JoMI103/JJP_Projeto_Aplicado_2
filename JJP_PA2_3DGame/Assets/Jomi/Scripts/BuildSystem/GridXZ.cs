using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jomi.Utils;
using Unity.VisualScripting;
using System;


public class GridXZ<TGridObject>
{

    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int z;
    }


    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    //func -->   () => new TGridObject()
    public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject> , int, int, TGridObject> createGridFunction)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];


        for (int x = 0; x < gridArray.GetLength(0); x++)
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                gridArray[x, z] = createGridFunction(this, x, z);
            }


        if (true) //Debug
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];

            for (int x = 0; x < gridArray.GetLength(0); x++)
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    debugTextArray[x, z] = UtilsCode.CreateWorldText(
                        gridArray[x, z]?.ToString(), 
                        null, 
                        GetWorldPosition(x, z) + new Vector3(cellSize,0, cellSize) * .5f, 
                        5, 
                        Color.red, 
                        TextAnchor.MiddleCenter,
                        TextAlignment.Left,
                        0,
                        new Vector3(90,0,0));

                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.blue, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.blue, 100f);
                }
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.blue, 100f);
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.blue, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>
            {
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
            };
        }


    }

    public float GetCellSize() { return cellSize; }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * cellSize + originPosition;
    }

    public void GetXZ(Vector3 WorldPosition, out int x, out int z)
    {
        x = Mathf.FloorToInt((WorldPosition - originPosition).x / cellSize);
       z = Mathf.FloorToInt((WorldPosition - originPosition).z / cellSize);
    }

    public Vector3 SnapWorldPositionToGrid(Vector3 pos)
    {
        GetXZ(pos, out int x, out int z);
        return GetWorldPosition(x, z);   
    }


    public void SetGridObject(int x, int z, TGridObject value)
    {
        if (x < 0 || z < 0 || x >= width || z >= height) return;
        gridArray[x, z] = value;
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
    }

    public void TriggerGridObjectChanged(int x, int z)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, z = z });
    }

    public void SetGridObject(Vector3 WorldPosition, TGridObject value)
    {
        int x, z;
        GetXZ(WorldPosition, out x, out z);
        SetGridObject(x, z, value);
    }

    public TGridObject GetGridObject(int x, int z)
    {
        if (x < 0 || z < 0 || x >= width || z >= height) return default(TGridObject);
        return gridArray[x, z];
    }

    public TGridObject GetGridObject(Vector3 WorldPosition)
    {
        int x, z;
        GetXZ(WorldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, width - 1),
            Mathf.Clamp(gridPosition.y, 0, height - 1)
        );
    }
}




///* 
//    ------------------- Code Monkey -------------------

//    Thank you for downloading this package
//    I hope you find it useful in your projects
//    If you have any questions let me know
//    Cheers!

//               unitycodemonkey.com
//    --------------------------------------------------
// */

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using CodeMonkey.Utils;

//public class GridXZ<TGridObject>
//{

//    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
//    public class OnGridObjectChangedEventArgs : EventArgs
//    {
//        public int x;
//        public int z;
//    }

//    private int width;
//    private int height;
//    private float cellSize;
//    private Vector3 originPosition;
//    private TGridObject[,] gridArray;

//    public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject)
//    {
//        this.width = width;
//        this.height = height;
//        this.cellSize = cellSize;
//        this.originPosition = originPosition;

//        gridArray = new TGridObject[width, height];

//        for (int x = 0; x < gridArray.GetLength(0); x++)
//        {
//            for (int z = 0; z < gridArray.GetLength(1); z++)
//            {
//                gridArray[x, z] = createGridObject(this, x, z);
//            }
//        }

//        bool showDebug = true;
//        if (showDebug)
//        {
//            TextMesh[,] debugTextArray = new TextMesh[width, height];

//            for (int x = 0; x < gridArray.GetLength(0); x++)
//            {
//                for (int z = 0; z < gridArray.GetLength(1); z++)
//                {
//                    debugTextArray[x, z] = UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSize, 0, cellSize) * .5f, 15, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
//                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
//                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
//                }
//            }
//            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
//            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

//            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
//                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString();
//            };
//        }
//    }

//    public int GetWidth()
//    {
//        return width;
//    }

//    public int GetHeight()
//    {
//        return height;
//    }

//    public float GetCellSize()
//    {
//        return cellSize;
//    }

//    public Vector3 GetWorldPosition(int x, int z)
//    {
//        return new Vector3(x, 0, z) * cellSize + originPosition;
//    }

//    public void GetXZ(Vector3 worldPosition, out int x, out int z)
//    {
//        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
//        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
//    }

//    public void SetGridObject(int x, int z, TGridObject value)
//    {
//        if (x >= 0 && z >= 0 && x < width && z < height)
//        {
//            gridArray[x, z] = value;
//            TriggerGridObjectChanged(x, z);
//        }
//    }

//    public void TriggerGridObjectChanged(int x, int z)
//    {
//        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
//    }

//    public void SetGridObject(Vector3 worldPosition, TGridObject value)
//    {
//        GetXZ(worldPosition, out int x, out int z);
//        SetGridObject(x, z, value);
//    }

//    public TGridObject GetGridObject(int x, int z)
//    {
//        if (x >= 0 && z >= 0 && x < width && z < height)
//        {
//            return gridArray[x, z];
//        }
//        else
//        {
//            return default(TGridObject);
//        }
//    }

//    public TGridObject GetGridObject(Vector3 worldPosition)
//    {
//        int x, z;
//        GetXZ(worldPosition, out x, out z);
//        return GetGridObject(x, z);
//    }

//    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
//    {
//        return new Vector2Int(
//            Mathf.Clamp(gridPosition.x, 0, width - 1),
//            Mathf.Clamp(gridPosition.y, 0, height - 1)
//        );
//    }

//}
