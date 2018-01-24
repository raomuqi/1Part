/************************************
 * 控制粒子系统、材质球Shader等特殊效果
 * 
 * 作者：饶牧旗(TA)   时间：2018年01月
 ***********************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class EffectControl : ControlBase
{
    #region Variable
    // all chilren particle system
    [Header("特效对象的父节点"), SerializeField] GameObject effectRoot;
    private ParticleSystem[] childrenPS;
    private bool readyForEffect = true;

    //chilren materials
    [Header("材质对象的父节点"), SerializeField] GameObject matRoot;
    private int childMeshCount;
    private Renderer[] childrenRender;

    private string dissolveName = "_Dissolve", emissionName = "_EmissionIntensity";
    private Tweener dissolveTween, emissionTween;
    private float dissolveProgress, curEmission;
    private float[] initEmissions;
    [Header("消融效果"), SerializeField] float dissolveDelay = 1;
    [SerializeField] float dissolveDuration = 2;

    public UnityEvent onDissolveComplete, onEffectComplete;
    private IEnumerator co_dissolve;

    //氧气罐爆炸的音效
    [SerializeField] AudioEvent audioEvent;
    #endregion

    public override void Initialize()
    {
        base.Initialize();
        InitParam();
    }

    [ContextMenu("开始播放")]
    void TestPlay()
    {
        Play(null);
    }

    [ContextMenu("恢复默认")]
    public override void ToDefault()
    {
        base.ToDefault();

        EffectComplete();
        dissolveTween.Rewind();
        emissionTween.Rewind();

        if (co_dissolve != null)
        {
            StopCoroutine(co_dissolve);
        }
    }

    /// <summary>
    /// 初出茅庐
    /// </summary>
    void InitParam()
    {

        childrenPS = effectRoot == null ? new ParticleSystem[0] : effectRoot.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < childrenPS.Length; i++)
        {
            var main = childrenPS[i].main;
            main.playOnAwake = false;
            main.loop = false;
        }

        childrenRender = matRoot == null ? new Renderer[0] : matRoot.GetComponentsInChildren<Renderer>();
        childMeshCount = childrenRender.Length;
        if (childMeshCount != 0)
        {
            initEmissions = new float[childMeshCount];
            for (int i = 0; i < childMeshCount; i++)
            {

                if (childrenRender[i].material.HasProperty(dissolveName))
                {
                    childrenRender[i].material.SetFloat(dissolveName, 0);
                }

                if (childrenRender[i].material.HasProperty(emissionName))
                {
                    initEmissions[i] = childrenRender[i].material.GetFloat(emissionName);
                    curEmission = initEmissions[i];
                }
            }
        }

        dissolveTween = DOTween.To(() => dissolveProgress, x => dissolveProgress = x, 1, dissolveDuration).SetAutoKill(false).Pause();
        emissionTween = DOTween.To(() => curEmission, x => curEmission = x, 0, 0.3f).SetAutoKill(false).Pause();
    }

    /// <summary>
    /// 血脉贲张
    /// </summary>
    public void StartParticleEffect()
    {
        if (readyForEffect == false)
            return;

        for (int i = 0; i < childrenPS.Length; i++)
        {
            childrenPS[i].Play();
        }

        InvokeRepeating("CheckParticlesAlive", 0.1f, 0.1f);

        //播放氧气罐爆炸的音效
        AudioManager.Instance.Play(audioEvent, gameObject);

        readyForEffect = false;
    }
    void CheckParticlesAlive()
    {
        int aliveNum = 0;
        for (int i = 0; i < childrenPS.Length; i++)
        {
            if (childrenPS[i].isPlaying)
            {
                ++aliveNum;
            }
        }
        if (aliveNum == 0)
        {
            EffectComplete();
        }
    }
    void EffectComplete()
    {
        if (onEffectComplete != null)
        {
            onEffectComplete.Invoke();
        }
        //Debug.Log(readyForEffect);
        readyForEffect = true;
        CancelInvoke();
    }

    /// <summary>
    /// 化为灰烬
    /// </summary>
    public void StartDissolve()
    {
        emissionTween.Rewind();     // ensure emission value not equal 0

        co_dissolve = DelayDissolve();
        StartCoroutine(co_dissolve);
    }

    /// <summary>
    /// 死要瞑目
    /// </summary>
    public void StartEmissionOff()
    {
        emissionTween.Play().OnUpdate(delegate
        {
            SetMatProperty(emissionName, curEmission);
        });

    }

    /// <summary>
    /// Dissolve delay
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayDissolve()
    {
        yield return new WaitForSeconds(dissolveDelay);
        dissolveTween.Play().OnUpdate(delegate
        {
            SetMatProperty(dissolveName, dissolveProgress);
        }).OnComplete(delegate
        {
            if (onDissolveComplete != null)
            {
                onDissolveComplete.Invoke();
            }
        });
    }

    void SetMatProperty(string name, float amount)
    {
        for (int i = 0; i < childMeshCount; i++)
        {
            if (childrenRender[i].material.HasProperty(name))
                childrenRender[i].material.SetFloat(name, amount);
        }
    }

}
