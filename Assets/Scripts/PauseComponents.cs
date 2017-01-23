using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseComponents : MonoBehaviour {

    public PauseComponents Pause;

    void Awake()
    {
        DontDestroyOnLoad(Pause);
    }
}
