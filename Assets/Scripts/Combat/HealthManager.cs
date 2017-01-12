using UnityEngine;
using System.Collections;

public class HealthManager : MonoBehaviour {
    public GameObject fullTex;
    private float health = PlayerFSM.player.Health;
    private float maxHealth = PlayerFSM.player.MaxHealth;
    private float relHealth;
    private float dispHealth;

    void Start() {
        relHealth = health / maxHealth;
        dispHealth = relHealth;
    }

    void Update() {
        relHealth = health / maxHealth;
        fullTex.transform.localScale = new Vector3(dispHealth, 1f, 1f);
        if (relHealth != dispHealth) {
            dispHealth -= (dispHealth - relHealth) / 35;
            if (Mathf.Abs(dispHealth - relHealth) <= 0.002) {
                dispHealth = relHealth;
            }
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus)) {
            LoseHealth(-10);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus)) {
            LoseHealth(20);
        }
    }

    public void LoseHealth(float amount) {
        health -= amount;
    }
}