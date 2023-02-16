using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData_Manager : MonoBehaviour
{
    public static SaveDataList arrangedDataList;


    void Start()
    {
        arrangedDataList = new SaveDataList();
        arrangedDataList.saveDatas = new SaveData[0];

        int numArrow = PlayerPrefs.GetInt(nameof(Player_Inventory.NumArrow));

        arrangedDataList = JsonUtility.FromJson<SaveDataList>("");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
