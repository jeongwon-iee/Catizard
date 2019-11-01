using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class N_Catnip : MonoBehaviour
{

    public N_CardSystem NCS;

    public void OnTriggerEnter2D(Collider2D coll)
    {
        // 고양이와 닿았다면
        if (coll.gameObject.CompareTag("cat"))
        {
            NCS.isCatnipOn = true;
            gameObject.SetActive(false);
        }

        // 다른 캣닢 또는 허수아비와 닿았다면
        if (coll.gameObject.CompareTag("catnip") || coll.gameObject.CompareTag("scrowCenter"))
        {
            NCS.isCatnip = true;
        }
    }

}
