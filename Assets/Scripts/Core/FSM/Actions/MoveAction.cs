using UnityEngine;
using System.Collections;
using Common.FSM;

public class MoveAction : FSMAction
{
	private Transform transform;
	private Vector3 positionFrom;
	private Vector3 positionTo;
	private float duration;
	private float cachedDuration;
	private string finishEvent;
	private float journeyLength;
	private float polledTime;


	public MoveAction (FSMState owner) : base (owner)
	{
	}

	public void Init (Transform transform, Vector3 from, Vector3 to, float duration, string finishEvent = null)
	{
		this.transform = transform;
		this.positionFrom = from;
		this.positionTo = to;
		this.duration = duration;
		this.cachedDuration = duration;
		this.finishEvent = finishEvent;
		this.journeyLength = Vector3.Distance (this.positionFrom, this.positionTo);
		this.polledTime = 0;
	}

	public override void OnEnter ()
	{

		if (duration <= 0) {
			Finish ();
			return;
		}

		SetPosition (this.positionFrom);
	}

	public override void OnUpdate ()
	{
		polledTime += Time.deltaTime;
		duration -= Time.deltaTime;

		if (duration <= 0) {
			Finish ();
			return;
		}

		SetPosition (Vector3.Lerp (this.positionFrom, this.positionTo, Mathf.Clamp (polledTime / cachedDuration, 0, 1)));
	}

	private void Finish ()
	{
		if (!string.IsNullOrEmpty (finishEvent)) {
			GetOwner ().SendEvent (finishEvent);
		}

		SetPosition (this.positionTo);
		this.polledTime = 0;
		duration = cachedDuration;
		this.journeyLength = Vector3.Distance (this.positionFrom, this.positionTo);
	}

	private void SetPosition (Vector3 position)
	{
		this.transform.position = position;
	}
}
