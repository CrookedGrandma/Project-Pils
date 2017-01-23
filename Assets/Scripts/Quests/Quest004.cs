using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest004 : MonoBehaviour {

    void OnTriggerEnter()
    {
        GameManager.instance.questManager.CompleteObjective("Quest004GoToPub");
        GameManager.instance.questManager.AddQuestToLog("Quest005");
    }

}
