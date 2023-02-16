using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    private static int numKey = 0;
    private static int numArrow = 30;
    private static int health = 3;
    public static int maxHealth = 3;

    public static int NumKey
    {
        get => numKey;
        set
        {
            if (numKey == value) return;
            numKey = value; Save();
        }
    }
    public static int NumArrow
    {
        get => numArrow;
        set
        {
            if (numArrow == value) return;
            numArrow = value; Save();
        }
    }
    public static int Health
    {
        get => health;
        set
        {
            value = Mathf.Clamp(value, 0, maxHealth);
            if (health == value) return;
            health = value; Save();
        }
    }

    private void Start()
    {
        Load();
        if (health == 0) Reset();
    }

    public static void Reset()
    {
        numKey = 0;
        numArrow = 30;
        health = maxHealth;
        Save();
    }

    public static void Load()
    {
        numKey = PlayerPrefs.GetInt(nameof(NumKey)); //0;
        numArrow = PlayerPrefs.GetInt(nameof(NumArrow)); // 30;
        health = PlayerPrefs.GetInt(nameof(Health));
    }

    public static void Save()
    {
        PlayerPrefs.SetInt(nameof(NumKey), NumKey);
        PlayerPrefs.SetInt(nameof(NumArrow), NumArrow);
        PlayerPrefs.SetInt(nameof(Health), Health);
    }


}
