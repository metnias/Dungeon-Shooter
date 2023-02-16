using UnityEngine;

public class Enemy_Follower : MonoBehaviour
{
    public State idleState = State.Idle;
    public float viewRange = 40f;
    public float idleSpeed = 2f;
    public float detectRadius = 20f;
    public float followSpeed = 3f;
    public int health = 3;

    public enum State
    {
        Idle,
        TurnCW,
        TurnCCW,
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

            case State.TurnCCW:
            case State.TurnCW:
                actTimer -= Time.deltaTime;
                if (actTimer <= 0f)
                {
                    actTimer = 3f;
                    AngleZ += state == State.TurnCCW ? 90f : -90f;
                    if (AngleZ > 180f) AngleZ -= 360f;
                }
                else if (CanSeePlayer()) ChangeState(State.Follow);
                return;

            case State.Follow:
                if (!CanSeePlayer()) ChangeState(State.Lost);
                else AngleZ = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
                break;
            case State.Lost:
                if (CanSeePlayer()) ChangeState(State.Follow);
                else
                {
                    actTimer -= Time.deltaTime;
                    if (actTimer <= 0f) ChangeState(idleState);
                }
                break;

            case State.Hurt:
                actTimer -= Time.deltaTime;
                if (actTimer <= 0f) ChangeState(State.Follow);
                break;
            case State.Die: return;
        }
    }

    private float actTimer;


    private bool CanSeePlayer()
    {
        if (player == null) return false;
        if (state != State.Follow && Vector2.Distance(player.transform.position, transform.position) > detectRadius) return false; // outside radius
        float relAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
        relAngle *= Mathf.Rad2Deg;
        relAngle = AngleZ - relAngle;
        if (relAngle > 180f) relAngle -= 360f;
        else if (relAngle < -180f) relAngle += 360f; // wrap angle
        if (Mathf.Abs(relAngle) > viewRange) return false; // check if it's looking at player direction
        // raytrace to player to see if this can see player
        if (!Physics2D.Linecast(transform.position, player.transform.position, 1 << LayerMask.NameToLayer("Map")))
            return true;
        return false;
    }

    private void ChangeState(State newState)
    {
        if (state == State.Hurt) anim.StopHurtAnimation();
        // Debug.Log($"{gameObject.name} goes {state} -> {newState}");
        switch (newState)
        {
            case State.TurnCCW:
            case State.TurnCW: actTimer = 3f; AngleZ = 90f * Random.Range(0, 4) - 180f; break;
            case State.Idle: AngleZ = 90f * Random.Range(0, 4) - 180f; break;
            case State.PatrolVert: AngleZ = -90f; break;
            case State.PatrolHorz: AngleZ = 0f; break;
            case State.Lost: actTimer = 2f; break;
            case State.Hurt:
                actTimer = 0.5f; anim.HurtAnimation(); break;
            case State.Die:
                GetComponent<CircleCollider2D>().enabled = false;
                Destroy(gameObject, 1f);
                anim.HurtAnimation(true);
                break;
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

            case State.Hurt:
            case State.Die:
                rBody.velocity *= 0.9f; break;

            default:
                rBody.velocity = Vector2.zero; break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            if (state == State.Die) return;
            var push = collision.gameObject.GetComponent<Rigidbody2D>().velocity;
            rBody.AddForce(push, ForceMode2D.Impulse);
            AngleZ = Mathf.Atan2(-push.y, -push.x) * Mathf.Rad2Deg; // look where the arrow came from
            health--;
            if (health < 1) { health = 0; ChangeState(State.Die); }
            else ChangeState(State.Hurt);
            return;
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            if (state == State.Hurt || state == State.Die) return; // not attacking
            collision.gameObject.GetComponent<Player_Controller>().Damage(gameObject);
            return;
        }
        if (state == State.Follow) return; // don't look away when following
        AngleZ += 180f; // turn around when hitting wall while idle
        if (AngleZ > 180f) AngleZ -= 360f;
    }

}
