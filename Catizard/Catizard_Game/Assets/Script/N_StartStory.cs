using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class N_StartStory : MonoBehaviour, IPointerDownHandler
{

    public GameObject SkipButton;
    public GameObject START;
    public Sprite[] Story;
    public AudioClip last;
    public AudioSource sound;

    private Image StoryImage;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        StoryImage = GetComponent<Image>();
        StoryImage.sprite = Story[0];
        START.SetActive(false);
        CardSetting();
    }

    // 초기 플레이어 덱 세팅
    public void CardSetting()
    {
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.block1] = 5;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.block2] = 1;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.block3] = 1;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.wild] = 2;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.provoke] = 2;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.scarecrow] = 2;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.catnip] = 4;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.SOS] = 5;
        N_PlayerInfo.CardNum[(int)N_PlayerInfo.Card.removeBlock] = 3;
        N_PlayerInfo.CardSum = 0;
        for (int i = 0; i < 9; i++)
        {
            N_PlayerInfo.CardSum += N_PlayerInfo.CardNum[i];
        }
    }

    // 마우스가 눌릴 때
    public void OnPointerDown(PointerEventData data)
    {
        // 시작 스토리 일러스트 차례로 보여주기
        if (index < 3)
        {
            sound.Play();
            index++;
            StoryImage.sprite = Story[index];
        }
        else if (index == 3)
            QuitStory();
        else
            GotoMenu();
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    // 스토리 끝냄
    public void QuitStory()
    {
        sound.clip = last;
        sound.Play();
        index = 4;
        StoryImage.sprite = Story[index];
        SkipButton.SetActive(false);
        START.SetActive(true);
    }

}
