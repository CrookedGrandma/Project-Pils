using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest000 : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameManager.instance.questManager.CompleteObjective("Quest000GetUp");

            GameManager.instance.questManager.AddQuestToLog("Quest001");
        }
    }
}
