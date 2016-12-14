using UnityEngine;
using System.Collections;
using Core.FSM;
using System.Collections.Generic;

public class PatrolDialogueNPCFSM : Entity {

    public float movementSpeed = 0.6f;
    public List<Vector3> waypoints = new List<Vector3>();

    private FSM fsm;
    private FSMState moveState;
    private FSMState idleState;
    private FSMState dialogueState;
    private WaypointMoveAction moveAction;
    private NPCIdleScoutAction idleAction;
    private NPCDialogueAction dialogueAction;


    // Use this for initialization
    void Start () {
        fsm = new Core.FSM.FSM("PatrolDialogueNPCFSM");
        moveState = fsm.AddState("MoveState");
        idleState = fsm.AddState("IdleState");
        dialogueState = fsm.AddState("DialogueState");

        moveAction = new WaypointMoveAction(moveState);
        idleAction = new NPCIdleScoutAction(idleState);
        dialogueAction = new NPCDialogueAction(dialogueState);


        moveState.AddAction(moveAction);
        idleState.AddAction(idleAction);
        dialogueState.AddAction(dialogueAction);

        moveAction.Init(GetComponent<Rigidbody>(), gameObject.transform, movementSpeed, waypoints, "ToIdle");
        idleAction.Init(5, gameObject.transform);
        dialogueAction.Init(this);

        idleState.AddTransition("ToNextWaypoint", moveState);
        idleState.AddTransition("ToDialogue", dialogueState);
        moveState.AddTransition("ToIdle", idleState);
        dialogueState.AddTransition("ToIdle", idleState);

        fsm.Start("MoveState");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.IsPaused)
            fsm.Update();
    }
}
