using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/***************************************
 * 
 * 此脚本由编辑器类SelectMeshObjects控制
 * 
 ***************************************/


[ExecuteInEditMode]
public class ShowAidInScene : MonoBehaviour {

    public float gizmosRadius;
    public Color gizmosColor = Color.green;

    private void OnDrawGizmos()
    {
        // Inner
        float innerRadius = 0.3f;
        DrawCircle(Color.red, innerRadius, transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, innerRadius);
        // Outer
        Gizmos.color = Color.green * 0.5f;
        Gizmos.DrawSphere(transform.position, gizmosRadius);
        DrawCircle(Color.green, gizmosRadius, transform.position);

    }

    /// <summary>
    /// Draw circle
    /// </summary>
    // Gizmos : Smaller and Smoother
    private float m_Theta = 0.01f;
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
