using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_NewsTable : MonoBehaviour
{
	public Transform NewsFather;
	public GameObject NewsFBX;
    private List<GameObject> _newsItemList = new List<GameObject>();

	private void Start()
	{
        _newsItemList.Clear();
        for (int i = 0; i < NewsFather.childCount; i++)
        {
            Transform childTf = NewsFather.GetChild(i);
            GameObject childGo = childTf.gameObject;
            // 只收集激活的物体
            if (childGo.activeSelf)
            {
                _newsItemList.Add(childGo);
            }
        }
        newsSystem = Root.Instance.news_System;
        SubscribeTick();
	}

    public int MaxShowNews = 20;
	public void UpdateNewsTable()
    {
        List<News> newsList = Root.Instance.data_Controller.news;
        int targetCount = newsList.Count;
        int currentCount = _newsItemList.Count;

        // 数量过多
        while (_newsItemList.Count > targetCount)
        {
            GameObject item = _newsItemList[_newsItemList.Count - 1];
            Destroy(item);
            _newsItemList.RemoveAt(_newsItemList.Count - 1);
        }

        // 数量不足
        while (_newsItemList.Count < Mathf.Min(targetCount,MaxShowNews))
        {
            GameObject newItem = Instantiate(NewsFBX, NewsFather);
            _newsItemList.Add(newItem);
        }

        // 遍历全部
        for (int i = 0; i < _newsItemList.Count; i++)
        {
            var itemObj = _newsItemList[i];
            var newsItem = itemObj.GetComponent<ui_NewsButton>();
            if (newsItem != null)
            {
                newsItem.UpdateMyNews(newsList[i]);
            }
        }
    }
    public int myTick = 0;
    private void OnNewsTick() // 更新热度
    {
        myTick++;
        if(myTick%2==0)
        {
            UpdateNewsTable();
        }

    }

    // 订阅
	
    private News_System newsSystem;
    public bool subscribeUpdateNewsList = false;

    
    private void OnEnable()
    {
        if (!subscribeUpdateNewsList)
            SubscribeTick();
    }
    private void SubscribeTick()
    {
        if (newsSystem != null)
        {
            //Debug.Log("已订阅");
            newsSystem.UpdateNewsList += OnNewsTick;
            subscribeUpdateNewsList = true;
        }
        //else Debug.Log("timeSystem订阅失败：null");
    }

    private void OnDisable()
    {
        if (newsSystem != null)
        {
            newsSystem.UpdateNewsList -= OnNewsTick;
            subscribeUpdateNewsList = false;
        }
    }
}
