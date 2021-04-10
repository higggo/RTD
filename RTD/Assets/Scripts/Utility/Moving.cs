using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CharacterKit;

public class Moving : MonoBehaviour
{
    public Vector3[] wayPoints;     //이동할 포인트들을 배열에 저장
    public Vector3 CurrentPosition; //현재위치
    private int wayPointIndex = 0;  //이동 포인트 인덱스
    private float speed = 10f;       //속도

    float rotTime = 1.0f;
    float rotSumDelta = 0.0f;

    public UnityAction DestroySpawnDelegate;

    // Start is called before the first frame update
    void Start()
    {
        wayPoints = new Vector3[12];

        wayPoints.SetValue(new Vector3(-9, 0, 9), 0);
        wayPoints.SetValue(new Vector3(-9, 0, 3), 1);
        wayPoints.SetValue(new Vector3(9, 0, 3), 2);
        wayPoints.SetValue(new Vector3(9, 0, 9), 3);
        wayPoints.SetValue(new Vector3(3, 0, 9), 4);
        wayPoints.SetValue(new Vector3(3, 0, -9), 5);
        wayPoints.SetValue(new Vector3(9, 0, -9), 6);
        wayPoints.SetValue(new Vector3(9, 0, -3), 7);
        wayPoints.SetValue(new Vector3(-9, 0, -3), 8);
        wayPoints.SetValue(new Vector3(-9, 0, -9), 9);
        wayPoints.SetValue(new Vector3(-3, 0, -9), 10);
        wayPoints.SetValue(new Vector3(-3, 0, 9), 11);

    }

    // Update is called once per frame
    void Update()
    {
        CurrentPosition = transform.position;
        if (wayPointIndex < wayPoints.Length &&
            GetComponent<EnemyController>().GetState() != ENEMYSTATE.DEAD)
        {
            float step = Time.deltaTime * speed;
            transform.position = Vector3.MoveTowards(CurrentPosition, wayPoints[wayPointIndex], step);

            Vector3 dir = (wayPoints[wayPointIndex] - CurrentPosition).normalized;
            if (transform.forward != dir)
            {
                rotSumDelta += Time.deltaTime;
                transform.forward = Vector3.Slerp(transform.forward, dir, rotSumDelta / Mathf.Clamp(rotTime, 0.1f, 1.0f));
            }
            else
            {
                rotSumDelta = 0.0f;
            }

            if (Vector3.Distance(wayPoints[wayPointIndex], CurrentPosition) == 0f)
            {
                wayPointIndex++;
                if (wayPointIndex == wayPoints.Length)  //목적지에 도착시 에네미 캐릭터를 삭제합니다.
                {
                    DestroySpawnDelegate?.Invoke();
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}