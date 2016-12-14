using UnityEngine;
using System.Collections;
using Core.FSM;
using System.Collections.Generic;

public class WaypointMoveAction : Core.FSM.FSMAction
{
    private Transform transform;
    private float magnitude;
    private string finishEvent;
    private List<Vector3> waypoints;
    private List<Vector3> waypointsCopy;

    public WaypointMoveAction(FSMState owner) : base(owner)
    {
    }

    public void Init(Transform transform, float mag, List<Vector3> waypoints, string finishEvent = null)
    {
        this.transform = transform;
        this.magnitude = mag;
        this.finishEvent = finishEvent;
        this.waypoints = waypoints;
        this.waypointsCopy = waypoints;
    }

    public override void OnUpdate()
    {
        if(waypoints.Count != 0)
        {
            Vector3 target = waypoints[0];
            transform.position += (target - transform.position).normalized  * magnitude * Time.deltaTime;
            Vector3 movementVector = (target - transform.position).normalized;
            movementVector.Scale(new Vector3(1, 0, 1));

            transform.position += movementVector * magnitude * Time.deltaTime;

            if(transform.position.x == target.x && transform.position.z == target.z)
            {
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
