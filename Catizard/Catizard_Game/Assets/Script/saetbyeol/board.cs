using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct boardMatrix
{
    public int x; // x좌표
    public int y; // y좌표
    public bool road; // 길이면 true, 벽을 설치하는 곳이면 false
    public bool install; // 벽을 설치할 수 있으면 true
    public bool move; // 고양이가 움직일 수 있으면 true

    public boardMatrix(int x, int y, bool road, bool install, bool move)
    {
        this.x = x;
        this.y = y;
        this.road = road;
        this.install = install; 
        this.move = move;
    }
};
public class board : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Debug.Log("board");
        transform.position = new Vector2(0, 0); // 초기 좌표
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
