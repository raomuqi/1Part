using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuadDecalPool : MonoBehaviour
{
    private const string metalTag = "Metal";
    private const string concreteTag = "Concrete";
    private const string cementTag = "Cement";
    private const string woodTag = "Wood";

    [SerializeField] int decalAmount = 30;
    [SerializeField] GameObject decalMetal, decalConcrete, decalWood;
    private GameObject[] decalMetals, decalConcretes, decalWoods;
    private int indexMetal, indexConcrete, indexWood;

    private void Start()
    {
        GenerateDecals(decalMetal, out decalMetals);
        GenerateDecals(decalConcrete, out decalConcretes);
        GenerateDecals(decalWood, out decalWoods);
    }

    public void SpawnDecal(GameObject hitGO, Vector3 normal, Vector3 pos)
    {
        switch (hitGO.tag)
        {
            case metalTag:
                PlaceDecal(ref indexMetal, decalMetals, hitGO, normal, pos); break;
            case concreteTag:
            case cementTag:
                PlaceDecal(ref indexConcrete, decalConcretes, hitGO, normal, pos); break;
            case woodTag:
                PlaceDecal(ref indexWood, decalWoods, hitGO, normal, pos); break;
            default:
                break;
        }
    }

    GameObject curDecalGO;
    private void PlaceDecal(ref int index, GameObject[] decals, GameObject hitGO, Vector3 normal, Vector3 pos)
    {
        curDecalGO = decals[index++ % decalAmount];
        if (!curDecalGO.activeSelf)
            curDecalGO.SetActive(true);
        curDecalGO.transform.position = pos;
        curDecalGO.transform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        curDecalGO.transform.parent = hitGO.transform;
        //Debug.Log(index);
    }

    void GenerateDecals(GameObject source, out GameObject[] targets)
    {
        targets = new GameObject[decalAmount];
        for (int i = 0; i < decalAmount; i++)
        {
            targets[i] = Instantiate(source) as GameObject;
            targets[i].transform.parent = transform;
            targets[i].GetComponent<QuadDecal>().onDisappear += ResetDecalToChild;
            targets[i].SetActive(false);
        }
    }

    void ResetDecalToChild(Transform childTran)
    {
        childTran.parent = transform;
    }
}
