using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_ys_sb : MonoBehaviour {
    Vector2 goalPosition;
    float speed;
    float time;

    void Start()
    {
        time = 0;
        transform.position = new Vector2(-6, 3); // 초기 좌표
        goalPosition = new Vector2(6, 3); // 목표 좌표
        speed = 0.1f; // 이동 속력
    }

	void Update () {
        float xPos = this.transform.position.x; // xPos는 용사의 현재 x 좌표값

        if (xPos >= goalPosition.x) // xPos가 목표위치보다 커지거나 같으면
        {
            Stop(); // 움직이는 것을 멈춘다
        }
        else // 목표 x좌표에 도달하지 않았으면
        {
            transform.position = Vector2.MoveTowards(transform.position, goalPosition, speed * Time.deltaTime); //goalPosition까지 일정 속도로 이동
        }
	}

    void Stop()
    {

    }

    public void changeSp()
    {
        time += Time.deltaTime;
        while(time < 10)
        {
            speed = 2.0f;
        }
        speed = 0.1f;
    }
}
