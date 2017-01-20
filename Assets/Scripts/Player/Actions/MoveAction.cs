using UnityEngine;
using System.Collections;
using Core.FSM;

public class MoveAction : Core.FSM.FSMAction
{
    private Transform transform;
    private Rigidbody PlayerRB;

    public float velocity = 20f;
    public float sprintVelocity = 30f;
    public float jumpSpeed = 3000f;
    private float usedVelocity;
    private float idleTime;
    private float totalIdleTime = 0.25f;

    private string finishEvent;
    public bool ableToJump, ableToMoveLeft, ableToMoveRight, ableToMoveForward, ableToMoveBackward;

    public MoveAction(FSMState owner) : base(owner)
    {
    }

    public void Init(Transform transform, Rigidbody rb, float vel, float sprintVel, float jumpSpeed, string finishEvent = null)
    {
        this.transform = transform;
        this.PlayerRB = rb;
        this.velocity = vel;
        this.sprintVelocity = sprintVel;
        this.jumpSpeed = jumpSpeed;
        this.finishEvent = finishEvent;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        idleTime = totalIdleTime;
    }

    public override void OnUpdate()
    {
        // Determine wether we are walking or sprinting
        if (Input.GetAxis("Sprint") <= 0)
        {
            // Walking
            usedVelocity = velocity;
        }
        else
        {
            // Sprinting
            usedVelocity = sprintVelocity;
        }

        // Walking or Sprinting
        float moveLeftRight = Input.GetAxis("Horizontal") * usedVelocity * Time.deltaTime;
        float moveForwardBackward = Input.GetAxis("Vertical") * usedVelocity * Time.deltaTime;

        // Prevent sticking on wall after jumping
        if (!ableToMoveLeft && moveLeftRight < 0)
        {
            moveLeftRight = 0;
        }
        if (!ableToMoveRight && moveLeftRight > 0)
        {
            moveLeftRight = 0;
        }
        if (!ableToMoveForward && moveForwardBackward > 0)
        {
            moveForwardBackward = 0;
        }
        if (!ableToMoveBackward && moveForwardBackward < 0)
        {
            moveForwardBackward = 0;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && ableToJump)
        {
            PlayerRB.velocity += Vector3.up * jumpSpeed;
            ableToJump = false;
        }

        PlayerRB.velocity = new Vector3(moveLeftRight, PlayerRB.velocity.y, moveForwardBackward);

        Debug.Log("X: " + PlayerRB.velocity.x + ", Y: " + PlayerRB.velocity.y + ", Z: " + PlayerRB.velocity.z);

        if (PlayerRB.velocity == Vector3.zero)
        {
            idleTime -= Time.deltaTime;
        }
        else
        {
            idleTime = totalIdleTime;
        }
        if (idleTime <= 0)
        {
            Finish();
        }
    }

    private void Finish()
    {
        if (!string.IsNullOrEmpty(finishEvent))
        {
            GetOwner().SendEvent(finishEvent);
        }
    }
}
