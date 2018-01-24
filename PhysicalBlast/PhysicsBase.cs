using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhysicalForce))]
public abstract class PhysicsBase : MonoBehaviour, IGamePlay
{
    protected PhysicalForce mForce;
    protected bool bInitialized, bReadyPlay;
    public UnityEvent onPlayEvent, onExplosionEvent, onDefaultEvent, onDelayCompleteEvent;
    protected System.Action onEffectCompleteEvent;

    [SerializeField, Header("延长多长时间后开始发力")] protected float waitTimeForce = 0.01f;
    [SerializeField, Header("发力后，手动触发完成的事件回调")] protected float delayComplete = 8f;
    protected IEnumerator co_delayPlay, co_basePlay, co_delayComplete;

#if UNITY_EDITOR
    //Testing in Editor
    [ContextMenu("Test Play")]
    public virtual void _TestPlay()
    {
        Play(delegate
        {
            Debug.Log("<color=yellow><b>[Tips]Play completed</b></color>", this);
        });
    }
    [ContextMenu("Test Default")]
    public virtual void _TestDefault()
    {
        ToDefault();
    }
#endif

    #region External Communication

    /// <summary>
    /// Play Effect
    /// </summary>
    /// <param name="onExplosionComplete"></param>
    public virtual void Play(Action onAllComplete)
    {
        if (!bReadyPlay) return;
        if(onPlayEvent != null)
        {
            onPlayEvent.Invoke();
        }
        
        // Delay for Explosion
        co_delayPlay = DelayPlay();
        StartCoroutine(co_delayPlay);


        onEffectCompleteEvent = onAllComplete;
    }

    /// <summary>
    /// when not be initialized external
    /// </summary>
    protected void Awake()
    {
        if (!bInitialized)
        {
            Initialize();
            bInitialized = true;
            bReadyPlay = true;
        }
    }

    /// <summary>
    /// Set all to default value
    /// </summary>
    public virtual void ToDefault()
    {
        // Delay play
        if (co_delayPlay != null)
        {
            StopCoroutine(co_delayPlay);
        }

        // Delay complete
        if(co_delayComplete != null)
        {
            StopCoroutine(co_delayComplete);
        }

        if(onDefaultEvent != null)
        {
            onDefaultEvent.Invoke();
        }

        bReadyPlay = true;
    }
    /// <summary>
    /// Initialization
    /// </summary>
    public virtual void Initialize()
    {
        if (bInitialized) return;
        mForce = GetComponent<PhysicalForce>();
        if (!mForce)
        {
            Debug.LogError("PhysicalForce Missing");
            mForce = gameObject.AddComponent<PhysicalForce>();
        }
    }

    #endregion

    /// <summary>
    /// When all effect complete, call this. 
    /// Designed for inspector UnityEvent call now
    /// </summary>
    public void OnEffectComplete()
    {
        if(onEffectCompleteEvent != null)
        {
            onEffectCompleteEvent();
        }
    }

    protected virtual IEnumerator DelayPlay()
    {
        yield return new WaitForSeconds(waitTimeForce);
        //Debug.Log("***Ready for explosion**", this);
        if(onExplosionEvent != null)
        {
            onExplosionEvent.Invoke();
        }

        // Delay for complete, if not use effect
        co_delayComplete = DelayComplete();
        StartCoroutine(co_delayComplete);

        bReadyPlay = false;
    }

    protected virtual IEnumerator DelayComplete()
    {
        yield return new WaitForSeconds(delayComplete);
        if (onDelayCompleteEvent != null)
        {
            onDelayCompleteEvent.Invoke();
        }
    }
}
