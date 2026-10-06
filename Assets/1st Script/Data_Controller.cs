using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;
using System.IO;
using System.Text;

public class Data_Controller : MonoBehaviour
{
    // 负责游戏数据的 读取 与 存储（包括表格的数据读取），包括存档和读档
    // 指定 UTF - 8 编码，表格内不能有回车
    
    public List<InterestTag> interestTags { get; private set; }
    public List<BehaviorTag> behaviorTags;
    public List<News> news;
    public List<Peo> peos;

    public void SetPersistentDataPath() { SavePath = Path.Combine(Application.persistentDataPath, "beastWave.save"); }
    string SavePath;
    
    string interestTagsPath = Path.Combine(Application.streamingAssetsPath,"新闻tag.csv");
    string NewsPath = Path.Combine(Application.streamingAssetsPath,"新闻列表.csv");




    public void Init_Data()
    {
        Read_SaveData();
        
        Read_InterestTag(interestTagsPath);
        Read_News(NewsPath);

     //* InterestTag
     //* WeightedTag
     //* 
     //* News
     //* Comment
     //* 
     //* BehaviorTag
     //* WeightedBehavior
     //* 
     //* Peo
    }
    public void Read_SaveData()
    {
        SetPersistentDataPath();
        System_Controller sys = Root.Instance.system_Controller;
        Vector2Int Res = new Vector2Int(1920, 1080);
        bool Full = sys.DefaultFullscreen;

        if (!File.Exists(SavePath))
        {
            Debug.Log("没有找到存档");
        }
        else
        {

            string json = File.ReadAllText(SavePath, Encoding.UTF8);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            Res.x = data.screenWidth;
            Res.y = data.screenHeight;
            Full = data.fullScreen;
        }
        sys.SetResolution(Res.x, Res.y);
        sys.SetFullScreen(Full);
    }
    public void Save_SaveData()
    {
        SaveData data = new SaveData
        {
            screenWidth = Screen.width,
            screenHeight = Screen.height,
            fullScreen = (Screen.fullScreenMode != FullScreenMode.Windowed)
        };
        string json = JsonUtility.ToJson(data, prettyPrint: true);
        File.WriteAllText(SavePath, json, Encoding.UTF8);
        Debug.Log($"画面设置已保存");
    }

    public void Read_InterestTag(string filePath)
    {
        interestTags = new List<InterestTag>();

        List<string> nameList = ReadSingleColumnFile(filePath); // 单行读取
        if (nameList == null) return;

        foreach (string tagName in nameList)
        {
            int ID = interestTags.Count; // ID其实就是下标
            interestTags.Add(new InterestTag
            {
                TagID = ID,
                TagName = tagName
            });
            Debug.Log($"InteresetTag - ({ID},{tagName})");
        }
    }
    //public void Read_BehavoirTag(string filePath)
    //{
    //}


    public void Read_News(string filePath)
    {
        List<News> newsList = new List<News>();

        if (!File.Exists(filePath))
        {
            Debug.LogError($"配表不存在：{filePath}");
            return;
        }

        string[] allLines = File.ReadAllLines(filePath, Encoding.UTF8);
        for (int lineIndex = 0; lineIndex < allLines.Length; lineIndex++)
        {
            if (lineIndex == 0) continue; // 忽略第一行

            string rawLine = allLines[lineIndex].Trim();
            if (string.IsNullOrEmpty(rawLine)) continue;
            string[] cells = rawLine.Split(',');

            string title = cells[0];
            if(string.IsNullOrWhiteSpace(cells[0]))
            {
                title = "天哪，这个标题是空的";
                Debug.LogWarning($"第{lineIndex+1}行，标题是空");
            }

            string body = cells[1];
            if(string.IsNullOrWhiteSpace(cells[1]))
            {
                body = "偶不，网络飞走咯（空的新闻正文内容）";
                Debug.LogWarning($"第{lineIndex+1}行，正文是空");
            }

            float heat = -1;
            List<WeightedTag> tagList = new List<WeightedTag>();
            for (int cellIdx = 2; cellIdx < cells.Length; cellIdx++) // 开始输入Tag权值
            {
                string weightText = cells[cellIdx].Trim();
                if (string.IsNullOrEmpty(weightText))
                    continue;

                if (float.TryParse(weightText, out float w))
                {
                    if (cellIdx == 2)
                        heat = w;
                    else
                    {
                        int tagId = cellIdx - 3;
                        tagList.Add(new WeightedTag(interestTags[tagId], w));
                    }
                }
                else
                {
                    Debug.LogWarning($"第{lineIndex+1}行，标签列{cellIdx}权重非法：{weightText}");
                }
            }
            
            int ID = newsList.Count; // ID其实就是下标
            News newsItem = new News
            {
                NewsID = ID,
                Title = title,
                MainBody = body,
                Heat = heat,
                Tags = tagList
            };
            newsList.Add(newsItem);
            
            Debug.Log($"News - {newsItem}");

        }
    }





    // -----------------------------------------------------------------
    
    public static List<string> ReadSingleColumnFile(string filePath) // 读取单列文件
    {
        List<string> result = new List<string>();

        if (!File.Exists(filePath))
        {
            Debug.LogError($"文件不存在：{filePath}");
            return result;
        }

        string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);
        for (int i = 0; i < lines.Length; i++)
        {
            string rawLine = lines[i].Trim();
            // 跳过空行
            if (string.IsNullOrEmpty(rawLine))
                continue;

            // 删除所有英文逗号
            string processed = rawLine.Replace(",", "");
            result.Add(processed);
        }
        return result;
    }
}
