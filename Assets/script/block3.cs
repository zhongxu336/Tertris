using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public GameObject[] blockPrefab; // 方块的预设
    private List<Vector3[]> shapes = new List<Vector3[]>(); // 存储所有形状的列表
    private GameObject currentShape; // 当前形状的实例

    void Start()
    {
        InitializeShapes();
        GenerateRandomShape();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
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
            new Vector3(0, 1, 0),
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
        Vector3 center = CalculateCenter(selectedShape);//计算形状的几何中心

        foreach (var position in selectedShape)
        {
            //根据形状的几何中心调整方块的位置
            GameObject block = Instantiate(blockPrefab[shapeIndex], currentShape.transform);
            block.transform.localPosition = position - center; // 调整位置
            
            //将形状移动到世界空间的中心点，以确保旋转围绕几何中心进行
            currentShape.transform.position = center;
            
            int prefabIndex = shapeIndex; // 随机选择一个预设的索引
            GameObject prefabToInstantiate = blockPrefab[prefabIndex]; // 获取随机选择的预设
            Instantiate(prefabToInstantiate, position, Quaternion.identity, currentShape.transform);
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

}