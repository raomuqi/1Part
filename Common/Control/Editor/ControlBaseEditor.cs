using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(ControlBase))]
public abstract class ControlBaseEditor : Editor
{
    SerializedProperty onPlay, onRest;
    bool showPlayReset;

    protected virtual void OnEnable()
    {
        onPlay = serializedObject.FindProperty("onPlayEvent");
        onRest = serializedObject.FindProperty("onResetEvent");
        EditorApplication.update += UpdateInspector;
    }

    protected virtual void OnDisable()
    {
        EditorApplication.update -= UpdateInspector;
    }

    protected virtual void UpdateInspector() { }

    public override void OnInspectorGUI()
    {
        GUI.backgroundColor = Color.green;
        DrawUnityEventGroup(ref showPlayReset, "Play & Reset Events", onPlay, onRest);
        GUI.backgroundColor = Color.white;
    }

    protected void DrawUnityEventGroup(ref bool bFoldout, string eventName, params SerializedProperty[] args)
    {
        EditorGUILayout.BeginHorizontal("box");
        EditorGUI.indentLevel++;
        bFoldout = EditorGUILayout.Foldout(bFoldout, eventName);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (bFoldout)
        {
            for (int i = 0; i < args.Length; i++)
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(args[i]);
                serializedObject.ApplyModifiedProperties();     // ensure the property can be change
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    protected void DrawPropertyGroup(ref bool bToggle, string title, params SerializedProperty[] args)
    {
        GUI.backgroundColor = Color.yellow;
        EditorGUILayout.BeginHorizontal("box");
        GUI.backgroundColor = Color.white;
        EditorGUI.indentLevel++;
        bToggle = EditorGUILayout.Foldout(bToggle, title);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndHorizontal();

        serializedObject.Update();
        if (bToggle)
        {
            for (int i = 0; i < args.Length; i++)
            {
                EditorGUILayout.PropertyField(args[i]);
                serializedObject.ApplyModifiedProperties();    // ensure the property can be change
            }
        }
    }
}
