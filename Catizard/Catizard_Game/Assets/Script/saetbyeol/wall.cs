using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour {
    private float timeCount;

    void Start () {
        timeCount = 0; // time 변수 초기화
	}

	void Update () {
        timeCount += Time.deltaTime; // time count
        
        if (timeCount < 7.0f) // clone 생성 시간
        {
            if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바 눌렀을 때
                transform.Rotate(0, 0, 90); // 90도 회전
        }

        else
            return; // 일정시간 지나면 더 이상 함수이용 X
    }
}
