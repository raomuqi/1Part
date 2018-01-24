using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/************************************
 * 作者：饶牧旗(TA)   时间：2017年11月
 ***********************************/
public class SceneMeasureEditor : ScriptableWizard
{

    string tips = "欢迎任何反馈---饶牧旗_TA";
    GUIStyle labelStype;

    int linePoint = 0;
    List<Vector3> points = new List<Vector3>();
    List<int> controlIDs = new List<int>();
    int curID;

    [SerializeField, Range(0, 1)] float pointScale = 0.1f;
    float handleSize;

    List<MeshCollider> meshColliders = new List<MeshCollider>();

    [MenuItem("JackRao/Measure Tool", false, 22)]
    static void MeasureWindow()
    {
        ScriptableWizard.DisplayWizard("Scene Measure Tool", typeof(SceneMeasureEditor), "Close", "Clear");
    }

    /// <summary>
    /// When press "Cancel" button
    /// </summary>
    private void OnWizardCreate() { }

    private void OnWizardOtherButton() { points.Clear(); }

    private void OnWizardUpdate()
    {
        helpString = "欢迎任何反馈---饶牧旗_TA";
        errorString = "正在使用测量工具，在模型上鼠标右键双击";
    }

    private void OnEnable()
    {
        GenerateStyle();

        // Add mesh collider
        var meshFilters = Object.FindObjectsOfType<MeshFilter>();
        for (int i = 0; i < meshFilters.Length; i++)
        {
            if (!meshFilters[i].GetComponent<Collider>())
            {
                meshColliders.Add(meshFilters[i].gameObject.AddComponent<MeshCollider>());
            }
        }

        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    public void OnSceneGUI(SceneView sceneView)
    {
        SceneRaycast();
        ShowPoints();

    }

    void SceneRaycast()
    {
        if (Event.current.type == EventType.mouseDown && Event.current.button == 1 && Event.current.clickCount > 1)
        {
            //Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);   not work
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                linePoint = (linePoint++) % 2;
                points.Add(hit.point);
                controlIDs.Add(0);
                //Debug.Log(hit.transform.name);
            }
        }
    }

    void ShowPoints()
    {

        for (int i = 0; i < points.Count; i++)
        {
            Handles.color = Color.yellow;
            if ((i + 1) % 2 == 0)
            {
                // Draw line
                Handles.DrawDottedLine(points[i - 1], points[i], 2);
                float dis = Vector3.Distance(points[i - 1], points[i]);
                Handles.Label((points[i - 1] + points[i]) * 0.5f, dis.ToString("f2") + " m", labelStype);
            }
            // Use constant screen size
            handleSize = HandleUtility.GetHandleSize(points[i]) * pointScale;
            // Use XYZ gizmos postion control
            points[i] = Handles.FreeMoveHandle(points[i], Quaternion.identity, 1f, Vector3.one * 0.01f, (id, pos, rot, size, eventType) =>
            {
                controlIDs[i] = id;
                Handles.color = Color.green;
                Handles.SphereHandleCap(id, pos, rot, handleSize, eventType);
                if (GUIUtility.keyboardControl == id && GUIUtility.keyboardControl != 0)
                {
                    curID = id;
                }
            });
            //CustomSphereCap(i, i, points[i], Quaternion.identity, handleSize, EventType.Layout);
        }

        // Show move gizmos
        for (int i = 0; i < points.Count; i++)
        {
            if (controlIDs[i] == curID)
            {
                points[i] = Handles.PositionHandle(points[i], Quaternion.identity);
            }
        }
        //Debug.Log(GUIUtility.hotControl);           // Get ContorlID of current handles

        HandleUtility.Repaint();    // fast respond modification
    }

    // TODO: Remove freeMoveHandle to reserve snap
    void CustomSphereCap(int index, int id, Vector3 pos, Quaternion rot, float size, EventType eventType)
    {
        controlIDs[index] = id;
        Handles.SphereHandleCap(0, pos, rot, size, eventType);
        if (GUIUtility.keyboardControl == id && GUIUtility.keyboardControl != 0)
        {
            curID = id;
        }
    }

    void GenerateStyle()
    {
        labelStype = new GUIStyle();
        labelStype.richText = true;
        labelStype.normal.textColor = Color.green;
        labelStype.fontStyle = FontStyle.Bold;
        labelStype.fontSize = 16;
        labelStype.alignment = TextAnchor.MiddleCenter;
    }

    private void OnDisable()
    {
        points.Clear();
        controlIDs.Clear();

        // Remove mesh collider
        for (int i = 0; i < meshColliders.Count; i++)
        {
            DestroyImmediate(meshColliders[i]);
        }
        meshColliders.Clear();
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }
}
