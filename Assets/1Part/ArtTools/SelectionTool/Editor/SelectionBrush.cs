using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectionBrush : EditorWindow {

	[MenuItem("JackRao/Brush Selection")]
    static void ShowBrushWindow()
    {
        var window = EditorWindow.GetWindow<SelectionBrush>(false, "Brush Selection");
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
