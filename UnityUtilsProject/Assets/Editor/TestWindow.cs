using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    [MenuItem("Window/TestWindow")]
    public static void Open()
    {
        TestWindow tw = GetWindow<TestWindow>();
        tw.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("XXXXX");
        GUILayout.Label(GUI.tooltip);
        if (GUILayout.Button("ClickMe"))
        {
            Debug.Log("Click GUILayoutButton !!");
        }
        GUILayout.Button("Button");
        GUILayout.Box("Box");
        GUILayout.Space(100);
        // GUILayout.Toggle("Toggle");
        GUILayout.EndVertical();
    }
}
