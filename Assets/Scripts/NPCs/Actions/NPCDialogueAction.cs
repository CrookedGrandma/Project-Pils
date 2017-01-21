using UnityEngine;
using System.Collections;
using Core.FSM;

public class NPCDialogueAction : Core.FSM.FSMAction {
    bool dialogueTriggered = false;
    Entity npcEntity;
    string identifier;

    public NPCDialogueAction(FSMState owner) : base(owner)
    {
    }

    public void Init(Entity npc, string dialogueIdentifier, string finishEvent = null)
    {
        npcEntity = npc;
        identifier = dialogueIdentifier;
    }


    // Update is called once per frame
    public override void OnUpdate () {

        if (!dialogueTriggered)
        {
            dialogueTriggered = true;

            Message m = new Message(npcEntity, GameManager.instance.dialogueManager, MsgType.DialogueResponse, new string[] { identifier });
            GameManager.instance.messageQueue.Add(m);
        }

        if (GameManager.instance.dialogueManager.DialogueHasEnded())
        {
            GetOwner().SendEvent("ToIdle");
        }

    }
}
