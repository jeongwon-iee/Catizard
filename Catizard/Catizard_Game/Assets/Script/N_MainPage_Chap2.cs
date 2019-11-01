using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class N_MainPage_Chap2 : MonoBehaviour, IPointerDownHandler
{
    
    public GameObject UnlockScreen, ErrorScreen;
    public Image ErrorImage;
    public Text GoldAmount;
    private Image Chap2Image;
    public Sprite[] Chap2;
    public AudioClip click, unLock;
    public AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        Chap2Image = GetComponent<Image>();
        if (!N_PlayerInfo.UnLock)
            Chap2Image.sprite = Chap2[0];
        else
            Chap2Image.sprite = Chap2[1];
    }

    // 마우스가 눌릴 때
    public void OnPointerDown(PointerEventData data)
    {
        if (!N_PlayerInfo.UnLock)
        {
            PlayClick();
            UnlockScreen.SetActive(true);
        }
        else
        {
            GotoGame_Hard();
        }
    }

    // 게임 진행 씬으로 이동
    public void GotoGame_Easy()
    {
        SceneManager.LoadScene("TestSystem");
    }

    public void GotoGame_Hard()
    {
        SceneManager.LoadScene("CardSystem");
    }

    // 챕터2 해금
    public void Unlock()
    {
        if (N_PlayerInfo.Gold >= 1000)
        {
            sound.clip = unLock;
            sound.Play();
            N_PlayerInfo.Gold -= 1000;
            GoldAmount.text = "" + N_PlayerInfo.Gold;
            N_PlayerInfo.UnLock = true;
            Chap2Image.sprite = Chap2[1];
            UnlockScreen.SetActive(false);
        }
        else
        {
            PlayClick();
            CloseUnlockScreen();
            StartCoroutine("Error");
        }
    }

    // 골드 부족할 때
    IEnumerator Error()
    {
        float valueA = 1;
        ErrorScreen.SetActive(true);
        for (int i = 1; i <= 10; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            valueA -= 0.1f;
            ErrorImage.color = new Color(1, 1, 1, valueA);
        }
        ErrorScreen.SetActive(false);
    }

    public void CloseUnlockScreen()
    {
        PlayClick();
        UnlockScreen.SetActive(false);
    }

    public void PlayClick()
    {
        sound.clip = click;
        sound.Play();
    }

}
