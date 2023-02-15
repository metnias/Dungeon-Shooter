using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Key,
    Arrow,
    Heart
}

public class Item_Handler : MonoBehaviour
{
    public ItemType type;
    public int amount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        switch (type)
        {
            case ItemType.Key: Player_Inventory.numKey += amount; break;
            case ItemType.Arrow: Player_Inventory.numArrow += amount; break;
            case ItemType.Heart: Player_Inventory.health += amount; break; // todo: reject if player health is max
        }
        gameObject.SetActive(false);
    }
}
