using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
	// 用于联系各大控制器，拥有唯一单例
    // 用于触发全游戏的初始化

    


     
    public static Root Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

	private void Start() // 在此处触发各大系统的初始化
	{
		
	}
}
