using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StateHandler : MonoBehaviour {

    private enum States { START, PLAYERCHOICE, PLAYERMOVE, ENEMYCHOICE, ENEMYMOVE }
    private bool RanOnce = false;
    private bool SRanOnce = false;
    private bool PCRanOnce = false;
    private bool PMRanOnce = false;
    private bool ECRanOnce = false;
    private bool EMRanOnce = false;
    private bool WRanOnce = false;
    private bool LRanOnce = false;
    private bool healing = false;
    private bool showEnterKey = false;
    private double flashTimer = 1.0;
    private float timeS = -1;
    private float timePM = -1;
    private float timeEC = -1;
    private float timeEM = -1;
    private float timeW = -1;
    private float timeL = -1;
    private int attNum = 0;
    private int actionValue;
    private int lostmoney;

    private States state;
    private Color enterKeyColor;
    private Enemy e;
    private GameObject player;
    private EnemySprite server;

    public EnemyChooser enemyChooser;
    public HealthManager healthManager;
    public EnterPlayer PlayerEnter;
    public AbilityChooser abilityChooser;
    public GameObject enterKey;
    public Text EndText;
    public Sprite[] listOfSprites = new Sprite[9];

	// Use this for initialization
	void Start () {
        state = States.START;
        enterKeyColor = enterKey.GetComponent<Image>().color;
        enterKeyColor.a = 0f;
        enterKey.GetComponent<Image>().color = enterKeyColor;
        player = GameObject.Find("Player");
        EnemySprite[] listOfEnemies = FindObjectsOfType<EnemySprite>();
        foreach (EnemySprite i in listOfEnemies) {
            if (i.ID == 7) {
                server = i;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        print("Current battle state: " + state);

        if (Input.GetKeyDown(KeyCode.L)) {
            Lose();
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            Win();
        }

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
                print("Ability chosen: " + abilityChooser.selectedAbility);
                if (abilityChooser.selectedAbility != 3) {
                    NextState();
                }
                else if (!healing) {
                    abilityChooser.WhiteText(false);
                    abilityChooser.SelectHealer(1);
                    healing = true;
                }
                else {
                    abilityChooser.WhiteHealText(false);
                    NextState();
                }
            }
            if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftArrow)) && healing) {
                healing = false;
                abilityChooser.WhiteHealText(false);
                abilityChooser.SelectAbility(3);
            }
        }

        if (state == States.PLAYERMOVE) {
            PCRanOnce = false; healing = false;
            if (!PMRanOnce) {
                timePM = Time.time;
                actionValue = abilityChooser.GetLastDoneDamage();
                abilityChooser.WhiteText(true);
                if (actionValue >= 0) {
                    healthManager.EnemyLoseHealth(actionValue);
                }
                else {
                    healthManager.LoseHealth(actionValue);
                }
                PMRanOnce = true;
            }
            if (Time.time - timePM >= 1.5f && timePM != -1) {
                if (healthManager.GetHealth_e() >= 1) {
                    NextState();
                }
                else if (healthManager.GetHealth_e() <= 0) {

                    if(e.ID == 5)
                    {

                        if (GameManager.instance.questManager.questLog.ContainsKey("Quest006"))
                        {
                            if (!GameManager.instance.questManager.questLog["Quest006"].QuestCompleted())
                            {
                                GameManager.instance.questManager.CompleteObjective("Quest006DefeatNoni");
                                GameManager.instance.questManager.AddQuestToLog("Quest007");
                            }
                        }

                        if (GameManager.instance.questManager.questLog.ContainsKey("Quest008"))
                        {
                            if (!GameManager.instance.questManager.questLog["Quest008"].QuestCompleted())
                            {
                                GameManager.instance.questManager.CompleteObjective("Quest008DefeatNoni");
                                GameManager.instance.questManager.AddQuestToLog("Quest009");
                            }
                        }
                    }

                    if(e.ID == 6)
                    {
                        if (GameManager.instance.questManager.questLog.ContainsKey("Quest009"))
                        {
                            if (!GameManager.instance.questManager.questLog["Quest009"].QuestCompleted())
                            {
                                GameManager.instance.questManager.CompleteObjective("Quest009FindDefeatMeemstar");
                                GameManager.instance.questManager.AddQuestToLog("Quest010");
                            }
                        }
                    }

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
                        if (e.Type != "server") {
                            enemyChooser.enemyStat.text = "<color=red>Attack 1: " + e.Attack1Title + "\n" +
                                                          "Damage: " + e.Attack1Damage + "</color>\n" +
                                                          "Attack 2: " + e.Attack2Title + "\n" +
                                                          "Damage: " + e.Attack2Damage;
                        }
                        else {
                            enemyChooser.enemyStat.text = "<color=red>Attack 1: " + e.Attack1Title + "\n" +
                                                          "Damage: ???</color>\n" +
                                                          "Attack 2: " + e.Attack2Title + "\n" +
                                                          "Damage: " + e.Attack2Damage;
                        }
                    }
                    if (attNum == 2) {
                        if (e.Type != "server") {
                            enemyChooser.enemyStat.text = "Attack 1: " + e.Attack1Title + "\n" +
                                                          "Damage: " + e.Attack1Damage + "\n" +
                                                          "<color=red>Attack 2: " + e.Attack2Title + "\n" +
                                                          "Damage: " + e.Attack2Damage + "</color>";
                        }
                        else {
                            enemyChooser.enemyStat.text = "Attack 1: " + e.Attack1Title + "\n" +
                                                          "Damage: ???\n" +
                                                          "<color=red>Attack 2: " + e.Attack2Title + "\n" +
                                                          "Damage: " + e.Attack2Damage + "</color>";
                        }
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
                int damage;
                if (attNum == 1) {
                    if (e.Type != "server") {
                        damage = e.Attack1Damage - PersistentInventoryScript.instance.itemDefense / 5;
                        if (damage < 1) {
                            damage = 1;
                        }
                        healthManager.LoseHealth(damage);
                    }
                    else {
                        damage = (e.Attack1Damage + Random.Range(-10, 10)) - PersistentInventoryScript.instance.itemDefense / 5;
                        if (damage < 1) {
                            damage = 1;
                        }
                        healthManager.LoseHealth(damage);
                        ChangeServerSprite();
                    }
                }
                if (attNum == 2) {
                    damage = e.Attack2Damage - PersistentInventoryScript.instance.itemDefense / 5;
                    if (damage < 1) {
                        damage = 1;
                    }
                }
                EMRanOnce = true;
                timeEM = Time.time;
            }
            if (Time.time - timeEM >= 1.5f && timeEM != -1) {
                if (healthManager.GetHealth() >= 1) {
                    NextState();
                }
                else if (healthManager.GetHealth() <= 0) {
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
                Vector3 s = enterKey.transform.localScale;
                enterKey.transform.localScale = new Vector3(s.x - s.x / 100, s.y - s.y / 100, 1f);
            }
        }
    }

    void LateUpdate() {
        if (!RanOnce) {
            e = enemyChooser.currentEnemy.linkedEnemy;
            RanOnce = true;
        }
        if (state == States.ENEMYCHOICE && !ECRanOnce) {
            ECRanOnce = true;
            attNum = GetAttack();
        }
    }

    private int GetAttack() {
        int attackNum = 0;
        int a1d = e.Attack1Damage;
        int a2d = e.Attack2Damage;
        //int a3d = e.Attack3Damage;
        int totalDam = a1d + a2d/* + a3d*/;

        int rndAtt = Random.Range(0, totalDam);
        if (rndAtt <= a1d) { attackNum = 1; }
        else if (rndAtt <= a1d + a2d) { attackNum = 2; }
        else { attackNum = 3; }

        return attackNum;
    }

    private void WhiteText() {
        if (e.Type != "server") {
            enemyChooser.enemyStat.text = "Attack 1: " + e.Attack1Title + "\n" +
                                          "Damage: " + e.Attack1Damage + "\n" +
                                          "Attack 2: " + e.Attack2Title + "\n" +
                                          "Damage: " + e.Attack2Damage;
        }
        else {
            enemyChooser.enemyStat.text = "Attack 1: " + e.Attack1Title + "\n" +
                                          "Damage: ???\n" +
                                          "Attack 2: " + e.Attack2Title + "\n" +
                                          "Damage: " + e.Attack2Damage;
        }
    }

    private void NextState() {
        state = (States)((int)state + 1);
        if ((int)state > 4) {
            state = States.PLAYERCHOICE;
        }
    }

    private void LoadLastScene() {
        string scene = PlayerPrefsManager.GetCurrentScene();
        SceneManager.LoadScene(scene);
        player.transform.position = PlayerPrefsManager.GetPositionInLevel(scene, player);
    }

    private int moneygain() {
        float moneygain_ = (e.HP * 1.25f);
        return (int)moneygain_;
    }

    private int xpgain() {
        float xpgain_ = (e.HP * 1.5f);
        return (int)xpgain_;
    }

    private int moneylost() {
        float moneylost_ = PersistentInventoryScript.instance.Currency * 0.15f;
        return (int)moneylost_;
    }

    private void ChangeServerSprite() {
        Sprite sprite = listOfSprites[Random.Range(0, listOfSprites.Length)];
        server.GetComponent<Image>().overrideSprite = sprite;
    }

    private void Win() {
        if (!WRanOnce) {
            timeW = Time.time;
            PersistentInventoryScript.instance.Currency += moneygain();
            XPManager.xpmanager.addxp(xpgain());
            PlayerPrefsManager.SetPlayerHealth((int)healthManager.GetHealth());
            WRanOnce = true;
        }
        if (Time.time - timeW >= 1f && timeW != -1) {
            EndText.text = "You have defeated <i><color=#ffff66>" + e.Title + "</color></i> and received <i><color=#ffff66> " + moneygain() + " currency</color></i> and gained <i><color=#ffff66>" + xpgain() + " experience.</color></i>" + "\nCurrent Level: <i><color=#ffff66>" + XPManager.xpmanager.playerlvl_() + "</color></i>\nExperience till level-up: <i><color=#ffff66>" + XPManager.xpmanager.xptonext_() + "XP</color></i>";
            showEnterKey = true;
            if (Input.GetKeyDown(KeyCode.Return)) {
                LoadLastScene();
            }
        }
    }

    private void Lose()
    {
        if (Time.time - timeL >= 1f && timeL != -1)
        {
            EndText.text = "You were defeated by <i><color=#ffff66>" + e.Title + "</color></i> and dropped <i><color=#ffff66>" + lostmoney + " currency</color></i>." + "You didn't gain any <i><color=#ffff66>experience.</color></i>" + "\nCurrent Level: <i><color=#ffff66>" + XPManager.xpmanager.playerlvl_() + "</color></i>\nExperience till level-up: <i><color=#ffff66>" + XPManager.xpmanager.xptonext_() + "XP</color></i>";
            showEnterKey = true;
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadLastScene();
            }
        }
        if (!LRanOnce)
        {
            lostmoney = moneylost();
            timeL = Time.time;
            PersistentInventoryScript.instance.Currency -= lostmoney;
            
            LRanOnce = true;
        }

    }
}