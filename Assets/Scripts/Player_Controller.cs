using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    /// <summary>
    /// Move speed
    /// </summary>
    public float speed = 3.0f;

    private float axisH;
    private float axisV;
    public float angleZ = -90f;

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
    private float hurtAnim = 0f;


    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        rBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator = GetComponent<Animator>();
        curAnimation = ANI_UP;
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
        // if die: show die
        if (hurtAnim > 0f)
        {
            hurtAnim -= Time.deltaTime;
            if (hurtAnim < 0f) hurtAnim = 0f;
            curAnimation = ANI_HURT;
        }

        if (lastAnimation != curAnimation)
        {
            lastAnimation = curAnimation;
            animator.Play(curAnimation);
        }
    }

    private void FixedUpdate()
    {
        var dir = new Vector2(axisH, axisV);
        dir = Vector2.ClampMagnitude(dir, 1f); // clamp diagonal speed
        rBody.velocity = speed * dir;
    }
}
