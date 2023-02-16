using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Chest : MonoBehaviour
{
    public Sprite sprOpen;
    public Sprite sprClose;
    public GameObject[] itemsPrefab;
    [HideInInspector]
    public bool isClosed = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isClosed) return;
        if (!collision.gameObject.CompareTag("Player")) return;

        int i = Random.Range(0, itemsPrefab.Length);
        if (itemsPrefab[i] != null)
        {
            var item = Instantiate(itemsPrefab[i], transform.position, Quaternion.identity);
            GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            Invoke(nameof(Close), 1f);
        }
        isClosed = false;
        GetComponent<SpriteRenderer>().sprite = sprOpen;
        Sound_Manager.Instance().PlaySE(SEType.DoorOpen);
    }

    private void Close()
    {
        isClosed = true;
        GetComponent<SpriteRenderer>().sprite = sprClose;
        Sound_Manager.Instance().PlaySE(SEType.DoorClose);
    }

}
