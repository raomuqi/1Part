/*=============================================
 *  作者：饶牧旗
 *  时间：2017年8月23日20:49:37
=============================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxisConstrain
{
    public bool x = true;
    public bool y = true;
    public bool z = true;
}

/// <summary>
/// Constrained by target with position and rotation
/// Enable or Disable this component to on/off this functionality
/// </summary>
public class MatchTransform : MonoBehaviour
{

    [SerializeField] Transform parentTransform;

    private Vector3 startParentPosition;
    private Quaternion startParentRotationQ;
    private Vector3 startParentScale;

    private Vector3 startChildPosition;
    private Quaternion startChildRotationQ;
    private Vector3 startChildScale;

    private Matrix4x4 parentMatrix;

    private readonly float[] midPos = { 0, 0, 0 };
    public AxisConstrain posConstrain;
    [SerializeField] bool rotConstrain = true;

    [SerializeField] bool matchScale, useAbsoluteTransform;

    private void OnEnable()
    {
        if (!parentTransform)
        {
            parentTransform = transform;
            Debug.Log("<color=red><b>[Tips]</b></color>Assign match target, if not use current by default");
        }

        InitTransform();
    }

    // Update is called once per frame
    void Update()
    {
        MatchTargetTran();
    }

    /// <summary>
    /// match target's position and rotation
    /// </summary>
    void MatchTargetTran()
    {
        parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.lossyScale);

        if (useAbsoluteTransform)
        {
            transform.position = GetSuitedPosition(parentTransform.position);
            transform.rotation = GetSuitedRotation(parentTransform.rotation);
        }
        else
        {
            // Parent constrain
            transform.position = GetSuitedPosition(parentMatrix.MultiplyPoint3x4(startChildPosition));
            transform.rotation = GetSuitedRotation((parentTransform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ);
        }

        // Incorrect scale code; it scales the child locally not gloabally; Might work in some cases, but will be inaccurate in others
        if (matchScale)
        {
            transform.localScale = Vector3.Scale(startChildScale, DivideVectors(parentTransform.lossyScale, startParentScale));
        }
    }

    /// <summary>
    /// Initialize all transform value
    /// </summary>
    void InitTransform()
    {
        startParentPosition = parentTransform.position;
        startParentRotationQ = parentTransform.rotation;
        startParentScale = parentTransform.lossyScale;

        startChildPosition = transform.position;
        startChildRotationQ = transform.rotation;
        startChildScale = transform.lossyScale;

        // Keeps child position from being modified at the start by the parent's initial transform
        startChildPosition = DivideVectors(Quaternion.Inverse(parentTransform.rotation) * (startChildPosition - startParentPosition), startParentScale);
    }

    void InitParam()
    {
        posConstrain.x = true;
        posConstrain.y = true;
        posConstrain.z = true;

        rotConstrain = true;
    }

    Vector3 DivideVectors(Vector3 num, Vector3 den)
    {
        return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z);
    }

    Vector3 GetSuitedPosition(Vector3 pos)
    {
        Vector3 args;
        args.x = posConstrain.x ? pos.x : transform.position.x;
        args.y = posConstrain.y ? pos.y : transform.position.y;
        args.z = posConstrain.z ? pos.z : transform.position.z;
        return args;
    }

    Quaternion GetSuitedRotation(Quaternion rot)
    {
        /*********************
         * Single axis rotation ?
         * ******************/
        //Vector3 args;
        //args.x = rotConstrain.x ? rot.eulerAngles.x : transform.eulerAngles.x;
        //args.y = rotConstrain.y ? rot.eulerAngles.y : transform.eulerAngles.y;
        //args.z = rotConstrain.z ? rot.eulerAngles.z : transform.eulerAngles.z;

        return rotConstrain ? rot : transform.rotation;
    }
}
