using UnityEngine;
using System.Collections;
using Core.FSM;

public class PlayerFSM : Entity {

    public Rigidbody PlayerRB;
    public float velocity = 20f;
    public float sprintVelocity = 30f;
    public float jumpSpeed = 3000f;

    private FSM fsm;
    private FSMState moveState;
    private FSMState idleState;
    private MoveAction moveAction;
    private IdleAction idleAction;

	// Use this for initialization
	void Start () {
        fsm = new Core.FSM.FSM("PlayerFSM");
        moveState = fsm.AddState("MoveState");
        idleState = fsm.AddState("IdleState");
        moveAction = new MoveAction(moveState);
        idleAction = new IdleAction(idleState);

        moveState.AddAction(moveAction);
        idleState.AddAction(idleAction);

        PlayerRB = gameObject.GetComponent<Rigidbody>();

        moveAction.Init(gameObject.transform, PlayerRB, velocity, sprintVelocity, jumpSpeed, "ToIdle");
        idleAction.Init();

        idleState.AddTransition("ToMove", moveState);
        moveState.AddTransition("ToIdle", idleState);

        fsm.Start("IdleState");
	}
	
	// Update is called once per frame
	void Update () {
        if(!GameManager.instance.IsPaused)
            fsm.Update();
	}

    public override void onMessage(Message m)
    {
        base.onMessage(m);

        switch (m.type)
        {
            case MsgType.Dialogue:
                GameManager.instance.dialogueManager.AddLine(m.from.name, m.data.ToString(), "red");
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // Determine if we can jump
        var normal = collision.contacts[0].normal;
        if (normal.y > 0)
        {
            //Hit Bottom
            moveAction.ableToJump = true;
            moveAction.ableToMoveBackward = moveAction.ableToMoveForward = moveAction.ableToMoveLeft = moveAction.ableToMoveRight = true;
        }
        if (collision.collider.tag != "Terrain")
        {
            /* 
             * Terrain does not have mass, so there was a NullReferenceException. 
             * The collision with the terrain is already handled just before this.
             */
           if (collision.rigidbody)
            {
                if (PlayerRB.mass <= collision.rigidbody.mass)
                {
                    if (normal.y < 0)
                    {
                        //Hit Roof  
                        // Maybe needed in houses or dungeons
                    }
                    else if (normal.x > 0)
                    {
                        //Hit Left
                        moveAction.ableToMoveLeft = false;
                    }
                    else if (normal.x < 0)
                    {
                        //Hit Right
                        moveAction.ableToMoveRight = false;
                    }
                    else if (normal.z < 0)
                    {
                        //Hit Front
                        moveAction.ableToMoveForward = false;
                    }
                    else if (normal.z > 0)
                    {
                        //Hit Back
                        moveAction.ableToMoveBackward = false;
                    }
                }
            }

        }
    }

    //Add Collision parameter if needed, remove if not to prevent any unnecessary calculations
    private void OnCollisionExit()
    {
        moveAction.ableToMoveBackward = moveAction.ableToMoveForward = moveAction.ableToMoveLeft = moveAction.ableToMoveRight = true;
        moveAction.ableToJump = false;
    }
    

}
