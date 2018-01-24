using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadDecal : MonoBehaviour
{
    [Header("纹理图集"), SerializeField, Range(1, 10)] int row = 1;
    [SerializeField, Range(1, 10)] int column = 1;
    private int itemRow = 1, itemColumn = 1;        // define single element

    [SerializeField, Header("贴花对象")] float size = 1;
    [SerializeField, Range(0, 0.1f)] float offsetY = 0;
    [SerializeField] float duration = 3;

    private Mesh mesh;
    bool useRandom = true;

    // when disable, reset parent transform
    public Action<Transform> onDisappear; 

    // Use this for initialization
    void Awake()
    {
        mesh = GetComponentInChildren<MeshFilter>().mesh;
        if (transform.childCount > 1)
        {
            Debug.LogError("GetChild(0) used");
        }
        else
        {
            transform.GetChild(0).localPosition = new Vector3(0, offsetY, 0);
        }
    }

    void SetMeshUV()
    {
        itemRow = useRandom ? UnityEngine.Random.Range(1, row + 1) : 1;
        itemColumn = useRandom ? UnityEngine.Random.Range(1, column + 1) : 1;

        Vector2[] uv_0 = new Vector2[4] { new Vector2(0, 0), new Vector2(1.0f / row, 1.0f / column), new Vector2(1.0f / row, 0), new Vector2(0, 1.0f / column) };  // left bottom corner, must use "1.0f/row" convert to float
        Vector2[] uv_new = new Vector2[4];
        for (int i = 0; i < 4; i++)             // 4 vertex
        {
            uv_new[i] = uv_0[i] + new Vector2((itemColumn - 1) * 1.0f / column, (itemRow - 1) * 1.0f / row);
            //Debug.Log(uv_new[i]);
        }
        mesh.uv = uv_new;
    }

    private void OnEnable()
    {
        SetMeshUV();

        transform.localScale = Vector3.one * size * UnityEngine.Random.Range(0.6f, 1.2f);
        transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);

        Invoke("DelayHide", duration);
    }

    void DelayHide()
    {
        onDisappear(transform);
        gameObject.SetActive(false);
    }
}
