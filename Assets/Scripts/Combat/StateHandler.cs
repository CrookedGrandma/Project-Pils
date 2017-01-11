using UnityEngine;
using System.Collections;

public class StateHandler : MonoBehaviour {

    enum States { START, PLAYERCHOICE, PLAYERMOVE, ENEMYCHOICE, ENEMYMOVE }
    States state;
    bool ranOnce = false;
    double flashTimer = 0.0;
    int attNum = 0;

    public EnemyChooser enemyChooser;

    Enemy e;

	// Use this for initialization
	void Start () {
        state = States.START;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.N)) {
            state = (States)((int)state + 1);
            if ((int)state > 4) {
                state = States.START;
            }
        }

        print("Current battle state: " + state);
        flashTimer = Mathf.PingPong(Time.time * 2, 1);

        if (ranOnce) {
            if (flashTimer >= 0.9) {
                if (attNum == 1) {
                    enemyChooser.enemyStat.text = "HP: " + e.HP + "\n" +
                                                  "Strength: " + e.Strength + "\n" +
                                                  "<color=red>Attack 1: " + e.Attack1Title + "\n" +
                                                  "Damage: " + e.Attack1Damage + "</color>\n" +
                                                  "Attack 2: " + e.Attack2Title + "\n" +
                                                  "Damage: " + e.Attack2Damage;
                }
                if (attNum == 2) {
                    enemyChooser.enemyStat.text = "HP: " + e.HP + "\n" +
                                                  "Strength: " + e.Strength + "\n" +
                                                  "Attack 1: " + e.Attack1Title + "\n" +
                                                  "Damage: " + e.Attack1Damage + "\n" +
                                                  "<color=red>Attack 2: " + e.Attack2Title + "\n" +
                                                  "Damage: " + e.Attack2Damage + "</color>";
                }
            }
            if (flashTimer <= 0.1) {
                enemyChooser.enemyStat.text = "HP: " + e.HP + "\n" +
                                              "Strength: " + e.Strength + "\n" +
                                              "Attack 1: " + e.Attack1Title + "\n" +
                                              "Damage: " + e.Attack1Damage + "\n" +
                                              "Attack 2: " + e.Attack2Title + "\n" +
                                              "Damage: " + e.Attack2Damage;
            }
        }
    }

    void LateUpdate() {
        if (!ranOnce) {
            e = enemyChooser.currentEnemy.linkedEnemy;
            ranOnce = true;
            attNum = GetAttack();
        }
    }

    int GetAttack() {
        int attackNum = 0;
        int a1d = e.Attack1Damage;
        int a2d = e.Attack2Damage;
        int a3d = e.Attack3Damage;
        int totalDam = a1d + a2d + a3d;

        int rndAtt = Random.Range(0, totalDam);
        if (rndAtt <= a1d) { attackNum = 1; }
        else if (rndAtt <= a1d + a2d) { attackNum = 2; }
        else { attackNum = 3; }

        return attackNum;
    }
}
