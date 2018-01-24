using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectTest : MonoBehaviour
{

    [SerializeField] BulletEffectManager bullet;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //Vector3 mousePos = Input.mousePosition;
            //mousePos.z = 1;
            //Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (bullet != null)
                {
                    //bullet.LaunchBullet(mousePos);
                    bullet.SpawnDecal(hit.transform.tag, hit.normal, hit.point);
                }
                else
                {
                    Debug.LogError("<color=yellow>" + this + "</color> : " + "BulletManager reference missing...", this);
                }
            }

        }
    }
}
