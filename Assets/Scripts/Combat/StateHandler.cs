using UnityEngine;
using System.Collections;

public class StateHandler : MonoBehaviour {

    enum States { START, PLAYERCHOICE, PLAYERMOVE, ENEMYCHOICE, ENEMYMOVE }
    States state;
    bool SRanOnce = false;
    bool ECRanOnce = false;
    double flashTimer = 0.0;
    int attNum = 0;
    float time = -1;

    public EnemyChooser enemyChooser;
    public HealthManager healthManager;
    public EnterPlayer PlayerEnter;

    Enemy e;

	// Use this for initialization
	void Start () {
        state = States.START;
	}
	
	// Update is called once per frame
	void Update () {
        print("Current battle state: " + state);

        if (Input.GetKeyDown(KeyCode.N)) {
            NextState();
        }

        if (state == States.START) {
            PlayerEnter.Enter = true;
            if (!SRanOnce) {
                time = Time.time;
                SRanOnce = true;
            }
            if (Time.time - time >= 2.5f && time != -1) {
                NextState();
            }
            
        }

        if (state == States.PLAYERCHOICE) {
            SRanOnce = false;
        }

        if (state == States.PLAYERMOVE) {

        }

        if (state == States.ENEMYCHOICE) {
            flashTimer = Mathf.PingPong(Time.time * 2, 1);

            if (ECRanOnce) {
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
                    TextWhite();
                }
            }
        }

        if (state == States.ENEMYMOVE) {
            if (ECRanOnce) {
                TextWhite();
            }
            ECRanOnce = false;
        }
    }

    void LateUpdate() {
        if (state == States.ENEMYCHOICE && !ECRanOnce) {
            e = enemyChooser.currentEnemy.linkedEnemy;
            ECRanOnce = true;
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

    void TextWhite() {
        enemyChooser.enemyStat.text = "HP: " + e.HP + "\n" +
                                      "Strength: " + e.Strength + "\n" +
                                      "Attack 1: " + e.Attack1Title + "\n" +
                                      "Damage: " + e.Attack1Damage + "\n" +
                                      "Attack 2: " + e.Attack2Title + "\n" +
                                      "Damage: " + e.Attack2Damage;
    }

    void NextState() {
        state = (States)((int)state + 1);
        if ((int)state > 4) {
            state = States.PLAYERCHOICE;
        }
    }
}