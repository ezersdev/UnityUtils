using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace EditorExtention
{
    public class CreateComponentCode : MonoBehaviour
    {
        // 这边优先级有值 则会在Hierarchy中显示
        [MenuItem("GameObject/@Editor Extention/Create Code", false, 10)]
        public static void CreateCode()
        {
            Debug.Log("Create Code!");

            var obj = Selection.objects.First() as GameObject;
            if (!obj)
            {
                return;
            }
            
            var scriptsPath = Application.dataPath + "/Scripts";
            if (!Directory.Exists(scriptsPath))
            {
                Directory.CreateDirectory(scriptsPath);
            }

            var gName = obj.name;
            var filePath = $"{scriptsPath}/{obj.name}.cs";
            var stream = File.CreateText(filePath);
            // 写入代码
            CodeTemplate.Write(stream, obj.name);
            
            stream.Close();
            // 刷新 项目资源库
            EditorPrefs.SetString("GENERATE_CLASS_NAME", obj.name);
            AssetDatabase.Refresh();
        }
        
        
        [DidReloadScripts] // 代码编译完成
        public static void AddComponent2GameObject()
        {
            var className = EditorPrefs.GetString("GENERATE_CLASS_NAME");
            EditorPrefs.DeleteKey("GENERATE_CLASS_NAME");
            Debug.Log(className);
            if (string.IsNullOrEmpty(className))
            {
                Debug.Log("Continue");
            }
            else
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var defaultAssembly = assemblies.First(assembly => assembly.GetName().Name == "Assembly-CSharp");
                var ComType = defaultAssembly.GetType(className);
                
                Debug.Log(ComType);

                var gameObject = GameObject.Find(className);
                gameObject.AddComponent(ComType);
            }
            
        }
        
    }
    

}

