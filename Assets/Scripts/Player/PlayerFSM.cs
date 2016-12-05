using UnityEngine;
using System.Collections;
using Core.FSM;

public class PlayerFSM : MonoBehaviour {


    public float movementSpeed = 0.6f;

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

        moveAction.Init(gameObject.transform, movementSpeed, "ToIdle");
        idleAction.Init();

        idleState.AddTransition("ToMove", moveState);
        moveState.AddTransition("ToIdle", idleState);

        fsm.Start("IdleState");
	}
	
	// Update is called once per frame
	void Update () {
        fsm.Update();
	}
}
