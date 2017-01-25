using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest004 : MonoBehaviour {

    void OnTriggerEnter()
    {
        if (!GameManager.instance.questManager.questLog.ContainsKey("Quest005"))
        {
            GameManager.instance.questManager.CompleteObjective("Quest004GoToPub");
            GameManager.instance.questManager.AddQuestToLog("Quest005");
        }
    }

}
