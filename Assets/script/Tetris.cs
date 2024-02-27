using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
}
//把这个序列化，以便在board中显示。这样就可以一个数组对应两个选项
[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;
    /// <summary>
    /// 形状对应的各个坐标
    /// </summary>
    public Vector2Int[] cells { get; private set; }

    public void Initialize()
    {
        //不会重命吗？
        //关联形状的字母和date里面具体的4个坐标点
        this.cells = Data.Cells[this.tetromino];
    }
    
    
}
