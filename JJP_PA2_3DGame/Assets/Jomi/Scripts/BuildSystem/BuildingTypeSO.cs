using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingTypeSO : ScriptableObject
{
    
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            case Dir.Up: return Dir.Right; 
            case Dir.Right: return Dir.Down; 
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            default: return dir;
        }
    }

    public enum Dir { Down, Left, Up ,Right}
    
    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width, height;

    public int GetRotationAngle(Dir dir) {
        switch (dir) {
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
            default: return 0;
        }
    }

    public Vector2Int GetRotationOffSet(Dir dir) {
        switch (dir) {
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0, 1);
            case Dir.Up: return new Vector2Int(1, 1);
            case Dir.Right: return new Vector2Int(1, 0);
            default: return new Vector2Int(0, 0);
        }
    }


    public List<Vector2Int> GetGridPosition(Vector2Int offSet, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        switch (dir) {
            case Dir.Down:
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        gridPositionList.Add(offSet + new Vector2Int(x, y));
                break;
            case Dir.Up:
                for (int x = 0; x < width; x++)
                    for (int y = 0; y < height; y++)
                        gridPositionList.Add(offSet - new Vector2Int(x, y));
                break;
            case Dir.Left:
                for (int x = 0; x < height; x++)
                    for (int y = 0; y < width; y++)
                        gridPositionList.Add(offSet - new Vector2Int(x, y));
                break;
            case Dir.Right:
                for (int x = 0; x < height; x++)
                    for (int y = 0; y < width; y++)
                        gridPositionList.Add(offSet + new Vector2Int(x, y));
                break;
        }
        return gridPositionList;
    }
}
