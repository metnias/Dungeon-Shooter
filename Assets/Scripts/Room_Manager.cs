using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room_Manager : MonoBehaviour
{
    [Serializable]
    public class RoomData
    {
        public int roomID = 0;
        public string roomTag = string.Empty;
    }

    public RoomData roomData;
    public BGMType bgm = BGMType.None;

    public static int doorNumber = 0;


    void Start()
    {
        MovePlayer();
        Sound_Manager.Instance().PlayBGM(bgm);
    }

    private static void MovePlayer()
    {
        // Move Player to linked portal from the last scene
        var exits = GameObject.FindGameObjectsWithTag("Exit");
        for (int i = 0; i < exits.Length; i++)
        {
            Portal_Handler pHandler = exits[i].GetComponent<Portal_Handler>();
            if (doorNumber != pHandler.doorNumber) continue;
            Vector2 pos = exits[i].transform.position;
            float angleZ;
            switch (pHandler.direction)
            {
                case ExitDirection.Up: pos.y += 1f; angleZ = 90f; break;
                default:
                case ExitDirection.Down: pos.y -= 1f; angleZ = -90f; break;
                case ExitDirection.Left: pos.x -= 1f; angleZ = 190f; break;
                case ExitDirection.Right: pos.x += 1f; angleZ = 0f; break;
            }

            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = pos;
                player.GetComponent<Player_Controller>().angleZ = angleZ;
            }
            break;
        }
    }

    internal static void ChangeScene(string sceneName, int nextdoorNumber)
    {
        doorNumber = nextdoorNumber;
        if (string.IsNullOrEmpty(sceneName) || sceneName == SceneManager.GetActiveScene().name)
            MovePlayer(); // Shortcut within scene
        else 
            SceneManager.LoadScene(sceneName); // change scene
    }

    private void OnDestroy()
    {
        string json = JsonUtility.ToJson(roomData);
    }
}
