using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent_Dragon : AnimEvent
{
    public UnityAction AttackInAirDel;
    public UnityAction SkillInAirStartDel;
    public UnityAction SkillInAirEndDel;

    public void OnAttackInAir()
    {
        AttackInAirDel?.Invoke();
    }

    public void OnSkillInAir()
    {
        SkillInAirStartDel?.Invoke();
    }

    public void OnSkillInAirEnd()
    {
        SkillInAirEndDel?.Invoke();
    }
}
