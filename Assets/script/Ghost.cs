using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ghost : MonoBehaviour
{
    public Tile tile;
    public Board board;
    public Piece trackingPiece;
    
    //private set 表示在类的内部可以对 position 进行赋值，但在类的外部只能进行读取操作（get）
    public Tilemap tilemap { get; private set; }
    public Vector3Int[]cells { get; private set; }
    public Vector3Int position { get; private set; }

    private void Awake()
    {
        //在当前游戏对象的子物体中查找并获取 Tilemap 组件，并将其赋值给 tilemap 变量。
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.cells = new Vector3Int[4];
    }

    /// <summary>
    /// LateUpdate() 方法在所有的 Update 循环之后被调用，通常用于处理那些需要在所有 Update 循环结束后进行的操作。
    /// </summary>
    private void LateUpdate()
    {
        Clear();
        Copy();
        Drop();
        Set();
    }

    private void Clear()
    {
        
    }

    private void Copy()
    {
        
    }

    private void Drop()
    {
        
    }

    private void Set()
    {
        
    }
    
}
