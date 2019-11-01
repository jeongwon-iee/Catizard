using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doveCardClick_sb : MonoBehaviour {

    void Start(){
        Debug.Log("Start");
    }

    /* void Update()
     {

         if (Input.GetMouseButtonDown(0))
         {
             Vector2 rayPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
             RaycastHit2D hit = Physics2D.Raycast(rayPosition, Vector2.zero, 100);

             if (hit)
             {
                 Debug.Log("Click!");
             }

         }

     }*/

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("click!");
            GameObject.Find("yongsa").GetComponent<move_ys_sb>().changeSp();
        }
    }
}
