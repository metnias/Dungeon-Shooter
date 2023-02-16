using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Counter : MonoBehaviour
{
    public bool isKey;

    private Text text;

    void Start()
    {
        text = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        int n = isKey ? Player_Inventory.NumKey : Player_Inventory.NumArrow;
        text.text = n.ToString();
    }
}
