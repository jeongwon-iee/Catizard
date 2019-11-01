using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class N_StoryNum : MonoBehaviour, IPointerDownHandler
{

    public int StoryNum;
    public GameObject StoryScreen;

    // 마우스가 눌릴 때
    public void OnPointerDown(PointerEventData data)
    {
        N_StoryBook.StoryNum = StoryNum;
        StoryScreen.SetActive(true);
    }

}
