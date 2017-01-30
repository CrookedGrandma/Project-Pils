using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest006 : MonoBehaviour {

    public GameObject noni;
    bool HasBeenFlagged = false;


    void OnTriggerEnter()
    {
        if (!HasBeenFlagged)
        {
            if (GameManager.instance.questManager.questLog.ContainsKey("Quest006"))
            {
                noni.SetActive(true);
                HasBeenFlagged = true;
            }
        }

        if (HasBeenFlagged)
        {
            if (GameManager.instance.questManager.questLog.ContainsKey("Quest007"))
            {
                    noni.SetActive(false);
            }
        }
    }
}
