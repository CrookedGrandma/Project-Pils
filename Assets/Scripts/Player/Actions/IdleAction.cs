using UnityEngine;
using System.Collections;
using Core.FSM;

public class IdleAction : Core.FSM.FSMAction
{
    public IdleAction(FSMState owner) : base(owner)
    {
    }

    public void Init(string finishEvent = null)
    {
    }

    public override void OnUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Jump") != 0)
        {
            GetOwner().SendEvent("ToMove");
        }
    }
}
