﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class BulletDamage : MonoBehaviour
{
    public GameObject hitEffect;
    protected ProjectileController controller;

    void Start()
    {
        InitComponent();
    }

    protected virtual void InitComponent()
    {
        controller = GetComponent<ProjectileController>();
        controller.HitDel += SetDamage;
    }

    protected virtual void SetDamage()
    {
        Debug.Log("SetDamage");

        GameObject target = controller.target;
        if (target == null)
            return;

        FDamageMessage msg = new FDamageMessage();
        msg.Causer = (controller.owner != null) ? controller.owner : this.gameObject;
        msg.amount = controller.bulletDmg;

        target.GetComponent<Damageable>()?.GetDamage(msg);
        PlayHitEffect();
    }

    protected virtual void PlayHitEffect()
    {
        if (hitEffect == null)
            return;

        Instantiate(hitEffect, transform.position, transform.rotation);
    }
}