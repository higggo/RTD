using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;


public class SkillController_Dragon : SkillController
{

    int skillCount = 0;
    [SerializeField] int skillEndCount;
    
    GameObject skillTarget;
    DragonController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<DragonController>();
        InitComponents();
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    protected override void InitComponents()
    {
        base.InitComponents();
        skillEndCount = (skillEndCount <= 0) ? 1 : skillEndCount;
        if (animEvent is AnimEvent_Dragon)
        {
            AnimEvent_Dragon tmpEvent = animEvent as AnimEvent_Dragon;
            tmpEvent.SkillInAirStartDel += SkillOnInAir;
            tmpEvent.SkillInAirEndDel += base.EndUseSkill;
        }
    }

    public override void EndUseSkill()
    {
        base.EndUseSkill();
        if (skillCount < skillEndCount - 1)
            skillCount++;
        else
        {
            skillCount = 0;
            ResetRemainCoolTime();
        }
    }


    protected override void SkillOn()
    {
        ProjectileManager manager = GetComponent<ProjectileManager>();
        float fireballDmg = controller.fireBallDamage;
        manager.FireProjectile(SkillParticleStartPos.position, skillTarget, fireballDmg);
        _readyToShot = false;
    }

    void SkillOnInAir()
    {
        
    }

    void SkillReadyInAir()
    {

    }

    public sealed override bool PrepareSkill()
    {
        if (controller == null)
        {
            Debug.LogWarning("SkillController_Dragon : controller ref is Null!");
            return false;
        }

        if (CharUtils.FindRandomTarget(transform, controller.EnemyLayer, controller.DetectRange, out skillTarget))
        {
            _readyToShot = true;
            StartCoroutine(StartRotateToTarget(skillTarget));
        }

        return base.PrepareSkill();
    }

    IEnumerator StartRotateToTarget(GameObject target)
    {
        Vector3 ClonePos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        while (canUseSkill)
        {
            if (target == null)
                CharUtils.FindRandomTarget(transform, controller.EnemyLayer, controller.DetectRange, out target);
            if (target != null)
            {
                ClonePos = target.transform.position;
                ClonePos.y = transform.position.y;
                dir = ClonePos - transform.position;
                dir.Normalize();
            }

            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10.0f);
            yield return null;
        }
    }


    protected override void ChangeState(SKILLSTATE state)
    {
        if (skillState == state)
            return;

        skillState = state;

        switch (skillState)
        {
            case SKILLSTATE.WAIT:
                ResetRemainCoolTime();
                break;
            case SKILLSTATE.STARTCOOLDOWN:
                break;
            case SKILLSTATE.STOPCOOLDOWN:
                // To Do SomeThing...
                break;
            case SKILLSTATE.USESKILL:
                _canUseSkill = true;
                break;
        }
    }

    protected override void StateProcess()
    {
        switch (skillState)
        {
            case SKILLSTATE.WAIT:
                if (controller != null && controller.canAction)
                    _startCoolDown = true;
                break;
            case SKILLSTATE.STARTCOOLDOWN:
                if (controller != null && !controller.canAction)
                    _startCoolDown = false;
                break;
        }
        base.StateProcess();
    }

}
