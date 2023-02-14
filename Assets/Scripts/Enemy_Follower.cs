using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Follower : MonoBehaviour
{
    public State idleState = State.Idle;
    public float viewRange = 40f;
    public float idleSpeed = 2f;
    public float followSpeed = 3f;

    public enum State
    {
        Idle,
        // Wander,
        PatrolVert,
        PatrolHorz,

        Follow,
        Lost,
        Hurt,
        Die
    }

    private State state;
    private GameObject player;

    private Enemy_Animator anim;
    private float AngleZ { get => anim.angleZ; set => anim.angleZ = value; }
    private Rigidbody2D rBody;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Enemy_Animator>();
        rBody = GetComponent<Rigidbody2D>();
        ChangeState(idleState);
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) state = idleState;
        }

        switch (state)
        {
            case State.Idle:
            case State.PatrolHorz:
            case State.PatrolVert:
                if (CanSeePlayer()) ChangeState(State.Follow);
                return;

            case State.Follow:
                if (!CanSeePlayer()) ChangeState(State.Lost);
                else AngleZ = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                break;
            case State.Lost:
                if (CanSeePlayer()) ChangeState(State.Follow);
                else
                {
                    lostTimer -= Time.deltaTime;
                    if (lostTimer <= 0f) ChangeState(idleState);
                }
                break;
                
            case State.Hurt:
            case State.Die:
                return;
        }
    }

    private float lostTimer;


    private bool CanSeePlayer()
    {
        if (player == null) return false;
        if (state == idleState) // when idling, check if this is looking at that dir
        {
            float relAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
            relAngle *= Mathf.Rad2Deg;
            // todo: solve wrapping issue when angle is going left
            if (Mathf.Abs(AngleZ - relAngle) > viewRange)
                return false;
        }
        // raytrace to player to see if this can see player
        if (!Physics2D.Linecast(transform.position, player.transform.position, LayerMask.NameToLayer("Map")))
            return true;
        return false;
    }

    private void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Idle: AngleZ = 90f * Random.Range(0, 4) - 180f; break;
            case State.PatrolVert: AngleZ = -90f; break;
            case State.PatrolHorz: AngleZ = 0f; break;
            case State.Lost: lostTimer = 2f; break;
        }

        state = newState;
    }

    private void FixedUpdate()
    {
        var v = new Vector2(Mathf.Cos(AngleZ * Mathf.Deg2Rad), Mathf.Sin(AngleZ * Mathf.Deg2Rad));
        switch (state)
        {
            case State.PatrolVert:
            case State.PatrolHorz:
                rBody.velocity = v * idleSpeed;
                break;
            case State.Follow:
                rBody.velocity = v * followSpeed;
                break;

            default:
                rBody.velocity = Vector2.zero; break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        if (state == State.Follow) return; // don't look away when following
        AngleZ += 180f;
        if (AngleZ > 180f) AngleZ -= 360f;
    }

}
