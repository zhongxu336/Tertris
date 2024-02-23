using UnityEngine;

public class CameraController : MonoBehaviour
{
     

    void Start()
    {
        AdjustCamera();
    }

    void AdjustCamera()
    {
        // 确保GridManager已经初始化
        if (GridManager.Instance == null) return;

        // 获取网格的宽度和高度
        float width = GridManager.Instance.width;
        float height = GridManager.Instance.height;
        float cellSize = GridManager.Instance.cellSize;

        // 计算摄像机需要的Size值
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float camHeight = height * cellSize / 2.0f;
        float camWidth = width * cellSize / 2.0f;
        
        // 根据屏幕宽高比调整摄像机的orthographicSize
        Camera.main.orthographicSize = Mathf.Max(camHeight, camWidth / screenAspect);

      
        // 更新摄像机位置，使其中心对齐网格的中心
        Camera.main.transform.position = new Vector3(0, 0, Camera.main.transform.position.z);
    }
}