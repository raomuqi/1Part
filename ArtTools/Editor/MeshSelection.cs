/************************************
 * 作者：饶牧旗(TA)   时间：2017年12月
 ***********************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Linq;

public class MeshSelection : EditorWindow
{
    #region Variable

    // Brush
    Vector3 center = Vector3.zero;
    Vector3 up = Vector3.up;
    float radius = 1f;
    bool bBrush;
    Ray mouseRay;
    //bool bSceneView;   // Whether use operation on sceneView

    // GUI
    string immediateTips = "";
    StringBuilder meshTips = new StringBuilder();
    string meshRichTips = "<color=green><b>{0:####.#}</b></color>";
    string[] toolBarStr = { "Brush", "Size", "Material" };
    int tbSelect = 1;
    GUIStyle keyTextStyle;

    // Filter status
    bool bStatic = true;
    bool bDynamic;
    bool bStatusFold = true;

    // Filter size
    Vector2 minMaxSize;
    float maxBoundsSize, curMinSize, curMaxSize;

    // Filter Material
    Vector2 scrollPos;
    Dictionary<MeshRenderer, int> meshMats = new Dictionary<MeshRenderer, int>();
    Vector2 minMaxMats = new Vector2(0, 1);

    // Selection
    private MeshRenderer[] allMeshes;
    private int sceneMeshCount, staticCount, dynamicCount;
    private List<GameObject> selections = new List<GameObject>();       // Must define GameOject to show selection

    #endregion

    [MenuItem("JackRao/Mesh Selection", false, 22)]
    static void ShowWindow()
    {
        var window = GetWindow(typeof(MeshSelection), false, "Mesh Selection");
        //args.position = new Rect(500, 20, 500, 200);
        window.minSize = new Vector2(350, 200);
        window.maxSize = new Vector2(600, 600);
        window.Show();
    }

    private void Awake()
    {
        Init();
        SceneView.onSceneGUIDelegate += OnSceneView;
    }

    void Init()
    {
        allMeshes = FindObjectsOfType<MeshRenderer>();
        sceneMeshCount = allMeshes.Length;

        GetMaxBoundsSize();

        keyTextStyle = new GUIStyle();
        keyTextStyle.richText = true;
        keyTextStyle.alignment = TextAnchor.MiddleCenter;
    }

    // when rename / create / destroy ... GameObject
    private void OnHierarchyChange()
    {
        Init();
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
            EditorGUI.indentLevel++;

            GUI.backgroundColor = Color.green;
            EditorGUILayout.BeginHorizontal("box");
            GUI.backgroundColor = Color.white;
            EditorGUIUtility.labelWidth = 52;
            bStatic = EditorGUILayout.Toggle("Static", bStatic);
            EditorGUIUtility.labelWidth = 70;
            bDynamic = EditorGUILayout.Toggle("Dynamic", bDynamic);
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        // Button group for operation of selections
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("All"))
        {
            for (int i = 0; i < sceneMeshCount; i++)
            {
                SafeAdd(allMeshes[i].gameObject);
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
                if (selections.Contains(allMeshes[i].gameObject))
                {
                    SafeRemove(allMeshes[i].gameObject);
                }
                else
                {
                    SafeAdd(allMeshes[i].gameObject);
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
                FilterWithSize();
                immediateTips = "Size Filter";
                break;
            case 2:
                // Material
                FilterWithMats();
                immediateTips = "Material Filter";
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

        FilterWithBrush();

        Selection.objects = selections.ToArray();

        sceneView.Repaint();    // Refresh scene view
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

    // Brush selection
    void FilterWithBrush()
    {
        if (bBrush == false) return;

        // Handle
        float handleSize = HandleUtility.GetHandleSize(center) * radius;
        Vector2 mousePos = Event.current.mousePosition;
        float screenRadius = radius * 100;      // Screen size

        // Ray
        mouseRay = HandleUtility.GUIPointToWorldRay(mousePos);
        center = mouseRay.origin + mouseRay.direction;
        up = Camera.current.transform.forward;

        // Disable default square selection of mouse drag & click selection
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
                Vector2 boundMin = HandleUtility.WorldToGUIPoint(allMeshes[i].bounds.min);
                Vector2 boundMax = HandleUtility.WorldToGUIPoint(allMeshes[i].bounds.max);
                float disMin = Vector2.Distance(boundMin, mousePos);
                float disMax = Vector2.Distance(boundMax, mousePos);
                //Debug.Log("DisMin: " + disMin + " DisMax: " + disMax + " Radius: " + radius);

                GameObject go = allMeshes[i].gameObject;
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

        Repaint();      // Refresh GUI content
    }

    // select GO within range of bounds size
    void FilterWithSize()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUIUtility.labelWidth = 50;
        EditorGUILayout.PrefixLabel("Range:");

        EditorGUI.BeginChangeCheck();
        float minMaxLabelWidth = 40;
        curMinSize = Mathf.Clamp(EditorGUILayout.FloatField(Mathf.Round(curMinSize), GUILayout.Width(minMaxLabelWidth)), 0, curMaxSize - 1);
        EditorGUILayout.LabelField(string.Format(meshRichTips, minMaxSize.x), keyTextStyle, GUILayout.Width(minMaxLabelWidth));
        EditorGUILayout.MinMaxSlider(ref minMaxSize.x, ref minMaxSize.y, curMinSize, curMaxSize);
        EditorGUILayout.LabelField(string.Format(meshRichTips, minMaxSize.y), keyTextStyle, GUILayout.Width(minMaxLabelWidth));
        EditorGUIUtility.labelWidth = .1f;
        curMaxSize = Mathf.Clamp(EditorGUILayout.FloatField(Mathf.Round(curMaxSize), GUILayout.Width(minMaxLabelWidth)), curMinSize + 1, maxBoundsSize);
        if (EditorGUI.EndChangeCheck())
        {
            minMaxSize.x = minMaxSize.x < curMinSize ? curMinSize : minMaxSize.x;
            minMaxSize.y = minMaxSize.y > curMaxSize ? curMaxSize : minMaxSize.y;
        }

        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Do it", GUILayout.Width(50)))
        {
            selections.Clear();
            float min = minMaxSize.x - 0.01f;    // Ensure object be wrapped
            float max = minMaxSize.y + 0.01f;    // Ensure object be wrapped
            for (int i = 0; i < sceneMeshCount; i++)
            {
                Vector3 size = allMeshes[i].bounds.size;
                // as long as one size within range 
                float maxEdge = Mathf.Max(size.x, Mathf.Max(size.y, size.z));
                if (maxEdge > min && maxEdge < max)
                {
                    SafeAdd(allMeshes[i].gameObject);
                }
                //Debug.Log(RichText.Show(size));
            }
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndHorizontal();
    }

    // Get mesh and materials
    void FilterWithMats()
    {
        meshMats.Clear();
        int maxCount = 1;

        for (int i = 0; i < allMeshes.Length; i++)
        {
            if (allMeshes[i].gameObject.isStatic == bStatic || !allMeshes[i].gameObject.isStatic == bDynamic)
            {
                int mats = allMeshes[i].sharedMaterials.Length;
                // material slot may be NULL
                for (int j = 0; j < allMeshes[i].sharedMaterials.Length; j++)
                {
                    if(allMeshes[i].sharedMaterials[j] == null)
                    {
                        mats--;
                    }
                }
                // check
                maxCount = maxCount < mats ? mats : maxCount;
                if (mats >= (int)minMaxMats.x && mats <= (int)minMaxMats.y)
                {
                    meshMats.Add(allMeshes[i], mats);
                }
                //Debug.Log(mats + "_" + minMaxMats.x);
            }
        }

        EditorGUILayout.BeginHorizontal();
        float minMaxLabeWidth = 20;
        EditorGUILayout.LabelField("Range: 0 --->", GUILayout.Width(85));
        EditorGUILayout.LabelField(string.Format(meshRichTips, (int)minMaxMats.x), keyTextStyle, GUILayout.Width(minMaxLabeWidth));
        EditorGUILayout.MinMaxSlider(ref minMaxMats.x, ref minMaxMats.y, 0, maxCount);
        EditorGUILayout.LabelField(string.Format(meshRichTips, (int)minMaxMats.y), keyTextStyle, GUILayout.Width(minMaxLabeWidth));
        EditorGUILayout.LabelField(string.Format("<--- {0}", maxCount.ToString()), GUILayout.Width(80));
        EditorGUILayout.EndHorizontal();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // TODO: the editor was very slow with the drag operation
        foreach (var item in meshMats.Keys)
        {
            EditorGUILayout.BeginHorizontal("box");
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(item.gameObject.name))
            {
                selections.Clear();
                SafeAdd(item.gameObject);
            }
            GUI.backgroundColor = Color.white;
            EditorGUILayout.LabelField("------> Materials: ", GUILayout.Width(110));
            EditorGUILayout.IntField(meshMats[item], GUILayout.Width(20));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndScrollView();
        //GUI.enabled = false;
        //GUI.enabled = true;
    }

    // Get Mesh type tips
    void GetMeshTips()
    {
        staticCount = 0;
        dynamicCount = 0;
        for (int i = 0; i < selections.Count; i++)
        {
            if (selections[i] == null) continue;      // Maybe be deleted in hierarchy

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
        curMaxSize = 3;
        maxBoundsSize = curMaxSize;
        for (int i = 0; i < sceneMeshCount; i++)
        {
            Vector3 size = allMeshes[i].bounds.size;
            float[] args = new float[3] { size.x, size.y, size.z };
            foreach (var item in args)
            {
                maxBoundsSize = Mathf.Max(maxBoundsSize, item);
            }
        }
        minMaxSize = new Vector2(0, curMaxSize * 0.5f);
    }

    private void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneView;
        selections.Clear();
    }
}
