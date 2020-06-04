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
    
    // Start is called before the first frame update
    void Start()
    {
        var rpSystem = new RedPointSystem();
        rpSystem.InitRedPointTreeNode();
        
        rpSystem.SetRedPointNodeCallback(RedPointConsts.task, OnTaskChangeCallback);
        rpSystem.SetRedPointNodeCallback(RedPointConsts.mainTask, OnMainTaskChangeCallback);
        rpSystem.SetRedPointNodeCallback(RedPointConsts.dailyTask, OnDailyTaskChangeCallback);
        
        rpSystem.SetInvoke(RedPointConsts.mainTask, 1);
        rpSystem.SetInvoke(RedPointConsts.dailyTask, 5);
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
