/************************************
 * Inspector上的UnityEvent常规操作
 * 
 * 作者：饶牧旗(TA)   时间：2018年01月
 ***********************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralControl : ControlBase {

    [SerializeField] UnityEvent onAwakeEvent, onEnableEvent, onDisableEvent;

#region Unity Message
    private void Awake()
    {
        if (onAwakeEvent != null)
            onAwakeEvent.Invoke();
    }
    private void OnEnable()
    {
        if (onEnableEvent != null)
            onEnableEvent.Invoke();
    }
    private void OnDisable()
    {
        if (onDisableEvent != null)
            onDisableEvent.Invoke();
    }
#endregion

    public void Spawn(GameObject source)
    {
        GameObject go = Instantiate(source);
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;
        go.transform.localRotation = Quaternion.identity;
    }
}
