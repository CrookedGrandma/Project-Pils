using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {

    public static ThirdPersonController player;
    int enemyID = -1;
    int envID = -1;
    bool pleaseDie = false;

    public int Enemy {
        get { return enemyID; }
        set { enemyID = value; }
    }

    public int Envi {
        get { return envID; }
        set { envID = value; }
    }

    public bool PleaseDie {
        get { return pleaseDie; }
        set { pleaseDie = value; }
    }

    // Use this for initialization
    void Awake () {
        if (player != null) {
            Destroy(gameObject);
        }else {
            player = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (enemyID != -1) {
            Debug.Log("Enemy Plate: " + enemyID);
        }
        if (envID != -1) {
            Debug.Log("Environment Plate: " + envID);
        }
        if (pleaseDie) {
            Destroy(gameObject);
        }
	}
}
