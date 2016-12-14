using UnityEngine;
using System.Collections;
using Core.FSM;
using System.Collections.Generic;

public class PatrolNPCFSM : MonoBehaviour {

    public float movementSpeed = 0.6f;
    public List<Vector3> waypoints = new List<Vector3>();

    private FSM fsm;
    private FSMState moveState;
    private FSMState idleState;
    private WaypointMoveAction moveAction;
    private NPCIdleAction idleAction;

    // Use this for initialization
    void Start()
    {
        fsm = new Core.FSM.FSM("PatrolNPCFSM");
        moveState = fsm.AddState("MoveState");
        idleState = fsm.AddState("IdleState");
        moveAction = new WaypointMoveAction(moveState);
        idleAction = new NPCIdleAction(idleState);

        moveState.AddAction(moveAction);
        idleState.AddAction(idleAction);

        moveAction.Init(GetComponent<Rigidbody>(), gameObject.transform, movementSpeed,waypoints, "ToIdle");
        idleAction.Init();

        idleState.AddTransition("ToNextWaypoint", moveState);
        moveState.AddTransition("ToIdle", idleState);

        fsm.Start("IdleState");
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.IsPaused)
            fsm.Update();
    }

}
