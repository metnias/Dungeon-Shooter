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
            case ItemType.Key: Player_Inventory.NumKey += amount; break;
            case ItemType.Arrow:
                Player_Inventory.NumArrow = Mathf.Min(999, Player_Inventory.NumArrow + amount); // clamp at 999
                break;
            case ItemType.Heart:
                if (Player_Inventory.Health == Player_Inventory.maxHealth) return; // reject
                Player_Inventory.Health += amount;
                break;
        }
        Sound_Manager.Instance().PlaySE(SEType.PickUp);
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        Rigidbody2D rBody = gameObject.GetComponent<Rigidbody2D>();
        rBody.gravityScale = -3f;
        rBody.AddForce(Vector2.down * 7f, ForceMode2D.Impulse);
        Destroy(gameObject, 2f);
    }
}
