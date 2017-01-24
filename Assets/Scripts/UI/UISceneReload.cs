using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISceneReload : MonoBehaviour {

    private static UISceneReload instance = null;

	void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
