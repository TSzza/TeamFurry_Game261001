using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_News : MonoBehaviour
{
    // 比较复杂的 Ai生成新闻文稿 内容

    public string CreateTitle(List<WeightedTag> tags) // 通过tag生成新闻标题
    {
        if (tags == null || tags.Count == 0)
            return "【简讯】暂无正文";
        string mainTag = tags[0].Tag.TagName;
        float mainWeight = tags[0].Weight;

        // 分级新闻标题模板
        List<string> templatePool;

        if (tags.Count >= 2 && UnityEngine.Random.value < 0.3f)
        {
            string subTag = tags[1].Tag.TagName;
            List<string> twoTagTemplates = new List<string>
            {
                "{0}与{1}领域出现联动事件",
                "{0}消息：{1}也受到波及",
                "{1}视角看{0}本次事件"
            };
            var t = twoTagTemplates[UnityEngine.Random.Range(0, twoTagTemplates.Count)];
            return string.Format(t, mainTag, subTag);
        }
        else if (mainWeight >= 0.9f)
        {
            templatePool = new List<string>()
            {
                "震惊，{0}界居然出了这种事",
                "突发！{0}领域发生重大意外",
                "轰动全网！{0}出现前所未有的状况",
                "紧急快讯：{0}爆出重大事件",
                "惊呆了！{0}发生罕见变故"
            };
        }
        else if (mainWeight >= 0.75f)
        {
            templatePool = new List<string>()
            {
                "最新消息，{0}传来新动向",
                "关注！{0}出现重大变化",
                "爆料：{0}领域迎来新情况",
                "热议！{0}相关事件引发讨论",
                "快讯｜{0}方面出现新动态"
            };
        }
        else if (mainWeight >= 0.5f)
        {
            templatePool = new List<string>()
            {
                "消息速递：{0}有新进展",
                "来看，{0}领域出现新变化",
                "关于{0}的一则新消息",
                "{0}相关情况更新",
                "行业观察：{0}迎来新发展"
            };
        }
        else
        {
            templatePool = new List<string>()
            {
                "科普来了！{0}相关小知识",
                "简讯：{0}的基础介绍",
                "知识分享，聊聊{0}",
                "资讯｜{0}基础信息一览",
                "小科普，带你了解{0}"
            };
        }

        // 随机挑选一条模板，把{0}替换为主标签
        int randomIdx = UnityEngine.Random.Range(0, templatePool.Count);
        string template = templatePool[randomIdx];
        string title = string.Format(template, mainTag);
        return title;
    }


    /// <summary>
    /// 根据新闻标题 智能生成对应风格正文
    /// 自动识别：震惊/突发/热议/科普/快讯 文风
    /// 自动兼容：单标签 / 双标签联动标题
    /// </summary>
    public string CreateMainBody(string title, List<WeightedTag> tags)
    {
        if (tags == null || tags.Count == 0)
            return "暂无详细报道，相关内容正在持续跟进中。";

        // 主标签、副标签
        string mainTag = tags[0].Tag.TagName;
        string subTag = tags.Count > 1 ? tags[1].Tag.TagName : "";

        // ========== 1. 文本检测：判断当前新闻文风等级 ==========
        bool isShock = title.Contains("震惊") || title.Contains("惊呆") || title.Contains("前所未有") || title.Contains("重大意外");
        bool isBreak = title.Contains("突发") || title.Contains("紧急快讯");
        bool isHot = title.Contains("热议") || title.Contains("爆料") || title.Contains("新动向") || title.Contains("新变化");
        bool isNormal = title.Contains("消息速递") || title.Contains("新进展") || title.Contains("行业观察");
        bool isScience = title.Contains("科普") || title.Contains("小知识") || title.Contains("简讯") || title.Contains("知识分享");

        // ========== 2. 分级文案库（前、中、后三段式） ==========
        List<string> prefixPool = new List<string>();
        List<string> middlePool = new List<string>();
        List<string> suffixPool = new List<string>();

        // 最高级：震惊/重大突发事件
        if (isShock || isBreak)
        {
            prefixPool = new List<string>()
        {
            "近日，{0}领域爆出出人意料的情况，迅速引发大众广泛关注。",
            "就在近期，{0}发生罕见异动，打破了以往的常规发展规律。",
            "本网最新核实，{0}出现突发状况，事态超出大众预期。"
        };
            middlePool = new List<string>()
        {
            "据多方渠道消息透露，本次{0}相关事件波及范围较广，影响程度远超以往同类情况。",
            "业内人士表示，此次{0}变故属于少见情况，背后蕴含的变化值得深度重视。",
            "不少民众对此表示意外，纷纷关注本次{0}事件的后续走向与具体原因。"
        };
            suffixPool = new List<string>()
        {
            "目前相关情况仍在持续跟进调查，后续进展我们将第一时间为大家播报。",
            "各界已开始密切关注本次事件，相关影响还在进一步发酵中。",
            "本次事件也为{0}领域的后续发展敲响了新的警钟。"
        };
        }
        // 高级：热点、热议、爆料
        else if (isHot)
        {
            prefixPool = new List<string>()
        {
            "近期，{0}领域出现全新动态，引发网络热议。",
            "随着行业不断发展，{0}迎来阶段性新变化，受到众多网友关注。",
            "有媒体爆料，{0}相关局势出现新的调整与变动。"
        };
            middlePool = new List<string>()
        {
            "本次{0}的变化，对行业整体环境产生了一定影响，也改变了大众的固有认知。",
            "众多爱好者与从业者纷纷参与讨论，分享自己对{0}新变化的看法。",
            "从目前形势来看，{0}的发展趋势正在逐步迎来全新的阶段。"
        };
            suffixPool = new List<string>()
        {
            "未来{0}的发展走向，也将持续受到大众和行业的关注。",
            "本次更新也为后续{0}领域的发展提供了新的参考方向。",
            "业内普遍认为，{0}的变化将会带来新一轮的行业调整。"
        };
        }
        // 中级：普通资讯、行业消息
        else if (isNormal)
        {
            prefixPool = new List<string>()
        {
            "近日，关于{0}的最新行业消息正式对外公布。",
            "随着整体环境不断迭代，{0}领域迎来阶段性进展。",
            "本网整理最新资讯，带你了解{0}的最新动态。"
        };
            middlePool = new List<string>()
        {
            "本次{0}的更新与调整，进一步完善了行业现有体系，优化了整体发展环境。",
            "相关工作人员表示，{0}的稳步发展将持续带动行业良性迭代。",
            "不少从业者表示，{0}的新变化符合行业整体发展趋势。"
        };
            suffixPool = new List<string>()
        {
            "后续{0}还将持续优化升级，为行业发展持续赋能。",
            "本次进展也将成为{0}发展历程中的重要阶段性节点。",
            "大众可持续关注，静待{0}更多新动态。"
        };
        }
        // 低级：科普、简讯、知识类
        else if (isScience)
        {
            prefixPool = new List<string>()
        {
            "在日常生活与行业发展中，{0}一直是备受关注的基础领域。",
            "很多人对{0}了解甚少，本次简讯将为大家简单科普相关知识。",
            "带你快速了解{0}的基础信息与行业常识。"
        };
            middlePool = new List<string>()
        {
            "{0}涵盖了丰富的基础内容，是相关领域不可或缺的重要组成部分，具备极高的参考与学习价值。",
            "长期以来，{0}在行业内保持着稳定的发展态势，也是大众认知度较高的领域之一。",
            "通过了解{0}的相关知识，能够帮助大家更好的认知对应行业与生活常识。"
        };
            suffixPool = new List<string>()
        {
            "以上就是关于{0}的基础科普内容，希望能为大家提供参考。",
            "后续我们也将持续更新更多与{0}相关的知识内容。",
            "感兴趣的朋友可以持续关注，深入了解{0}相关内容。"
        };
        }
        // 默认兜底文风
        else
        {
            prefixPool = new List<string>() { "近日，{0}领域出现全新动态。" };
            middlePool = new List<string>() { "相关情况正在稳步发展，受到不少关注。" };
            suffixPool = new List<string>() { "后续相关进展将持续更新。" };
        }

        // ========== 3. 随机选取三段正文 ==========
        string pre = prefixPool[Random.Range(0, prefixPool.Count)];
        string mid = middlePool[Random.Range(0, middlePool.Count)];
        string suf = suffixPool[Random.Range(0, suffixPool.Count)];

        // ========== 4. 双标签特殊拼接逻辑（适配你双标签标题） ==========
        // 如果是双标签标题，自动插入联动语句
        if (!string.IsNullOrEmpty(subTag) && (title.Contains("联动") || title.Contains("波及") || title.Contains("视角")))
        {
            List<string> linkPool = new List<string>()
        {
            "与此同时，{1}领域与{0}产生深度关联，相互影响，形成了全新的发展态势。",
            "本次{0}的变化，也间接对{1}领域造成了一定的影响与联动效应。",
            "从{1}的角度分析，本次{0}的变动具备极高的参考与研究意义。"
        };
            string link = linkPool[Random.Range(0, linkPool.Count)];
            mid += " " + link;
        }

        // ========== 5. 填充标签并拼接完整正文 ==========
        string fullPre = string.Format(pre, mainTag, subTag);
        string fullMid = string.Format(mid, mainTag, subTag);
        string fullSuf = string.Format(suf, mainTag, subTag);

        // 拼接最终正文（自然三段式文章）
        return $"{fullPre}{fullMid}{fullSuf}";
    }

}
