﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ProjectileKit;

namespace ProjectileKit
{
    public enum STATE
    {
        CREATE, READY, WAIT, SHOOT, HIT
    }
}


public class ProjectileController : MonoBehaviour
{
    [SerializeField] protected STATE _bulletState = STATE.CREATE;
    [SerializeField] protected float _bulletDmg;
    [SerializeField] protected float _bulletSpeed = 0.0f;
    [SerializeField] protected bool _isTargeting = true;

    protected GameObject _target = null;
    public UnityAction<Transform, float> moveDel = null;
    public UnityAction<float> HitDel = null;

    // Trigger
    public bool fireTrigger = false;

    // property
    public float bulletDmg
    {
        get
        {
            if (_bulletDmg < Mathf.Epsilon)
                _bulletDmg = 1;

            return _bulletDmg;
        }
        set
        {
            if (value < Mathf.Epsilon)
                value = 1;

            _bulletDmg = value;
        }
    }
    
    public float bulletSpeed
    {
        get
        {
            if (_bulletSpeed < Mathf.Epsilon)
                _bulletSpeed = 1;

            return _bulletSpeed;
        }
        set
        {
            if (value < Mathf.Epsilon)
                value = 1;

            _bulletSpeed = value;
        }
    }

    public GameObject target
    {
        get { return _target; }
    }


    void Awake()
    {
        _bulletState = STATE.CREATE;
    }

    // Basic Func    
    void Start()
    {

    }

    void Update()
    {
        StateProcess();
    }

    // Colision Set
    void OnTriggerEnter(Collider other)
    {
        if (!_isTargeting)
            return;

        Debug.Log("충돌");

        if (other.gameObject == target)
            ChangeState(STATE.HIT);
    }

    // STATE MACHINE

    void ChangeState(STATE state)
    {
        if (_bulletState == state)
            return;

        _bulletState = state;

        switch (_bulletState)
        {
            case STATE.CREATE:
                break;
            case STATE.READY:
                break;
            case STATE.SHOOT:
                moveDel?.Invoke(target.transform, bulletSpeed);
                break;
            case STATE.HIT:
                HitDel?.Invoke(bulletDmg);
                Destroy(this.gameObject, 0.1f);
                break;
        }
    }

    void StateProcess()
    {
        switch (_bulletState)
        {
            case STATE.CREATE:
                break;
            case STATE.READY:
                if (fireTrigger)
                    ChangeState(STATE.SHOOT);
                break;
            case STATE.SHOOT:
                break;
            case STATE.HIT:
                break;
        }
    }



    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 

    /// <summary>
    /// 이 함수를 총알 생성 직후에 바로 call 해주면 됩니다.
    /// </summary>
    /// <param name="target">공격 대상</param>
    /// <param name="bulletDmg">공격력</param>
    /// <param name="bulletSpeed">총알 속도(Time.deltaTime * bulletSpeed)</param>
    public void InitBullet(GameObject target, float bulletDmg, float bulletSpeed)
    {
        Debug.Log("InitBullet");
        if (!(_bulletState == STATE.CREATE || _bulletState == STATE.WAIT))
            return;


        
        if (target == null)
            Debug.Log("Target is Null!");

        _target = target;
        this.bulletDmg = bulletDmg;
        this.bulletSpeed = bulletSpeed;
        ChangeState(STATE.READY);
    }

    /// <summary>
    /// bullet의 state를 HIT로 바꿉니다.
    /// </summary>
    public void SetHit()
    {
        if (_bulletState != STATE.SHOOT)
            return;

        Debug.Log("SetHIT");
        ChangeState(STATE.HIT);
    }


}
