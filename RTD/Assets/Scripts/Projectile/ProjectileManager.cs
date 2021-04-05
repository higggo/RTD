using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;

    public void FireProjectile(Vector3 StartPos, GameObject target, float bulletDamage)
    {
        GameObject obj = Instantiate(bullet, StartPos, Quaternion.identity);
        obj.GetComponent<ProjectileController>().InitBullet(target, bulletDamage, bulletSpeed);
        obj.GetComponent<ProjectileController>().fireTrigger = true;
    }
}
