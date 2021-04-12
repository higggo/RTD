using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityAction AttackDel;
    public UnityAction BasicAttackEffectDel;
    public UnityAction DeadDel;
    public UnityAction MainSkillDel;

    public void OnAttack()
    {
        AttackDel?.Invoke();
        BasicAttackEffectDel?.Invoke();
    }

    public void OnDead()
    {
        DeadDel?.Invoke();
    }

    public void OnSkillFire()
    {
        MainSkillDel?.Invoke();
    }
}
