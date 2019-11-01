using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotActive : MonoBehaviour
{
    private float blockSize, blockBuffer;
    public GridView gridView;

    private Transform thisTransform;
    private int cardType;

    void Awake()
    {
        blockSize = gridView.blockSize;
        blockBuffer = gridView.blockBuffer;
        thisTransform = GetComponent<Transform>();
        cardType = 0;
    }

    public void wallPreview(int column, int row)
     {
        bool isColumn = column % 2 == 1 ? true : false;
        bool isRow = row % 2 == 1 ? true : false;
        float xSize = 0, ySize = 0;

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

        thisTransform.position = new Vector3(xSize-7.3f, ySize+2.22f); // dot 위치로 이동

        gameObject.SetActive(true); // 보이게 하기
    }

    public void exitPreview()
    {
        gameObject.SetActive(false);
    }
}
/*SpriteRenderer spr = GetComponent<SpriteRenderer>();
Color color = spr.color;
color.a = 0.5f;
spr.color = color;*/ // 투명도 조절 코드인데 왜인지 안먹는... 그래서 그냥 렌더러에서 직접 수정
