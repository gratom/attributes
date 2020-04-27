#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using UnityEditor.SceneManagement;

namespace Tools
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Assert : Attribute { }

    [InitializeOnLoad]
    public class AssertTool
    {
        static AssertTool()
        {
            EditorApplication.delayCall += GlobalAssert;
            EditorSceneManager.sceneSaved += (x) => { GlobalAssert(); };
        }

        private static void GlobalAssert()
        {
            StackTraceLogType stackTraceLogType = Application.GetStackTraceLogType(LogType.Error);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);

            MonoBehaviour[] allMonoOnActiveScene = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
            foreach (MonoBehaviour monoBehaviour in allMonoOnActiveScene)
            {
                if (Attribute.GetCustomAttribute(monoBehaviour.GetType(), typeof(Assert)) as Assert != null)
                {
                    SerializedObject serializedObject = new SerializedObject(monoBehaviour);
                    foreach (FieldInfo field in monoBehaviour.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                    {
                        if (field.FieldType.IsClass)
                        {
                            SerializeField atr = Attribute.GetCustomAttribute(field, typeof(SerializeField)) as SerializeField;
                            if (atr != null)
                            {
                                if (serializedObject.FindProperty(field.Name).objectReferenceValue == null)
                                {
                                    Debug.LogErrorFormat(monoBehaviour.gameObject, monoBehaviour.gameObject.name + "." + field.Name + ", typeof " + field.FieldType.Name + " is null");
                                }
                            }
                        }
                    }
                }
            }
            Application.SetStackTraceLogType(LogType.Error, stackTraceLogType);
        }
    }
}

#endif