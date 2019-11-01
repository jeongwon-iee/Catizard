using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class N_StoryBook : MonoBehaviour, IPointerDownHandler
{

    public static int StoryNum;
    public GameObject SkipButton, StoryScreen;
    public Sprite[] Story1, Story2, Story3;

    private Image StoryImage;
    private int index = 0;

    void Awake()
    {
        StoryImage = GetComponent<Image>();
    }

    // OnEnable is called when it is enable
    void OnEnable()
    {
        index = 0;
        switch (StoryNum)
        {
            case 1:
                StoryImage.sprite = Story1[0];
                break;
            case 2:
                StoryImage.sprite = Story2[0];
                break;
            case 3:
                StoryImage.sprite = Story3[0];
                break;
        }
    }

    // 마우스가 눌릴 때
    public void OnPointerDown(PointerEventData data)
    {
        switch (StoryNum)
        {
            case 1:
                // 시작 스토리 일러스트 차례로 보여주기
                if (index < 3)
                {
                    index++;
                    StoryImage.sprite = Story1[index];
                }
                else
                    QuitStory();
                break;
            case 2:
                // 챕터1 스토리 일러스트 차례로 보여주기
                if (index < 1)
                {
                    index++;
                    StoryImage.sprite = Story2[index];
                }
                else
                    QuitStory();
                break;
            case 3:
                // 챕터2 스토리 일러스트 차례로 보여주기
                if (index < 1)
                {
                    index++;
                    StoryImage.sprite = Story3[index];
                }
                else
                    QuitStory();
                break;
        }
    }

    // 스토리 끝냄
    public void QuitStory()
    {
        StoryScreen.SetActive(false);
    }

}
