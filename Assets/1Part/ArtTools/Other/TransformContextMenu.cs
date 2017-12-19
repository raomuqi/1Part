
/************************************
 * 
 *Extract a certain keyframe of animation as pose
 * 
 * 作者：饶牧旗(TA)   时间：2017年12月
 ***********************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TransformContextMenu
{

    private class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public TransformData(Vector3 p, Quaternion r, Vector3 s)
        {
            position = p;
            rotation = r;
            scale = s;
        }
    }

    private static Dictionary<Transform, TransformData> childrenTranDatas = new Dictionary<Transform, TransformData>();
    private static int selectionCount;

    [MenuItem("CONTEXT/Transform/Copy Children Transform")]
    static void CopyChildrenTran()
    {
        childrenTranDatas.Clear();

        var childrenTrans = Selection.activeTransform.GetComponentsInChildren<Transform>();
        selectionCount = childrenTrans.Length;
        foreach (var item in childrenTrans)
        {
            childrenTranDatas.Add(item, new TransformData(item.localPosition, item.localRotation, item.localScale));
        }
    }

    [MenuItem("CONTEXT/Transform/Paste Children Transform")]
    static void PasteChildrenTran()
    {
        var childrenTrans = Selection.activeTransform.GetComponentsInChildren<Transform>();
        Undo.RegisterCompleteObjectUndo(childrenTrans, "Paste Children Transform");
        for (int i = 0; i < childrenTrans.Length; i++)
        {
            if (childrenTranDatas.ContainsKey(childrenTrans[i]))
            {
                childrenTrans[i].localPosition = childrenTranDatas[childrenTrans[i]].position;
                childrenTrans[i].localRotation = childrenTranDatas[childrenTrans[i]].rotation;
                childrenTrans[i].localScale= childrenTranDatas[childrenTrans[i]].scale;
            }
            else
            {
                EditorUtility.DisplayDialog("拷贝/粘贴子物体的变换值", "血型不匹配，你选择了不同的物体?", "知道了");
                return;
            }
        }
    }
}
