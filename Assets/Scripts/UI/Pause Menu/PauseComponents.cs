using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseComponents : MonoBehaviour
{

    public PauseComponents Pause;
    public PauseMenuManager Manager;
    public int Count;

    void Awake()
    {
       


    }
    private void Update()
    {
        if (Pause == null)
            Pause = this;
        else if (Pause != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

}

