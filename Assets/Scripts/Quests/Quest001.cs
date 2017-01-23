using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest001 : MonoBehaviour {

	void OnTriggerEnter()
    {
        GameManager.instance.questManager.CompleteObjective("Quest001FindPhone");

        Message m = new Message(GameManager.instance.questManager, GameManager.instance.dialogueManager, MsgType.Dialogue, "You found your phone in the closet!");
        GameManager.instance.messageQueue.Add(m);


        GameManager.instance.questManager.AddQuestToLog("Quest002");
    }
}
