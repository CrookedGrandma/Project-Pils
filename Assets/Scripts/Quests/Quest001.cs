using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest001 : MonoBehaviour {

	void OnTriggerEnter()
    {
        if (!GameManager.instance.questManager.questLog["Quest001"].QuestCompleted())
        {
            GameManager.instance.questManager.CompleteObjective("Quest001FindPhone");
            GameManager.instance.questManager.AddQuestToLog("Quest002");
        }
    }
}
