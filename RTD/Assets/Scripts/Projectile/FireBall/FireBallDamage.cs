﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;



public class FireBallDamage : BulletDamage
{
    bool drawDebugRange = false;
    [SerializeField] float explosionRange;
    [SerializeField] LayerMask targetLayer;

    // Start is called before the first frame update
    void Start()
    {
        InitComponent();
    }

    protected override void SetDamage()
    {
        Debug.Log("SetDamage");

        GameObject target = controller.target;
        if (target == null)
            return;

        FDamageMessage msg = new FDamageMessage();
        msg.Causer = controller.gameObject;
        msg.amount = controller.bulletDmg;
        List<GameObject> hitList = new List<GameObject>();
        if (CharUtils.FindTargetAll(transform, targetLayer, ref hitList, explosionRange))
        {
            foreach (GameObject hitObj in hitList)
            {
                if (hitObj.GetComponent<Damageable>() != null)
                    hitObj.GetComponent<Damageable>().GetDamage(msg);
            }
        }
    }

    protected override void PlayHitEffect()
    {
        if (hitEffect == null)
            return;

        Instantiate(hitEffect, transform.position, transform.rotation);
#if UNITY_EDITOR
        drawDebugRange = true;
#endif
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!drawDebugRange)
            return;

        DebugExtension.DebugCircle(transform.position, explosionRange, 1.0f, true);
        drawDebugRange = false;
#endif
    }
}