using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    public static int numKey = 0;
    public static int numArrow = 50;
    public static int health = 3;

    private void Start()
    {
        
    }

    public static void Reset()
    {
        numKey = 0;
        numArrow = 10;
        health = 3;
    }


    void Update()
    {
        
    }
}
