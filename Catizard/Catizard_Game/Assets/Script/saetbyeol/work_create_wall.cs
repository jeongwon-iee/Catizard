using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class work_create_wall : MonoBehaviour
{
    public GameObject wall; // prefab
    private bool canCreate;
    private float timeCount;
    private Vector2 init;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start");
        canCreate = true;
        timeCount = 0;
        init = new Vector2(-2.89f, -1.48f);
    }

    // Update is called once per frame
    void Update()
    {

        timeCount += Time.deltaTime;

        if (timeCount > 5.0f) // 5초정도 쿨타임 만들어둠 확인용
        {
            canCreate = true; // 플래그 이용
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A click!");
            if (canCreate) // true == 쿨타임 지남
                CreateWalls(); // 함수 호출가능
        }
    }

    private GameObject[] walls; // 20개 벽을 넣어두는 함수
    private int wallNum = 20;
    private int i = 0;

    void CreateWalls() // 벽 설치하는 함수
    {
        Debug.Log("call CreateWalls()");
        walls = new GameObject[wallNum];
        if (i == 0)
            walls[i] = Instantiate(wall, init, Quaternion.identity) as GameObject; // 생성
        else
            walls[i] = Instantiate(wall, Vector2.zero, Quaternion.identity) as GameObject; // 생성
        walls[i].SetActive(true); // 비활성화
        i++;
        canCreate = false;
        timeCount = 0;
    }
}


/* clone 오브젝트 삭제

void Update() { 
        if (Input.GetMouseButtonDown(0)){ 
            Debug.Log("Pressed left click."); 
Vector2 p = Input.mousePosition; 
//p.y = Screen.height - p.y; 

Ray ray = Camera.main.ScreenPointToRay(p);	
RaycastHit hit; 

if(Physics.Raycast(ray, out hit)){ 
if(hit.collider.tag == "test"){ 
Debug.Log("delete");	
Debug.Log(hit); 
Destroy(gameObject); 
} 
} 

} 
 
        
    }*/
