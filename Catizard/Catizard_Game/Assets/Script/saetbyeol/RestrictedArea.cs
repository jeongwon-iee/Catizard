using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    public GameObject restricted; // 프리팹
    public static float width = 0.95f;
    public static float height = 0.95f;
    public GameObject[] area; // restricted area 오브젝트 담는 배열
    public Vector2[] pos; // 이동불가지역 담는 배열

    private Vector2[] dist; // 구역별 좌표
    private int i;
    private int num = 10;

    void Start()
    {
        pos = new Vector2[num];
        area = new GameObject[num];
        dist = new Vector2[15]; // 구역 0의 15개 원소를 담는다

        for (i = 0; i < 5; i++)
        {
            dist[4 - i].x = (width * 0.5f + width * (5 + i)) * (-1); // x좌표 넣기 posX[0] ~ [4]
            dist[i + 5].x = dist[4 - i].x;
            dist[i + 10].x = dist[4 - i].x;
        }
        // 확장하기 전 원소 15개
        // 0  1   2  3  4
        // 5  6   7  8  9
        // 10 11 12 13 14

        for (i = 0; i < 15; i++)
        {
            if (i < 5)
                dist[i].y = height * 3;
            else if(i<10)
                dist[i].y = height * 2;
            else if(i<15)
                dist[i].y = height * 1;
        }

        // 구역 나누기
        // 0 1 2 3
        //  8   9
        // 4 5 6 7

        for (i = 0; i < 8; i++) // 구역 0부터 7 x,y
        {
            pos[i].x = dist[Random.Range(0, 5)].x+width*5*(i%4);

            if(i<4)
                pos[i].y = dist[Random.Range(0, 15)].y;
            else
                pos[i].y = dist[Random.Range(0, 15)].y-4*height;
        }

        pos[8].x = dist[Random.Range(1, 5)].x + width * 5 * (Random.Range(0, 2)); // 구역 8의 x좌표, 단 출발지점 선택X
        pos[8].y = 0;
        pos[9].x = dist[Random.Range(0, 5)].x + width * 5 * (Random.Range(2, 4)); // 구역 9의 x좌표
        pos[9].y = 0;


        for (i = 0; i < num; i++)
        {
            area[i] = Instantiate(restricted, pos[i], Quaternion.identity) as GameObject; // 생성
            area[i].SetActive(true); 
        }
    }

}