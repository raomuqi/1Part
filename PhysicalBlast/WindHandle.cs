using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

[RequireComponent(typeof(PhysicalForce))]
public class WindHandle : MonoBehaviour
{

    #region VARIABLE
    [Header("刚体对象所在的父节点"), SerializeField] Transform rigidbodyRoot;
    private Rigidbody[] childRigidbodys;

    private PhysicalForce forceHandle;
    [Header("风力")] public Vector3 windForce;
    private Vector3 initForce;
    [SerializeField] bool useRandomWind;
    private Vector3 disturbForce;
    [Header("扰动强度"), SerializeField] float distScale = 10;

    private Tweener easeWindTween;
    private Vector3 curProgress;
    private bool bWindOn;

    // Left : wind from left, blow to right
    public UnityEvent fromLeftWindEvent, fromRightWindEvent;
    #endregion

    // Use this for initialization
    void Start()
    {
        forceHandle = GetComponent<PhysicalForce>();
        if (!rigidbodyRoot)
        {
            Debug.LogError("Need assign root of rigidbodys, use self by default", this);
            rigidbodyRoot = transform;
        }
        else
        {
            childRigidbodys = rigidbodyRoot.GetComponentsInChildren<Rigidbody>();
        }

        //SetActiveState();

        initForce = windForce;
    }

    private void FixedUpdate()
    {
        if (!bWindOn) return;

        for (int i = 0; i < childRigidbodys.Length; i++)
        {
            if (useRandomWind)
            {
                disturbForce.Set(Random.Range(-distScale, distScale), Random.Range(-distScale, distScale), Random.Range(-distScale, distScale));
            }
            forceHandle.SetWindForce(childRigidbodys[i], windForce + disturbForce);
        }

        if (easeWindTween != null)
        {
            //Debug.Log("北风吹啊吹..." + curForce);
        }

    }

    /// <summary>
    /// Wind on or off
    /// </summary>
    /// <param name="_bWindOn"></param>
    public void SetWindState(bool _bWindOn)
    {

        if (easeWindTween != null)
        {
            if (easeWindTween.IsPlaying())
            {
                easeWindTween.Kill();
            }
        }

        Vector3 destForce = _bWindOn ? initForce : Vector3.zero;
        float easeTime = _bWindOn ? 2f : 6f;

        easeWindTween = DOTween.To(() => curProgress, x => curProgress = x, destForce, easeTime).OnPlay(delegate
        {
            if (_bWindOn)
            {
                bWindOn = _bWindOn;
            }
        }).OnUpdate(delegate
        {
            windForce = curProgress;
        }).OnComplete(delegate
        {
            if (!_bWindOn)
            {
                bWindOn = _bWindOn;
            }
        });
    }

    /// <summary>
    /// On or Off gameObjects
    /// </summary>
    //void SetActiveState()
    //{
    //    if (SettingManager.Instance == null)
    //    {
    //        Debug.LogError("SettingManager missing", this);
    //        return;
    //    }
    //    else
    //    {
    //        switch (SettingManager.Instance.windDir)
    //        {
    //            case e_wind_dir.Left:
    //                fromLeftWindEvent.Invoke();
    //                break;
    //            case e_wind_dir.Right:
    //                fromRightWindEvent.Invoke();
    //                break;
    //            default:
    //                break;
    //        }

    //        Debug.Log(SettingManager.Instance.windDir);
    //    }

    //}

    public void SetWindLeft()
    {
        if (windForce.z < 0)
        {
            windForce.z *= -1;
        }
    }

    public void SetWindRight()
    {
        if (windForce.z > 0)
        {
            windForce.z *= -1;
        }
    }

    public void ToDefault()
    {
        curProgress = Vector3.zero;
    }

}
