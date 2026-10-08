using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    // 用于联系各大控制器，拥有唯一单例
    // 用于触发全游戏的初始化
    
    //

    public Data_Controller data_Controller;
    public System_Controller system_Controller;
    public Audio_Controller audio_Controller;
    public Debug_Controller debug_Controller;
    public UI_Controller ui_Controller;
    
    public Time_System time_System;
    public News_System news_System;

     
    public static Root Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        Debug.Log("Root Instance 初始化");
    }

	private void Start() // 在此处触发各大系统的初始化
	{
        data_Controller.Init_Data();
        ui_Controller.Init_UI();


        time_System.StartTick();

	}

    private void OnApplicationQuit() // 退出时存档
    {
        data_Controller.Save_SaveData();
    }
}
