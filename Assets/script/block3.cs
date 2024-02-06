using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

public class ShapeGenerator : MonoBehaviour
{
    public GameObject blockPrefab; // 方块的预设

    void Start()
    {
        GenerateTShape();
    }

    void GenerateTShape()
    {
        var positions = new Vector3[]
        {
           new Vector3(0, 0, 0), 
           new Vector3(-1, 0, 0),
           new Vector3(1, 0, 0), 
           new Vector3(0, 1, 0), 
        };

        // T形状的中心
        // Vector3 centerPosition = new Vector3(0, 0, 0);

        // 实例化中心方块
        Instantiate(blockPrefab,   new Vector3(0, 0, 0), Quaternion.identity, transform);
        // 实例化T形状的其他部分
        Instantiate(blockPrefab,   new Vector3(-1, 0, 0), Quaternion.identity, transform); // 左侧
        Instantiate(blockPrefab,   new Vector3(1, 0, 0), Quaternion.identity, transform); // 右侧
        Instantiate(blockPrefab,   new Vector3(0, 1, 0), Quaternion.identity, transform); // 顶部
        //随机调用四个旋转中的一个
    }

    void GenerateIShape()
    {
        var positions = new Vector3[]
        {
           new Vector3(0, 0, 0), 
            new Vector3(0, 1, 0),
            new Vector3(0, 2, 0),
            new Vector3(0, 3, 0),
        };
        
        // T形状的中心
        // Vector3 centerPosition = new Vector3(0, 0, 0);

        // 实例化中心方块
        Instantiate(blockPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);

        // 实例化T形状的其他部分
        Instantiate(blockPrefab,  new Vector3(0, 1, 0), Quaternion.identity, transform); // 左侧
        Instantiate(blockPrefab,  new Vector3(0, 2, 0), Quaternion.identity, transform); // 右侧
        Instantiate(blockPrefab,  new Vector3(0, 3, 0), Quaternion.identity, transform); // 顶部
        //随机调用四个旋转中的一个
    }
}