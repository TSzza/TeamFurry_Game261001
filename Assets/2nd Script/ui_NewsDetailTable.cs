using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ui_NewsDetailTable : MonoBehaviour
{
	public News myNews;
	public TMP_Text TitleText, HeatText,ViewText, TagText, MainBodyText;

	public void UpdateMyNews(News news)
	{
        if (myNews == null || news.NewsID != myNews.NewsID)
		{
			myNews = news;
			UpdateMyTexts();
		}
	}
	public void UpdateMyTexts()
	{
        if (myNews == null) return;
		TitleText.text = myNews.Title;
		HeatText.text = Mathf.RoundToInt(myNews.Heat).ToString();
		ViewText.text = myNews.Viewer_Num.ToString();
		TagText.text = MyTagString();
		MainBodyText.text = myNews.MainBody;
	}
    public string MyTagString()
    {
        string ans = "";
        foreach (WeightedTag t in myNews.Tags)
        {
            if (ans == "")
                ans += t.Tag.TagName;
            else
                ans += " | " + t.Tag.TagName;
        }
        return ans;
    }


    public int myTick = 0;
    private void OnTimeTick() // 更新热度
    {
        myTick++;
        if(myTick%2==0)
        {
            UpdateMyTexts();
        }

    }
    private void OnNewsTick() // 更新热度
    {
        myTick++;
        //if(myTick%2==0)
        //{
            UpdateMyTexts();
        //}

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
