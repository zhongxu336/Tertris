using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//piece类：单独的形状控制，移动，旋转等
public class Piece : MonoBehaviour
{
    public Board board { get; private set; }

    /// <summary>
    /// data是 TetrominoData的类，里面有每个方块对应的I,0,T,J,L,S,Z,以及对应色块
    /// </summary>
    public TetrominoData data { get; private set; }

    //要Tetromino里面具体位置坐标的的位置属性拉过来放大缩小

    /// <summary>
    ///  TetrominoData cell的坐标将传进来
    /// </summary>
    public Vector3Int[] cells { get; private set; }

    //因为 Tilemap 是三位向量的，所以这里声明一个三维向量
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;


    //方块的初始位置，和数据，需要传递对board面板的引用
    /// <summary>
    /// 方块的实例，使其准备好被放置在游戏板上。它确保了方块知道自己属于哪个游戏板（board），它的位置（position），它的类型和形状（通过data），以及具体的单元格位置（this.cells）。这样，方块就可以被正确地显示和操作了
    /// 4个坐标
    /// </summary>
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        //board是游戏板的引用，position是方块应该出现的初始位置，data就是我们通过Random.Range得到的selectedData。通过这几行代码，方块知道了它属于哪个游戏板，它的起始位置在哪里，以及它的形状和单元格配置。
        this.board = board;
        this.position = position;
        this.data = data;
        //每初始化一个方块它都将被赋值为0
        this.rotationIndex = 0;
        //Time.time是当前时间
        //stepTime可能是自动下落的时间
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        //初始化this.cells数组，以便为this.cells数组分配足够的空间来存储data.cells中的所有元素
        //如果TetrominoData的I形状有4个单元格，那么data.cells.Length将为4，this.cells = new Vector3Int[4];就会创建一个能够存储4个Vector3Int元素的数组。
        //因为new Vector3Int[data.cells.Length];中的 data.cells.Length ，根据传入的 TetrominoData data 它就知道对应哪个形状了

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
        //如果是初始形状，位置没有变化，不断的clear和set时，方块在视觉上保持不变
        this.board.Clear(this);
        this.lockTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            //Vector2Int.right是Vector2Int向量左右方向的属性
            Move(Vector2Int.right);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if (Time.time >= this.stepTime)
        {
            Step();
        }
        this.board.Set(this);
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        //Vector2Int.dow 是往下偏移（0.-1）是UNITY自带的方法
        Move(Vector2Int.down);
        //如果锁定的时间超过了指定的时间量，那它将被锁定
        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void HardDrop()
    {
        //while 循环：while 是一个基本的控制流语句，用于重复执行一段代码块，直到条件不再满足。while 循环的基本语法是 while (condition) { /* code to be executed */ }。在这里，条件是调用 Move(Vector2Int.down) 方法的返回值。
        while (Move(Vector2Int.down))
        {
            continue;
        }
        Lock();
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }

    //移动是二维的，而坐标点是3维的
    /// <summary>
    /// 移动的方法，里面有检测piece的位置是否有效的方法
    /// </summary>
    /// <param name="translation"></param>
    /// <returns></returns>
    private bool Move(Vector2Int translation)
    {
        //复制而不是实例化
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        //direction通常是1或-1，分别表示顺时针或逆时针旋转。
        //将当前的旋转索引（this.rotationIndex）与方向（direction）相加，得到一个可能的新旋转索引
        //使用Wrap函数确保这个新索引在0到3的范围内循环（因为索引是从0开始，所以最大值是4，但不包括4）。
        //this关键字用于引用当前实例（或对象）的成员。
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);
        if (!TestWallKicks(this.rotationIndex, direction))
        {
            //撤销之前的旋转索引更新
            this.rotationIndex = originalRotation;
            //以相反的方向应用旋转矩阵来撤销先前对方块形状或位置的更改
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            //因为I和O的中心相对于别的偏移半个单位，因此这里用浮点数
            Vector3 cell = this.cells[i];
            int x, y;
            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    //不管浮点数是多少向上取整到最接近的整数。例如，Mathf.CeilToInt(2.1f)和Mathf.CeilToInt(2.9f)都将返回3。
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) +
                                        (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) +
                                        (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    //这个函数将给定的浮点数四舍五入到最接近的整数。Mathf.RoundToInt(2.5f)将返回3，而Mathf.RoundToInt(2.4f)将返回2。
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) +
                                         (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) +
                                         (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDrection)
    {
        int wallkickIndex = GetWallkickIndex(rotationIndex, rotationDrection);
        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            //[wallkickIndex, i];wallkickIndex是二维数组，i是二维数组里面的索引
            Vector2Int translation = this.data.wallKicks[wallkickIndex, i];
            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    //墙踢索引
    private int GetWallkickIndex(int rotationIndex, int rotationDrection)
    {
        //为每个旋转状态创造唯一的墙踢索引。由于每个旋转状态可能需要两种不同的墙踢测试（一个用于顺时针旋转，一个用于逆时针旋转），所以通过将rotationIndex乘以2，
        //对于每个旋转状态，我们有两个索引：一个用于顺时针旋转（rotationIndex * 2），另一个用于逆时针旋转（rotationIndex * 2 - 1）。
        //因为rotationIndex是4个数，而墙踢是8个数，使用这里*2
        int wallkickIndex = rotationIndex * 2;
        //如果旋转负数墙踢减一
        if (rotationDrection < 0)
        {
            wallkickIndex--;
        }

        return Wrap(wallkickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    /// <summary>
    ///要实现从0，1，2，3再到3，2，1，0的循环要用数学函数绕回
    /// </summary>
    //要实现从0，1，2，3再到3，2，1，0的循环要用数学函数绕回
    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            //%是取余运算符
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}