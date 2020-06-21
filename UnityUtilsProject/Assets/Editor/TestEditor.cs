using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestEditor : EditorWindow
{
    // validate 是否有效， MenuItem 同名然后 返回 bool
    [MenuItem("EditorTools/1. SetupUIRoot", validate = true)]
    public static bool ValidateUIRoot()
    {
        return !GameObject.Find("UIRoot");
    }
    // 这里创建 UIRoot 通过工具创建节点
    [MenuItem("EditorTools/1. SetupUIRoot")]
    public static void CreateUIRoot()
    {
        var tw = GetWindow<TestEditor>();
        tw.Show();

    }

    private string _uiRootWidth = "";
    private string _uiRootHeight = "";

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("width", GUILayout.Width(45));
        // 这里传入接收结果的变量
        _uiRootWidth = GUILayout.TextField("1080");
        GUILayout.Label("*", GUILayout.Width(10));
        GUILayout.Label("Height", GUILayout.Width(50));
        _uiRootHeight = GUILayout.TextField("720");
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Setup UI Root"))
        {
            this.SetupUIRoot(int.Parse(_uiRootWidth) , int.Parse(_uiRootHeight));
            CloseWindow();
        }
    }


    private void SetupUIRoot(float width, float hight)
    {
        var uiRoot = new GameObject("UIRoot");
        uiRoot.layer = LayerMask.NameToLayer("UI");
        var uiRootCp = uiRoot.AddComponent<TestUIRoot>();
        
        var canvas = new GameObject("Canvas");
        canvas.transform.SetParent(uiRoot.transform);
        canvas.AddComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        canvas.AddComponent<CanvasScaler>().referenceResolution = new Vector2(width, hight);
        canvas.AddComponent<GraphicRaycaster>();
        canvas.layer = LayerMask.NameToLayer("UI");


        var events = new GameObject("EventSystem");
        events.transform.SetParent(uiRoot.transform);
        
        events.AddComponent<EventSystem>();
        events.AddComponent<StandaloneInputModule>();
        events.layer = LayerMask.NameToLayer("UI");
        
        var bg = new GameObject("BG");
        bg.AddComponent<RectTransform>();
        bg.transform.SetParent(canvas.transform);
        // 给成员赋值
        uiRootCp.bg = bg.transform;
        
        // 这里给序列化的成员赋值
        var uiRootSerialized = new SerializedObject(uiRootCp);
        uiRootSerialized.FindProperty("canvas").objectReferenceValue = canvas.GetComponent<Canvas>();
        uiRootSerialized.ApplyModifiedProperties();

        // 这里获取路径
        string prefabFloder = Application.dataPath + "/Resources";
        if (!Directory.Exists(prefabFloder))
        {
            Directory.CreateDirectory(prefabFloder);
        }
        // 存储 Prefab
        string prefabPath = prefabFloder + "/UIRoot.prefab";
        PrefabUtility.SaveAsPrefabAssetAndConnect(uiRoot, prefabPath, InteractionMode.AutomatedAction);
    }

    private void CloseWindow()
    {
        var window = GetWindow<TestEditor>();
        window.Close();
    }
}
