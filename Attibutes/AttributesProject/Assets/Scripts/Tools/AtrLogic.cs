using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

[InitializeOnLoad]
public class AtrLogic
{
    static AtrLogic()
    {
        EditorApplication.delayCall += Wow;
    }

    private static void Wow()
    {
        MonoScript[] sceneActive = GetScriptAssetsOfType<MonoBehaviour>();
        foreach (MonoScript mono in sceneActive)
        {
            Atr attribute = Attribute.GetCustomAttribute(mono.GetClass(), typeof(Atr)) as Atr;
            if (attribute != null)
            {
                //Debug.Log(mono.GetClass());
                Magic(mono);
            }
        }
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