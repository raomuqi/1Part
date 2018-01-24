using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDecalPool : MonoBehaviour
{
    [Header("对象池里的贴花"), SerializeField] int maxDecals = 100;
    [SerializeField] float decalSizeMin = .5f, decalSizeMax = 1.5f;
    [Header("局域坐标上的Y轴偏移量"), SerializeField, Range(-1, 1)] float offsetY;
    [Header("熄火多长时间后，开始回收"), SerializeField] float delayRecycle = 5;

    private ParticleSystem ptDecal;

    private int ptDecalDataIndex;
    private ParticleSystem.Particle[] particles, tempPTs;

    // Use this for initialization
    void Start()
    {
        ptDecal = GetComponent<ParticleSystem>();

        particles = new ParticleSystem.Particle[maxDecals];
    }

    /// <summary>
    /// Place decal
    /// </summary>
    /// <param name="normal"></param>
    /// <param name="pos"></param>
    IEnumerator co_complete;
    public void ParticleHit(Vector3 normal, Vector3 pos)
    {
        if (particles == null)
        {
            Debug.LogError("Need create <color=yellow>" + this + "</color> instance before using....", this);
            return;
        }

        ptDecalDataIndex %= maxDecals;

        if (normal == Vector3.zero) return;
        Vector3 euler = Quaternion.LookRotation(normal).eulerAngles;
        euler.x += 180;
        euler.z = Random.Range(0, 360);

        particles[ptDecalDataIndex].position = pos + normal * offsetY * 0.2f;
        particles[ptDecalDataIndex].rotation3D = euler;
        particles[ptDecalDataIndex].startSize = Random.Range(decalSizeMin, decalSizeMax);
        SetDecalParticle();

        // Recycle
        if (co_complete != null)
        {
            StopCoroutine(co_complete);
        }
        co_complete = DelayRecycle();
        StartCoroutine(co_complete);

        ptDecalDataIndex++;
    }

    /// <summary>
    /// Recycle bullet hole
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayRecycle()
    {
        yield return new WaitForSeconds(delayRecycle);
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].startSize = 0;
            SetDecalParticle();
            yield return new WaitForSeconds(0.3f);
        }
    }

    void SetDecalParticle()
    {
        ptDecal.SetParticles(particles, particles.Length);
    }
}
