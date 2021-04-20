using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;

    public void FireProjectile(Vector3 StartPos, GameObject owner, GameObject target, float bulletDamage)
    {
        GameObject obj = Instantiate(bullet, StartPos, Quaternion.identity);
        obj.GetComponent<ProjectileController>().InitBullet(owner, target, bulletDamage, bulletSpeed);
        obj.GetComponent<ProjectileController>().fireTrigger = true;
    }
}
