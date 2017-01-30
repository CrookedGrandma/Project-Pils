using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest013 : MonoBehaviour {

    public GameObject server;

    void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.questManager.questLog.ContainsKey("Quest014"))
        {
            server.SetActive(false);
        }
    }
}
