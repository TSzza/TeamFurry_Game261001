using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 订阅 OnTick 并且在每一次Tick时获取 GetTimeString ，并修改自身TMP 的Text

public class ui_TimeTable : MonoBehaviour
{
    private Time_System timeSystem;
    private TMP_Text timeText;
    public bool subscribeTick = false;

    private void Start()
    {
        timeSystem = Root.Instance.time_System;
        timeText = GetComponent<TMP_Text>();
        SubscribeTick();
    }
    
    private void OnEnable()
    {
        if (!subscribeTick)
            SubscribeTick();
    }
    private void SubscribeTick()
    {
        if (timeSystem != null)
        {
            //Debug.Log("已订阅");
            timeSystem.OnTick += OnTimeTick;
            subscribeTick = true;
        }
        //else Debug.Log("timeSystem订阅失败：null");
    }

    private void OnDisable()
    {
        if (timeSystem != null)
        {
            timeSystem.OnTick -= OnTimeTick;
            subscribeTick = false;
        }
    }

    // 每Tick执行，更新UI文字
    private void OnTimeTick()
    {
        if (timeText == null) return;
        string timeStr = timeSystem.GetTimeString();
        timeText.text = timeStr;
    }
}