using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileKit
{
    //public interface ProjectileMovement
    //{
    //    void SetMovement(Transform target, float bulletSpeed);
    //}

    public class ProjectileMovement : MonoBehaviour
    {
        protected bool fireBullet = false;
        protected bool endMove = false;
        protected Transform target = null;
        protected float bulletSpeed = 0.0f;

        public virtual void SetMovement(Transform target, float bulletSpeed)
        {

        }

        public virtual void SetRotate(Vector3 target)
        {

        }
    }

}