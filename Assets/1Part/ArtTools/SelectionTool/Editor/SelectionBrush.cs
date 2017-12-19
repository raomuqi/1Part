using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectionBrush : EditorWindow {

	[MenuItem("工具/笔刷选择工具")]
    static void ShowBrushWindow()
    {
        var window = EditorWindow.GetWindow<SelectionBrush>(false, "笔刷选择工具");
        window.Show();
    }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneView;
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneView;
    }

    private void OnSceneView(SceneView sceneView)
    {
        Handles.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
