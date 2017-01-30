using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest008 : MonoBehaviour {

    public GameObject noni;
    public GameObject meemstar;

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            GameManager.instance.questManager.CompleteObjective("Quest007GoToPiPiHQ");

            if (!GameManager.instance.questManager.questLog.ContainsKey("Quest008"))
            {
                GameManager.instance.questManager.AddQuestToLog("Quest008");
            }
        }

        if (GameManager.instance.questManager.questLog.ContainsKey("Quest009"))
        {
            noni.SetActive(false);

            if (GameManager.instance.questManager.questLog["Quest009"].QuestCompleted())
            {
                meemstar.SetActive(false);
            } else
            {
                meemstar.SetActive(true);
            }
        }
    }
}
