using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 10; // 网格的宽度
    public int height = 20; // 网格的高度
    public float cellSize = 1f; // 每个网格单元的大小
    private Transform[,] gridArray; // 存储网格中每个位置的Transform

    void Start()
    {
        gridArray = new Transform[width, height];
    }

    // 将形状添加到网格中
    public bool AddShapeToGrid(GameObject shape, Vector3 position)
    {
        Vector3Int gridPosition = WorldToGridPosition(position);
        if (IsPositionInsideGrid(gridPosition) && IsCellEmpty(gridPosition))
        {
            gridArray[gridPosition.x, gridPosition.y] = shape.transform;
            return true;
        }
        return false;
    }

    // 将世界坐标转换为网格坐标
    public Vector3Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt(worldPosition.x / cellSize);
        int y = Mathf.RoundToInt(worldPosition.y / cellSize);
        return new Vector3Int(x, y, 0);
    }

    // 检查指定的网格坐标是否在网格内
    public bool IsPositionInsideGrid(Vector3Int gridPosition)
    {
        return gridPosition.x >= 0 && gridPosition.x < width && gridPosition.y >= 0 && gridPosition.y < height;
    }

    // 检查网格中的指定单元格是否为空
    public bool IsCellEmpty(Vector3Int gridPosition)
    {
        return gridArray[gridPosition.x, gridPosition.y] == null;
    }
}