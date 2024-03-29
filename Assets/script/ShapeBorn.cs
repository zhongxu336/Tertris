using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public GameObject[] blockPrefab; // 方块的预设
    private List<Vector3[]> shapes = new List<Vector3[]>(); // 存储所有形状的列表
    private GameObject currentShape; // 当前形状的实例
    public float fallSpeed = 1.0f; 
    //private GridManager gridManager;

    void Start()
    {
        //gridManager = GetComponent<GridManager>();
        InitializeShapes();
        GenerateRandomShape();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) )
        {
            RotateShape();
        }
    }
    void RotateShape()
    {
        if (currentShape != null)
        {
            currentShape.transform.Rotate(0, 0, -90);
        }
    }


    void InitializeShapes()
    {
         // T形状
         shapes.Add(new Vector3[]
         {
             new Vector3(-1, 0, 0),
             new Vector3(0, 0, 0),
             new Vector3(1, 0, 0),
             new Vector3(0, 1, 0)
         });
        
         // I形状
         shapes.Add(new Vector3[]
         {
             new Vector3(0, 1, 0),
             new Vector3(0, 0, 0),
             new Vector3(0, -1, 0),
             new Vector3(0, -2, 0)
         });
         //J形状
         shapes.Add(new Vector3[]
         {
             new Vector3(-1, 0, 0),
             new Vector3(0, 0, 0),
             new Vector3(1, 0, 0),
             new Vector3(-1, -1, 0)
         });
        //L形状
        shapes.Add(new Vector3[]
        {
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0)
        });
        //  0形状
        shapes.Add(new Vector3[]
        {
            new Vector3(-0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(0.5f, -0.5f, 0)
        });
        
        //  s形状
        shapes.Add(new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(-1, 1, 0),
            new Vector3(0, 1, 0)
        });
        //  z形状
        shapes.Add(new Vector3[]
        {
            new Vector3(-1, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        });
    }

    void GenerateRandomShape()
    {
        int shapeIndex = Random.Range(0, shapes.Count); // 随机选择一个形状
        Vector3[] selectedShape = shapes[shapeIndex]; // 获取选中形状的坐标数组

        if (currentShape != null)
        {
            Destroy(currentShape); // 销毁当前形状（如果有）
        }

        currentShape = new GameObject("Shape"); // 创建一个新的形状对象，用作所有方块的父对象
        
        Vector3 center = CalculateCenter(selectedShape); // 计算形状的几何中心

        // 为每个方块位置实例化预制体，并设置为当前形状的子对象
        foreach (var position in selectedShape)
        {
            GameObject prefabToInstantiate = blockPrefab[shapeIndex]; // 让颜色块在获取随机选择的预设上
            GameObject block = Instantiate(prefabToInstantiate, currentShape.transform);
            block.transform.localPosition = position - center; // 调整位置
        }
        //附加ShapeMover组件
        ShapeMover mover = currentShape.AddComponent<ShapeMover>();
        mover.FallSpeed = fallSpeed; // 使用Inspector中设置的值
        // 获取GridManager引用
        
        // 计算形状应该出现的位置
        float startX = 0;
        float startY = GridManager.Instance.height/2.0f - 1 ;// 网格高度的一半减去一定值，确保从顶部开始但稍微向下一点
        //  设置形状出现的初始位置
        currentShape.transform.position = new Vector3(startX, startY, 0);
    }
        Vector3 CalculateCenter(Vector3[] shapePositions)
        {
            Vector3 sum = Vector3.zero;
            foreach (var pos in shapePositions)
            {
                sum += pos;
            }
            return sum / shapePositions.Length; // 返回平均位置作为中心
        }
    }
    