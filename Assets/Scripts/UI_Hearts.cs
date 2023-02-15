using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hearts : MonoBehaviour
{
    public Sprite[] sprHearts;

    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        int i = Mathf.Clamp(Player_Inventory.health, 0, sprHearts.Length);
        image.sprite = sprHearts[i];
    }
}
