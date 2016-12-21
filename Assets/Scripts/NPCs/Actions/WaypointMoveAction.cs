using UnityEngine;
using System.Collections;
using Core.FSM;
using System.Collections.Generic;

public class WaypointMoveAction : Core.FSM.FSMAction
{
    private Rigidbody rigidbody;
    private Transform transform;
    private float magnitude;
    private string finishEvent;
    private List<Vector3> waypoints;

    public WaypointMoveAction(FSMState owner) : base(owner)
    {
    }

    public void Init(Rigidbody rb, Transform transform, float mag, List<Vector3> wps, string finishEvent = null)
    {
        this.transform = transform;
        this.magnitude = mag;
        this.waypoints = wps;
        this.rigidbody = rb;
        this.finishEvent = finishEvent;
    }

    public override void OnUpdate()
    {
        if(waypoints.Count != 0)
        {
            Vector3 target = waypoints[0];
            Vector3 movementVector = (target - transform.position).normalized;

            //rigidbody.velocity = new Vector3(movementVector.x * magnitude, 0, movementVector.z * magnitude);
            transform.position += new Vector3(movementVector.x * magnitude, 0, movementVector.z * magnitude); //For use with Kinematic bodies

            if(Mathf.Abs(transform.position.x - target.x) <= 0.1 && Mathf.Abs(transform.position.z - target.z) <= 0.1)
            {
                rigidbody.velocity = Vector3.zero;
                waypoints.Remove(target);
                waypoints.Add(target);
                GetOwner().SendEvent("ToIdle");
            }
        } else
        {
            GetOwner().SendEvent("ToIdle");
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
