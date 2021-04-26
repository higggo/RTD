using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterKit
{

    /// <summary>
    /// 캐릭터 등급을 세팅합니다.
    /// </summary>
    public enum GRADE
    {
        NORMAL, MAGIC, RARE, UNIQUE
    }

    /// <summary>
    /// 추후 캐릭터 업그레이드에 쓰일 진영값
    /// </summary>
    public enum UNION
    {
        ENEMY, MAGE, WARRIOR, ARCHER
    }

    /// <summary>
    /// 캐릭터 고유 ID값입니다.
    /// 해당 값을 CharacterStat의 인스펙터창에서 넣어주면 됩니다. (UNKNOWN, NORMAL, MAGIC, RARE, UNIQUE는 넣으면 x)
    /// </summary>
    public enum ID
    {
        UNKNOWN = -1,
        NORMAL = 0 , N_HERO, N_CASTLEKNIGHT, N_GLADIATOR, N_BERSERKER, N_SWORDMASTER,
                     N_MARINE = 20,
                     N_LICH   = 40,
        MAGIC = 100, M_HERO, M_CASTLEKNIGHT, M_GLADIATOR, M_BERSERKER, M_SWORDMASTER,
                     M_MARINE = 120,
                     M_LICH   = 140,
        RARE = 200,  R_HERO, R_CASTLEKNIGHT, R_GLADIATOR, R_BERSERKER, R_SWORDMASTER,
                     R_MARINE = 220,
                     R_LICH   = 240,
        UNIQUE= 300, Q_HERO, Q_CASTLEKNIGHT, Q_GLADIATOR, Q_BERSERKER, Q_SWORDMASTER,
                     Q_MARINE = 220,
                     Q_LICH   = 240
    }
    
    public enum SIMBOL_WEAPON
    {
        UNKNOWN, 
        SHORTED, RANGED, MAGIC
    }

    public enum SIMBOL_ARMOR
    {
        UNKNOWN,
        LIGHT, HEAVY, MAGICAL
    }

    /// <summary>
    /// For PlayerCharacter STATE MACHINE
    /// </summary>
    public enum BASICSTATE
    {
        NONE, CREATE, POSTCREATE,
        WAIT, ATTACHFIELD, DETACHFIELD,
        MOVE,
        DETECT, ATTACK, 
        READYSKILL, USESKILL,
        DEAD
    }


    /// <summary>
    /// Struct: 아군, 적의 스텟값을 저장하기 위한 구조체입니다.
    /// 인스펙터창에서 해당 값을 수정할 수 있습니다.
    /// </summary>
    [System.Serializable]
    public struct FCharacterStat
    {
        public float MaxHP;
        public float HP;
        public float attackDamage;
        public float attackSpeed;
        public float attackRange;
        public float moveSpeed;
        public float rotateSpeed;
    }


}
