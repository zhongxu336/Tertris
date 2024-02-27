using UnityEngine;

public class ShapeMover : MonoBehaviour
{
    public float FallSpeed { get; set; } = 1.0f; // 形状的下落速度，可以在Inspector中调整
    private float lastFallTime; // 上次形状下落的时间
    private float lastMoveTime; // 上次形状移动的时间
    public float MoveInterval = 0.1f; // 按键移动间隔，可以根据需要调整

    void FixedUpdate()
    {
        HandleVerticalMovement();
        HandleHorizontalMovement();
    }

    void HandleVerticalMovement()
    {
        if (Time.time - lastFallTime > FallSpeed)
        {
            // 自动下落
            MoveShape(Vector3.down * GridManager.Instance.cellSize);
            lastFallTime = Time.time;
        }
        else if (Input.GetKey(KeyCode.DownArrow) && Time.time - lastMoveTime > MoveInterval)
        {
            // 按键加速下落
            MoveShape(Vector3.down * GridManager.Instance.cellSize);
            lastMoveTime = Time.time;
        }
    }

    void HandleHorizontalMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection = Vector3.left;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection = Vector3.right;
        }

        if (moveDirection != Vector3.zero && Time.time - lastMoveTime > MoveInterval)
        {
            // 按键左右移动
            MoveShape(moveDirection * GridManager.Instance.cellSize);
            lastMoveTime = Time.time;
        }
    }

    void MoveShape(Vector3 direction)
    {
        transform.position += direction;
        if (!IsValidGridPos())
        {
            transform.position -= direction;
        }
    }

    bool IsValidGridPos()
    {
        foreach (Transform child in transform)
        {
            Vector3Int gridPosition = GridManager.Instance.WorldToGridPosition(child.position)+new Vector3Int(5,10,0);
            if (!GridManager.Instance.IsPositionInsideGrid(gridPosition) || !GridManager.Instance.IsCellEmpty(gridPosition))
            {
                return false;
            }
        }
        return true;
    }
}
