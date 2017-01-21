using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest003 : MonoBehaviour {

	void OnTriggerEnter()
    {
        GameManager.instance.questManager.CompleteObjective("Quest003EnterIanHouse");
    }
}
