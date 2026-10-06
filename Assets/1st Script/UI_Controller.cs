using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public class CameraEntry
{
    public int cameraId;       // 编号：0,1,2,9
    public Camera camera;
}
public class UI_Controller : MonoBehaviour
{
	// 唯一单例、统筹各UI小代码的请求，统一实现部分复杂请求（比如说切换界面相关的相机操作）

	// 切换界面：在此处发起请求，更换当前的显像摄像机； 开闭组件：由动画UI脚本自行处理； 数值控件：各UI自行处理
    

    // 摄像机相关
    public List<CameraEntry> cameraEntries;
    private Dictionary<int, Camera> cameraDict;
    private int currentActiveCameraID;

    public int DefaultInitCamera = 0;
	public void Init_UI()
    {

        SwitchCamera(DefaultInitCamera);




    }

	public bool SwitchCamera(int targetId)
    {
        if (currentActiveCameraID == targetId)
        {
            Debug.LogWarning($"相机 {targetId} 重复切换");
            return false;
        }
        if (!cameraDict.TryGetValue(targetId, out Camera targetCam))
        {
            Debug.LogError($"相机 {targetId} 不存在");
            return false;
        }

        // 关闭所有相机
        foreach (var cam in cameraDict.Values)
        {
            cam.enabled = false;
        }

        // 激活目标相机
        targetCam.enabled = true;
        currentActiveCameraID = targetId;

        return true;
    }
}
