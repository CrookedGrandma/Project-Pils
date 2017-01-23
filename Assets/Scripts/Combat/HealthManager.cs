using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {
    public GameObject GreenBar;
    public Text HPText;
    public GameObject RedBar;
    public Text HPText_e;
    public EnemyChooser enemyChooser;
    private bool started = false;
    private float initHealth;
    private float maxHealth = XPManager.xpmanager.Health() + PersistentInventoryScript.instance.itemHealth;
    private float health;
    private float relHealth;
    private float dispHealth;
    private float maxHealth_e;
    private float health_e;
    private float relHealth_e;
    private float dispHealth_e;

    void Start() {
        health = PlayerPrefsManager.GetPlayerHealth();
        relHealth = health / maxHealth;
        dispHealth = relHealth;
        initHealth = health;
        started = true;
    }

    void StartEnemyHealth() {
        maxHealth_e = enemyChooser.currentEnemy.linkedEnemy.HP;
        health_e = maxHealth_e;
        relHealth_e = health_e / maxHealth_e;
        dispHealth_e = relHealth_e;
    }

    void Update() {
        if (started) {
            StartEnemyHealth();
            started = false;
        }
        // Update player's health bar
        HPText.text = "HP: " + health + " / " + maxHealth;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (health < 0) {
            health = 0;
        }
        relHealth = health / maxHealth;
        GreenBar.transform.localScale = new Vector3(dispHealth, 1f, 1f);
        if (relHealth != dispHealth) {
            dispHealth -= (dispHealth - relHealth) / 30;
            if (Mathf.Abs(dispHealth - relHealth) <= 0.002) {
                dispHealth = relHealth;
            }
        }

        //Update enemy's health bar
        HPText_e.text = "HP: " + health_e + " / " + maxHealth_e;
        if (health_e > maxHealth_e) {
            health_e = maxHealth_e;
        }
        if (health_e < 0) {
            health_e = 0;
        }
        relHealth_e = health_e / maxHealth_e;
        RedBar.transform.localScale = new Vector3(dispHealth_e, 1f, 1f);
        if (relHealth_e != dispHealth_e) {
            dispHealth_e -= (dispHealth_e - relHealth_e) / 30;
            if (Mathf.Abs(dispHealth_e - relHealth_e) <= 0.002) {
                dispHealth_e = relHealth_e;
            }
        }

        //Purely for development{
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            LoseHealth(-10);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            EnemyLoseHealth(25);
        }
        //}
    }

    public void LoseHealth(float amount) {
        health -= amount;
    }

    public void EnemyLoseHealth(float amount) {
        health_e -= amount;
    }

    public float GetHealth() {
        return health;
    }

    public float GetHealth_e() {
        return health_e;
    }
}