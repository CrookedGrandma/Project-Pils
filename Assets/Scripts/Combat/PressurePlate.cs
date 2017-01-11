using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PressurePlate : MonoBehaviour {

    public int FightAgainst = -1;
    public int FightInEnvironment = -1;

	void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Player") {
            if (FightAgainst != -1) {
                ThirdPersonController.player.Enemy = FightAgainst;
            }
            if (FightInEnvironment != -1) {
                ThirdPersonController.player.Envi = FightInEnvironment;
            }
            SceneManager.LoadScene("Combat");
        }
    }
}
