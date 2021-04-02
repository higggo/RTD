using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterKit
{
    public enum GRADE
    {
        NORMAL, MAGIC, RARE, UNIQUE
    }

    public enum UNION
    {
        ENEMY, UNION001
    }

    
    public enum BASICSTATE
    {
        NONE, CREATE, POSTCREATE,
        WAIT, ATTACHFIELD, DETACHFIELD,
        DETECT, ATTACK, USESKILL,
        DEAD
    }

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
