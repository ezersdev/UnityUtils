


using System.Collections.Generic;
using UnityEngine;

public class RedPointSystem
{
    public delegate void OnPointNumChange(RedPointNode node);// 红点变化通知
    /** 红点树 Root 节点 */
    private RedPointNode m_rootNode;

    private static List<string> s_RedPointTreeList = new List<string>
    {
        RedPointConsts.main,
        RedPointConsts.task,
        RedPointConsts.mainTask,
        RedPointConsts.dailyTask
    };
    
    /// <summary>
    /// 初始化红点节点树
    /// </summary>
    public void InitRedPointTreeNode()
    {
        m_rootNode = new RedPointNode(RedPointConsts.main, null);
        foreach (var str in s_RedPointTreeList)
        {
            var node = m_rootNode;
            var treeNodeAry = str.Split('.');
            if (treeNodeAry[0] != m_rootNode.nodeName)
            {
                Debug.LogError("RedPointRree Root Node Error:" + treeNodeAry[0]);
                continue;
            }

            if (treeNodeAry.Length > 1)
            {
                for (int i = 1; i < treeNodeAry.Length; i++)
                {
                    if (!node.childrenDic.ContainsKey(treeNodeAry[i]))
                    {
                        node.childrenDic.Add(treeNodeAry[i], new RedPointNode(treeNodeAry[i], node));
                    }

                    node = node.childrenDic[treeNodeAry[i]];
                }
            }
        }
    }
    
    /// <summary>
    /// 设置红点节点的回调
    /// </summary>
    /// <param name="strNode"></param>
    /// <param name="callback"></param>
    public void SetRedPointNodeCallback(string strNode, OnPointNumChange callback)
    {
        var nodeList = strNode.Split('.');
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConsts.main)
            {
                Debug.LogError("Get Wrong Root Node! Current Is: " + nodeList[0]);
                return;
            }
        }

        var node = m_rootNode;
        for (int i = 1; i < nodeList.Length; i++)
        {
            if (!node.childrenDic.ContainsKey(nodeList[i]))
            {
                Debug.LogError("Does Not Contains Child Node: " + nodeList[i]);
                return;
            }

            node = node.childrenDic[nodeList[i]];
            if (i == nodeList.Length - 1)
            {
                node.numChangeFunc = callback;
                return;
            }
        }
    }
    
    /// <summary>
    /// 激发数据变化
    /// </summary>
    /// <param name="strNode"></param>
    /// <param name="number"></param>
    public void SetInvoke(string strNode, int number)
    {
        var nodeList = strNode.Split('.');
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConsts.main)
            {
                Debug.Log("Get Wrong Root Node! Current Is: " + nodeList[0]);
                return;
            }
        }
        
        var node = m_rootNode;
        for (int i = 1; i < nodeList.Length; i++)
        {
            if (!node.childrenDic.ContainsKey(nodeList[i]))
            {
                Debug.Log("Does Not Contains Child Node: " + nodeList[i]);
                return;
            }
            node = node.childrenDic[nodeList[i]];

            if (i == nodeList.Length - 1) // 最后一个节点了
            {
                node.SetLefRedPointNum(number); // 设置节点的红点数量
            }
        }
    }
    
    
}