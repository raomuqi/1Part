using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/************************************
 * 作者：饶牧旗(TA)   时间：2017年11月
 ***********************************/
[CustomEditor(typeof(SceneMeasure))]
public class SceneMeasureEditor : Editor
{
    SceneMeasure measure;
    GameObject measureGO;

    string tips = "正在使用测量工具...\n" +
        "鼠标<color=red>右键双击</color>打点\n" +
        "欢迎任何建议---饶牧旗";
    GUIStyle labelStype;

    int linePoint = 0;
    List<Vector3> points = new List<Vector3>();
    float handleSize;

    List<MeshCollider> meshColliders = new List<MeshCollider>();

    private void OnEnable()
    {
        measure = target as SceneMeasure;
        measureGO = measure.gameObject;
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
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Welcome any suggestion -------- Jack Rao(TA)", MessageType.Error);
    }

    public void OnSceneGUI()
    {

        Handles.Label(measureGO.transform.position, tips, labelStype);

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
                //Debug.Log(hit.transform.name);
            }
        }
    }

    void ShowPoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Handles.color = Color.green;
            if ((i + 1) % 2 == 0)
            {
                // Draw line
                Handles.DrawDottedLine(points[i - 1], points[i], 2);
                float dis = Vector3.Distance(points[i - 1], points[i]);
                Handles.Label((points[i - 1] + points[i]) * 0.5f, dis.ToString("f2") + " m", labelStype);
            }
            // Use constant screen size
            handleSize = HandleUtility.GetHandleSize(points[i]) * 0.1f;
            // Use XYZ gizmos postion control
            points[i] = Handles.PositionHandle(points[i], Quaternion.identity);
            Handles.color = Color.yellow;
            // Draw sphere
            Handles.SphereHandleCap(0, points[i], Quaternion.identity, handleSize, EventType.repaint);
        
        }
    }

    void GenerateStyle()
    {
        labelStype = new GUIStyle();
        labelStype.richText = true;
        labelStype.normal.textColor = Color.yellow;
        labelStype.fontStyle = FontStyle.Bold;
        labelStype.fontSize = 16;
        labelStype.alignment = TextAnchor.MiddleCenter;
    }

    private void OnDisable()
    {
        points.Clear();

        // Remove mesh collider
        for (int i = 0; i < meshColliders.Count; i++)
        {
            DestroyImmediate(meshColliders[i]);
        }
        meshColliders.Clear();
    }
}
