using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class putWall : MonoBehaviour
{
    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;
    public GameObject Dots;
    public List<GameObject> wallList;
    public List<int> wallType;
    public List<int> wallRotation;
    public List<int> blockIndex;
    public Vector3 pos;
    private int i = 0;
    private int wallNum = 100;
    private bool checkPos = false;
    public int last = 0;

    private float blockSize, blockBuffer;
    public GridView gridview;
    public reNum n;
    public N_CardSystem card;

    public int cardType;
    public bool canPut = false;
    private int bIndex;

    void Awake()
    {
        List<GameObject> wallList = new List<GameObject>();
        List<int> wallType = new List<int>();
        List<int> wallRotation = new List<int>();
        List<int> blockIndex = new List<int>();
        Dots.SetActive(false);
        blockSize = gridview.blockSize;
        blockBuffer = gridview.blockBuffer;
        cardType = -1;
        bIndex = -1;
    }

    public void checkAble(int column, int row, int cardtype)
    {
        bIndex = row * 37 + column;
        this.cardType = cardtype;
        bool able = true;
        for(int k = 0; k < gridview.rand_x.Length; k++)
        {
            if (gridview.grid.gridNodes[2 * gridview.rand_y[k] * 37 + 2 * gridview.rand_x[k]] == gridview.grid.gridNodes[bIndex + 38] 
                || gridview.grid.gridNodes[2 * gridview.rand_y[k] * 37 + 2 * gridview.rand_x[k]] == gridview.grid.gridNodes[bIndex + 36]
                || gridview.grid.gridNodes[2 * gridview.rand_y[k] * 37 + 2 * gridview.rand_x[k]] == gridview.grid.gridNodes[bIndex - 36] 
                || gridview.grid.gridNodes[2 * gridview.rand_y[k] * 37 + 2 * gridview.rand_x[k]] == gridview.grid.gridNodes[bIndex - 38])
            {
                card.On_ErrorUI(0);
                able = false;
                return;
            }
        }

        if (able == true)
            checkPosition(column, row);
    }

    public void checkPosition(int column, int row)
    {
        // 벽이 겹치면 안됨
        if(cardType == 0)
        {
            if (n.shape == 0 || n.shape == 2) //  l 모양
            {
                if (gridview.grid.gridNodes[bIndex - 37].isObstacle == false && gridview.grid.gridNodes[bIndex + 37].isObstacle == false )
                    checkPos = true;
            }
            else // ㅡ 모양
            {
                if (gridview.grid.gridNodes[bIndex - 1].isObstacle == false && gridview.grid.gridNodes[bIndex + 1].isObstacle == false)
                    checkPos = true;
            }
        }
        else if(cardType == 1)
        {
            switch (n.shape)
            {
                case 0:
                    if (gridview.grid.gridNodes[(row + 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column + 1)].isObstacle == false)
                        checkPos = true;
                    break;
                case 1:
                    if (gridview.grid.gridNodes[(row - 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column + 1)].isObstacle == false)
                        checkPos = true;
                    break;
                case 2:
                    if (gridview.grid.gridNodes[(row - 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column - 1)].isObstacle == false)
                        checkPos = true;
                    break;
                case 3:
                    if (gridview.grid.gridNodes[(row + 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column - 1)].isObstacle == false)
                        checkPos = true;
                    break;
            }
        }
        else if (cardType == 2)
        {
            switch (n.shape)
            {
                case 0:
                    if (gridview.grid.gridNodes[(row + 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[(row-1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column + 1)].isObstacle == false)
                        checkPos = true;
                    break;
                case 1:
                    if (gridview.grid.gridNodes[(row - 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column + 1)].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column - 1)].isObstacle == false)
                        checkPos = true;
                    break;
                case 2:
                    if (gridview.grid.gridNodes[(row + 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[(row - 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column - 1)].isObstacle == false)
                        checkPos = true;
                    break;
                case 3:
                    if (gridview.grid.gridNodes[(row + 1) * 37 + column].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column + 1)].isObstacle == false && gridview.grid.gridNodes[row * 37 + (column - 1)].isObstacle == false)
                        checkPos = true;
                    break;
            }
        }

        // 겹치지 않는다면 장애물 처리해봄
        if (checkPos == true)
        {
            switch (cardType)
            {
                case 0:
                    if (n.shape == 0 || n.shape == 2) 
                    {
                        gridview.grid.gridNodes[bIndex - 37].isObstacle = true;
                        gridview.grid.gridNodes[bIndex + 37].isObstacle = true;
                    }
                    else 
                    {
                        gridview.grid.gridNodes[bIndex - 1].isObstacle = true;
                        gridview.grid.gridNodes[bIndex + 1].isObstacle = true;
                    }
                    break;
                case 1:
                    switch (n.shape)
                    {
                        case 0:
                            gridview.grid.gridNodes[bIndex - 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex + 1].isObstacle = true;
                            break;
                        case 1:
                            gridview.grid.gridNodes[bIndex + 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex + 1].isObstacle = true;
                            break;
                        case 2:
                            gridview.grid.gridNodes[bIndex + 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex - 1].isObstacle = true;
                            break;
                        case 3:
                            gridview.grid.gridNodes[bIndex - 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex - 1].isObstacle = true;
                            break;
                    }
                    break;
                case 2:
                    switch (n.shape)
                    {
                        case 0:
                            gridview.grid.gridNodes[bIndex + 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex - 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex + 1].isObstacle = true;
                            break;
                        case 1:
                            gridview.grid.gridNodes[bIndex + 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex - 1].isObstacle = true;
                            gridview.grid.gridNodes[bIndex + 1].isObstacle = true;
                            break;
                        case 2:
                            gridview.grid.gridNodes[bIndex + 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex - 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex - 1].isObstacle = true;
                            break;
                        case 3:
                            gridview.grid.gridNodes[bIndex - 37].isObstacle = true;
                            gridview.grid.gridNodes[bIndex - 1].isObstacle = true;
                            gridview.grid.gridNodes[bIndex + 1].isObstacle = true;
                            break;
                    }
                    break;
            }

            // 그리고 길을 찾아봄
            gridview.Reset();
            gridview.JPS();

            // 그랬는데 길을 못찼았다면
            if(gridview.minIndex < 0)
            {
                print("벽 설치 취소");
                card.On_ErrorUI(0);
                canPut = false;
                switch (cardType)
                {
                    case 0:
                        if (n.shape == 0 || n.shape == 2)
                        {
                            gridview.grid.gridNodes[bIndex - 37].isObstacle = false;
                            gridview.grid.gridNodes[bIndex + 37].isObstacle = false;
                        }
                        else
                        {
                            gridview.grid.gridNodes[bIndex - 1].isObstacle = false;
                            gridview.grid.gridNodes[bIndex + 1].isObstacle = false;
                        }
                        break;
                    case 1:
                        switch (n.shape)
                        {
                            case 0:
                                gridview.grid.gridNodes[bIndex - 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex + 1].isObstacle = false;
                                break;
                            case 1:
                                gridview.grid.gridNodes[bIndex + 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex + 1].isObstacle = false;
                                break;
                            case 2:
                                gridview.grid.gridNodes[bIndex + 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex - 1].isObstacle = false;
                                break;
                            case 3:
                                gridview.grid.gridNodes[bIndex - 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex - 1].isObstacle = false;
                                break;
                        }
                        break;
                    case 2:
                        switch (n.shape)
                        {
                            case 0:
                                gridview.grid.gridNodes[bIndex + 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex - 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex + 1].isObstacle = false;
                                break;
                            case 1:
                                gridview.grid.gridNodes[bIndex + 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex - 1].isObstacle = false;
                                gridview.grid.gridNodes[bIndex + 1].isObstacle = false;
                                break;
                            case 2:
                                gridview.grid.gridNodes[bIndex + 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex - 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex - 1].isObstacle = false;
                                break;
                            case 3:
                                gridview.grid.gridNodes[bIndex - 37].isObstacle = false;
                                gridview.grid.gridNodes[bIndex - 1].isObstacle = false;
                                gridview.grid.gridNodes[bIndex + 1].isObstacle = false;
                                break;
                        }
                        break;
                }

                // 그리고 길을 찾아 원상태로 돌아감
                gridview.Reset();
                gridview.JPS();
            }
            // 길을 찾았다면 그대로 벽설치 진행
            else
            {
                canPut = true;
                put(column, row);
                card.PlaySoundA(17);
            }
        }
    }

    void put(int column, int row)
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
        // dot 위치로 이동
        Vector3 putPosition;
        if (cardType == 0)
        {
            putPosition = new Vector3(xSize - 7.35f, ySize + 2.23f);

            wallList.Add(Instantiate(wall1, putPosition, Quaternion.identity) as GameObject);
            wallList[last].transform.Rotate(0, 0, -90 * n.shape);
            wallType.Add(cardType);
            wallRotation.Add(n.shape);
            blockIndex.Add(bIndex);

            wallList[last++].SetActive(true);
            card.wallCard = -1;
            bIndex = -1;
            card.CardCover.SetActive(false);
            card.UIArray_N[2].SetActive(false);
        }
        else if (cardType == 1)
        {
            putPosition = new Vector3(xSize - 7.353f, ySize + 2.30f);

            wallList.Add(Instantiate(wall2, putPosition, Quaternion.identity) as GameObject);
            wallList[last].transform.Rotate(0, 0, -90 * n.shape);
            wallType.Add(cardType);
            wallRotation.Add(n.shape);
            blockIndex.Add(bIndex);

            wallList[last++].SetActive(true);
            card.wallCard = -1;
            bIndex = -1;
            card.CardCover.SetActive(false);
            card.UIArray_N[2].SetActive(false);
        }
        else if (cardType == 2)
        {
            putPosition = new Vector3(xSize - 7.35f, ySize + 2.28f);

            wallList.Add(Instantiate(wall3, putPosition, Quaternion.identity) as GameObject);
            wallList[last].transform.Rotate(0, 0, -90 * n.shape);
            wallType.Add(cardType);
            wallRotation.Add(n.shape);
            blockIndex.Add(bIndex);

            wallList[last++].SetActive(true);
            card.wallCard = -1;
            bIndex = -1;
            card.CardCover.SetActive(false);
            card.UIArray_N[2].SetActive(false);
        }
        Dots.SetActive(false);
    }
}
