using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Vector3[] wayPoints;     //이동할 포인트들을 배열에 저장
    public Vector3 CurrentPosition; //현재위치
    private int wayPointIndex = 0;  //이동 포인트 인덱스
    private float speed = 4f;       //속도


    // Start is called before the first frame update
    void Start()
    {
        wayPoints = new Vector3[4];

        wayPoints.SetValue(new Vector3(7, 1, 7), 0);
        wayPoints.SetValue(new Vector3(7, 1, -7), 1);
        wayPoints.SetValue(new Vector3(-7, 1, -7), 2);
        wayPoints.SetValue(new Vector3(-7, 1, 7), 3);

    }

    // Update is called once per frame
    void Update()
    {
        CurrentPosition = transform.position;

        if (wayPointIndex < wayPoints.Length)
        {
            float step = Time.deltaTime * speed;
            transform.position = Vector3.MoveTowards(CurrentPosition, wayPoints[wayPointIndex], step);

            if (Vector3.Distance(wayPoints[wayPointIndex], CurrentPosition) == 0f)
            {
                wayPointIndex++;
            }
        }
    }
}
