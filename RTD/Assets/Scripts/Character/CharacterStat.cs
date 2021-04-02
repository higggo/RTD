using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class CharacterStat : MonoBehaviour
{
    [SerializeField] GRADE _grade;
    [SerializeField] UNION _union;
    [SerializeField] FCharacterStat basicStat;
    [SerializeField] FCharacterStat bonusStat;
    FCharacterStat currentStat;

    // Delegate
    public UnityAction BuffDel = null;

    void Start()
    {
        currentStat = basicStat;
        UpdateStat();
        // To Do: CharController의 LevelUpDel에 UpdateStat을 연결?
    }

    public void UpdateStat()
    {
        // To Do: 캐릭터의 Stat이 변경될 때, 호출되는 함수 작성
        //         1) ex: UNION LevelUp시 Level을 받아 보너스 스텟만큼 더한다.

        // currentStat = basicStat + bonusStat * (UnionLevel - 1);
        BuffDel?.Invoke();
    }

    // Property
    public GRADE grade
    {
        get { return _grade; }
    }
    
    public UNION union
    {
        get { return _union; }
    }

    public float MaxHP 
    { 
        get { return currentStat.MaxHP; } 
        set 
        {
            currentStat.MaxHP = value;
        } 
    }
    public float HP
    {
        get { return currentStat.HP; }
        set
        {
            currentStat.HP = value;
        }
    }
    public float attackRange
    {
        get { return currentStat.attackRange; }
        set
        {
            currentStat.attackRange = value;
        }
    }
    public float attackSpeed
    {
        get { return currentStat.attackSpeed; }
        set
        {
            currentStat.attackSpeed = value;
        }
    }
    public float attackDamage
    {
        get { return currentStat.attackDamage; }
        set
        {
            currentStat.attackDamage = value;
        }
    }


    void OnDrawGizmos()
    {
        Color color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;
        
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}