using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityAction AttackDel;
    public UnityAction DeadDel;


    public void OnAttack()
    {
        AttackDel?.Invoke();
    }

    public void OnDead()
    {
        DeadDel?.Invoke();
    }
}
