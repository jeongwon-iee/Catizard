using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class remove : MonoBehaviour
{
    public putWall pw;
    public removeWall rw;
    public int index;

    private SpriteRenderer thisSprite;

    void Awake()
    {
        index = -1;
        thisSprite = GetComponent<SpriteRenderer>();
    }

    void OnMouseOver()
    {
        if (rw.canRemove)
        {
            thisSprite.color = new Color(0.55f, 0.55f, 0.55f, 1);
        }
    }

    private void OnMouseExit()
    {
        if (rw.canRemove)
        {
            thisSprite.color = new Color(1, 1, 1, 1);
        }
    }

    void OnMouseDown()
    {
        destroy();
    }

    void destroy()
    {
        if (rw.canRemove)
        {
            if (pw.wallList.Exists(x => x.gameObject)) //if (pw.wallList.Exists(x => x.wall == gameObject))
            {
                index = pw.wallList.FindIndex(x => x.gameObject);//                index = pw.wallList.FindIndex(x => x.wall == gameObject);
                print("제거된 벽의 index : " + index);
                rw.remove(index);
                Destroy(gameObject);
            }
        }
    }
}
