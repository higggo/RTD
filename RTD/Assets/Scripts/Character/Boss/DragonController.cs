using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class DragonController : BossController
{
    // BossController.SetCanAction(bool)으로 행동 제어 가능


    public enum STATE
    {
        NONE, CREATE, POSTCREATE,
        WAIT, MOVE, FLYSTART,
        DETECT, ATTACK,
        READYSKILL, USESKILL,
        DEAD
    }

    // 추가 정보
    [SerializeField] int Level;
    [SerializeField] float _fireBallDamage;
    [SerializeField] float detectRange;
    [SerializeField] float flyAttackSpeed;
    float attackDelay = 0.0f;

    AnimatorStateInfo info;
    
    // STATE MACHINE
    STATE dragonState = STATE.NONE;

    // flags
    bool isInAir = false;
    bool isAttackNow = false;

    // property
    public float fireBallDamage
    {
        get 
        { 
            if (_fireBallDamage <= 0.0f)
                _fireBallDamage = 1.0f;
            
            return _fireBallDamage;
        }
    }

    public float DetectRange
    {
        get { return detectRange; }
    }


    // 타겟을 발견하면 1/4 확률로 타겟을 변경?
    // 타겟을 발견하면 기본공격이 우선
    // 스킬 쿨타임이 다 차면, 스킬 사용. (Ground일때, Sky일때 다름)

    [ContextMenu("_canAction")]
    void SetAction()
    {
        _canAction = true;
    }





    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.CREATE);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }


    protected override void InitComponents()
    {
        base.InitComponents();

        // 추가
    }

    void ChangeState(STATE state)
    {
        if (dragonState == state)
            return;

        dragonState = state;
        switch (dragonState)
        {
            case STATE.CREATE:
                Debug.Log("Dragon : CREATE");
                InitComponents();
                AnimEvent_Dragon tmpEvent;
                if (bossAnimEvent is AnimEvent_Dragon)
                {
                    tmpEvent = bossAnimEvent as AnimEvent_Dragon;
                    tmpEvent.AttackDel += OnAttack;
                    tmpEvent.AttackInAirDel += OnAttack;
                }
                ChangeState(STATE.POSTCREATE);
                break;
            case STATE.POSTCREATE:
                statInfo.UpdateStat(Level);
                ChangeState(STATE.WAIT);
                break;
            case STATE.WAIT:
                break;
            case STATE.FLYSTART:
                StartCoroutine(Fly());
                break;
            case STATE.MOVE:
                break;
            case STATE.DETECT:
                break;
            case STATE.ATTACK:
                bossAnimator.SetTrigger("T_Basic Attack");
                break;
            case STATE.READYSKILL:
                break;
            case STATE.USESKILL:
                bossAnimator.SetTrigger("T_Skill");
                break;
            case STATE.DEAD:
                Destroy(this.gameObject, destroyDelay);
                break;
        }
    }

    void StateProcess()
    {
        // phase one
        switch (dragonState)
        {
            case STATE.CREATE:
                break;
            case STATE.POSTCREATE:
                break;
            case STATE.WAIT:
                if (canAction)
                    ChangeState(STATE.DETECT);
                break;
            case STATE.MOVE:
                break;
            case STATE.DETECT:
                if (!canAction)
                    ChangeState(STATE.WAIT);

                if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                {
                    if (!CharUtils.IsInRange(this.transform, _target.transform, detectRange))
                        _target = null;
                }
                else
                {
                    _target = null;
                    if (!CharUtils.FindTarget(this.transform, enemyLayer, detectRange, out _target))
                    {
                        ChangeState(STATE.WAIT);
                    }
                }

                // use to skill
                if (_skillController != null)
                {
                    if (_skillController.canUseSkill)
                    {
                        ChangeState(STATE.READYSKILL);
                        break;
                    }
                }

                if (_target != null && !_target.GetComponent<Damageable>().IsDead)
                {
                    if (!CharUtils.IsInRange(this.transform, _target.transform, statInfo.attackRange))
                    {
                        // Move
                    }
                    else
                    {
                        ChangeState(STATE.ATTACK);
                    }
                }

                break;
            case STATE.ATTACK:
                if (_target == null || _target.GetComponent<Damageable>().IsDead)
                {
                    ChangeState(STATE.DETECT);
                }
                else
                {
                    // Rotate
                    Vector3 TargetPos = _target.transform.position;
                    TargetPos.y = transform.position.y;
                    Vector3 dir = TargetPos - transform.position;
                    dir.Normalize();
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10.0f);
                }
                break;
            case STATE.READYSKILL:
                if (_skillController.PrepareSkill())
                    ChangeState(STATE.USESKILL);
                    break;
            case STATE.USESKILL:
                if (!_skillController.canUseSkill)
                    ChangeState(STATE.DETECT);
                break;
            case STATE.DEAD:
                break;
        }
    }

    public void StartFly()
    {
        if (isInAir)
            return;

        ChangeState(STATE.FLYSTART);
    }

    IEnumerator Fly()
    {
        while (!info.IsName("Dragon Fly Idle(W)") && !isDead)
        {
            UpdateAnimInfo();
            yield return null;
        }

        isInAir = true;
        ChangeState(STATE.WAIT);
    }

    void UpdateAnimInfo()
    {
        info = bossAnimator.GetCurrentAnimatorStateInfo(0);
    }


    void OnAttack()
    {
        
        if (_target == null)
            return;

        Debug.LogWarning("OnAttack : Dragon");
        StartCoroutine(StartAttack());
    }

    IEnumerator StartAttack()
    {
        if (!isInAir)
            _attackDelay = 1.0f / statInfo.attackSpeed;
        else
            _attackDelay = 1.0f / flyAttackSpeed;

        while (_attackDelay > Mathf.Epsilon)
        {
            _attackDelay -= Time.deltaTime;
            yield return null;
        }
        _attackDelay = 0.0f;
        ChangeState(STATE.DETECT);
    }
}
