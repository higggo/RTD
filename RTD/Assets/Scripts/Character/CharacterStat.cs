using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class CharacterStat : MonoBehaviour
{
    GRADE _grade;
    [SerializeField] UNION _union;
    [SerializeField] ID _id = ID.UNKNOWN;
    [SerializeField] FCharacterStat basicStat;
    [SerializeField] FCharacterStat bonusStat;
    [SerializeField] FCharacterStat currentStat;
    
    // Delegate
    public UnityAction BuffDel = null;

    void Awake()
    {
        currentStat = basicStat;
        // To Do: CharController의 LevelUpDel에 UpdateStat을 연결?
    }


    /// <summary>
    /// 해당 인스턴스 캐릭터의 스텟을 업데이트 합니다.
    /// </summary>
    /// <param name="unionLevel">각 캐릭터의 union에 맞는 레벨값을 넘겨주세요.</param>
    public void UpdateStat(int unionLevel)
    {
        if (unionLevel <= 0)
        {
            currentStat = basicStat;
            return;
        }
        // level이 1인 경우 bonusStat이 더해지면 안되므로 -1 계산
        unionLevel -= 1;


        FCharacterStat tempStat = basicStat;
        tempStat.attackDamage += bonusStat.attackDamage * unionLevel;
        tempStat.attackRange += bonusStat.attackRange * unionLevel;
        tempStat.attackSpeed += bonusStat.attackSpeed * unionLevel;
        tempStat.MaxHP += bonusStat.MaxHP * unionLevel;
        tempStat.HP += bonusStat.HP * unionLevel;

        // To Do: 버프나 디버프가 있으면, 해당 델리게이트를 발동시켜서 작동할 수 있도록
        //BuffDel?.Invoke();

        currentStat = tempStat;
    }

    // Property
    public GRADE grade
    {
        get { return _grade; }
        set { _grade = value; }
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

    public ID id
    {
        get { return _id; }
    }
    
    /// <summary>
    /// 캐릭터 고유 id를 Get합니다.
    /// </summary>
    /// <returns>ID정보를 Get</returns>
    public ID GetID()
    {
        return _id;
    }

    void OnDrawGizmosSelected()
    {
        Color color = Color.green;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}