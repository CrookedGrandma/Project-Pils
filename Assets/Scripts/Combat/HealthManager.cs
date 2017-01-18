using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {
    public GameObject GreenBar;
    public Text HPText;
    public GameObject RedBar;
    public Text HPText_e;
    public EnemyChooser enemyChooser;
    private float initHealth;
    private float maxHealth = XPManager.xpmanager.Health() /* + hp van items */ + PersistentInventoryScript.instance.itemHealth;
    private float health = 100; //uit save
    private float relHealth;
    private float dispHealth;
    private float maxHealth_e;
    private float health_e;
    private float relHealth_e;
    private float dispHealth_e;

    void Start() {
        relHealth = health / maxHealth;
        dispHealth = relHealth;
        initHealth = health;
        maxHealth_e = enemyChooser.currentEnemy.linkedEnemy.HP;
        health_e = maxHealth_e;
        relHealth_e = health_e / maxHealth_e;
        dispHealth_e = relHealth_e;
    }

    void Update() {

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

        //Purely for development{
        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            LoseHealth(-25);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            LoseHealth(10);
        }
        //}
    }

    public void LoseHealth(float amount) {
        health -= amount;
    }

    public void EnemyLoseHealth(float amount) {

    }

    public float Health { get { return health; } }
}