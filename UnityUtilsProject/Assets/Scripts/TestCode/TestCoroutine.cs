using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCoroutine : MonoBehaviour
{
    private bool _session = true;

    private Coroutine _oneFrameCoroutine = null;
    // Start is called before the first frame update
    void Start()
    {
        _oneFrameCoroutine = StartCoroutine(UpdateOneFrame());
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_session)
        {
            Debug.Log("first enter Update Function --- ");
            _session = false;
        }

        if (Input.GetKey(KeyCode.F))
        {
            StopCoroutine(_oneFrameCoroutine);
        }
    }

    IEnumerator UpdateOneFrame()
    {
        // 在 Update 函数执行完后执行
        yield return null;
        Debug.Log("Yield Return Null ---");
        
        // 在等待时间后执行 在执行优先级 次于 yield return null
        yield return new WaitForSeconds(5.0f);
        Debug.Log("WaitForSeconds --- ");
        for (;;)
        {
            Debug.Log("WaitForEndOfFrame ---");
            // 在这一帧执行结束后执行
            yield return new WaitForEndOfFrame();
        }
    }
}
