using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class CharController : MonoBehaviour
{
    // Basic Component
    Rigidbody CharacterRigidbody;
    CharacterStat _statInfo;
    Animator CharacterAnimator;
    AnimEvent CharacterAnimEvent;

    // 함수 진행에 필요한 변수들
    public LayerMask enemyLayer;
    

    // StateMachine
    public BASICSTATE characterState = BASICSTATE.NONE;


    // flags
    [SerializeField, Tooltip("켜주면 공격합니다.")]
    bool _isInField = false;

    // Delay
    float _attackDelay = 0.0f;
    [SerializeField] float destroyDelay = 3.0f;

    // Object
    List<GameObject> Targets;
    GameObject Target = null;

    // Property
    public CharacterStat statInfo
    {
        get { return _statInfo; }
    }


    public bool isInField
    {
        get { return _isInField; }
    }

    [ContextMenu("Attatch To Field")]
    public void AttatchField()
    {
        if (characterState != BASICSTATE.WAIT)
            return;

        _isInField = true;
    }


    // Function
    void Start()
    {
        ChangeState(BASICSTATE.CREATE);
    }

    void Update()
    {
        StateProcess();
    }

    void ChangeState(BASICSTATE state)
    {
        if (characterState == state)
            return;

        characterState = state;
        switch (characterState)
        {
            case BASICSTATE.CREATE:
                Debug.Log("CREATE");
                CharacterRigidbody = GetComponent<Rigidbody>();
                _statInfo = GetComponent<CharacterStat>();
                CharacterAnimator = GetComponentInChildren<Animator>();
                CharacterAnimEvent = GetComponentInChildren<AnimEvent>();
                CharacterAnimEvent.AttackDel += OnAttack;
                GetComponent<Damageable>().onDeadDel += OnDead;
                CharUtils.SettingGradeRing(statInfo.grade, this.transform);
                Targets = new List<GameObject>();
                break;
            case BASICSTATE.POSTCREATE:
                statInfo.UpdateStat();
                Debug.Log("POSTCREATE");
                break;
            case BASICSTATE.WAIT:
                _isInField = false;
                Debug.Log("WAIT");
                break;
            case BASICSTATE.ATTACHFIELD:
                // To Do SomeThing
                ChangeState(BASICSTATE.DETECT);
                Debug.Log("ATTATCH");
                break;
            case BASICSTATE.DETACHFIELD:
                // To Do SomeThing
                Target = null;
                Targets.Clear();
                ChangeState(BASICSTATE.WAIT);
                break;
            case BASICSTATE.DETECT:
                Debug.Log("DETECT");
                break;
            case BASICSTATE.ATTACK:
                CharacterAnimator.SetTrigger("T_Attack");
                break;
            case BASICSTATE.USESKILL:
                break;
            case BASICSTATE.DEAD:
                Destroy(this.gameObject, destroyDelay);
                break;
        }
    }

    void StateProcess()
    {
        switch (characterState)
        {
            case BASICSTATE.CREATE:
                ChangeState(BASICSTATE.POSTCREATE);
                break;
            case BASICSTATE.POSTCREATE:
                ChangeState(BASICSTATE.WAIT);
                break;
            case BASICSTATE.WAIT:
                if (_isInField)
                    ChangeState(BASICSTATE.ATTACHFIELD);
                break;
            case BASICSTATE.DETECT:
                if (!isInField)
                {
                    ChangeState(BASICSTATE.DETACHFIELD);
                    return;
                }    
                
                if (Target != null)
                {
                    if (!CharUtils.IsInRange(this.transform, Target.transform, statInfo.attackRange)
                        || Target.GetComponent<Damageable>().IsDead)
                        Target = null;
                }
                else
                {
                    if (CharUtils.FindTargetAll(this, enemyLayer, ref Targets))
                    {
                        Target = CharUtils.GetCloseTarget(this, Targets);
                    }
                }
                if (Target != null && _attackDelay < Mathf.Epsilon)
                    ChangeState(BASICSTATE.ATTACK);

                break;
            case BASICSTATE.ATTACK:
                // if Target is null 
                if (Target == null)
                {
                    ChangeState(BASICSTATE.DETECT);
                }
                else
                {
                    // Rotate to Target
                    Vector3 dir = Target.transform.position - transform.position;
                    dir.Normalize();
                    Quaternion targetRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10.0f);
                }
                break;
            case BASICSTATE.USESKILL:
                break;
            case BASICSTATE.DEAD:
                break;
        }
    }
    
    public void OnDead()
    {
        ChangeState(BASICSTATE.DEAD);
        // CharacterAnimator.SetBool("isDead", true);
    }

    // @Summary: 공격 애니메이션의 Call Function에서 실행될 델리게이트에 붙여질 함수입니다.
    //           기본적으로는 BasicAttack()의 OnAttack함수를 여기서 호출합니다.
    void OnAttack()
    {
        if (Target == null)
            return;
        
        GetComponent<BasicAttack>().OnAttack(Target);
        StartCoroutine(StartAttack());
    }

    // @Summary: OnAttack이 호출되는 동시에 바로 공격을 할 수 없게 Delay를 시켜주는 코루틴 함수.
    IEnumerator StartAttack()
    {
        // attackDelay = 1초에 몇번 공격할 것인지 따라 다르다.
        _attackDelay = 1.0f / statInfo.attackSpeed;
        while (_attackDelay > Mathf.Epsilon)
        {
            _attackDelay -= Time.deltaTime;
            yield return null;
        }
        _attackDelay = 0.0f;
        ChangeState(BASICSTATE.DETECT);
    }

}


// BasicAttack.cs를 새로 만들자.
