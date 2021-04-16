using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileKit;

public class IceBoltMovement : ProjectileMovement
{
    // Start is called before the first frame update
    void Start()
    {
        InitController();
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected sealed override void SetMovement()
    {
        Debug.Log("MoveSet");
        base.SetMovement();
        
        StartCoroutine(Move(target));
        Debug.DrawLine(transform.position, target.transform.position, Color.blue, 2.0f);
    }

    IEnumerator Move(Transform target)
    {
        Vector3 ClonePos = target.position;
        Vector3 dir = transform.forward;
        dir.Normalize();
        SetRotate(ClonePos);
        float distance = Vector3.Distance(ClonePos, transform.position);
        float delta = Time.deltaTime * bulletSpeed;

        while (distance > Mathf.Epsilon)
        {
            SetRotate(ClonePos);
            dir = transform.forward;
            if (delta > distance)
                delta = distance;

            distance -= delta;
            transform.Translate(dir * delta, Space.World);
            
            if (target != null)
            {
                ClonePos = target.position;
                distance = Vector3.Distance(ClonePos, transform.position);
            }
            yield return null;
        }
        endMove = true;
        SetHit();
    }
    

    void SetHit()
    {
        // Stop Movement or Destroy
        controller.SetHit();
    }

    void OnTriggerEnter(Collider other)
    {
        //
    }
}
