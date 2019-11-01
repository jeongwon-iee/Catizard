using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class removeWall : MonoBehaviour
{
    public putWall p;
    public bool canRemove;
    public GridView gridview;
    public N_CardSystem card;
    private int bIndex;
    private int type;
    private int rotation;

    void Awake()
    {
        canRemove = false;
    }

    public void remove(int i)
    {
        card.PlaySoundA(18);

        bIndex = p.blockIndex[i];
        type = p.wallType[i];
        rotation = p.wallRotation[i];

        switch (type)
        {
            case 0:
                {
                    if (rotation % 2 ==0)
                    {
                        gridview.grid.gridNodes[bIndex - 37].isObstacle = false;
                        gridview.grid.gridNodes[bIndex + 37].isObstacle = false;
                    }
                    else
                    {
                        gridview.grid.gridNodes[bIndex - 1].isObstacle = false;
                        gridview.grid.gridNodes[bIndex + 1].isObstacle = false;
                    }
                }
                break;
            case 1:
                {
                    switch (rotation)
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
                }
                break;
            case 2:
                {
                    switch (rotation)
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
                }
                break;
        }

        p.wallList.RemoveAt(i);
        p.wallType.RemoveAt(i);
        p.wallRotation.RemoveAt(i);
        p.blockIndex.RemoveAt(i);
        p.last--;

        gridview.Reset();
        gridview.JPS();
        card.wallCard = -1;
        card.CardCover.SetActive(false);
        card.UIArray_N[3].SetActive(false);
        canRemove = false;
    }
}
