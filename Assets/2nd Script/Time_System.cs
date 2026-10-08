using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Time_System : MonoBehaviour
{
	// 游戏时间的操控器，被 各种按钮 相调用（例如开始游戏按钮、暂停按钮……）

	// 使用携程循环触发自己， 在每一个 Tick 调用其他系统。 

	[Header("Tick")]
    public float tickInterval = 1.0f;
    public int tickTimes = 0;

    public bool IsTicking { get; private set; }
    private Coroutine tickCoroutine;
    
    public event Action OnTick; // 外部接口


    public void StartTick() // 系统时间开始流动
    {
        if (IsTicking) return;
        tickCoroutine = StartCoroutine(TickLoop());
        IsTicking = true;
    }
    public void PauseTick() // 系统时间暂停
    {
        if (!IsTicking) return;
        StopCoroutine(tickCoroutine);
        IsTicking = false;
    }

    // 自循环
    private IEnumerator TickLoop()
    {
        while (true)
        {
            tickTimes++;
            OnTick?.Invoke();




            yield return new WaitForSeconds(tickInterval);
        }
    }



    // 可视化时间戳系统

    private readonly DateTime _baseTime = new DateTime(2026, 10, 8, 19, 30, 0); // 起始时间
    public float TickMult = 60f; // 一Tick等于60秒

    public string GetTimeString()
    {
        TimeSpan delta = TimeSpan.FromSeconds(tickTimes * TickMult);
        DateTime target = _baseTime + delta;

        string weekAbbr = "???";
        switch (target.DayOfWeek)
        {
            case DayOfWeek.Monday: weekAbbr = "MON"; break;
            case DayOfWeek.Tuesday: weekAbbr = "TUE"; break;
            case DayOfWeek.Wednesday: weekAbbr = "WED"; break;
            case DayOfWeek.Thursday: weekAbbr = "TUR"; break;
            case DayOfWeek.Friday: weekAbbr = "FRI"; break;
            case DayOfWeek.Saturday: weekAbbr = "SAT"; break;
            case DayOfWeek.Sunday: weekAbbr = "SUN"; break;
            default: weekAbbr = "???"; break;
        }
        return $"{target:yyyy/MM/dd} {weekAbbr} {target:HH:mm}";
    }

}
