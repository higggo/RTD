using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CharacterKit 
{
    public struct FDamageMessage
    {
        public GameObject Causer;
        public float amount;
    }

    /*
     * @Summary: 해당 클래스는 CharacterStat컴포넌트와 같은 하이어아키에 존재해야 합니다.
     */
    public class Damageable : MonoBehaviour
    {
        public UnityAction onDeadDel = null;
        public Transform HitPoint = null;
        float HP;
        bool isDead = false;


        public bool IsDead 
        {
            get { return isDead; } 
        }

        void Start()
        {
            HP = GetComponent<CharacterStat>().HP;
            GetComponent<CharacterStat>().OnHPChangeDel += () => { HP = GetComponent<CharacterStat>().HP; };
        }


        // msg의 amount는 양의 값을 전달해야 합니다.
        public void GetDamage(FDamageMessage msg)
        {
            if (msg.amount < 1)
                msg.amount = 1;

            HP = Mathf.Clamp(HP - msg.amount, 0.0f, GetComponent<CharacterStat>().MaxHP);
            GetComponent<CharacterStat>()?.UpdateHP(HP);

            if (HP <= Mathf.Epsilon)
            {
                Debug.Log("SetDead");
                isDead = true;
                onDeadDel?.Invoke();
            }
                
        }

        // @Summary: 아직 사용 x
        public static FDamageMessage GetDamageMessage(CharController Causer, CharController Victim)
        {
            FDamageMessage msg = new FDamageMessage();
            float dmgAmount = 0.0f;
            //dmgAmount = CharController.
            return msg;
        }
    }
}

