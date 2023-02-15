using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    /// <summary>
    /// Move speed
    /// </summary>
    public float speed = 3.0f;

    private float axisH;
    private float axisV;
    internal float angleZ = -90f;

    private Rigidbody2D rBody;
    private bool isMoving = false;

    private const string ANI_UP = "PlayerUp";
    private const string ANI_LEFT = "PlayerLeft";
    private const string ANI_RIGHT = "PlayerRight";
    private const string ANI_DOWN = "PlayerDown";
    private const string ANI_HURT = "PlayerHurt";
    private const string ANI_DIE = "PlayerDie";

    private string curAnimation, lastAnimation;
    private Animator animator;
    private SpriteRenderer spr;
    private float hurtAnim = 0f;
    private bool invincible = false;
    public bool Alive => Player_Inventory.health > 0;

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        curAnimation = ANI_DOWN;
        invincible = true;
        Invoke(nameof(DisableInvincibility), 2f);
    }

    private void Update()
    {
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");
        isMoving = axisH != 0f || axisV != 0f;
        if (isMoving) angleZ = Mathf.Atan2(axisV, axisH) * Mathf.Rad2Deg;

        curAnimation = angleZ switch
        {
            float a when a > -45f && a < 45f => ANI_RIGHT,
            float a when a >= 45f && a <= 135f => ANI_UP,
            float a when a <= -45f && a >= -135f => ANI_DOWN,
            _ => ANI_LEFT,
        };
        spr.enabled = true;
        if (!Alive) //dead
        {
            curAnimation = ANI_DIE;
        }
        else if (hurtAnim > 0f) // hurt
        {
            hurtAnim -= Time.deltaTime;
            if (hurtAnim < 0f) hurtAnim = 0f;
            curAnimation = ANI_HURT;
        }
        else if (invincible) // iframe flicker
            spr.enabled = Mathf.Sin(Time.timeSinceLevelLoad * 50f) > 0f;

        if (lastAnimation != curAnimation)
        {
            lastAnimation = curAnimation;
            animator.Play(curAnimation);
        }
    }

    private void FixedUpdate()
    {
        if (!Alive) return;
        if (hurtAnim > 0f) { rBody.velocity *= 0.9f; return; }
        var dir = new Vector2(axisH, axisV);
        dir = Vector2.ClampMagnitude(dir, 1f); // clamp diagonal speed
        rBody.velocity = speed * dir;
    }

    public void Damage(GameObject hazard)
    {
        if (invincible) return; // invulnability time
        invincible = true;
        Invoke(nameof(DisableInvincibility), 2f);
        Player_Inventory.health--;
        if (Player_Inventory.health < 1) { Die(); return; }

        var push = transform.position - hazard.transform.position;
        push = push.normalized * 300f;
        GetComponent<Rigidbody2D>().AddForce(push, ForceMode2D.Impulse);
        hurtAnim = 1f;
    }

    public void Die()
    {
        Player_Inventory.health = 0;
        GetComponent<CircleCollider2D>().enabled = false;
        rBody.velocity = Vector2.zero;
        rBody.AddForce(Vector2.up * 100f, ForceMode2D.Impulse);
        rBody.gravityScale = 2.5f;
    }

    private void DisableInvincibility() => invincible = false;

}
