using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest000 : MonoBehaviour {

    void OnTriggerEnter()
    {
        GameManager.instance.questManager.CompleteObjective("Quest000GetUp");

        GameManager.instance.questManager.AddQuestToLog("Quest001");
    }

}
