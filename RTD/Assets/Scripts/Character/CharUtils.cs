﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CharacterKit
{
    public class CharUtils : MonoBehaviour
    {
        // @Summary: Target이 Instance로부터 Range안에 있는지 검사합니다. (높낮이의 차이는 무시됩니다.)
        public static bool IsInRange(Transform Instance, Transform Target, float Range)
        {
            Vector3 Targetpos = Target.position;
            Targetpos.y = Instance.position.y;

            if (Range < Vector3.Distance(Targetpos, Instance.position))
                return false;

            return true;
        }

        // @Summary: 해당 Instance의 위치에서 LayerMask에 따른 SphereCast를 하고, 목표가 있으면 TargetContainer에 담고 true를 리턴합니다.
        public static bool FindTargetAll(CharController Instance, LayerMask Mask, ref List<GameObject> TargetContainer)
        {
            float range = Instance.statInfo.attackRange;
            RaycastHit[] hitInfo = Physics.SphereCastAll(Instance.transform.position, range, Instance.transform.forward, 0.0f, Mask);
            if (hitInfo.Length == 0)
                return false;
            
            TargetContainer.Clear();
            foreach (RaycastHit hit in hitInfo)
            {
                Debug.Log("FindTargetAll");
                GameObject hitObj = hit.collider.gameObject;
                TargetContainer.Add(hitObj);
            }

            return true;
        }

        // @Summary: Targets중 Instance와 가장 가까운 적을 리턴합니다.
        public static GameObject GetCloseTarget(CharController Instance, List<GameObject> Targets)
        {
            if (Targets.Count == 0)
            {
                Debug.Log("Targets is Empty!");
                return null;
            }

            GameObject TargetObj = Targets[0];
            float MinDist = Vector3.Distance(Instance.transform.position, Targets[0].transform.position);

            foreach (GameObject target in Targets)
            {
                float dist = Vector3.Distance(Instance.transform.position, target.transform.position);
                if (MinDist > dist)
                {
                    MinDist = dist;
                    TargetObj = target;
                }
            }

            Debug.DrawLine(Instance.transform.position, TargetObj.transform.position, Color.red, 0.5f);
            return TargetObj;
        }

    }
}
