using System;
using System.IO;
using UnityEngine;

namespace EditorExtention
{
    public class CodeTemplate : MonoBehaviour
    {
        public static void Write(StreamWriter stream, string name)
        {
            stream.WriteLine("using UnityEngine;");
            stream.WriteLine();
            stream.WriteLine($"public class {name} : MonoBehaviour ");
            stream.WriteLine("{");
            stream.WriteLine();
            stream.WriteLine("}");
        }
    }
}