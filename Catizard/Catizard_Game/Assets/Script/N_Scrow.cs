using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_Scrow : MonoBehaviour
{
    public N_CardSystem NCS;

    public static bool[] ScrowOn = new bool[11];
    int num;
    bool flag = false;

    public void Awake()
    {
        for (int i = 0; i < 11; i++)
        {
            ScrowOn[i] = false;
        }
    }

    public void OnTriggerStay2D(Collider2D coll)
    {
        // 허수아비와 닿았다면
        if (coll.gameObject.CompareTag("scrow"))
        {
            NCS.isScrowOn = true;
            num = coll.GetComponent<N_ScrowNum>().num;
            ScrowOn[num] = true;
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        // 허수아비가 나갔다면
        if (coll.gameObject.CompareTag("scrow"))
        {
            num = coll.GetComponent<N_ScrowNum>().num;
            ScrowOn[num] = false;

            // 허수아비 모두 Exit상태면 false
            for (int i = 0; i < 11; i++)
            {
                if (ScrowOn[i])
                    flag = true;
            }
            if (!flag)
                NCS.isScrowOn = false;
            else
            {
                flag = false;
                print("아직 허수아비");
            }

            // 상태에 따라 이미지 변경
            if (NCS.isProvoke)
                NCS.graphic_change(2);
            else if (NCS.isScrowOn)
                NCS.graphic_change(3);
            else if (NCS.isCatnipOn)
                NCS.graphic_change(4);
            else
                NCS.graphic_change(0);
        }
    }

}
