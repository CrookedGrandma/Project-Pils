using UnityEngine;
using System.Collections;
using Core.FSM;

public class NPCDialogueAction : Core.FSM.FSMAction {

    float timer = Time.time + Random.Range(5, 7);
    bool dialogueTriggered = false;
    Entity npcEntity;

    public NPCDialogueAction(FSMState owner) : base(owner)
    {
    }

    public void Init(Entity npc, string finishEvent = null)
    {
        DialogueOption startingDialogue = new DialogueOption("-1", "Hello?", "Hey, I'm a stalker and I just found you!", new string[] { "0" });

        GameManager.instance.dialogueManager.AddDialogueOption("-1", startingDialogue);
        npcEntity = npc;
    }


    // Update is called once per frame
    public override void OnUpdate () {

        if (!dialogueTriggered)
        {
            dialogueTriggered = true;

            Debug.Log("Dialogue triggered");
            Message m = new Message(npcEntity, GameManager.instance.dialogueManager, MsgType.DialogueResponse, new string[] { "-1" });
            GameManager.instance.messageQueue.Add(m);

        }

        if (Time.time >= timer)
        {
            GetOwner().SendEvent("ToIdle");
            timer = Time.time + Random.Range(3, 5);
        }
    }
}
