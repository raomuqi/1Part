using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BulletEffectManager : MonoBehaviour
{
    private const string metalTag = "Metal";
    private const string concreteTag = "Concrete";
    private const string cementTag = "Cement";
    private const string glassTag = "Glass";
    private const string woodTag = "Wood";

    private ParticleSystem ptLaunch;
    private GameObject decalRoot;

    private List<ParticleCollisionEvent> ptEvents = new List<ParticleCollisionEvent>();

    // Temp
    public QuadDecalPool quadDecal;

    // Decal Type: when add new one, must create instance then use to spawn
    [SerializeField] ParticleDecalPool decalMetal, decalConcrete, decalGlass, decalWood;
    // Use this for initialization
    void Awake()
    {
        ptLaunch = GetComponent<ParticleSystem>();

        if (decalRoot == null)
        {
            decalRoot = new GameObject("BulletHole Decals For (" + gameObject.name + ")");
            decalRoot.transform.parent = transform.parent;
        }

        /***************** 
        * Create instance
        *****************/
        decalMetal = GenerateDecalInstance(decalMetal);
        decalConcrete = GenerateDecalInstance(decalConcrete);
        decalGlass = GenerateDecalInstance(decalGlass);
        decalWood = GenerateDecalInstance(decalWood);
    }

    #region OUTSIDE
    // Generate decal via bullet Collision & use parent transform as bullet position and rotation
    public void LaunchBullet()
    {
        ptLaunch.Emit(1);
    }

    // Generate decal via bullet Collision
    public void LaunchBullet(Vector3 pos, Vector3 rot = default(Vector3))
    {
        ptLaunch.transform.position = pos;
        ptLaunch.transform.rotation = Quaternion.Euler(rot);
        ptLaunch.Emit(1);
    }

    // Manual generate decal
    public void SpawnDecal(string tag, Vector3 normal, Vector3 pos)
    {
        switch (tag)
        {
            /***************** 
            * Use instance
            *****************/
            case metalTag:
                PlaceDecal(decalMetal, normal, pos); break;
            case concreteTag:
                PlaceDecal(decalConcrete, normal, pos); break;
            case cementTag:
                PlaceDecal(decalConcrete, normal, pos); break;
            case glassTag:
                PlaceDecal(decalGlass, normal, pos); break;
            case woodTag:
                PlaceDecal(decalWood, normal, pos); break;
            default:
                break;
        }
        //Debug.Log(tag);
    }

    #endregion

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(ptLaunch, other, ptEvents);

        for (int i = 0; i < ptEvents.Count; i++)
        {
            //SpawnDecal(other.tag, ptEvents[i].normal, ptEvents[i].intersection);
           quadDecal.SpawnDecal(other, ptEvents[i].normal, ptEvents[i].intersection);
        }
    }

    void PlaceDecal(ParticleDecalPool decal, Vector3 normal, Vector3 pos)
    {
        if (decal != null)
        {
            decal.ParticleHit(normal, pos);
        }
        else
        {
            Debug.Log("<color=yellow>" + this + "</color> some decal not used", this);
        }
    }

    ParticleDecalPool GenerateDecalInstance(ParticleDecalPool decal)
    {
        if (decal == null) return null;
        ParticleDecalPool ptDecal = Instantiate(decal);
        ptDecal.transform.parent = decalRoot.transform;
        return ptDecal;
    }
}
