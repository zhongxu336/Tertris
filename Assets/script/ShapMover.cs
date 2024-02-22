using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    public float FallSpeed { get; set; } // 形状的下落速
    private float lastFallTime; // 上次形状下落的时间
    private float lastMoveTime; // 上次形状移动的时间
    public float MoveInterval = 0.1f; // 按键移动间隔，可以根据需要调整

    
    
    void Awake() {
        FallSpeed = 1.0f; // 根据需要调整下落速度
    }
    void FixedUpdate()
    {
        // 用户输入，向下加速
        if (Input.GetKey(KeyCode.DownArrow))
        {
           
            MoveShape(Vector3.down * GridManager.Instance.cellSize);
            lastFallTime = Time.time; // 重置下落时间
        }
        // 自动下落
        else if (Time.time - lastFallTime > FallSpeed)
        {
            MoveShape(Vector3.down * GridManager.Instance.cellSize);
            lastFallTime = Time.time; // 重置下落时间
        }
        
        // 左移动
        if (Input.GetKey(KeyCode.LeftArrow)) {
            MoveShape(Vector3.left * GridManager.Instance.cellSize);
        }

        // 右移动
        if (Input.GetKey(KeyCode.RightArrow)) {
            MoveShape(Vector3.right * GridManager.Instance.cellSize);
        }
    }


    void MoveShape(Vector3 direction) {
        
        transform.position += direction; // 使用Time.fixedDeltaTime确保平滑移动;

        if (!IsValidGridPos()) {
            // 如果移动无效，撤回移动
            transform.position -= direction; // 使用Time.fixedDeltaTime确保平滑移动;
        } else {
            // 更新最后移动时间，使形状响应下一次移动
            lastFallTime = Time.time;
        }
    }

    bool IsValidGridPos() {
        if (GridManager.Instance == null) {
            Debug.LogError("GridManager is not found!");
            return false;
        }
        foreach (Transform child in transform) {
            Vector3Int gridPosition = GridManager.Instance.WorldToGridPosition(child.position);

            // 检查是否在画面范围内
            if (!GridManager.Instance.IsPositionInsideGrid(gridPosition) || !GridManager.Instance.IsCellEmpty(gridPosition)) {
                return false; // 移动无效
            }
            
        }
        return true;// 移动有效
       
    }
   
}

