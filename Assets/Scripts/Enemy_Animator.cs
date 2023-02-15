using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Animator : MonoBehaviour
{
    internal float angleZ = -90f;

    private Animator anim;

    public string aniUp;
    public string aniDown;
    public string aniLeft;
    public string aniRight;
    public string aniHurt;
    public string aniDie;

    private string curAni, lastAni;
#nullable enable
    private string? forcedAni = null;
#nullable restore

    void Start()
    {
        anim = GetComponent<Animator>();
        lastAni = aniDown;
    }

    void Update()
    {
        curAni = angleZ switch
        {
            float a when a > -45f && a < 45f => aniRight,
            float a when a >= 45f && a <= 135f => aniUp,
            float a when a <= -45f && a >= -135f => aniDown,
            _ => aniLeft,
        };
        curAni = forcedAni ?? curAni;

        if (lastAni != curAni)
        {
            lastAni = curAni;
            anim.Play(curAni);
        }
    }

    internal void HurtAnimation(bool die = false)
    {
        forcedAni = die ? aniDie : aniHurt;
    }

    internal void StopHurtAnimation() => forcedAni = null;
}
