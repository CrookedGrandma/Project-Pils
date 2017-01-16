using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {
    public GameObject fullTex;
    private float initHealth;
    private float maxHealth = XPManager.xpmanager.Health();
    private float health = XPManager.xpmanager.playercurrhp;
    private float relHealth;
    private float dispHealth;

    void Start() {
        relHealth = health / maxHealth;
        dispHealth = relHealth;
        initHealth = health;
    }

    void Update() {
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
            LoseHealth(-100);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            LoseHealth(250);
        }
    }

    public void LoseHealth(float amount) {
        health -= amount;
    }

    public void LoseCombat() {
        XPManager.xpmanager.playercurrhp = initHealth;
    }

    public void WinCombat() {
        XPManager.xpmanager.playercurrhp = health;
    }

    public float Health { get { return health; } }
}