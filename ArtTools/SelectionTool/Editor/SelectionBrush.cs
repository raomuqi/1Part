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

    // Filter  TODO:Current
    [SerializeField, Header("Filter")] bool bStatic = true;
    [SerializeField] bool bDynamic;
    bool bStatusFold;

    Ray mouseRay;

    private MeshRenderer[] allMeshs;
    private List<GameObject> selections = new List<GameObject>();

    [MenuItem("JackRao/Mesh Selection Brush")]
    static void ShowBrushWindow()
    {
        ScriptableWizard.DisplayWizard("Mesh Selection", typeof(SelectionBrush), "Close");
    }

    private void OnWizardCreate() { }

    private void OnEnable()
    {
        Init();
        SceneView.onSceneGUIDelegate += OnSceneView;
        //Debug.Log(allMeshs.Length);
    }

    private void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneView;
        selections.Clear();
    }

    private void OnSceneView(SceneView sceneView)
    {
        float handleSize = HandleUtility.GetHandleSize(center) * radius;
        Vector2 mousePos = Event.current.mousePosition;
        float screenRadius = radius * 100;      // Screen size

        mouseRay = HandleUtility.GUIPointToWorldRay(mousePos);
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
                Vector2 boundMin = HandleUtility.WorldToGUIPoint(allMeshs[i].bounds.min);
                Vector2 boundMax = HandleUtility.WorldToGUIPoint(allMeshs[i].bounds.max);
                float disMin = Vector2.Distance(boundMin, mousePos);
                float disMax = Vector2.Distance(boundMax, mousePos);
                //Debug.Log("DisMin: " + disMin + " DisMax: " + disMax + " Radius: " + radius);

                GameObject go = allMeshs[i].gameObject;
                if (disMin < screenRadius && disMax < screenRadius && !Event.current.alt)
                {
                    SafeAdd(go);

                    if (Event.current.control)
                    {
                        SafeRemove(go);
                    }

                    //Debug.Log(go.transform.position + go.name + pos);
                }
            }
            Selection.objects = selections.ToArray();
        }

        // Selection
        if (Event.current.type == EventType.MouseUp)
        {
        }

        // Gizmos
        Handles.color = new Color(0, 1, 0, 0.2f);
        Handles.DrawSolidDisc(center, up, handleSize);
        Handles.color = new Color(1, 1, 0, 1f);
        Handles.DrawWireDisc(center, up, handleSize);

        sceneView.Repaint();
    }

    void SafeAdd(GameObject args)
    {
        bool bFilter = (bStatic && args.isStatic) || (bDynamic && !args.isStatic);
        if (!selections.Contains(args) && bFilter)
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

    void Init()
    {
        allMeshs = FindObjectsOfType<MeshRenderer>();
        helpString = "Select Static Mesh......";
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical("textArea");
        EditorGUILayout.LabelField("Select static or dynamic mesh using brush tool");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        radius = EditorGUILayout.Slider("Radius of brush", radius, 0, 10);

        bStatusFold = EditorGUILayout.Foldout(bStatusFold, "Mesh filter");
        if (bStatusFold)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUIUtility.labelWidth = 40;
            bStatic = EditorGUILayout.Toggle("Static", bStatic);
            EditorGUIUtility.labelWidth = 60;
            bDynamic = EditorGUILayout.Toggle("Dynamic", bDynamic);
            EditorGUILayout.EndHorizontal();
        }

    }
}
