using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GeneralControl))]
public class GeneralControlEditor : ControlBaseEditor {

    SerializedProperty onAwakeEvent, onEnableEvent, onDisableEvent;
    bool showAwake, showEnable;

    protected override void OnEnable()
    {
        base.OnEnable();
        onAwakeEvent = serializedObject.FindProperty("onAwakeEvent");
        onEnableEvent = serializedObject.FindProperty("onEnableEvent");
        onDisableEvent = serializedObject.FindProperty("onDisableEvent");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DrawUnityEventGroup(ref showAwake, "Awake Events", onAwakeEvent);
        DrawUnityEventGroup(ref showEnable, "OnEnable & OnDisable Events", onEnableEvent, onDisableEvent);
    }
}
