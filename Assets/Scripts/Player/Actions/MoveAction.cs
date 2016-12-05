using UnityEngine;
using System.Collections;
using Core.FSM;

public class MoveAction : Core.FSM.FSMAction
{
	private Transform transform;
    private float magnitude;
    private string finishEvent;

	public MoveAction (FSMState owner) : base (owner)
	{
	}

	public void Init (Transform transform, float mag, string finishEvent = null)
	{
		this.transform = transform;
        this.magnitude = mag;
        this.finishEvent = finishEvent;
	}

    public override void OnUpdate()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            float val = Input.GetAxis("Horizontal");
            transform.position += new Vector3(val, 0, 0) * magnitude;

        } else if(Input.GetAxis("Vertical") != 0)
        {
            float val = Input.GetAxis("Vertical");
            transform.position += new Vector3(0, 0, val) * magnitude;
        } else
        {
            Finish();
        }
	}

	private void Finish ()
	{
		if (!string.IsNullOrEmpty (finishEvent)) {
			GetOwner ().SendEvent (finishEvent);
		}
	}

}
