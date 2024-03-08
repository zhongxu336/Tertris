using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Tilemaps;
using Random = System.Random;

//board类控制整体情况
public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;

    public Vector3Int spawnPosition;

    //Vector2Int 也可以用于表示任何需要一对整数的场景
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public GameObject GameoverUI;
    
    /// <summary>
    /// 第一步：声明一个变量用来声明游戏是否结束 
    /// </summary>
    public static bool isGameOver;

    /// <summary>
    /// 这个方法规定了游戏方格的矩形尺寸，返回一个矩形起始位置和矩形尺寸的值
    /// </summary>
    //这是一个只读属性，只能get
    public RectInt Bounds
    {
        
        get
        {
            //求出矩形左下角的值，再以（10，20）作为矩形的尺寸，这样矩形正好覆盖整个区域
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            //只读属性，创建并返回返回“Rectint"类型的值，表示游戏板的边界
            //RectInt 是unity自带的结构体，用于定义一个矩形的整数位置和大小。一个参数是矩形的起始位置，第二个参数决定其尺寸
            
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
        //当前游戏对象及其子对象中查找 Tilemap 组件,并将其引用存储在 tilemap 变量中。
        this.tilemap = GetComponentInChildren<Tilemap>();
        // 为它 初始化：给它赋值,就是当前的piece实例
        this.activePiece = GetComponentInChildren<Piece>();

        //调用生成的函数并初始化,索引每个元素并调用函数的初始化，把颜色块和方块的形状和坐标关联上
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
        // 第三步，如果游戏已结束，不再生成Piece
        if (isGameOver)
        {
            return;
        }
        
        //得到一个随机数，然后把随机数给索引
        int random = UnityEngine.Random.Range(0, this.tetrominoes.Length);
        //什么意思
        TetrominoData data = this.tetrominoes[random];
        this.activePiece.Initialize(this, spawnPosition, data);//5
        if (IsValidPosition(this.activePiece, this.spawnPosition))//6
        {
            Set(this.activePiece);
        }else
        {
            GameOver();//7
            
        }
    }

    private void GameOver()
    {
        Debug.Log("111111");
        // 第二步：将游戏结束状态变成 true
        isGameOver = true;

        GameoverUI.SetActive(true);
        Invoke("ClearAllTiles",3);
        //...
    }

    private void ClearAllTiles()
    {
        GameoverUI.SetActive(false);
        this.tilemap.ClearAllTiles();
        
        // 第4步： 过三秒，清空界面，重新开始
        // 将 isGameOver 设置成 false （继续游戏）
        isGameOver = false;
        // 重新开始生成 Piece
        SpawnPiece();
    }
    /// <summary>
    /// 将俄罗斯方块形状的位置在tilemap上进行可视化表现。
    /// Piece对象的每个单元格，计算该单元格在Tilemap中的实际位置，并在该位置上设置相应的瓦片，以此来展示形状。
    /// </summary>
    /// <param name="piece"></param>
    public void Set(Piece piece)
    {
        Debug.Log("22222");
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //SetTile 是 Tilemap 类的一个方法，它用于在 Tilemap 上设置或更改瓦片（Tile）。
            //piece.data.tile指定了要设置的瓦片类型，通过Piece对象的data属性（包含了TetrominoData信息，其中包括瓦片信息）来获取的。
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            //SetTile 是 Tilemap 类的一个方法，它用于在 Tilemap 上设置或更改瓦片（Tile）。
            //piece.data.tile指定了要设置的瓦片类型，通过Piece对象的data属性（包含了TetrominoData信息，其中包括瓦片信息）来获取的。
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    /// <summary>
    /// 检测piece的位置是否有效
    /// </summary>
    /// <param name="piece"></param>
    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;
            //bounds.Contains需要2d向量，因此这里用了强制类型转换
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            //检测这个位置是否以已经有tilemap
            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }
/// <summary>
/// 这里传进来的row是检测成功  if (IsLineFull(row))的row 
/// </summary>
/// <param name="row"></param>
    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(position,null);
        }

        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                //获取上方行的位置
                Vector3Int position = new Vector3Int(col, row+1, 0);
                //TileBase，用于表示 2D 地图瓦片（tile）的基类。
                TileBase above = this.tilemap.GetTile(position);
                position = new Vector3Int(col, row, 0);
                //用上面相同的图块在当前位置创建图块
                this.tilemap.SetTile(position,above);
            }
            //消除行的每一行都要往下移动
            row++;
        }
    }
}