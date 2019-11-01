using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_CameraEvent : MonoBehaviour
{

    public float speed = 2f, speedXY = 1f;
    public bool isMove = false;

    private Camera thisCamera;
    private Transform thisTransform;
    private float scroll, moveHorizontal, moveVertical;
    private Vector3 temp = new Vector3(0, 0, -10), origin = new Vector3(0, 0, -10);

    // Start is called before the first frame update
    void Awake()
    {
        thisCamera = GetComponent<Camera>();
        thisTransform = GetComponent<Transform>();
    }

    // Update is called once per frame
    private void Update()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel") * speed;

        // 최대 줌인
        if (thisCamera.orthographicSize <= 3.0f && scroll < 0)
        {
            thisCamera.orthographicSize = 3.0f;
        }
        // 최대 줌아웃
        else if (thisCamera.orthographicSize >= 7.0f && scroll > 0)
        {
            thisCamera.orthographicSize = 7.0f;
        }
        // 줌인/줌아웃
        else
        {
            thisCamera.orthographicSize += scroll;
        }

        // 원상태로 돌아가기
        if (Input.GetKey(KeyCode.R))
        {
            thisCamera.orthographicSize = 5;
            thisTransform.position = origin;
        }

        // 화면 이동
        if (isMove)
        {
            moveHorizontal = Input.GetAxis("Mouse X") * speedXY;
            moveVertical = Input.GetAxis("Mouse Y") * speedXY;

            temp.x = thisTransform.position.x + moveHorizontal;
            temp.y = thisTransform.position.y + moveVertical;

            if (temp.x > 5)
                temp.x = 5;
            else if (temp.x < -5)
                temp.x = -5;

            if (temp.y > 2)
                temp.y = 2;
            else if (temp.y < -2)
                temp.y = -2;

            thisTransform.position = temp;
        }

    }

    public void MoveOn()
    {
        isMove = true;
    }

    public void MoveOff()
    {
        isMove = false;
    }

}
