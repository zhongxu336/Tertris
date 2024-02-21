using UnityEngine;

public class ShapeMover : MonoBehaviour
{
   public float FallSpeed { get; set; } // 形状的下落速
    
    private float lastFallTime; // 上次形状下落的时间
    

    void Update()
    {
        // 用户输入，向下加速
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 尝试将形状下移
            MoveShapeDown();
            // 更新最后下落时间，使形状立即响应下一次下落
            lastFallTime = Time.time;
        }
        // 自动下落
        else if (Time.time - lastFallTime > FallSpeed)
        {
            MoveShapeDown();
        }
    }

    void MoveShapeDown()
    {
        // 将形状向下移动
        transform.position += new Vector3(0, -1, 0);

        // 检查是否触底或触碰到其他形状
        if (!IsValidGridPos())
        {
            // 如果移动无效，则将形状回移
            transform.position -= new Vector3(0, -1, 0);

            // TODO: 将形状添加到网格中，这将在网格管理脚本中处理

            // 禁用此脚本，停止形状的下落
            enabled = false;

            // TODO: 通知游戏控制器生成新的形状
        }

        lastFallTime = Time.time;
    }

    bool IsValidGridPos()
    {
        // TODO: 检查新位置是否有效（是否在游戏区域内且没有与现有方块冲突）
        // 这需要与网格管理脚本交互，检查每个方块的新位置是否都是空的
        return true; // 暂时返回true，后续需要实现真正的检查逻辑
    }
}

