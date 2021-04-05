using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class TestBulletDamage : MonoBehaviour
{
    ProjectileController controller;

    void Start()
    {
        controller = GetComponent<ProjectileController>();
        controller.HitDel += SetDamage;
    }

    void SetDamage(float bulletDamage)
    {
        Debug.Log("SetDamage");

        GameObject target = controller.target;
        if (target == null)
            return;

        FDamageMessage msg = new FDamageMessage();
        msg.Causer = controller.gameObject;
        msg.amount = bulletDamage;

        target.GetComponent<Damageable>()?.GetDamage(msg);
    }
}
