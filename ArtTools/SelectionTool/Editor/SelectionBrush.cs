using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectionBrush : ScriptableWizard
{
    // Gizmos
    Vector3 center = Vector3.zero;
    Vector3 up = Vector3.up;
    [SerializeField, Range(0, 10)] float radius = 1f;

    Ray mouseRay;

    private MeshRenderer[] allMeshs;
    private List<GameObject> selections = new List<GameObject>();

    [MenuItem("JackRao/Brush Selection")]
    static void ShowBrushWindow()
    {
        ScriptableWizard.DisplayWizard("Brush Selection", typeof(SelectionBrush), "Close");
    }

    private void OnWizardCreate() { }

    private void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneView;
        allMeshs = FindObjectsOfType<MeshRenderer>();
        Debug.Log(allMeshs.Length);
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneView;
        selections.Clear();
    }

    private void OnSceneView(SceneView sceneView)
    {
        mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        center = mouseRay.origin + mouseRay.direction;
        up = Camera.current.transform.forward;

        // Disable default square selection of mouse drag
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(0));
        // Disable move tools
        Tools.current = Tool.None;

        // Mouse Drag or click
        if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseUp)
        {
            //selections.Clear();
            for (int i = 0; i < allMeshs.Length; i++)
            {
                // Encapsulation completely
                float disMin = Vector3.Distance(allMeshs[i].bounds.min, center);
                float disMax = Vector3.Distance(allMeshs[i].bounds.max, center);
                GameObject go = allMeshs[i].gameObject;
                if (disMin < radius && disMax < radius && !Event.current.alt)
                {
                    SafeAdd(go);

                    if (Event.current.control)
                    {
                        SafeRemove(go);
                    }

                    //Debug.Log(go.transform.position + go.name + pos);
                }
            }
        }

        // Selection
        Selection.objects = selections.ToArray();

        float size = HandleUtility.GetHandleSize(center) * radius;
        // Gizmos
        Handles.color = new Color(0, 1, 0, 0.2f);
        Handles.DrawSolidDisc(center, up, size);
        Handles.color = new Color(1, 1, 0, 1f);
        Handles.DrawWireDisc(center, up, size);

        sceneView.Repaint();
        Debug.Log(size);
    }

    bool SceneRaycast()
    {
        //Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);   not work

        RaycastHit hit;
        if (Physics.Raycast(mouseRay, out hit))
        {
            //pos = hit.point;

            //up = hit.normal;
            return true;
        }
        else
        {
            return false;
        }
    }

    void SafeAdd(GameObject args)
    {
        if (!selections.Contains(args))
        {
            selections.Add(args);
        }
    }

    void SafeRemove(GameObject args)
    {
        if (selections.Contains(args))
        {
            selections.Remove(args);
        }
    }
}
