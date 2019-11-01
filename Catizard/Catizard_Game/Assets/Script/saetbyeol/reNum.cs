using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reNum : MonoBehaviour // 스페이스바 누르는 횟수
{
    public GameObject wallPre1;
    public GameObject wallPre2;
    public GameObject wallPre3;

    public int shape;
    public int spaceNum;
    private int n;

    void Start()
    {
        spaceNum = 0; // 0으로 초깃값 설정
        n = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 누르면
        {
            n++; // n값 1 증가
            spaceNum = n % 4; // n module 4 ( 0도 90도 180도 270도)
            shape = spaceNum;
            switch (shape)
            {
                case 0:
                    {
                        wallPre1.transform.Rotate(0, 0, -90);
                        wallPre2.transform.Rotate(0, 0, -90);
                        wallPre3.transform.Rotate(0, 0, -90);
                        break;
                    }
                case 1:
                    {
                        wallPre1.transform.Rotate(0, 0, -90);
                        wallPre2.transform.Rotate(0, 0, -90);
                        wallPre3.transform.Rotate(0, 0, -90);
                        break;
                    }
                case 2:
                    {
                        wallPre1.transform.Rotate(0, 0, -90);
                        wallPre2.transform.Rotate(0, 0, -90);
                        wallPre3.transform.Rotate(0, 0, -90);
                        break;
                    }
                case 3:
                    {
                        wallPre1.transform.Rotate(0, 0, -90);
                        wallPre2.transform.Rotate(0, 0, -90);
                        wallPre3.transform.Rotate(0, 0, -90);
                        break;
                    }
            } // switch
        }
    }
    
}