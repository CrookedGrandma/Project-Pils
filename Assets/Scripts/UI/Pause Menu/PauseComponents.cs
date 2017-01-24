using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseComponents : MonoBehaviour
{
    public static PauseComponents Pause;
    public PauseMenuManager Manager;
    public int Count;

    void Awake()
    {
        if (Pause != null)
        {
            Debug.Log("destroying");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("not destroying");
            Pause = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}

