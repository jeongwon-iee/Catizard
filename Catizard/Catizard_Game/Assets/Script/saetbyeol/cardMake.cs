using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cardMake : MonoBehaviour
{
    public GameObject wall; // prefab
    public GameObject card;
    private bool canCreate; // 쿨타임 측정하는 플래그
    private float timeCount; // 쿨타임 측정하기 // 변동가능
    public Vector2 makePos; // 최종적으로 벽을 설치할 좌표
    private Vector2 temp;
    public static bool clickMake; // 카드 클릭 확인

    private GameObject[] walls; // 20개 벽을 넣어두는 함수
    private int[] wallIndex; // 프리팹 wall 인덱스 담는 함수, 벽을 제거하면 해당 인덱스 배열에서 삭제됨
    private int wallNum = 20;
    private int i = 0;

    Camera cam;
    GameObject cardObj;
    Vector2 origin;
    Vector2 dir;
    private GameObject target;
    RaycastHit2D hitMain;

    void Start()
    {
        canCreate = true;
        timeCount = 0;
        target = null;

        cam = Camera.main;
        cardObj = null;
        origin = Vector2.zero;
        dir = Vector2.zero;

        clickMake = false;
    }

    /*
    void Update()
    {
        timeCount += Time.deltaTime; // 시간 체크

        if (timeCount > 7.0f) // timeCount가 7초 이상이면 -> 벽 생성 가능
            canCreate = true; // 플래그 이용

        if (cam.orthographic)
        {
            origin = Camera.main.ScreenToWorldPoint(Input.mousePosition); //origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            origin = ray.origin;
            dir = ray.direction;
        }
        hitMain = Physics2D.Raycast(origin, dir);
        if (hitMain && Input.GetMouseButtonDown(0)) // 마우스가 카드 위에 있고 클릭하면
        {
            cardObj = hitMain.collider.gameObject;
            if (cardObj != null && cardObj.tag == "cardMake") // hit된 오브젝트가 cardMake tag라면
            {
                Debug.Log("Card click");
                StartCoroutine("CallMakeCo"); // 코루틴 시작
                clickMake = true;
            }
        } // 카드 클릭 if

    }*/

    public void CreateBlock()
    {
        Debug.Log("Card click");
        StartCoroutine("CallMakeCo"); // 코루틴 시작
        clickMake = true;
    }

    IEnumerator CallMakeCo() // 코루틴
    {
        //RaycastHit2D hitCo = Physics2D.Raycast(origin, dir);
        Debug.Log("callMake");
        while (true)
        {
            yield return null;
            if (hitMain && Input.GetMouseButtonDown(0))  // 마우스 클릭했다면
            {
                temp = Input.mousePosition;
                target = hitMain.collider.gameObject; // hitCo와 hit된 오브젝트 target
                 if (target != null && target.tag == "dots")
                        CreateWalls(temp); // 함수 호출가능
            } // 클릭 판별 if
        }
    }

    void CreateWalls(Vector2 pos) // 벽 설치하는 함수
    {
        Debug.Log("call CreateWalls()");
        walls = new GameObject[wallNum];
        walls[i] = Instantiate(wall, pos, Quaternion.identity) as GameObject; // 생성
        Debug.Log("walls~~~");
        walls[i++].SetActive(true); // 활성화
        canCreate = false;
        timeCount = 0; // 다시 시간 초기화
    } // CreateWalls 함수
}
/*if (true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))        //마우스 근처에 오브젝트가 있는지 확인
{
    Debug.Log("there is an object near the mouse");
    target = hit.collider.gameObject; // 있으면 오브젝트 저장
}*/
