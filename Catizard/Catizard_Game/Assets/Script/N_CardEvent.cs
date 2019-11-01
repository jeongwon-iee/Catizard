using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class N_CardEvent : MonoBehaviour
{

    public int number;
    public N_CardDeckSys CDS;
    public Text energyText;
    private int cardType, energy;
    public static bool isPress = false;

    private void OnEnable()
    {
        CDS.RemoveMark(number);
        SetEnergy();
    }

    public void SetEnergy()
    {
        cardType = CDS.GetCardType(number);
        switch (cardType)
        {
            case 0:
                energyText.text = "" + 20;
                energy = 20;
                break;
            case 1:
                energyText.text = "" + 25;
                energy = 25;
                break;
            case 2:
                energyText.text = "" + 25;
                energy = 25;
                break;
            case 3:
                energyText.text = "" + 0;
                energy = 0;
                break;
            case 8:
                energyText.text = "" + 100;
                energy = 100;
                break;
            case 4:
                energyText.text = "" + 30;
                energy = 30;
                break;
            case 5:
                energyText.text = "" + 45;
                energy = 45;
                break;
            case 6:
                energyText.text = "" + 15;
                energy = 15;
                break;
            case 7:
                energyText.text = "" + 25;
                energy = 25;
                break;
        }
    }

    // 마우스가 눌렀다 땠을 때
    public void ClickUp()
    {
        // 마우스 왼쪽인 경우
        if (Input.GetMouseButtonUp(0))
        {
            if (cardType == 8 && CDS.pw.wallList.Count <= 0)
            {
                // 에러창을 띄운다.
                CDS.CS.On_ErrorUI(1);
                return;
            }
            if (CDS.CS.isWild || CDS.SetEnergy(energy))
            {
                CDS.RemoveInfo();
                CDS.ClickLeftCard(number);
                SetEnergy();
                if (isActiveAndEnabled && isPress)
                {
                    CDS.PrintInfo(cardType);
                }
            }
        }
        // 마우스 오른쪽인 경우
        else if (Input.GetMouseButtonUp(1))
        {
            isPress = false;
            CDS.RemoveInfo();
        }
    }

    // 마우스를 누를 때
    public void ClickDown()
    {
        // 마우스 오른쪽인 경우
        if (Input.GetMouseButtonDown(1))
        {
            isPress = true;
            CDS.PrintInfo(cardType);
        }
    }

    // 마우스가 들어온 순간
    public void PointEnter()
    {
        if (isPress)
        {
            CDS.PrintInfo(cardType);
        }
        CDS.SetMark(number);
    }

    // 마우스가 나가는 순간
    public void PointExit()
    {
        CDS.RemoveInfo();
        CDS.RemoveMark(number);
    }



}
