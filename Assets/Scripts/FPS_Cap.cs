using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Cap : MonoBehaviour
{
    public int maxFPS = 60;

    private static FPS_Cap instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        Application.targetFrameRate = maxFPS;
    }
}
