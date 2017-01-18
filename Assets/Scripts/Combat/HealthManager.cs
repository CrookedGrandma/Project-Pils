using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthManager : MonoBehaviour {
    public GameObject fullTex;
    public Text HPText;
    private float initHealth;
    private float maxHealth = XPManager.xpmanager.Health() /* + hp van items */ + PersistentInventoryScript.instance.itemHealth;
    private float health = 100; //uit save
    private float relHealth;
    private float dispHealth;

    void Start() {
        relHealth = health / maxHealth;
        dispHealth = relHealth;
        initHealth = health;
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
        fullTex.transform.localScale = new Vector3(dispHealth, 1f, 1f);
        if (relHealth != dispHealth) {
            dispHealth -= (dispHealth - relHealth) / 30;
            if (Mathf.Abs(dispHealth - relHealth) <= 0.002) {
                dispHealth = relHealth;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            LoseHealth(-10);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            LoseHealth(10);
        }
    }

    public void LoseHealth(float amount) {
        health -= amount;
    }

    /*public void LoseCombat() {
        health = initHealth;
    }

    public void WinCombat() {
        XPManager.xpmanager.playercurrhp = health;
    }*/

    public float Health { get { return health; } }
}