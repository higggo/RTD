using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

public class BasicAttack : MonoBehaviour
{
    CharacterStat statInfo;

    float _attackDelay = 0.0f;
    bool _nowAttack = false;

    void Start()
    {
        statInfo = GetComponent<CharacterStat>();
    }

    // 이 함수를 override해서 다른 캐릭터의 기본공격 로직을 바꾸세요.
    public void OnAttack(GameObject Target)
    {
        if (Target == null)
            return;

        // 데미지 전달.
        var targetDamageComp = Target.GetComponent<Damageable>();
        if (targetDamageComp == null)
            return;

        FDamageMessage msg = new FDamageMessage();
        msg.Causer = this.gameObject;
        msg.amount = this.statInfo.attackDamage;
        targetDamageComp.GetDamage(msg);
    }

}
