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

    private string finishEvent;
    public bool ableToJump, ableToMoveLeft, ableToMoveRight, ableToMoveForward, ableToMoveBackward;

    public MoveAction (FSMState owner) : base (owner)
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

    public override void OnUpdate()
    {
        PlayerRB.velocity = new Vector3(0, 0, 0);

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
         
        PlayerRB.velocity = new Vector3(moveLeftRight * 15, 0, moveForwardBackward * 15);

        // Jump
        if (Input.GetButtonDown("Jump") && ableToJump)
        {
            PlayerRB.AddForce(Vector3.up * jumpSpeed);
            ableToJump = false;
        }

        Finish();
    }

	private void Finish ()
	{
		if (!string.IsNullOrEmpty (finishEvent)) {
			GetOwner ().SendEvent (finishEvent);
		}
	}


}
