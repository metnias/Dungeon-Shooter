using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Follower : MonoBehaviour
{
    public State idleState = State.Idle;

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

    private void Start()
    {
        state = idleState;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null) state = idleState;
        }
        if (state == idleState)
        {
            IdleUpdate();
            return;
        }
        
    }

    private void IdleUpdate()
    {
        if (player != null)
        {
            // check if this is looking at that dir
            // raytrace to player to see if this can see player
            Physics2D.Linecast(transform.position, player.transform.position);
        }

        if (state == State.Idle) return;

    }



    private void FixedUpdate()
    {
        
    }
}
