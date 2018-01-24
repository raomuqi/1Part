using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalReaction : PhysicalForce {

    [SerializeField] LayerMask impactLayer;

    TriggerGear[] dustGears;
    //LampControl[] lamps;

    private void Start()
    {
        dustGears = FindObjectsOfType<TriggerGear>();
        //lamps = FindObjectsOfType<LampControl>();
    }

    /// <summary>
    /// 爆炸力场，用于Inpspector UnityEvent
    /// TODO : 基于性能考虑，需要重构
    /// </summary>
    /// <param name="rig"></param>
    public void OnExplosionReaction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, forceRadius, impactLayer);

        foreach (var item in colliders)
        {
            Rigidbody rig = item.GetComponent<Rigidbody>();
            if (rig != null && !rig.isKinematic)
            {
                SetExplosionForce(rig);
            }
        }

        #region TODO : temporary use for this project indoor "VRZombie"
        //for (int i = 0; i < dustGears.Length; i++)
        //{
        //    dustGears[i].Play();
        //}

        //for (int i = 0; i < lamps.Length; i++)
        //{
        //    lamps[i].LightEaseOff(0.5f);
        //}
        #endregion

        Camera.main.GetComponent<SF_PPE_DamageReact>().ShakeCamera(2f, 2f);
    }
}
