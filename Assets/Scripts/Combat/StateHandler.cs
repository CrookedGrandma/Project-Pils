using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StateHandler : MonoBehaviour {

    private enum States { START, PLAYERCHOICE, PLAYERMOVE, ENEMYCHOICE, ENEMYMOVE }
    private States state;
    private bool SRanOnce = false;
    private bool PCRanOnce = false;
    private bool PMRanOnce = false;
    private bool ECRanOnce = false;
    private bool EMRanOnce = false;
    private bool healing = false;
    private bool showEnterKey = false;
    private double flashTimer = 1.0;
    private int attNum = 0;
    private float timeS = -1;
    private float timePM = -1;
    private float timeEC = -1;
    private float timeEM = -1;
    private int damage;

    public EnemyChooser enemyChooser;
    public HealthManager healthManager;
    public EnterPlayer PlayerEnter;
    public AbilityChooser abilityChooser;
    public GameObject enterKey;

    private Color enterKeyColor;
    private Enemy e;

	// Use this for initialization
	void Start () {
        state = States.START;
        enterKeyColor = enterKey.GetComponent<Image>().color;
        enterKeyColor.a = 0f;
        enterKey.GetComponent<Image>().color = enterKeyColor;
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
            EMRanOnce = false;
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
            if (Time.time - timePM >= 1.5f && timePM != -1) {
                if (healthManager.GetHealth_e() > 0) {
                    NextState();
                }
                else {
                    Win();
                }
            }
        }

        if (state == States.ENEMYCHOICE) {
            PMRanOnce = false;
            flashTimer = Mathf.PingPong(Time.time * 2, 1);

            if (!ECRanOnce) {
                timeEC = Time.time;
            }
            else {
                if (flashTimer >= 0.9) {
                    if (attNum == 1) {
                        enemyChooser.enemyStat.text = "<color=red>Attack 1: " + e.Attack1Title + "\n" +
                                                      "Damage: " + e.Attack1Damage + "</color>\n" +
                                                      "Attack 2: " + e.Attack2Title + "\n" +
                                                      "Damage: " + e.Attack2Damage;
                    }
                    if (attNum == 2) {
                        enemyChooser.enemyStat.text = "Attack 1: " + e.Attack1Title + "\n" +
                                                      "Damage: " + e.Attack1Damage + "\n" +
                                                      "<color=red>Attack 2: " + e.Attack2Title + "\n" +
                                                      "Damage: " + e.Attack2Damage + "</color>";
                    }
                }
                if (flashTimer <= 0.1) {
                    WhiteText();
                }
            }
            if (Time.time - timeEC >= 1f && timeEC != -1) {
                showEnterKey = true;
                if (Input.GetKeyDown(KeyCode.Return)) {
                    NextState();
                }
            }
        }

        if (state == States.ENEMYMOVE) {
            ECRanOnce = false;
            showEnterKey = false;
            WhiteText();
            if (!EMRanOnce) {
                if (attNum == 1) {
                    healthManager.LoseHealth(e.Attack1Damage);
                }
                if (attNum == 2) {
                    healthManager.LoseHealth(e.Attack2Damage);
                }
                EMRanOnce = true;
                timeEM = Time.time;
            }
            if (Time.time - timeEM >= 1.5f && timeEM != -1) {
                if (healthManager.GetHealth() > 0) {
                    NextState();
                }
                else {
                    Lose();
                }
            }
        }

        if (showEnterKey) {
            if (enterKeyColor.a < 1f) {
                enterKeyColor.a += 0.005f;
                enterKey.GetComponent<Image>().color = enterKeyColor;
            }
            enterKey.transform.localScale = new Vector3(Mathf.PingPong(Time.time * 0.5f, 0.3f) + 1f, Mathf.PingPong(Time.time * 0.5f, 0.3f) + 1f, 1f);
        }
        else {
            if (enterKeyColor.a > 0) {
                enterKeyColor.a -= 0.01f;
                enterKey.GetComponent<Image>().color = enterKeyColor;
                enterKey.transform.localScale = new Vector3(enterKey.transform.localScale.x - enterKey.transform.localScale.x / 100, enterKey.transform.localScale.y - enterKey.transform.localScale.y / 100, 1f);
            }
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

    void WhiteText() {
        enemyChooser.enemyStat.text = "Attack 1: " + e.Attack1Title + "\n" +
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

    void Win() {
        showEnterKey = true;
        XPManager.xpmanager.addxp((int)(e.HP * 1.5f));
    }

    void Lose() {

    }
}