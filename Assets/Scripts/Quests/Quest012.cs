using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest012 : MonoBehaviour {

    void OnTriggerEnter()
    {
        if (GameManager.instance.questManager.questLog.ContainsKey("Quest012") && !GameManager.instance.questManager.questLog.ContainsKey("Quest013"))
        {
            GameManager.instance.questManager.CompleteObjective("Quest012FindServerRoom");
            GameManager.instance.questManager.AddQuestToLog("Quest013");
        }
    }
}
