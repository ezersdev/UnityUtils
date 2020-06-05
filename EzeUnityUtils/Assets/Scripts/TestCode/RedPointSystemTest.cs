using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedPointSystemTest : MonoBehaviour
{
    public GameObject taskRed;
    public Text taskRedNumText;

    public GameObject mainTaskRed;
    public Text mainTaskRedNumText;
    
    public GameObject dailyTaskRed;
    public Text dailyTaskRedNumText;

    private RedPointSystem _rpSystem = null;
    
    // Start is called before the first frame update
    void Start()
    {
        this._rpSystem = new RedPointSystem();
        this._rpSystem.InitRedPointTreeNode();
        
        this._rpSystem.SetRedPointNodeCallback(RedPointConsts.task, OnTaskChangeCallback);
        this._rpSystem.SetRedPointNodeCallback(RedPointConsts.mainTask, OnMainTaskChangeCallback);
        this._rpSystem.SetRedPointNodeCallback(RedPointConsts.dailyTask, OnDailyTaskChangeCallback);
        
        this._rpSystem.SetInvoke(RedPointConsts.mainTask, 1);
        this._rpSystem.SetInvoke(RedPointConsts.dailyTask, 5);
        
        // 启动的时候刷新数量
        this.refershRedPoints();
    }

    private void refershRedPoints()
    {
        int number = _rpSystem.GetNumber(RedPointConsts.task);
        taskRed.SetActive(number > 0);
        taskRedNumText.text = number.ToString();
        number = _rpSystem.GetNumber(RedPointConsts.mainTask);
        mainTaskRed.SetActive(number > 0);
        mainTaskRedNumText.text = number.ToString();
        number = _rpSystem.GetNumber(RedPointConsts.dailyTask);
        dailyTaskRed.SetActive(number > 0);
        dailyTaskRedNumText.text = number.ToString();
    }


    private void OnTaskChangeCallback(RedPointNode node)
    {
        Debug.Log("OnTaskChangeCallback : " + node.pointNum);
        taskRed.SetActive(node.pointNum > 0);
        taskRedNumText.text = node.pointNum.ToString();
    }

    private void OnMainTaskChangeCallback(RedPointNode node)
    {
        Debug.Log("OnMainTaskChangeCallback : " + node.pointNum);
        mainTaskRed.SetActive(node.pointNum > 0);
        mainTaskRedNumText.text = node.pointNum.ToString();
    }
    
    private void OnDailyTaskChangeCallback(RedPointNode node)
    {
        Debug.Log("OnDailyTaskChangeCallback : " + node.pointNum);
        dailyTaskRed.SetActive(node.pointNum > 0);
        dailyTaskRedNumText.text = node.pointNum.ToString();
    }
    
    
    
}
