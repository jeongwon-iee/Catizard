using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat_move : MonoBehaviour {
    private Vector2 StartPosition;
    private Vector2 EndPosition;

    //private float speed = 1f;
    private float startTime;
    private float distanceLength;
    private int r;

    private Vector2[] posRandom;

    // Use this for initialization
    void Start () {
        Debug.Log("start cat move");
        transform.position = new Vector2(-9, -2); // 초기 좌표

        posRandom = new Vector2[4];
        StartCoroutine("BuffCoroutine"); // coroutine 시작
	}

    IEnumerator BuffCoroutine()
    {
        int i = 0;
        while (i < 30)
        {
            CalPos();
            Move();
            Debug.Log("move " + i);
            i++;
            yield return new WaitForSeconds(2.0f); // 1초 wait
        }

    }

    void Move()
    {
        //moveFloat += Time.deltaTime * 2.5f;
        //float distCovered = (Time.time - startTime) * speed; // 현재까지 이동거리
        //float franJourney = distCovered / distanceLength; // 총 거리에서 현재위치 비율

        transform.position = Vector2.Lerp(StartPosition, EndPosition, 1.0f); // 세번째 파라미터는 0.0~1.0
    }

    void CalPos()
    {
        StartPosition = transform.position;
        posRandom[0] = new Vector2(StartPosition.x + 0.95f, StartPosition.y); // 우로 이동
        posRandom[1] = new Vector2(StartPosition.x, StartPosition.y + 1); // 상으로 이동
        posRandom[2] = new Vector2(StartPosition.x + 0.95f, StartPosition.y); // 우로 이동
        //posRandom[1] = new Vector2(StartPosition.x - 0.95f, StartPosition.y); // 좌로 이동
        //posRandom[2] = new Vector2(StartPosition.x, StartPosition.y + 1); // 상으로 이동
        posRandom[3] = new Vector2(StartPosition.x, StartPosition.y - 1); // 하로 이동

        if (StartPosition.x > 8.6 && StartPosition.y > 2.3)
            return;
        else if (StartPosition.y > 2.3)
            r = 0;
        else if (StartPosition.x > 8.6)
            r = 1;
        else
            r = Random.Range(0, 3);

        // 보드판을 벗어나지 않도록 r 선택
        /*
        if (StartPosition.x < -8.6)
        {
            do
                r = Random.Range(0, 4);
            while (r == 1);
        }
        else if (StartPosition.y > 2.3)
        {
            do
                r = 0;
            //r = Random.Range(0, 4);
            while (r == 2);
        }
        else if(StartPosition.y<-2.6){
            do
                r = Random.Range(0, 4);
            while (r == 3);
        }
        else if (StartPosition.x > 8.6)
        {
            do
                r = 1;
            //r = Random.Range(0, 4);
            while (r == 0);
        }
        else
            r = Random.Range(0, 4);
        */
        Debug.Log(r);

        EndPosition = posRandom[r]; // 랜덤으로 상하좌우 이동 선택


        //startTime = Time.time; // 시작시간
        //distanceLength = Vector2.Distance(StartPosition, EndPosition); // 시작 - 종료 전체거리 계산
    }
}

