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
/// <summary>
/// 每个方块对应的i,0,t,j,l,s,z,以及对应色块
/// 在TetrominoData的实例化通常发生在编辑时，开发者在Inspector中为每种形状指定对应的Tile资源和形状类型。
/// </summary>
//[System.Serializable]属性使得“struct"可以被序列化
[System.Serializable]
public struct TetrominoData
  {
    public Tetromino tetromino;
    public Tile tile;
    /// <summary>
    /// 形状对应的各个坐标
    /// 4个坐标
    /// </summary>
    public Vector2Int[] cells { get; private set; }
    public void Initialize()
    {
        //不会重命吗？
        //关联形状的字母和date里面具体的4个坐标点
        this.cells = Data.Cells[this.tetromino];
    }
    
    
}
