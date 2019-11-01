using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class N_CardSystem : MonoBehaviour
{

    public bool isGame = true, isPause = false, isCurse = false, isProvoke = false;
    public bool isSOS = false, isWild = false, isCatnip = false, isCatnipOn = false, isScrow = false, isScrowOn = false;
    public int GameMinute = 5, HeroSpeed = 1, cat_wait = 4, Cat_num_prev = 0, Cat_num_curr = 0, provoke_time = 18;
    public int total_gold = 300, bonus_gold = 0, remain_time = 0, RemainPath = 0, skill_hard = 5, skill_normal = 10;
    public Slider HeroSlider;
    public Animator HeroAnimator;
    public GameObject HeroSOS, HeroCurse, HeroDual;
    public RectTransform Hero;
    public RectTransform[] HeroABC;
    public Transform Cat, SP_bar;
    public GridView gridView;
    public Slider SP_Slider;
    public GameObject[] Cat_graphic;
    public GameObject CardCover;
    public Image[] White_Card;
    public GameObject[] UIArray_N;
    public GameObject[] UIArray_E;
    public Image[] UIImage_E;
    public GameObject OptionScreen, FakeBoard, RedObj, ClawObj;
    public Image Claw;
    public GameObject catnipView, scrowView;
    public Transform catnipVxy, scrowVxy;
    public GameObject[] catnip;
    public Transform[] catnipXY;
    public GameObject[] scrow;
    public Transform[] scrowXY;
    public N_CardDeckSys CDS;
    public Text goldText, timeText_C, timeText_O;
    public GameObject Game_clear, Game_Over;
    

    public AudioClip S_Click, S_Reflection, S_BlockOn, S_BlockOff, S_Scrow, S_Catnip, S_Provoke, S_Attack, S_Win;
    public AudioSource SoundA;

    public removeWall rw;
    public int wallCard;

    private int catnipIndex = 0, scrowIndex = 0, maxCatnip, maxScrow, SOS_repeat = 0, Provoke_repeat = 0;
    private bool countRest = false;
    private float blockSize, blockBuffer;
    private Point next = new Point(6, 0);
    public GameObject Dots;

    // 효과음
    public void PlaySoundA(int n)
    {
        // 효과음 선택
        switch (n)
        {
            // 카드 사용
            case 12:
                SoundA.clip = S_Win;
                break;
            case 17:
                SoundA.clip = S_BlockOn;
                break;
            case 18:
                SoundA.clip = S_BlockOff;
                break;
            case 19:
                SoundA.clip = S_Scrow;
                break;
            case 20:
                SoundA.clip = S_Provoke;
                break;
            case 21:
                SoundA.clip = S_Catnip;
                break;
            // 마법사 스킬
            case 22:
                SoundA.clip = S_Attack;
                break;
            // 옵션 버튼
            case 23:
                SoundA.clip = S_Click;
                break;
            case 24:
                SoundA.clip = S_Reflection;
                break;
        }
        // 효과음 재생
        SoundA.Play();
    }

    // Start is called before the first frame update
    void Awake()
    {
        wallCard = -1;
        blockSize = gridView.blockSize;
        blockBuffer = gridView.blockBuffer;
        HeroSlider.maxValue = GameMinute * 120;
        maxCatnip = catnip.Length;
        maxScrow = scrow.Length;
        StartCoroutine("HeroTimer");
        StartCoroutine("CatMove");
        graphic_change(0);
        RedObj.SetActive(false);
        ClawObj.SetActive(false);
        CardCover.SetActive(false);
        SP_Slider.value = 0;
        for(int i = 0; i < maxCatnip; i++)
        {
            catnip[i].SetActive(false);
        }
    }

    private void Update()
    {
        Vector3 temp = Cat.position;
        SP_bar.position = new Vector3(temp.x + 0.05f, temp.y - 0.4f);

        // Escape 눌리면 일시정지와 함께 옵션창 열기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionOn();
        }
    }

    IEnumerator HeroTimer()
    {
        while (isGame)
        {
            if (HeroSlider.value <= 0)
            {
                isGame = false;
                gridView.isGame = false;
                yield return new WaitForSeconds(1f);
                gridView.JPS();
                yield return new WaitForSeconds(1f);
                Win();
            }
            if (HeroSlider.value < HeroSlider.maxValue && isCurse)
            {
                HeroSlider.value += HeroSpeed;
            }
            else
            {
                HeroSlider.value -= HeroSpeed;
            }

            for(int i = 0; i < 3; i++)
            {
                HeroABC[i].anchorMin = new Vector2(Hero.anchorMin.x, 0);
                HeroABC[i].anchorMax = new Vector2(Hero.anchorMax.x, 0);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    // 옵션 버튼 기능
    public void OptionOn()
    {
        if (!isPause)
        {
            isPause = true;
            PlaySoundA(24);
            SP_bar.localScale = new Vector3(1, 0, 1);
            OptionScreen.SetActive(true);
            FakeBoard.SetActive(true);
            Time.timeScale = 0;     // 일시정지
        }
    }

    public void OptionOff()
    {
        if (isPause)
        {
            isPause = false;
            Time.timeScale = 1;     // 일시정지 해제
            PlaySoundA(23);
            SP_bar.localScale = new Vector3(1, 1, 1);
            OptionScreen.SetActive(false);
            FakeBoard.SetActive(false);
        }
    }

    // 메인 화면으로 이동
    public void GotoMain()
    {
        Time.timeScale = 1;     // 일시정지 해제
        SceneManager.LoadScene("MenuScene");
    }

    // 재시작
    public void RestartEasy()
    {
        Time.timeScale = 1;     // 일시정지 해제
        SceneManager.LoadScene("CardSystem");
    }

    public void RestartHard()
    {
        Time.timeScale = 1;     // 일시정지 해제
        SceneManager.LoadScene("TestSystem");
    }

    // 카드 기능 함수
    public void CardFunction(int num)
    {
        switch (num)
        {
            case 0:
                wallCard = 0;
                CardCover.SetActive(true);
                UIArray_N[2].SetActive(true);
                Dots.SetActive(true);
                break;
            case 1:
                wallCard = 1;
                CardCover.SetActive(true);
                UIArray_N[2].SetActive(true);
                Dots.SetActive(true);
                break;
            case 2:
                wallCard = 2;
                CardCover.SetActive(true);
                UIArray_N[2].SetActive(true);
                Dots.SetActive(true);
                break;
            case 3:
                wild();
                break;
            case 4:
                PlaySoundA(20);
                provoke();
                break;
            case 5:
                On_Scrow();
                break;
            case 6:
                On_Catnip();
                break;
            case 7:
                On_SOS();
                break;
            case 8:
                wallCard = 8;
                rw.canRemove = true;
                CardCover.SetActive(true);
                UIArray_N[3].SetActive(true);
                break;
        }
    }

    // 비둘기 전갈 카드 (시간 누적 O)
    public void On_SOS()
    {
        isSOS = true;
        SOS_repeat++;

        if (SOS_repeat == 1)
        {
            HeroAnimator.SetBool("run", true);
            HeroSpeed = 2;
            HeroSOS.SetActive(true);
            StartCoroutine("Off_SOS");
        }
        
    }

    IEnumerator Off_SOS()
    {

        while (SOS_repeat > 0)
        {
            yield return new WaitForSeconds(10f);
            SOS_repeat--;
        }
        isSOS = false;
        HeroDual.SetActive(false);
        HeroAnimator.SetBool("run", false);
        HeroSpeed = 1;
        HeroSOS.SetActive(false);
    }

    // 와일드 카드
    public void wild()
    {
        isWild = true;
        UIArray_N[4].SetActive(true);
        WhiteColorChange(0.4f);
    }

    public void WhiteColorChange(float b)
    {
        for (int i = 0; i < White_Card.Length; i++)
        {
            White_Card[i].color = new Color(1, 1, b, 1);
        }
    }

    // 도발 카드 (시간 누적O)
    public void provoke()
    {
        isProvoke = true;
        Provoke_repeat++;
        if (Provoke_repeat == 1)
        {
            cat_wait = cat_wait - 1;
            graphic_change(2);
            SP_Slider.value = 0;
            StartCoroutine("After_provoke");
        }

    }

    IEnumerator After_provoke()
    {
        while (Provoke_repeat > 0)
        {
            yield return new WaitForSeconds(18f);
            Provoke_repeat--;
        }
        cat_wait = cat_wait + 1;
        SP_Slider.value = 0;
        graphic_change(0);
        isProvoke = false;
    }

    // JPS
    IEnumerator CatMove()
    {
        // 게임 시작 후 잠시동안은 움직이지 않음
        Cat.position = new Vector3(blockSize - 7.4f, 6 * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize + 2.3f);
        yield return new WaitForSeconds(4f);

        while (isGame)
        {
            bool provokeFlag = false;
            // 캣닢에 닿았다면
            if (isCatnipOn)
            {
                graphic_change(4);
                int stopTime = Random.Range(4, 7);
                // 허수아비 범위 내에서 캣닢을 밟았다면
                if (isScrowOn)
                {
                    CDS.Energy += 15;
                    CDS.EnergyText.text = "" + CDS.Energy;
                    SP_Slider.value -= 15;
                }
                print("캣닢 : 4 + " + stopTime + "초 추가 정지");
                yield return new WaitForSeconds(4 + stopTime);
                isCatnipOn = false;
                // 상태에 따라 이미지 변경
                if (isProvoke)
                    graphic_change(2);
                else if (isScrowOn)
                    graphic_change(3);
                else if (isCatnipOn)
                    graphic_change(4);
                else
                    graphic_change(0);
            }
            // 허수아비에 닿았다면
            else if (isScrowOn)
            {
                graphic_change(3);
                CDS.Energy += 15;
                CDS.EnergyText.text = "" + CDS.Energy;
                SP_Slider.value -= 15;
                yield return new WaitForSeconds(4);
            }
            else
            {
                // 마법사가 움직이는 타이밍 조절
                for (int i = 0; i < cat_wait; i++)
                {
                    if (SP_Slider.value < 100 && !isProvoke)
                    {
                        SP_Slider.value++;
                    }
                    if (isProvoke)
                        provokeFlag = true;
                    yield return new WaitForSeconds(1f);
                }
            }

            // 다음 순서의 길이 있다면 다음 노드로 이동
            if (!provokeFlag)
            {
                if (gridView.minIndex >= 0 && gridView.isPath && gridView.CatIndex < gridView.CatPath[gridView.minIndex].Count)
                {
                    gridView.CatIndex++;
                    next = gridView.CatPath[gridView.minIndex][gridView.CatIndex];
                    bool isColumn = next.column % 2 == 1 ? true : false;
                    bool isRow = next.row % 2 == 1 ? true : false;
                    float xSize = 0, ySize = 0;

                    // 위치 지정
                    if (isColumn)
                    {
                        xSize = (next.column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
                    }
                    else
                    {
                        xSize = next.column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
                    }
                    if (isRow)
                    {
                        ySize = (next.row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
                    }
                    else
                    {
                        ySize = next.row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
                    }

                    // 시작 위치 변경
                    gridView.temp_x = next.column;
                    gridView.temp_y = next.row;

                    Cat.position = new Vector3(xSize - 7.4f, ySize + 2.3f, 5); // 수동으로 변경할 부분 좌표계
                    yield return new WaitForSeconds(0.000000001f);
                }

                // 마법사가 맨 오른쪽 칸에 도달한다면 게임종료
                if (next.column == 36)
                {
                    isGame = false;
                    gridView.isGame = false;
                    Lose();
                }
            }
            else
                provokeFlag = false;
        }

    }

    // 마법사 sprite 변경
    public void graphic_change(int curr)
    {
        for (int i = 0; i < Cat_graphic.Length; i++)
        {
            Cat_graphic[i].SetActive(false);
        }
        Cat_graphic[curr].SetActive(true);
    }

    // 휴식 스킬
    public void Cat_rest()
    {
        graphic_change(1);
        cat_wait = 12;
        Invoke("Cat_SPplus", 12f);
    }
    
    void Cat_SPplus()
    {
        if (SP_Slider.value <= 70)
        {

            SP_Slider.value += 30;
        }
        else
            SP_Slider.value = 100;
        cat_wait = 4;
        // 상태에 따라 이미지 변경
        if (isProvoke)
            graphic_change(2);
        else if (isScrowOn)
            graphic_change(3);
        else if (isCatnipOn)
            graphic_change(4);
        else
            graphic_change(0);
    }

    // 저주 스킬
    public void Cat_curse()
    {
        if (SP_Slider.value < 30)
            return;

        On_ErrorUI(4);
        graphic_change(1);
        isCurse = true;
        Hero.localScale = new Vector3(-1, 1, 1);
        HeroCurse.SetActive(true);
        SP_Slider.value -= 30;
        if (isSOS)
        {
            HeroSOS.SetActive(false);
            HeroDual.SetActive(true);
        }
        Invoke("After_curse", 15f);

    }

    void After_curse()
    {
        isCurse = false;
        HeroCurse.SetActive(false);
        HeroDual.SetActive(false);
        if (isSOS)
            HeroSOS.SetActive(true);
        Hero.localScale = new Vector3(1, 1, 1);
        // 상태에 따라 이미지 변경
        if (isProvoke)
            graphic_change(2);
        else if (isScrowOn)
            graphic_change(3);
        else if (isCatnipOn)
            graphic_change(4);
        else
            graphic_change(0);
    }

    // 공격 스킬
    public void Cat_attack()
    {
        if (SP_Slider.value < 30)
            return;

        graphic_change(1);
        SP_Slider.value -= 30;
        StartCoroutine("Attack");
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(2.5f);
        ClawObj.SetActive(true);
        CardCover.SetActive(true);
        Claw.fillOrigin = 1;
        for(int i = 0; i <= 10; i++)
        {
            Claw.fillAmount = i * 0.1f;
            yield return new WaitForSeconds(0.002f);
            if (i == 4)
            {
                RedObj.SetActive(true);
                CDS.CardAttack();
                PlaySoundA(22);
            }
        }
        CardCover.SetActive(false);
        Claw.fillOrigin = 0;
        for (int i = 15; i >= 0; i--)
        {
            Claw.fillAmount = i * 0.066f;
            yield return new WaitForSeconds(0.002f);
            if (i == 5)
                RedObj.SetActive(false);
        }
        ClawObj.SetActive(false);
        // 상태에 따라 이미지 변경
        if (isProvoke)
            graphic_change(2);
        else if (isScrowOn)
            graphic_change(3);
        else if (isCatnipOn)
            graphic_change(4);
        else
            graphic_change(0);
    }

    // UI관련 함수
    IEnumerator On_UI(int num)
    {
        float valueA = 1;
        UIArray_E[num].SetActive(true);
        yield return new WaitForSeconds(0.1f);
        // 그 UI 이미지가 점점 투명해지다가 꺼지게 한다.
        for (int i = 1; i <= 10; i++)
        {
            yield return new WaitForSeconds(0.1f);
            valueA -= 0.1f;
            UIImage_E[num].color = new Color(1, 1, 1, valueA);
        }
        UIArray_E[num].SetActive(false);
    }

    // 에러UI 띄우기
    public void On_ErrorUI(int num)
    {
        StopCoroutine("On_UI");
        StartCoroutine("On_UI", num);
    }

    // 뇌물 카드 함수
    public void On_Catnip()
    {
        isCatnip = true;
        CardCover.SetActive(true);
        UIArray_N[0].SetActive(true);
    }

    public void CreateCatnip(int column, int row)
    {
        // 고양이 주변에는 캣닢 설치 불가
        if (next.column - 2 <= column && column <= next.column + 2 && next.row - 2 <= row && row <= next.row + 2)
        {
            On_ErrorUI(0);
            return;
        }

        isCatnip = false;
        bool isColumn = column % 2 == 1 ? true : false;
        bool isRow = row % 2 == 1 ? true : false;
        float xSize = 0, ySize = 0;

        // 위치 지정
        if (isColumn)
        {
            xSize = (column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
        }
        else
        {
            xSize = column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
        }
        if (isRow)
        {
            ySize = (row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
        }
        else
        {
            ySize = row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
        }
        catnip[catnipIndex].SetActive(true);
        catnipXY[catnipIndex].position = new Vector3(xSize-7.3f, ySize+2.3f, 5f);

        StartCoroutine("Check_Catnip", catnipIndex);

    }

    IEnumerator Check_Catnip(int index)
    {
        yield return new WaitForSeconds(0.0000000001f);
        // 캣닢이 설치된 곳에 캣닢 또는 허수아비가 있지 않다면 정상작동
        if (!isCatnip)
        {
            PlaySoundA(21);
            UIArray_N[0].SetActive(false);
            CardCover.SetActive(false);
            catnipIndex = (catnipIndex + 1) % maxCatnip;
            yield return new WaitForSeconds(15f);
            catnip[index].SetActive(false);
        }
        // 아니라면 캣닢설치 취소 및 에러창 생성
        else
        {
            catnip[index].SetActive(false);
            On_ErrorUI(0);
        }
    }

    // 캣닢 미리보기
    public void CatnipPreview(int column, int row)
    {
        // 고양이 주변에는 캣닢 설치 불가
        if (next.column - 2 <= column && column <= next.column + 2 && next.row - 2 <= row && row <= next.row + 2)
        {
            return;
        }

        bool isColumn = column % 2 == 1 ? true : false;
        bool isRow = row % 2 == 1 ? true : false;
        float xSize = 0, ySize = 0;

        // 위치 지정
        if (isColumn)
        {
            xSize = (column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
        }
        else
        {
            xSize = column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
        }
        if (isRow)
        {
            ySize = (row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
        }
        else
        {
            ySize = row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
        }
        catnipView.SetActive(true);
        catnipVxy.position = new Vector3(xSize - 7.3f, ySize + 2.3f, 0);
    }

    public void CatnipRemove()
    {
        catnipView.SetActive(false);
    }

    // 허수아비 카드 함수
    public void On_Scrow()
    {
        isScrow = true;
        CardCover.SetActive(true);
        UIArray_N[1].SetActive(true);
    }

    public void CreateScrow(int column, int row)
    {
        // 고양이 주변에는 허수아비 설치 불가
        if (next.column - 2 <= column && column <= next.column + 2 && next.row - 2 <= row && row <= next.row + 2)
        {
            On_ErrorUI(0);
            return;
        }

        isScrow = false;
        bool isColumn = column % 2 == 1 ? true : false;
        bool isRow = row % 2 == 1 ? true : false;
        float xSize = 0, ySize = 0;

        // 위치 지정
        if (isColumn)
        {
            xSize = (column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
        }
        else
        {
            xSize = column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
        }
        if (isRow)
        {
            ySize = (row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
        }
        else
        {
            ySize = row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
        }
        scrow[scrowIndex].SetActive(true);
        scrowXY[scrowIndex].position = new Vector3(xSize - 7.325f, ySize + 2.275f, -1);

        StartCoroutine("Check_Scrow", scrowIndex);

    }

    IEnumerator Check_Scrow(int index)
    {
        yield return new WaitForSeconds(0.0000000001f);
        // 허수아비가 설치된 곳에 캣닢 또는 허수아비가 있지 않다면 정상작동
        if (!isScrow)
        {
            PlaySoundA(19);
            UIArray_N[1].SetActive(false);
            CardCover.SetActive(false);
            scrowIndex = (scrowIndex + 1) % maxScrow;
            yield return new WaitForSeconds(15f);
            scrow[index].SetActive(false);
            N_Scrow.ScrowOn[index] = false;
            if (isScrowOn)
            {
                isScrowOn = false;
                // 상태에 따라 이미지 변경
                if (isProvoke)
                    graphic_change(2);
                else if (isScrowOn)
                    graphic_change(3);
                else if (isCatnipOn)
                    graphic_change(4);
                else
                    graphic_change(0);
            }
        }
        // 아니라면 허수아비설치 취소 및 에러창 생성
        else
        {
            scrow[index].SetActive(false);
            On_ErrorUI(0);
        }
    }

    // 허수아비 미리보기
    public void ScrowPreview(int column, int row)
    {
        // 고양이 주변에는 허수아비 설치 불가
        if (next.column - 2 <= column && column <= next.column + 2 && next.row - 2 <= row && row <= next.row + 2)
        {
            return;
        }

        bool isColumn = column % 2 == 1 ? true : false;
        bool isRow = row % 2 == 1 ? true : false;
        float xSize = 0, ySize = 0;

        // 위치 지정
        if (isColumn)
        {
            xSize = (column + 1) * 0.5f * (blockSize * 7f + blockBuffer) - blockSize * 3f;
        }
        else
        {
            xSize = column * 0.5f * (blockSize * 7f + blockBuffer) + blockSize;
        }
        if (isRow)
        {
            ySize = (row + 1) * 0.5f * -(blockSize * 7f + blockBuffer) + blockSize * 3f;
        }
        else
        {
            ySize = row * 0.5f * -(blockSize * 7f + blockBuffer) - blockSize;
        }
        scrowView.SetActive(true);
        scrowVxy.position = new Vector3(xSize - 7.325f, ySize + 2.275f, 0);
    }

    public void ScrowRemove()
    {
        scrowView.SetActive(false);
    }

    // 플레이어가 이겼을 때
    public void Win()
    {
        PlaySoundA(12);
        Game_clear.SetActive(true);
        int pathCount = gridView.CatPath[gridView.minIndex].Count - gridView.CatIndex;
        bonus_gold = (pathCount - 1) * 40;
        StartCoroutine("Clear", pathCount);
    }

    IEnumerator Clear(int pathCount)
    {
        //Clear_window.SetActive(true);
        remain_time = 4 * pathCount;
        timeText_C.text = "" + remain_time;
        yield return new WaitForSeconds(1f);
        while (bonus_gold >= total_gold)
        {
            if (bonus_gold - total_gold > 10)
            {
                total_gold += 10;
                goldText.text = "" + total_gold;
                yield return new WaitForSeconds(0.02f);
            }
            else
            {
                total_gold++;
                goldText.text = "" + total_gold;
                yield return new WaitForSeconds(0.01f);
            }
        }
        N_PlayerInfo.Gold += total_gold;
    }

    // 플레이어가 졌을 때
    public void Lose()
    {
        print("Lose");
        CDS.StopAllCoroutines();
        CardCover.SetActive(true);
        Game_Over.SetActive(true);
        remain_time = (int)HeroSlider.value / 2;
        timeText_O.text = "" + remain_time;
    }

    //스킬 사용 타이밍 판단 함수
    public void CatSkill()
    {
        // 도발상태, 캣닢상태일때는 스킬을 사용하지 않는다.
        if (isWild || isCatnipOn)
            return;

        RemainPath = gridView.CatPath[gridView.minIndex].Count - gridView.CatIndex;
        
        if (skill_hard <= RemainPath && SP_Slider.value>=80)
        {
            //폭발 스킬 쓰기
            print("폭발스킬 써야함");
        }
        else if (SP_Slider.value >= 30)
        {
            if (Random.Range(0, 2) == 0)
            {
                print("공격");
                Cat_attack();
            }
            else
            {
                print("저주");
                Cat_curse();
            }
        }
        // 허수아비 위에서는 휴식 스킬 사용 X
        else if(countRest && !isScrowOn)
        {
            print("휴식");
            Cat_rest();
        }

        if (!countRest)
            countRest = true;
        
    }

}
