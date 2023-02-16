using UnityEngine;

public enum BGMType
{
    None = -1,
    Title,
    InGame,
    Boss
}

public enum SEType
{
    GameClear,
    GameOver,
    Shoot,
    Button,
    DoorOpen,
    DoorClose,
    Damage,
    PickUp,
    Killed,
    BossKill
}

public class Sound_Manager : MonoBehaviour
{
    public AudioClip bgmTitle;
    public AudioClip bgmInGame;
    public AudioClip bgmBoss;

    public AudioClip seGameClear;
    public AudioClip seGameOver;
    public AudioClip seShoot;
    public AudioClip seButton;
    public AudioClip seDoorOpen;
    public AudioClip seDoorClose;
    public AudioClip seDamage;
    public AudioClip sePickUp;
    public AudioClip seKilled;
    public AudioClip seBossKill;

    private static Sound_Manager instance;
    public static Sound_Manager Instance() => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
            Destroy(gameObject);
    }

    private BGMType curBGM = BGMType.None;

    public void PlayBGM(BGMType type)
    {
        if (type == curBGM) return;
        if (type == BGMType.None) { StopBGM(); return; }
        curBGM = type;
        var audio = GetComponent<AudioSource>();
        AudioClip clip = type switch
        {
            BGMType.Title => bgmTitle,
            BGMType.InGame => bgmInGame,
            BGMType.Boss => bgmBoss,
            _ => null,
        };
        audio.clip = clip;
        audio.Play();
    }

    public void StopBGM()
    {
        curBGM = BGMType.None;
        GetComponent<AudioSource>().Stop();
    }

    public void PlaySE(SEType type)
    {
        var audio = GetComponent<AudioSource>();
        AudioClip clip = type switch
        {
            SEType.GameClear => seGameClear,
            SEType.GameOver => seGameOver,
            SEType.Shoot => seShoot,
            SEType.Button => seButton,
            SEType.DoorOpen => seDoorOpen,
            SEType.DoorClose => seDoorClose,
            SEType.Damage => seDamage,
            SEType.PickUp => sePickUp,
            SEType.Killed => seKilled,
            SEType.BossKill => seBossKill,
            _ => null,
        };
        audio.PlayOneShot(clip);
    }
}
