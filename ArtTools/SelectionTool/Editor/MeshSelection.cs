/************************************
 * 作者：饶牧旗(TA)   时间：2017年12月
 ***********************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text;

public class MeshSelection : EditorWindow
{
    #region Variable

    // Gizmos of brush
    Vector3 center = Vector3.zero;
    Vector3 up = Vector3.up;
    float radius = 1f;
    bool bBrush;

    // GUI
    string immediateTips = "";
    StringBuilder meshTips = new StringBuilder();
    string meshRichTips = "<color=green><b>{0:####.#}</b></color>";
    string[] toolBarStr = { "Brush", "Size" };
    int tbSelect = 1;
    GUIStyle keyTextStyle;

    // Filter status
    bool bStatic = true;
    bool bDynamic;
    bool bStatusFold;

    // Filter size
    Vector2 minMax;
    float maxBoundsSize, curMin, curMax;

    Ray mouseRay;

    // Selection
    private MeshRenderer[] allMeshs;
    private int sceneMeshCount, staticCount, dynamicCount;
    private List<GameObject> selections = new List<GameObject>();       // Must define GameOject to show selection

    #endregion

    [MenuItem("JackRao/Mesh Selection")]
    static void ShowWindow()
    {
        var window = GetWindow(typeof(MeshSelection), false, "Mesh Selection");
        //args.position = new Rect(500, 20, 500, 200);
        window.minSize = new Vector2(350, 200);
        window.maxSize = new Vector2(600, 300);
        window.Show();
    }

    private void Awake()
    {
        Init();
        SceneView.onSceneGUIDelegate += OnSceneView;
    }

    void Init()
    {
        allMeshs = FindObjectsOfType<MeshRenderer>();
        sceneMeshCount = allMeshs.Length;

        GetMaxBoundsSize();

        keyTextStyle = new GUIStyle();
        keyTextStyle.richText = true;
        keyTextStyle.alignment = TextAnchor.MiddleCenter;
    }

    /// <summary>
    /// GUI appearance
    /// </summary>
    private void OnGUI()
    {
        // Label Info
        EditorGUILayout.BeginVertical("textArea");
        EditorGUILayout.LabelField("Select " + meshTips + " [" + immediateTips + "]", keyTextStyle);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        // Filter mesh status
        bStatusFold = EditorGUILayout.Foldout(bStatusFold, "Mesh Type");
        if (bStatusFold)
        {
            EditorGUILayout.BeginHorizontal("box");
            EditorGUIUtility.labelWidth = 40;
            bStatic = EditorGUILayout.Toggle("Static", bStatic);
            EditorGUIUtility.labelWidth = 60;
            bDynamic = EditorGUILayout.Toggle("Dynamic", bDynamic);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        // Button group for operation of selections
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("All"))
        {
            for (int i = 0; i < sceneMeshCount; i++)
            {
                SafeAdd(allMeshs[i].gameObject);
            }
        }
        if (GUILayout.Button("None"))
        {
            selections.Clear();
        }
        if (GUILayout.Button("Invert"))
        {
            for (int i = 0; i < sceneMeshCount; i++)
            {
                if (selections.Contains(allMeshs[i].gameObject))
                {
                    SafeRemove(allMeshs[i].gameObject);
                }
                else
                {
                    SafeAdd(allMeshs[i].gameObject);
                }
            }
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.Space();
        // Selection tool switch
        tbSelect = GUILayout.Toolbar(tbSelect, toolBarStr);
        bBrush = tbSelect == 0 ? true : false;
        GUILayout.Space(6);
        switch (tbSelect)
        {
            case 0:
                // Brush
                EditorGUIUtility.labelWidth = 50;
                radius = EditorGUILayout.Slider("Radius", radius, 0, 5);
                immediateTips = "Brush Tool";
                break;
            case 1:
                // Size
                EditorGUILayout.BeginHorizontal();
                EditorGUIUtility.labelWidth = 50;
                EditorGUILayout.PrefixLabel("Range:");

                EditorGUI.BeginChangeCheck();
                float minMaxLabelWidth = 40;
                curMin = Mathf.Clamp(EditorGUILayout.FloatField(Mathf.Round(curMin), GUILayout.Width(minMaxLabelWidth)), 0, curMax - 1);
                EditorGUILayout.LabelField(string.Format(meshRichTips, minMax.x), keyTextStyle, GUILayout.Width(minMaxLabelWidth));
                EditorGUILayout.MinMaxSlider(ref minMax.x, ref minMax.y, curMin, curMax);
                EditorGUILayout.LabelField(string.Format(meshRichTips, minMax.y), keyTextStyle, GUILayout.Width(minMaxLabelWidth));
                EditorGUIUtility.labelWidth = .1f;
                curMax = Mathf.Clamp(EditorGUILayout.FloatField(Mathf.Round(curMax), GUILayout.Width(minMaxLabelWidth)), curMin + 1, maxBoundsSize);
                if (EditorGUI.EndChangeCheck())
                {
                    minMax.x = minMax.x < curMin ? curMin : minMax.x;
                    minMax.y = minMax.y > curMax ? curMax : minMax.y;
                }

                GUI.backgroundColor = Color.green;
                if (GUILayout.Button("Do it", GUILayout.Width(50)))
                {
                    AddProperSizeMesh();
                }
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                immediateTips = "Size Filter";
                break;
        }
        EditorGUILayout.Space();

        // Close
        GUILayout.FlexibleSpace();
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Close", GUILayout.Height(30)))
        {
            this.Close();
        }
        EditorGUILayout.Space();

        GetMeshTips();
    }

    // Scene View Draw
    private void OnSceneView(SceneView sceneView)
    {
        // Handle
        float handleSize = HandleUtility.GetHandleSize(center) * radius;
        Vector2 mousePos = Event.current.mousePosition;
        float screenRadius = radius * 100;      // Screen size

        // Ray
        mouseRay = HandleUtility.GUIPointToWorldRay(mousePos);
        center = mouseRay.origin + mouseRay.direction;
        up = Camera.current.transform.forward;

        if (bBrush == true)
        {
            // Disable default square selection of mouse drag
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(0));
            // Disable move tools
            Tools.current = Tool.None;

            // Mouse Drag or click
            if (Event.current.type == EventType.MouseDrag || Event.current.type == EventType.MouseUp)
            {
                //selections.Clear();
                for (int i = 0; i < sceneMeshCount; i++)
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
            }

            // Gizmos
            Handles.color = new Color(0, 1, 0, 0.2f);
            Handles.DrawSolidDisc(center, up, handleSize);
            Handles.color = new Color(1, 1, 0, 1f);
            Handles.DrawWireDisc(center, up, handleSize);
        }

        Selection.objects = selections.ToArray();
        sceneView.Repaint();

        Repaint();      // Refresh GUI content
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

    // select GO within range of bounds size
    void AddProperSizeMesh()
    {
        selections.Clear();
        float min = minMax.x - 0.1f;    // Ensure object be wrapped
        float max = minMax.y + 0.1f;    // Ensure object be wrapped
        for (int i = 0; i < sceneMeshCount; i++)
        {
            Vector3 size = allMeshs[i].bounds.size;
            // as long as one size within range 
            float maxEdge = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
            if (maxEdge > min && maxEdge < max)
            {
                SafeAdd(allMeshs[i].gameObject);
            }
            //Debug.Log(RichText.Show(size));
        }
    }

    // Get Mesh type tips
    void GetMeshTips()
    {
        staticCount = 0;
        dynamicCount = 0;
        for (int i = 0; i < selections.Count; i++)
        {
            if (selections[i].isStatic)
            {
                staticCount++;
            }
            else
            {
                dynamicCount++;
            }
        }

        meshTips.Length = 0;
        if (staticCount != 0)
        {
            meshTips.AppendFormat(meshRichTips, staticCount);
            meshTips.AppendFormat(" Static Meshes");
        }
        if (staticCount != 0 && dynamicCount != 0)
        {
            meshTips.Append(" & ");
        }
        if (dynamicCount != 0)
        {
            meshTips.AppendFormat(meshRichTips, dynamicCount);
            meshTips.AppendFormat(" Dynamic Meshes");
        }
        if (staticCount == 0 && dynamicCount == 0)
        {
            meshTips.Append("nothing");
        }
    }

    void GetMaxBoundsSize()
    {
        for (int i = 0; i < sceneMeshCount; i++)
        {
            Vector3 size = allMeshs[i].bounds.size;
            float[] args = new float[3] { size.x, size.y, size.z };
            foreach (var item in args)
            {
                maxBoundsSize = Mathf.Max(maxBoundsSize, item);
            }
        }
        curMax = 3;
        minMax = new Vector2(0, curMax * 0.5f);
    }

    private void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneView;
        selections.Clear();
    }
}
