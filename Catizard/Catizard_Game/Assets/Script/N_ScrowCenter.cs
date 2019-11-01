using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_ScrowCenter : MonoBehaviour
{
    public N_CardSystem NCS;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        // 다른 캣닢 또는 허수아비와 닿았다면
        if (coll.gameObject.CompareTag("catnip") || coll.gameObject.CompareTag("scrowCenter"))
        {
            NCS.isScrow = true;
        }
    }

}
