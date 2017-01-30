using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest010 : MonoBehaviour {

	void OnTriggerEnter()
    {
        if (GameManager.instance.questManager.questLog.ContainsKey("Quest010"))
        {
            if (!GameManager.instance.questManager.questLog["Quest010"].QuestCompleted())
            {
                GameManager.instance.questManager.CompleteObjective("Quest010FindFacebeer");
                GameManager.instance.questManager.AddQuestToLog("Quest011");
            }
        }
    }
}
