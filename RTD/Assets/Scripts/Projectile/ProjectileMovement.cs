using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileKit
{
    public interface ProjectileMovement
    {
        void SetMovement(Transform target, float bulletSpeed);
    }

    
}