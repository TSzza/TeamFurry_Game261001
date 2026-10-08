using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class News_System : MonoBehaviour
{
	// 新闻系统

	// 1、负责根据输入，生成新闻的细节内容
    
    public int MyTimes;
    public Data_Controller data_Controller;
    public generate_News generate_News;

    public event Action UpdateNewsList;

    [Header("Generate")]
    public int GenerateFrequency = 4;

    // 每Tick执行
    private void OnTimeTick()
    {

        MyTimes++;
        bool NeedSort = false;

        if (MyTimes % GenerateFrequency == 0)
        {
            GenerateNews();
            NeedSort = true;
        }

        if (MyTimes % DownFrequency == 0)
        {
            DownHeat();
            NeedSort = true;
        }

        if (NeedSort)
        {
            SortNews();
        }


    }
    
    private List<InterestTag> PickRandomTags(int num) // 抽取随机Tag，用洗牌算法保证不重复抽取
    {
        List<InterestTag> result = new List<InterestTag>();
        List<InterestTag> shuffTags = data_Controller.interestTags;
        ShuffleList(shuffTags);

        //foreach (InterestTag i in shuffTags)
        //{
        //    Debug.Log($"shuffTags: {i.TagName}");
        //}

        int takeCount = Math.Min(num, shuffTags.Count);

        //Debug.Log($"takeCount {takeCount}");

        for (int i=0;i<takeCount;i++)
        {
            result.Add(shuffTags[i]);
        }
        return result;
    }

    private List<WeightedTag> PickRandomWeightedTags(int num)// 生成随机权值Tag
    {
        //Debug.Log($"生成随机权值Tag：{num}");

        List<InterestTag> shuffTags = PickRandomTags(num);

        List<WeightedTag> result = new List<WeightedTag>();
        float MaxWeight = 1.0f;
        for (int i = 0; i < shuffTags.Count; i++)
        {
            float randomWeight = MaxWeight * UnityEngine.Random.Range(0.4f, 1.0f);
            randomWeight = RoundDecimals(randomWeight, 2); // 四舍五入到第二位
            if (randomWeight >= 0.1f)
            {
                MaxWeight -= randomWeight; // 前一个标签的权值一定是最重的
                result.Add(new WeightedTag(shuffTags[i], randomWeight));
            } // 忽略过小的权值
        }
        return result;
    }


    public void GenerateNews() // 随机生成一条新闻
    {
        int tagnum = 1;
        float r = UnityEngine.Random.Range(0.0f, 1.0f); // 有几条tag
        if (r < 0.2f) tagnum = 1;
        else if (r < 0.6f) tagnum = 2;
        else tagnum = 3;

        List<WeightedTag> tags = PickRandomWeightedTags(tagnum);

        //Debug.Log(tags);
        string title = generate_News.CreateTitle(tags);
        //Debug.Log(title);
        string mainBody = generate_News.CreateMainBody(title,tags);
        //Debug.Log(mainBody);

        float weight = tags[0].Weight;
        if (tags.Count >= 2) weight += tags[1].Weight;
        float heat = weight * 80 * UnityEngine.Random.Range(0.9f, 1.2f); // 随机生成热度

        News newNews = new News
        {
            NewsID = data_Controller.newsID++,
            Title = title,
            MainBody = mainBody,
            Heat = heat,
            Viewer_Num = 0,
            Comment_Num = 0,

            Tags = tags,
            Viewers = new List<Peo>(),
            Comments = new List<Comment>()
        };
        data_Controller.news.Add(newNews);
    }

    public static void ShuffleList<T>(List<T> list) // 洗牌算法
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    public static float RoundDecimals(float val, int decimals) //四舍五入到小数点第D位
    {
        return (float)Math.Round(val + 1e-6, decimals);
    }






    // 新闻失去新鲜度
    
    [Header("Down Heat")]
    public float DownValue = 0.9f;
    public int DownFrequency = 2;
    public float MinHeat = 10f;

    public void DownHeat()
    {
        for (int i=data_Controller.news.Count-1;i>=0;i--)
        {
            data_Controller.news[i].Heat *= DownValue;
            if(data_Controller.news[i].Heat<=MinHeat)
            {
                data_Controller.news.RemoveAt(i);
            }
        }
    }

    
    public void SortNews() // 按热度更新新闻列表，热度高的在前
    {
        data_Controller.news.Sort((a,b)=> b.Heat.CompareTo(a.Heat));
        UpdateNewsList?.Invoke();
    }
    public void InvokeUpdateNewsList()
    {
        UpdateNewsList?.Invoke();
    }

    // 订阅相关

    private Time_System timeSystem;
    public bool subscribeTick = false;

    private void Start()
    {
        data_Controller = Root.Instance.data_Controller;
        timeSystem = Root.Instance.time_System;
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
}
