using UnityEngine;
using System.Collections;
using Core.FSM;

public class NPCIdleScoutAction : Core.FSM.FSMAction
{
    public int scoutingRange;
    public Transform transform;
    float timer = Time.time + Random.Range(1, 2);

    public NPCIdleScoutAction(FSMState owner) : base(owner)
    {
    }

    public void Init(int scoutRange, Transform t, string finishEvent = null)
    {
        scoutingRange = scoutRange;
        transform = t;
    }

    public override void OnUpdate()
    {
        Vector3 playerPos = GameManager.instance.player.gameObject.transform.position;
        
        if(Vector3.Distance(transform.position, playerPos) <= scoutingRange)
        {
            GetOwner().SendEvent("ToDialogue");
        }

        if (Time.time >= timer)
        {
            GetOwner().SendEvent("ToNextWaypoint");
            timer = Time.time + Random.Range(3, 5);
        }
    }

}
