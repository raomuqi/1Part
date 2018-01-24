using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

[CustomEditor(typeof(EffectControl))]
public class EffectControlEditor : ControlBaseEditor
{
    bool showPTEffect, showShaderEffect, showDissolveEvent;

    SerializedProperty effectRoot, onEffectComplete;
    SerializedProperty matRoot, dissolveDelay, dissolveDuration, onDissolveComplete;
    GameObject effectGO, matGO;

    string effectTips, matTips;
    const string invalidTips = "( ---> Unused <--- )";

    SerializedProperty audioEvent;

    protected override void OnEnable()
    {
        base.OnEnable();

        effectRoot = serializedObject.FindProperty("effectRoot");
        onEffectComplete = serializedObject.FindProperty("onEffectComplete");

        matRoot = serializedObject.FindProperty("matRoot");
        dissolveDelay = serializedObject.FindProperty("dissolveDelay");
        dissolveDuration = serializedObject.FindProperty("dissolveDuration");
        onDissolveComplete = serializedObject.FindProperty("onDissolveComplete");

        CheckTips();

        audioEvent = serializedObject.FindProperty("audioEvent");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        DrawPropertyGroup(ref showPTEffect, string.Format("Particle System Effects {0}", effectTips), effectRoot, onEffectComplete);
        DrawPropertyGroup(ref showShaderEffect, string.Format("Shader Effects {0}", matTips), matRoot, dissolveDelay, dissolveDuration, onDissolveComplete);
        if (EditorGUI.EndChangeCheck())
        {
            CheckTips();
        }

        // Audio Event
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(audioEvent);
        serializedObject.ApplyModifiedProperties();
    }

    void CheckTips()
    {
        effectTips = GetInValidTips(effectGO, effectRoot);
        matTips = GetInValidTips(matGO, matRoot);
    }
    string GetInValidTips<T>(T go, SerializedProperty prop) where T : Object
    {
        go = prop.objectReferenceValue as T;
        return go == null ? invalidTips : "";
    }
}
