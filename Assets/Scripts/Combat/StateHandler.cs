using UnityEngine;
using System.Collections;

public class StateHandler : MonoBehaviour {

    private enum States { START, PLAYERCHOICE, PLAYERMOVE, ENEMYCHOICE, ENEMYMOVE }
    private States state;
    private bool SRanOnce = false;
    private bool PCRanOnce = false;
    private bool PMRanOnce = false;
    private bool ECRanOnce = false;
    private bool healing = false;
    private double flashTimer = 0.0;
    private int attNum = 0;
    private float timeS = -1;
    private float timePM = -1;
    private int damage;

    public EnemyChooser enemyChooser;
    public HealthManager healthManager;
    public EnterPlayer PlayerEnter;
    public AbilityChooser abilityChooser;

    private Enemy e;

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
                timeS = Time.time;
                SRanOnce = true;
            }
            if (Time.time - timeS >= 2.5f && timeS != -1) {
                NextState();
            }
        }

        if (state == States.PLAYERCHOICE) {
            SRanOnce = false;
            if (!PCRanOnce) {
                abilityChooser.GetStarted();
                PCRanOnce = true;
            }
            if (!healing) {
                if (Input.GetKeyDown(KeyCode.DownArrow) && abilityChooser.selectedAbility < 3) {
                    abilityChooser.SelectAbility(abilityChooser.selectedAbility + 1);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && abilityChooser.selectedAbility > 1) {
                    abilityChooser.SelectAbility(abilityChooser.selectedAbility - 1);
                }
            }
            else {
                if (Input.GetKeyDown(KeyCode.DownArrow) && abilityChooser.selectedHealer < 7) {
                    abilityChooser.SelectHealer(abilityChooser.selectedHealer + 1);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow) && abilityChooser.selectedHealer > 1) {
                    abilityChooser.SelectHealer(abilityChooser.selectedHealer - 1);
                }
            }
            if (Input.GetKeyDown(KeyCode.Return)) {
                abilityChooser.WhiteText();
                if (abilityChooser.selectedAbility != 3) {
                    NextState();
                }
                else {
                    healing = true;
                }
            }
        }

        if (state == States.PLAYERMOVE) {
            PCRanOnce = false; healing = false;
            if (!PMRanOnce) {
                timePM = Time.time;
                // If player has not healed
                if (abilityChooser.selectedAbility != 3) {
                    damage = abilityChooser.GetLastDoneDamage();
                    healthManager.EnemyLoseHealth(damage);
                }
                PMRanOnce = true;
            }
            if (Time.time - timePM >= 4f && timePM != -1) {
                NextState();
            }
        }

        if (state == States.ENEMYCHOICE) {
            PMRanOnce = false;
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