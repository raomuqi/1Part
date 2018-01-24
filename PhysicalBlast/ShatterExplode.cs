using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ShatterExplode : PhysicsBase
{

    #region Variable

    [SerializeField, Header("模型碎片父节点")] Transform shatterRoot;
    [SerializeField, Header("是否增加随机性")] bool useRandom = true;
    private Vector3 initShatterRootPos;
    private Quaternion initShatterRootRot;

    // Shatters
    private GameObject[] children;
    private Rigidbody[] m_rigs;
    private Collider[] m_colliders;
    private int childCount;         // Shatter Count
    private Vector3[] initPositions;
    private Quaternion[] initRots;
    [Header("是否一直显示Mesh"), SerializeField] bool alwaysShow = true;

    // Use DOTween as Timer
    private Tweener timeTween;
    private float progress;

    #endregion

    #region External Communication

    public override void Play(System.Action onExplodeComplete)             // Use System.Action
    {
        base.Play(onExplodeComplete);

        // Shatter
        if (useRandom)
        {
            Vector3 localVector = shatterRoot.eulerAngles;
            localVector.y += Random.Range(-180, 180);
            shatterRoot.rotation = Quaternion.Euler(localVector);
        }

        if (!alwaysShow)
        {
            SetMeshStatus(true);
        }
        SetDynamicStatus(true);

    }

    public override void ToDefault()
    {
        base.ToDefault();

        if (!alwaysShow)
        {
            SetMeshStatus(false);
        }
        SetDynamicStatus(false);

        // To default position and rotation
        for (int i = 0; i < childCount; i++)
        {
            children[i].transform.localPosition = initPositions[i];
            children[i].transform.localRotation = initRots[i];
        }

        // shatter root
        shatterRoot.localPosition = initShatterRootPos;
        shatterRoot.localRotation = initShatterRootRot;

        // Timer
        timeTween.Rewind();

        //Debug.Log("ToDefault");
    }

    public override void Initialize()
    {
        base.Initialize();

        // Shatter
        if (!shatterRoot)
        {
            Debug.LogError("需要指定碎片模型所在的根对象", this);
            return;
        }
        childCount = shatterRoot.childCount;

        children = new GameObject[childCount];
        m_rigs = new Rigidbody[childCount];
        m_colliders = new Collider[childCount];

        // Default Value
        initPositions = new Vector3[childCount];
        initRots = new Quaternion[childCount];

        // Dynamic Component
        for (int i = 0; i < childCount; i++)
        {
            children[i] = shatterRoot.GetChild(i).gameObject;
            if (children[i] == null)
            {
                Debug.LogError("碎片模型丢失", this);
            }

            // Ensure component required exist
            m_rigs[i] = children[i].GetComponent<Rigidbody>();
            if (m_rigs[i] == null)
            {
                m_rigs[i] = children[i].AddComponent<Rigidbody>();
            }
            m_colliders[i] = children[i].GetComponent<Collider>();
            if (m_colliders[i] == null)
            {
                m_colliders[i] = children[i].AddComponent<BoxCollider>();
            }

            // Store default position and rotation
            initShatterRootPos = shatterRoot.localPosition;
            initShatterRootRot = shatterRoot.localRotation;
            initPositions[i] = children[i].transform.localPosition;
            initRots[i] = children[i].transform.localRotation;

        }

        SetDynamicStatus(false);

    }

    #endregion

    protected override IEnumerator DelayPlay()
    {
        yield return StartCoroutine(base.DelayPlay());

        float randomForceTimes = useRandom ? Random.Range(0.8f, 1.2f) : 1;

        for (int i = 0; i < children.Length; i++)
        {
            mForce.SetExplosionForce(m_rigs[i], randomForceTimes);
        }

        //Debug.Log("<color=yellow>XXXXXXXXXXXXXXXXXXX</color>", this);
    }


    // Shift Dynamic Simulation
    private void SetDynamicStatus(bool isActive)
    {
        for (int i = 0; i < childCount; i++)
        {
            m_colliders[i].enabled = isActive;
            m_rigs[i].useGravity = isActive;
            m_rigs[i].isKinematic = !isActive;      // Warning: Dynamic & Kinematic
        }
    }

    private void SetMeshStatus(bool isActive)
    {
        for (int i = 0; i < childCount; i++)
        {
            children[i].GetComponent<MeshRenderer>().enabled = isActive;
        }
    }
}
