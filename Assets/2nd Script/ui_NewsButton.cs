using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Data;

public class ui_NewsButton : MonoBehaviour
{
	public News myNews;
	public TMP_Text TitleText, HeatText, ViewText;
	public void ShowDetails() // 按下按钮
	{
        myNews.Viewer_Num++;
        Root.Instance.news_System.InvokeUpdateNewsList();
		Root.Instance.ui_Controller.ui_NewsDetailTable.UpdateMyNews(myNews);
	}
	public void UpdateMyTexts()
	{
        if (myNews == null) return;
		TitleText.text = CutChinese(myNews.Title, 13);
		HeatText.text = Mathf.RoundToInt(myNews.Heat).ToString();
		ViewText.text = myNews.Viewer_Num.ToString();
	}
    private string CutChinese(string source, int maxLen) // 省略过多字数
    {
        if (string.IsNullOrEmpty(source)) return "";
        if (source.Length <= maxLen) return source;
        return source.Substring(0, maxLen) + "…";
    }
    public void UpdateMyNews(News news)
    {
        if (myNews==null || news.NewsID != myNews.NewsID)
        {
            myNews = news;
            UpdateMyTexts();
        }
    }

    public int myTick = 0;
    private void OnTimeTick() // 更新热度
    {
        myTick++;
        //if(myTick%2==0)
        //{
            UpdateMyTexts();
        //}
    }
    private void OnNewsTick()
    {
         UpdateMyTexts();
    }

    // 订阅
	
    private News_System newsSystem;
    public bool subscribeUpdateNewsList = false;
    private Time_System timeSystem;
    public bool subscribeTick = false;

    private void Start()
    {
        timeSystem = Root.Instance.time_System;
        newsSystem = Root.Instance.news_System;
        SubscribeTick();
    }
    
    private void OnEnable()
    {
        if (!subscribeUpdateNewsList||!subscribeTick)
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
        if (timeSystem != null)
        {
            timeSystem.OnTick -= OnTimeTick;
            subscribeTick = false;
        }
        if (newsSystem != null)
        {
            newsSystem.UpdateNewsList -= OnNewsTick;
            subscribeUpdateNewsList = false;
        }
    }


}
