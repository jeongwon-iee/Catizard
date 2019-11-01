using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class appUI : MonoBehaviour
{
    public reNum temp;
    public dotActive posWall;
    private int a;

    private Renderer rend;

    void Start()
    {
        a = 0;
        gameObject.SetActive(false);
        //rend = gameObject.GetComponent<Renderer>();
        //rend.enabled = false;
    }

}
