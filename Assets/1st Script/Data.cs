using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

// 负责定义游戏数据的数据结构

namespace Data
{
    /***
     * 
     * InterestTag
     * WeightedTag
     * 
     * News
     * Comment
     * 
     * BehaviorTag
     * WeightedBehavior
     * 
     * Peo
     * 
     ***/

    /***
     * 
     * SaveData
     * 
     ***/


    public struct InterestTag // 兴趣标签，标示用户与新闻的兴趣点，配表
    {
        public int TagID;
        public string TagName;
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(TagName) ? "空Tag名" : TagName;
        }
    }
    public struct WeightedTag // 带权标签
    {
        public InterestTag Tag;
        public float Weight;
        public WeightedTag(InterestTag tag, float val)
        {
            Tag = tag;
            Weight = val;
        }
        public override string ToString()
        {
            return $"{Tag}({Weight:F2})";
        }
    }

    // 新闻
    public struct News
    {
        public int NewsID;
        public string Title;
        public string MainBody; // 正文

        public List<WeightedTag> Tags; // 一组带权标签

        public float Heat; // 炸裂程度，与稀有度有关

        public int Viewer_Num;
        public List<Peo> Viewers;

        public int Comment_Num;
        public List<Comment> Comments;

        public override string ToString()
        {
            string tagStr = "";
            if (Tags != null && Tags.Count > 0)
            {
                tagStr = string.Join(" | ", Tags);
            }
            return
$@"== 新闻ID【{NewsID}】==
标题：{Title}
正文：{MainBody}
炸裂程度：{Heat}
标签：{tagStr}
============";
        }
    }
    // 评论
    public struct Comment
    {
        public Peo Reviewer;
        public int Time;
        public string MainBody;
    }



    
    public struct BehaviorTag // 行为标签，标示用户行为，例如转发、评论等，配表
    {
        public int BehaviorID;
        public string BehaviorName;
    }
    public struct WeightedBehavior // 带权行为标签
    {
        public BehaviorTag Behavior;
        public float Weight;
    }


    // 人
    public struct Peo
    {
        public int PeoID;
        public string PeoName;

        // 记忆
        public List<News> Memory;
        // 兴趣标签
        public List<WeightedTag> Characteristics;
        // 行为倾向
        public List<WeightedBehavior> BehaviorTendency;
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(PeoName) ? "空用户名" : PeoName;
        }
    }

    // =========================================================

    public struct SaveData
    {
        public int screenWidth;
        public int screenHeight;
        public bool fullScreen;
    }


}
