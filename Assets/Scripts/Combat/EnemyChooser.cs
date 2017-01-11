using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EnemyChooser : MonoBehaviour {

    public List<Enemy> EnemyDatabase = new List<Enemy>();
    public JsonData EnemyStats;

    public EnemySprite currentEnemy;

    int enemyC = -1;
    int enviC = -1;
    bool enemyChosen;
    bool enemyFound;
    bool enemyEntered;
    bool enemyLoaded;
    bool enviSet;

    public Text announce;
    public Text enemyStat;

	// Use this for initialization
	public void Start () {
        if (ThirdPersonController.player != null) {
            enemyC = ThirdPersonController.player.Enemy;
            enviC = ThirdPersonController.player.Envi;
        }
        enviSet = false;
        enemyChosen = false;
        enemyFound = false;
        enemyEntered = false;

        EnemyStats = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Enemies.json"));
        ConstructEnemyDatabase();

        ThirdPersonController.player.PleaseDie = true;
	}

	// Update is called once per frame
	void Update () {
	    if (enemyC != -1) {
            Debug.Log("In combat: " + enemyC);
        }
        if (enviC != -1) {
            Debug.Log("In environment: " + enviC);
        }
        if (!enemyChosen) {
            EnemySprite[] currentEnemies = Object.FindObjectsOfType<EnemySprite>();
            foreach (EnemySprite e in currentEnemies) {
                e.opacity(0f);
                if (e.ID == enemyC) {
                    currentEnemy = e;
                    enemyFound = true;
                    announce.text = "You're up against: " + currentEnemy.linkedEnemy.Title;
                    enemyStat.text = "HP: " + currentEnemy.linkedEnemy.HP + "\n" +
                                     "Strength: " + currentEnemy.linkedEnemy.Strength + "\n" +
                                     "Attack 1: " + currentEnemy.linkedEnemy.Attack1Title + "\n" +
                                     "Damage: " + currentEnemy.linkedEnemy.Attack1Damage + "\n" +
                                     "Attack 2: " + currentEnemy.linkedEnemy.Attack2Title + "\n" +
                                     "Damage: " + currentEnemy.linkedEnemy.Attack2Damage;
                    e.opacity(1f);
                }
            }
            if (!enemyFound) {
                currentEnemy = null;
            }
            else if (!enemyEntered) {
                currentEnemy.pleaseEnter = true;
                enemyEntered = true;
            }
            enemyChosen = true;
        }

        if (!enviSet) {
            GetComponent<Environment>().changebg(enviC);
            enviSet = true;
        }

        if (Input.GetKeyDown(KeyCode.C)) {
            SceneManager.LoadScene("Main");
        }
    }

    private void ConstructEnemyDatabase() {
        for (int i = 0; i < EnemyStats.Count; i++) {
            if (EnemyStats[i]["type"].ToString() == "2attack") {
                EnemyDatabase.Add(new Enemy((int)EnemyStats[i]["id"], EnemyStats[i]["title"].ToString(),
                    EnemyStats[i]["type"].ToString(), (int)EnemyStats[i]["hp"], (int)EnemyStats[i]["strength"],
                    EnemyStats[i]["attack1title"].ToString(), (int)EnemyStats[i]["attack1damage"],
                    (double)EnemyStats[i]["attack1scaling"], EnemyStats[i]["attack2title"].ToString(),
                    (int)EnemyStats[i]["attack2damage"], (double)EnemyStats[i]["attack2scaling"]));
            }
        }
    }
}