using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GridManager gridManager; // 引用GridManager脚本

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        if (gridManager == null) return;

        // 获取网格的宽度和高度
        float width = gridManager.width;
        float height = gridManager.height;
        float cellSize = gridManager.cellSize;

        // 计算摄像机需要的Size值
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHeight = height * cellSize / 2.0f;
        float camWidth = width * cellSize / 2.0f;

        Camera.main.orthographicSize = Mathf.Max(camHeight, camWidth / screenAspect);

        // 设置摄像机的位置，使网格中心位于视野中心
        Camera.main.transform.position = new Vector3(width * cellSize / 2.0f, height * cellSize / 2.0f, Camera.main.transform.position.z);
    }
}