/************************************
 * 作者：饶牧旗(TA)   时间：2017年12月
 ***********************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ControlBase : MonoBehaviour, IGamePlay
{

    // TODO: contextMenuItem not work
    public UnityEvent onPlayEvent, onResetEvent;

    private bool bInitialized;

    private void Start()
    {
        if (!bInitialized)
        {
            Initialize();
            bInitialized = true;
        }
    }

    public virtual void Initialize()
    {
        if (bInitialized) return;
    }

    public virtual void Play(Action onExplosionComplete)
    {
        if (onPlayEvent != null)
            onPlayEvent.Invoke();
    }

    public virtual void ToDefault()
    {
        if (onResetEvent != null)
            onResetEvent.Invoke();
    }

}
