using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    public Vector3[] wayPoints;     //이동할 포인트들을 배열에 저장
    public Vector3 CurrentPosition; //현재위치
    private int wayPointIndex = 0;  //이동 포인트 인덱스
    private float speed = 10f;       //속도 (캐릭터 머릿수가 좀 되기때문에 속도를 높였습니다.)


    // Start is called before the first frame update
    void Start()
    {
        wayPoints = new Vector3[11];                //현재로서는 조금 빙빙 돌아가게끔 디자인 되어 있습니다.

        wayPoints.SetValue(new Vector3(9, 1, -9), 0);
        wayPoints.SetValue(new Vector3(3, 1, -9), 1);
        wayPoints.SetValue(new Vector3(3, 1, 9), 2);
        wayPoints.SetValue(new Vector3(-3, 1, 9), 3);
        wayPoints.SetValue(new Vector3(-3, 1, -9), 4);
        wayPoints.SetValue(new Vector3(-9, 1, -9), 5);
        wayPoints.SetValue(new Vector3(-9, 1, -3), 6);
        wayPoints.SetValue(new Vector3(9, 1, -3), 7);
        wayPoints.SetValue(new Vector3(9, 1, 3), 8);
        wayPoints.SetValue(new Vector3(-9, 1, 3), 9);
        wayPoints.SetValue(new Vector3(-9, 1, 9), 10);

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
                if (wayPointIndex == wayPoints.Length)  //목적지에 도착시 에네미 캐릭터를 삭제합니다.
                {
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }
}