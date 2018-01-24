/************************************
 * 控制渲染、物理、碰撞、位置、方位等状态
 * 
 * 作者：饶牧旗(TA)   时间：2018年01月
 ***********************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusControl : ControlBase {


    [SerializeField] Renderer[] renders;
    [SerializeField] Collider[] colliders;
    [SerializeField] Rigidbody[] rigidbodys;

    Vector3 initPos;
    Quaternion initRot;

    public override void Initialize()
    {
        base.Initialize();
        initPos = transform.position;
        initRot = transform.rotation;
    }

    #region For Inpsector
    public void OnRenderStatus(bool bActive)
    {
        foreach (var item in renders)
        {
            item.enabled = bActive;
        }
    }

    public void OnDynamicStatus(bool bDynamic)
    {
        foreach (var item in rigidbodys)
        {
            item.isKinematic = !bDynamic;
        }

        foreach (var item in colliders)
        {
            item.enabled = bDynamic;
        }
    }

    public void OnDefaultTran()
    {
        transform.position = initPos;
        transform.rotation = initRot;
    }
    #endregion

}
