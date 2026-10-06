using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Debug_Controller : MonoBehaviour
{
    // 控制Debug页面的可视化输出

    // 支持 左右大屏幕 输入 / 清空 / 暂停

    public Transform Content0,Content1;
    public TMP_Text DebugTextPrefab;
    private bool Pause0, Pause1;
    
    public void Log(string message) // 默认输入右边大屏幕
    {
        if (Pause1) return;

        TMP_Text newLogItem = Instantiate(DebugTextPrefab, Content1);
        newLogItem.text = $"> {message}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(newLogItem.GetComponent<RectTransform>());
    }
    public void Log_Left(string message) // 输入左边小屏幕
    {
        if (Pause0) return;

        TMP_Text newLogItem = Instantiate(DebugTextPrefab, Content0);
        newLogItem.text = $"> {message}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(newLogItem.GetComponent<RectTransform>());
    }
    public void Log_Clear(int id = 1) // 默认清空右侧，但是输入0清空左侧
    {
        Transform con = Content1;
        if (id == 0) con = Content0;
        for (int i = con.childCount - 1; i >= 0; i--)
        {
            Destroy(con.GetChild(i).gameObject);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(con.GetComponent<RectTransform>());
    }
    public void Log_Pause(int id = 1) // 默认暂停右侧，但是输入0暂停左侧
    {
        if (id == 0) Pause0 = true;
        if (id == 1) Pause1 = true;
    }
    public void Log_Restart(int id = 1)
    {
        if (id == 0) Pause0 = false;
        if (id == 1) Pause1 = false;
    }
}
