using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileKit;
public class TestBulletMovement : MonoBehaviour, ProjectileMovement
{
    public bool fireBullet = false;
    public bool endMove = false;
    Transform target = null;
    float bulletSpeed = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ProjectileController>().moveDel += SetMovement;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMovement(Transform target, float bulletSpeed)
    {
        Debug.Log("MoveSet");
        this.target = target;
        this.bulletSpeed = bulletSpeed;
        fireBullet = true;
        StartCoroutine(Move(target, bulletSpeed));
        Debug.DrawLine(transform.position, target.transform.position, Color.blue, 2.0f);
    }

    public void SetRotate(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir);
    }

    IEnumerator Move(Transform target, float bulletSpeed)
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
        GetComponent<ProjectileController>().SetHit();
    }

    void OnTriggerEnter(Collider other)
    {
        //
    }
}
