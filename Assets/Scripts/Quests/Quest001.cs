using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest001 : MonoBehaviour {

	void OnTriggerEnter()
    {
        GameManager.instance.questManager.CompleteObjective("Quest001FindPhone");
        GameManager.instance.questManager.AddQuestToLog("Quest002");
    }
}
