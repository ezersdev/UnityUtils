using System.Collections.Generic;
using UnityEngine;

public class RedPointNode
{
    /** 节点名称 */
    public string nodeName;
    /** 红点数量 */
    public int pointNum = -1;
    /** 父节点 */
    public RedPointNode parent = null;
    /** 回调函数 */
    public RedPointSystem.OnPointNumChange numChangeFunc;
    /** 字节点 */
    public Dictionary<string, RedPointNode> childrenDic = new Dictionary<string, RedPointNode>();

    public RedPointNode(string name, RedPointNode parent)
    {
        this.nodeName = name;
        this.parent = parent;
    }
    /** 对外的设置叶子节点的Num */
    public void SetLefRedPointNum(int number)
    {
        if (pointNum == number) return;
        
        // 只可以对叶子节点设置红点
        if (childrenDic.Count > 0)
        {
            Debug.LogError("Only Can Set Leaf Node!");
            return;
        }
        SetRedPointNum(number);
    }

    private void SetRedPointNum(int number)
    {
        pointNum = number;
        // 调用当前的节点回调
        NotifyPointNumberChange();
        // 调用父节点的刷新红点数量
        parent?.RefershRedPointNum();
    }

    private void RefershRedPointNum()
    {
        int num = 0;
        foreach (var node in childrenDic.Values)
        {
            num += node.pointNum;
        }

        if (num != pointNum)
        { 
            SetRedPointNum(num);
        }
    }

    private void NotifyPointNumberChange()
    {
        numChangeFunc?.Invoke(this);
    }
}