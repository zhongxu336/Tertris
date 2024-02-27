using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

//board类控制整体情况
public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPosition;

    private void Awake()
    {
        //当前游戏对象及其子对象中查找 Tilemap 组件,并将其引用存储在 tilemap 变量中。
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        //调用生成的函数并初始化,索引每个元素并调用函数的初始化
        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }
    }
    public void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        //得到一个随机数，然后把随机数给索引
        int random = UnityEngine.Random.Range(0,this.tetrominoes.Length);
        //什么意思
        TetrominoData data = this.tetrominoes[random];
        this.activePiece.Initialize(this,spawnPosition,data);
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //SetTile 是 Tilemap 类的一个方法，它用于在 Tilemap 上设置或更改瓦片（Tile）。
            this.tilemap.SetTile(tilePosition,piece.data.tile);
        }
    }
}