using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class N_CardDeckSys : MonoBehaviour
{

    public bool istest;
    public static bool isTest;                  // 테스트 모드 : 카드 덱 상태를 Inspector창에서 설정
    public float SuffleTime = 20, BlinkTime = 5, DrawTime = 0.2f;
    public int MaxEnergy = 100;  // 에너지 최대치
    public int TotalCard;               // 총 카드 수
    public int[] CardOrder;           // 카드 순서 관리
    private int[] HandOrder = { 0, 3, 4, 5, 8 };         // 수중의 카드 순서 관리
    public N_CardSystem CS;
    public Text CardNText, EnergyText;
    public Animator EnergyAnimator;
    public Animator[] CardAnimator;
    public int CardN, CardIndex, Energy;     // 남은 카드 수, 에너지양
    public GameObject[] WhiteMark;
    public Image[] CardImage;
    public GameObject[] CardObject;
    public Image InfoImage;
    public GameObject InfoObject, InfoWhiteObject;
    public Sprite[] CardSprite, InfoSprite;     // 카드 그래픽 (뒷면)
    public GameObject[] Removed_Card;
    public Image[] Removed_Image;
    public Animator[] Removed_Animator;
    public GameObject[] DeckObject;
    

    public putWall pw;

    private int left, right, total;
    private bool isCardFuc = true;

    // Start is called before the first frame update
    void Awake()
    {
        isTest = istest;
        // test상황이면 Inspector창에서 설정한 값으로 덱 상태 설정.
        // 아니면 N_PlayerInfo 스크립트 상에 설정된 값을 불러옴.
        if (!isTest)
        {
            TotalCard = N_PlayerInfo.CardSum;
            CardOrder = new int[TotalCard];
            // 카드를 차곡차곡 CardOrder에 쌓음.
            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < N_PlayerInfo.CardNum[i]; j++)
                {
                    CardOrder[index++] = i;
                }
            }
        }
        else
        {
            if(CardOrder.Length != TotalCard)
            {
                print("Error(Inspector) : 카드 덱 설정 오류");
            }
        }

        InfoWhiteObject.SetActive(false);
        InfoObject.SetActive(false);
        CardN = TotalCard;
        CardNText.text = "" + CardN;
        CardIndex = 0;
        CardShuffle();
        StartCoroutine("CardDeckSystem");
        
    }
    
    IEnumerator CardDeckSystem()
    {
        // 변수 초기화
        float TimeToSleep = SuffleTime - BlinkTime - (DrawTime * 5) - 1;
        int index = 0;

        yield return new WaitForSeconds(1);

        // 카드덱 시스템
        while (CS.isGame)
        {
            TimeToSleep = SuffleTime - BlinkTime - (DrawTime * 5) - 1;
            // 카드를 버린다. 카드 함수 진행 상황을 중단한다.
            if (CS.isCatnip)
            {
                CS.isCatnip = false;
                CS.UIArray_N[0].SetActive(false);
                CS.CardCover.SetActive(false);
            }
            if (CS.isScrow)
            {
                CS.isScrow = false;
                CS.UIArray_N[1].SetActive(false);
                CS.CardCover.SetActive(false);
            }
            if (CS.isWild)
            {
                CS.isWild = false;
                CS.UIArray_N[4].SetActive(false);
                CS.WhiteColorChange(1);
            }
            if (CS.wallCard >= 0)
            {
                CS.wallCard = -1;
                CS.UIArray_N[2].SetActive(false);
                CS.UIArray_N[3].SetActive(false);
                CS.CardCover.SetActive(false);
            }
            RemoveInfo();
            N_CardEvent.isPress = false;
            total = 0;
            for (int i = 0; i < 5; i++)
            {
                Removed_Animator[i].SetTrigger("end");
                StartCoroutine(RemoveCard(i, true));
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 5; i++)
            {
                Removed_Card[i].SetActive(false);
            }
            yield return new WaitForSeconds(0.25f);
            EnergyAnimator.SetTrigger("Blink");
            yield return new WaitForSeconds(0.25f);

            // 에너지 회복
            Energy = MaxEnergy;
            EnergyText.text = "" + Energy;
            
            //마법사 스킬 타이밍 판단
            CS.CatSkill();
            print("스킬사용판단중");

            // 카드를 뽑는다.
            left = right = 0;
            for (int i = 0; i < 5; i++)
            {
                if (CardN > 0)
                {
                    CardNText.text = "" + --CardN;
                    index = CardOrder[CardIndex++];
                    HandOrder[i] = index;
                    CardObject[i].SetActive(true);
                    CardImage[i].sprite = CardSprite[index];
                    CardAnimator[i].SetTrigger("Draw");
                    // + 카드 드로우 애니메이션
                    if (CardN == 2)
                        DeckObject[2].SetActive(false);
                    else if (CardN == 1)
                        DeckObject[1].SetActive(false);
                    else if (CardN == 0)
                        DeckObject[0].SetActive(false);
                    yield return new WaitForSeconds(DrawTime);
                }
                // 뽑을 카드가 없으면 덱을 루프시킨다.
                else
                {
                    CardShuffle();
                    // + 카드 루프 애니메이션
                    yield return new WaitForSeconds(0.3f);
                    for (int k = 0; k < TotalCard; k++)
                    {
                        if (k < 3)
                        {
                            DeckObject[k].SetActive(true);
                        }
                        CardN = k + 1;
                        CardNText.text = "" + CardN;
                        CardIndex = 0;
                        if (k < 3)
                            yield return new WaitForSeconds(0.1f);
                        else
                            yield return new WaitForSeconds(0.01f);
                    }
                    yield return new WaitForSeconds(0.3f);
                    i--;
                    TimeToSleep -= 0.6f + 0.01f * (TotalCard - 3);
                }
            }

            yield return new WaitForSeconds(TimeToSleep);

            // BlinkTime만큼 깜박인다.
            for (int i = 1; i <= BlinkTime; i++)
            {
                for (int m = 0; m < 5; m++)
                {
                    CardImage[m].color = new Color(0.5f, 0.5f, 0.5f, 1);
                }
                yield return new WaitForSeconds(0.5f);
                for (int n = 0; n < 5; n++)
                {
                    CardImage[n].color = new Color(1, 1, 1, 1);
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    // 카드를 섞는다.
    void CardShuffle()
    {
        for (int i = 0; i < TotalCard; i++)
        {
            int random = Random.Range(0, TotalCard);
            int temp = CardOrder[random];
            CardOrder[random] = CardOrder[i];
            CardOrder[i] = temp;
        }
    }

    // 사용된 카드는 버려짐
    IEnumerator RemoveCard(int index, bool isAni)
    {
        HandOrder[index] = -1;
        RemoveMark(index);
        // + 카드 버리는 애니메이션
        if (isAni)
        {
            CardAnimator[index].SetTrigger("Remove");
            yield return new WaitForSeconds(0.5f);
        }
        CardObject[index].SetActive(false);
    }

    // 마우스가 닿은 카드에는 Marking
    public void SetMark(int number)
    {
        if(HandOrder[number] >= 0)
            WhiteMark[number].SetActive(true);
    }

    public void RemoveMark(int number)
    {
        WhiteMark[number].SetActive(false);
    }

    // 수중의 카드 정보 출력
    public int GetCardType(int number)
    {
        return HandOrder[number];
    }

    // 에너지 소비
    public bool SetEnergy(int e)
    {
        // 에너지 부족할 시
        if (Energy < e)
            return false;

        if (!CS.isWild)
        {
            Energy -= e;
        }
        
        EnergyText.text = "" + Energy;
        return true;
    }

    // 공격 스킬
    public void CardAttack()
    {
        // 수중에 카드가 없으면 실행X
        int left = 5 - total;
        if (left == 0)
            return;

        int number = 0, rnd = Random.Range(1, left + 1);
        for (int i = 0; i < 5; i++)
        {
            if (HandOrder[i] >= 0)
                number++;
            if (number == rnd)
            {
                number = i;
                break;
            }
        }
        isCardFuc = false;
        ClickLeftCard(number);
    }

    // 마우스 왼쪽으로 카드를 클릭햇을 때
    public void ClickLeftCard(int number)
    {
        int CardType = HandOrder[number];
        int TempType = CardType;
        print(CardType + "번 카드 사용 (isCardFuc : " + isCardFuc + " )");

        // wild 카드에 한해 수중의 카드 체크
        if (CardType == 3)
        {
            bool flag = false;
            for (int i = 0; i < 5; i++)
            {
                int Typei = HandOrder[i];
                if (Typei != -1 && Typei != 3)
                    flag = true;
            }
            // 복사할 카드가 없으면 에러창 띄우고 함수 취소
            if (!flag)
            {
                CS.On_ErrorUI(3);
                return;
            }
        }

        if (!CS.isWild)
        {
            if (number < 2)
                left++;
            else if (number > 2)
                right++;

            // 사용된 카드는 버려짐
            Removed_Card[total].SetActive(true);
            Removed_Image[total].sprite = CardSprite[CardType];
            Removed_Animator[total].SetTrigger("remove");
            total++;
            StartCoroutine(RemoveCard(number, false));

            // 카드 가운데로 모으기
            if (number == 1 && HandOrder[0] != -1)
            {
                CardType = HandOrder[0];
                StartCoroutine(RemoveCard(0, false));
                HandOrder[1] = CardType;
                CardObject[1].SetActive(true);
                CardImage[1].sprite = CardSprite[CardType];
                if (isCardFuc)
                    SetMark(1);
            }
            else if (number == 3 && HandOrder[4] != -1)
            {
                CardType = HandOrder[4];
                StartCoroutine(RemoveCard(4, false));
                HandOrder[3] = CardType;
                CardObject[3].SetActive(true);
                CardImage[3].sprite = CardSprite[CardType];
                if (isCardFuc)
                    SetMark(3);
            }
            else if (number == 2 && left * right < 4)
            {
                if (left > right)
                {
                    for (int i = 3; i <= 4; i++)
                    {
                        CardType = HandOrder[i];
                        StartCoroutine(RemoveCard(i, false));
                        HandOrder[i - 1] = CardType;
                        CardObject[i - 1].SetActive(true);
                        CardImage[i - 1].sprite = CardSprite[CardType];
                        if (right == 1)
                            break;
                    }
                    right++;
                }
                else
                {
                    for (int i = 1; i >= 0; i--)
                    {
                        CardType = HandOrder[i];
                        StartCoroutine(RemoveCard(i, false));
                        HandOrder[i + 1] = CardType;
                        CardObject[i + 1].SetActive(true);
                        CardImage[i + 1].sprite = CardSprite[CardType];
                        if (left == 1)
                            break;
                    }
                    left++;
                }
                if (isCardFuc)
                    SetMark(2);
            }
        }
        else
        {
            CS.isWild = false;
            CS.UIArray_N[4].SetActive(false);
            CS.WhiteColorChange(1);
        }

        // + HandOrder[number]에 해당하는 카드 함수 실행
        if (isCardFuc)
        {
            CS.CardFunction(TempType);
        }
        else
            isCardFuc = true;

    }

    // 마우스 오른쪽 누르면
    public void PrintInfo(int cardType)
    {
        InfoWhiteObject.SetActive(true);
        InfoObject.SetActive(true);
        InfoImage.sprite = InfoSprite[cardType];
    }

    // 마우스 오른쪽 떼면 or 마우스가 카드 밖으로 나가면
    public void RemoveInfo()
    {
        InfoWhiteObject.SetActive(false);
        InfoObject.SetActive(false);
    }

}
