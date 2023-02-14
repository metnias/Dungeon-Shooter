using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room_Manager : MonoBehaviour
{
    public static int doorNumber = 0;

    void Start()
    {
        var exits = GameObject.FindGameObjectsWithTag("Exit");
        for (int i = 0; i < exits.Length; i++)
        {
            Portal_Handler pHandler = exits[i].GetComponent<Portal_Handler>();
            if (doorNumber != pHandler.doorNumber) continue;
            Vector2 pos = exits[i].transform.position;
            switch (pHandler.direction)
            {
                case ExitDirection.Up: pos.y += 1f; break;
                default:
                case ExitDirection.Down: pos.y -= 1f; break;
                case ExitDirection.Left: pos.x -= 1f; break;
                case ExitDirection.Right: pos.x += 1f; break;
            }

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) player.transform.position = pos;
            break;
        }
    }

    internal static void ChangeScene(string sceneName, int doorNumber)
    {
        Room_Manager.doorNumber = doorNumber;
        SceneManager.LoadScene(sceneName);
    }
}
