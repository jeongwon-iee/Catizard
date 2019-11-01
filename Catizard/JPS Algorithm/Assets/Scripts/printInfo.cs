using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class printInfo : MonoBehaviour
{

    public enum eDirections
    {
        NORTH = 0,
        NORTH_EAST = 1,
        EAST = 2,
        SOUTH_EAST = 3,
        SOUTH = 4,
        SOUTH_WEST = 5,
        WEST = 6,
        NORTH_WEST = 7,
    }

    private eDirections[] allDirections = Enum.GetValues(typeof(eDirections)).Cast<eDirections>().ToArray();

    public void printArray()
    {
        string s;
        for (int i = 0; i < allDirections.Length; i++)
        {
            s = i + " : " + allDirections[i];
            print(s);
        }
    }





}
