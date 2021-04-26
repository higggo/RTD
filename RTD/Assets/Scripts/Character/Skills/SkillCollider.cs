using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterKit;

// 해당 스크립트를 캐릭터 외 파티클에 붙여 사용 (사용 XXXXX)
public class SkillCollider : MonoBehaviour
{
    [SerializeField] float ParticleDamage;
    [SerializeField] LayerMask mask;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("파티클 충돌");
    }
}
