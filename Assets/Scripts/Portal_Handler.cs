using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ExitDirection
{
    Down,
    Right,
    Up,
    Left
}

public class Portal_Handler : MonoBehaviour
{
    public string sceneName = "";
    public int doorNumber = 0;
    public int targetDoorNumber = 0;
    public ExitDirection direction = ExitDirection.Down;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        Room_Manager.ChangeScene(sceneName, targetDoorNumber);
    }
}
