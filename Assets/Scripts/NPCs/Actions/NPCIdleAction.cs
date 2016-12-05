using UnityEngine;
using System.Collections;
using Core.FSM;

public class NPCIdleAction : Core.FSM.FSMAction
{

    float timer = Time.time + Random.Range(1, 2);

    public NPCIdleAction(FSMState owner) : base(owner)
    {
    }

    public void Init(string finishEvent = null)
    {
    }

    public override void OnUpdate()
    {

        if (Time.time >= timer)
        {
            GetOwner().SendEvent("ToNextWaypoint");
            timer = Time.time + Random.Range(3, 5);
        }
    }

}
