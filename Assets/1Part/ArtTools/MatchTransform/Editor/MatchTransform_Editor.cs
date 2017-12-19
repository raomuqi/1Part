using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(AxisConstrain))]
public class ConstrainPropertyDraw : PropertyDrawer
{
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        float elementWidth = 30;
        EditorGUI.PrefixLabel(pos, new GUIContent("Position Constrain Axis"));
        Rect nRect = pos;
        nRect.x += pos.width/2;
        nRect.width = elementWidth;

        DrawElement(nRect, prop, "x");
        nRect.x += elementWidth;
        DrawElement(nRect, prop, "y");
        nRect.x += elementWidth;
        DrawElement(nRect, prop, "z");

        Debug.Log(pos.x);
    }

    void DrawElement(Rect rect, SerializedProperty prop, string axis)
    {
        EditorGUI.PropertyField(rect, prop.FindPropertyRelative(axis), GUIContent.none);
        rect.x += 15;
        EditorGUI.LabelField(rect, axis.ToUpper());
    }

}

[CustomEditor(typeof(MatchTransform))]
public class MatchTransform_Editor : Editor
{

    private MatchTransform mTarget;
    private string[] tbNames = {"Relative Constrain", "Absolute Constrain"};
    private int tbSelect = 0;

    private void OnEnable()
    {
        mTarget = target as MatchTransform;
        //mTarget = serializedObject.targetObject as MatchTransform;
        //_matchScale = serializedObject.FindProperty("matchScale").boolValue;
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        base.OnInspectorGUI();
        tbSelect = GUILayout.Toolbar(tbSelect, tbNames);

        switch (tbSelect)
        {
            case 0:
                //EditorGUILayout.PropertyField(mTarget.posConstrain);
                break;
            case 1:
                break;
            default:
                break;
        }


    }

}
