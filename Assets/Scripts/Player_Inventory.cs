using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    public static int numKey = 0;
    public static int numArrow = 30;
    public static int health = 3;
    public static int maxHealth = 3;

    public static void Reset() // todo: call this on title scren
    {
        numKey = 0;
        numArrow = 30;
        health = maxHealth;
    }


}
