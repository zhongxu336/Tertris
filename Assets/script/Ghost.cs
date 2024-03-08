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
    public Vector3Int[] cells { get; private set; }
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
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + this.position;
            this.tilemap.SetTile(tilePosition,null);
        }
    }

    private void Copy()
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            this.cells[i] = this.trackingPiece.cells[i];
        }

    }

    private void Drop()
    {
        if (Board.isGameOver)
        {
            return;
        }
        Vector3Int position = this.trackingPiece.position;
        //当前行
        int current = position.y;
        int bottom = -this.board.boardSize.y / 2 - 1;
        //如果没有这行代码，我们想要的位置就会被占据，并返回flase
        this.board.Clear(this.trackingPiece);
        //从当前位置的顶部循环到底部
        for (int row = current; row >= bottom; row--)
        {
            position.y = row;
            if (this.board.IsValidPosition(this.trackingPiece, position))
            {
                this.position = position;
            }
            else
            {
                break;
            }
        }
        this.board.Set(this.trackingPiece);
    }
    private void Set()
    {
        if (Board.isGameOver)
        {
            return;
        }
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3Int tilePosition = this.cells[i] + position;
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }

}
