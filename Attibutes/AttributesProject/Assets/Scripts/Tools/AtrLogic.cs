using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class AtrLogic
{
    static AtrLogic()
    {
        EditorApplication.delayCall += Wow;
        EditorSceneManager.sceneSaved += (x) => { Wow(); };
    }

    private static void Wow()
    {
        StackTraceLogType stackTraceLogType = Application.GetStackTraceLogType(LogType.Error);
        Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.None);
        MonoBehaviour[] sceneActive = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        foreach (MonoBehaviour mono in sceneActive)
        {
            Atr attribute = Attribute.GetCustomAttribute(mono.GetType(), typeof(Atr)) as Atr;
            if (attribute != null)
            {
                Debug.Log(mono.GetType().Name);
                SerializedObject serializedObject = new SerializedObject(mono);
                foreach (FieldInfo field in mono.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    if (field.FieldType.IsClass)
                    {
                        SerializeField atr = Attribute.GetCustomAttribute(field, typeof(SerializeField)) as SerializeField;
                        if (atr != null)
                        {
                            if (serializedObject.FindProperty(field.Name).objectReferenceValue == null)
                            {
                                Debug.LogErrorFormat(mono.gameObject, mono.gameObject.name + "." + field.Name + ", typeof " + field.FieldType.Name + " is null");
                            }
                        }
                    }
                }
            }
            Application.SetStackTraceLogType(LogType.Error, stackTraceLogType);
        }

        //MonoScript[] allMono = GetScriptAssetsOfType<MonoBehaviour>();
        //foreach (MonoScript mono in allMono)
        //{
        //    Atr attribute = Attribute.GetCustomAttribute(mono.GetClass(), typeof(Atr)) as Atr;
        //    if (attribute != null)
        //    {
        //        //Debug.Log(mono.GetClass());
        //        Magic(mono);
        //    }
        //}
    }

    private static void OnProjectChanged()
    {
        Debug.Log("OnProjectChanged");
    }

    private static void Magic(MonoScript mono)
    {
        foreach (FieldInfo field in mono.GetClass().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            SerializeField attribute = Attribute.GetCustomAttribute(field, typeof(SerializeField)) as SerializeField;
            if (attribute != null)
            {
                Debug.Log(field.FieldType.Name + "  " + field.Name);
            }
        }
    }

    private static MonoScript[] GetScriptAssetsOfType<T>()
    {
        MonoScript[] scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
        List<MonoScript> result = new List<MonoScript>();

        foreach (MonoScript m in scripts)
        {
            if (m.GetClass() != null && m.GetClass().IsSubclassOf(typeof(T)))
            {
                result.Add(m);
            }
        }
        return result.ToArray();
    }
}