using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.EventSystems;

//piece类：单独的形状控制，移动，旋转等
public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }

    //要Tetromino里面具体位置坐标的的位置属性拉过来放大缩小
    public Vector3Int[] cells { get; private set; }

    //因为 Tilemap 是三位向量的，所以这里声明一个三维向量
    public Vector3Int position { get; private set; }

    //方块的初始位置，和数据，需要传递对board面板的引用
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }else if (Input.GetKeyDown(KeyCode.D))
        {
            //Vector2Int.right是Vector2Int向量左右方向的属性
            Move(Vector2Int.right);
        }
    }

    private void Move(Vector2Int translation)
    {
        Vector3Int newPosion = this.position;
        newPosion.x += translation.x;
        newPosion.y += translation.y;
    }
}
