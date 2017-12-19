using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectMeshObjects : ScriptableWizard {

    [Header("目标")] public Transform target;
    [Header("半径")] public float radius = 5;
    private List<GameObject> selections = new List<GameObject>();
    private ShowAidInScene aidObject;
    [MenuItem("JackRao/Select GameObjects")]
    static void DisplayInfo()
    {
        ScriptableWizard.DisplayWizard("选择一定范围内所有带MeshRender组件的静态物体", typeof(SelectMeshObjects), "关闭", "搜索");
    }

    // When new window is opened
    private void Awake()
    {
        // Make the first selection as Target
        if(Selection.transforms.Length == 1)
        {
            target = Selection.transforms[0];
        }
    }

    private void OnWizardCreate()
    {
        RemoveAidComponent();
    }

    private void OnWizardOtherButton()
    {
        MakeSelection();
    }

    private void OnWizardUpdate()
    {
        helpString = "目标附近一定范围内";

        SetAidGizmos();

        ShowTips();
    }

    // 选中所有符合条件的对象
    void MakeSelection()
    {
        // 清空先
        selections.Clear();

        var gos = (Renderer[])GameObject.FindObjectsOfType<Renderer>();
        foreach(var args in gos)
        {
            // 添加在以target为圆心，radius为半径的球体内，被完整包裹的物体，
            float dis = Vector3.Distance(args.bounds.center, target.position) + args.bounds.extents.magnitude;
            if (dis <= radius && args.gameObject.isStatic)
            {
                selections.Add(args.gameObject);
            }
        }

        if(selections.Count == 0)
        {
            errorString = "当前范围内没找到合适的【静态】对象，需要扩大搜索范围";
        }
        else
        {
            errorString = "搜索完毕！共找到 " + selections.Count + " 个【静态】对象";
        }

        Selection.objects = selections.ToArray();
        //Debug.Log(Selection.objects.Length);

    }

    bool bOnceTargetSet;
    // 设置场景辅助对象
    void SetAidGizmos()
    {
        if (target != null)
        {
            if (!bOnceTargetSet)            //只初始化一次
            {
                aidObject = target.GetComponent<ShowAidInScene>();
                if (aidObject == null)
                {
                    aidObject = target.gameObject.AddComponent<ShowAidInScene>();
                }

                aidObject.gizmosColor = Color.green;
                bOnceTargetSet = true;
            }

        }
        else
        {
            bOnceTargetSet = false;
        }

        // 更改半径
        if (null != aidObject)
        {
            aidObject.gizmosRadius = radius;
            
        }

    }

    // 显示提示信息
    void ShowTips()
    {
        if (null == target) {
            errorString = "需要设置 目标";
            return;
        }

        if(radius <= 0)
        {
            errorString = "需要设置 半径";
            return;
        }
        else
        {
            errorString = "温馨提示：只针对【静态】对象";
        }

    }

    private void OnDestroy()
    {
        RemoveAidComponent();
    }

    // 移除场景中用来作视觉辅助的组件
    void RemoveAidComponent()
    {
        // 移除组件
        if(aidObject != null)
        {
            DestroyImmediate(aidObject);
        }
    }
}
