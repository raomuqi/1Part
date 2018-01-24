/*=============================================
 *  Jack 饶牧旗
 *  2017/8/23
=============================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// Simulate prop be shock by force
/// </summary>
public class PropShock : PhysicsBase {

    private Joint[] joints;
    private int jointCount;
    private Vector3[] anchors;

    [SerializeField, Header("物理关节所在的游戏对象")] GameObject jointRoot;
    private Rigidbody rig;

    [SerializeField, Header("关节等待冲击的时间")] float waitShock;
    [SerializeField, Header("被打断关节之间的时间间隔")] float breakInterval;

    #region External 
    /// <summary>
    /// Shock prop and make it fly out
    /// </summary>
    /// <param name="onShockComplete"></param>
    public override void Play(System.Action onShockComplete)
    {
        base.Play(onShockComplete);

        if (!rig)
        {
            Debug.Log("Rigidbody missing");
        }
        rig.isKinematic = false;

    }

    /// <summary>
    /// Set to default value
    /// </summary>
    public override void ToDefault()
    {
        base.ToDefault();
        StopCoroutine(co_basePlay);

        if(!rig)
        {
            Debug.Log("Rigidbody missing");
        }
        rig.isKinematic = true;

        // Init position and rotation
        jointRoot.transform.localPosition = Vector3.zero;
        jointRoot.transform.localRotation = Quaternion.identity;

        // Joint
        for (int i = 0; i < jointCount; i++)
        {
            if(null == joints[i])
            {
                joints[i] = jointRoot.AddComponent<HingeJoint>();
                joints[i].anchor = anchors[i];
                joints[i].breakForce = Mathf.Infinity;
            }
        }
    }
    #endregion
    /// <summary>
    /// separate prop from other gameObject by break joint force
    /// use multi-joint component by self now
    /// </summary>
    protected override IEnumerator DelayPlay()
    {
        co_basePlay = base.DelayPlay();
        yield return StartCoroutine(co_basePlay);

        for (int i = 0; i < jointCount; i++)
        {
            if (!joints[i]) continue;

            if (i < jointCount - 1)
            {
                joints[i].breakForce = 0;
                mForce.SetExplosionForce(rig);
            }
            else
            {
                joints[i].breakForce = 0;
                yield return new WaitForSeconds(waitShock);
                mForce.SetExplosionForce(rig);
            }
            yield return new WaitForSeconds(breakInterval);
        }
    }

    /// <summary>
    /// Initialization
    /// </summary>
    public override void Initialize()
    {
        base.Initialize();

        // Rigidbody
        rig = jointRoot.GetComponent<Rigidbody>();
        if (!rig)
        {
            rig = jointRoot.AddComponent<Rigidbody>();
        }

        // Joint
        joints = jointRoot.GetComponents<Joint>();
        jointCount = joints.Length;
        if (jointCount == 0)
        {
            Debug.LogError("Need add joint component");
        }
        anchors = new Vector3[jointCount];
        for (int i = 0; i < jointCount; i++)
        {
            anchors[i] = joints[i].anchor;
        }   
    }


}
