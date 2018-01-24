/************************************
 * 作者：饶牧旗(TA)   时间：2017年8月
 ***********************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulate physical force
/// </summary>
[ExecuteInEditMode]
public class PhysicalForce : MonoBehaviour
{

    [SerializeField, Header("力场的中心位置"), Tooltip("默认使用当前对象")] public Transform forceCenter;
    [SerializeField, Header("力场的偏移量")] public Vector3 forceOffset;
    [SerializeField, Header("力场的强度")] public float forceIntensity = 5;
    [SerializeField, Header("力场的半径")] public float forceRadius = 2;
    private Vector3 forcePos;

    [Header("怪物受力参数")]
    public float applyEnemyForce = 40;        // TODO: temporary use for c/s synch
    public float applyEnemyRadius = 5.0f;

    // Gizmos : Smaller and Smoother
    private float m_Theta = 0.01f;

    private void Start()
    {
        if (forceCenter == null)
        {
            forceCenter = transform;
        }
    }

    private void Update()
    {
        forcePos = forceCenter.TransformPoint(forceOffset);
    }

    /// <summary>
    /// 爆炸力场
    /// > forceScale: 力的预乘倍数，可于此加入随机
    /// </summary>
    /// <param name="力作用的刚体"></param>
    /// <param name="力的预乘倍数，可于此加入随机"></param>
    /// <param name="力作用的模式"></param>
    public void SetExplosionForce(Rigidbody rig, float forceScale = 1, ForceMode mode = ForceMode.Impulse)
    {
        if (forceScale < 0) forceScale = 1;
        float intensity = forceScale * forceIntensity;
        rig.AddExplosionForce(intensity, forcePos, forceRadius, 0, mode);
    }

    /// <summary>
    /// 风场
    /// </summary>
    /// <param name="rig"></param>
    /// <param name="force"></param>
    public void SetWindForce(Rigidbody rig, Vector3 force)
    {
        rig.AddForce(force);
    }

    /// <summary>
    /// Aid navigating scene with force range adjusted
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        //Vector3 worldPos = transform.TransformPoint(forceOffset);

        // Inner
        float innerRadius = 0.08f * forceRadius;
        DrawCircle(Color.red, innerRadius, forcePos);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(forcePos, innerRadius);
        // Outer
        Gizmos.color = Color.green * 0.5f;
        Gizmos.DrawSphere(forcePos, forceRadius);
        DrawCircle(Color.green, forceRadius, forcePos);
    }

    /// <summary>
    /// Draw circle
    /// </summary>
    /// <param name="m_Color"></param>
    /// <param name="radius"></param>
    /// <param name="pos"></param>
    void DrawCircle(Color m_Color, float radius, Vector3 pos)
    {
        // Set matrix
        Matrix4x4 defaultMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.LookAt(pos, Camera.current.transform.position, transform.up);

        // Set color
        Color defaultColor = Gizmos.color;
        Gizmos.color = m_Color;

        // Draw Circle
        Vector3 beginPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;
        for (float theta = 0; theta < 2 * Mathf.PI; theta += m_Theta)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 endPoint = new Vector3(x, y, 0);
            if (theta == 0)
            {
                firstPoint = endPoint;
            }
            else
            {
                Gizmos.DrawLine(beginPoint, endPoint);
            }
            beginPoint = endPoint;
        }
        // draw last line segment
        Gizmos.DrawLine(firstPoint, beginPoint);
        // Recover default color
        Gizmos.color = m_Color;
        // Recover default matrix
        Gizmos.matrix = defaultMatrix;
    }
}
