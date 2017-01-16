using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour {

    public static XPManager xpmanager;

    void Awake() {
        if (xpmanager != null) {
            Destroy(gameObject);
        }
        else {
            xpmanager = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    public double xpscaler = 0.25;
    public double hpscaler = 0.10;
    public int playerxp = 5098; // waarde normaal gehaald uit save
    public double startxpbound = 100; // level 1 => XP van 0-100
    public int playerlvl = 1;
    public int xptonext = 0;
    public int playermaxhp = 100;
    public float playercurrhp = 100; // uit save

    // berekent level opnieuw op basis van xp
    public void Level() {

        double calcxp = startxpbound;
        double currxp = (double)playerxp;
        double xpleft = xptonext;

        for (int lvl = 1; lvl <= 100; lvl++) {

            if (currxp < calcxp) {
                playerlvl = lvl;
                xpleft = (calcxp - currxp) + 1;
                xptonext = (int)xpleft;
                break;
            }
            if (currxp == calcxp) {
                playerlvl = lvl++;
                xpleft = calcxp + 1;
                xptonext = (int)xpleft;
                break;
            }
            if (currxp > calcxp)
                calcxp = calcxp + calcxp * xpscaler;
        }
    }

    public int Health() {
        double calchp = playermaxhp;
        for (int i = 0; i < playerlvl_(); i++) {
            calchp = calchp + calchp * hpscaler;
        }
        playermaxhp = (int)calchp;
        return playermaxhp;
    }

    public int xptonext_() {
        Level();
        return xptonext;
    }

    public int playerlvl_() {
        Level();
        return playerlvl;
    }

    private void Start() {
        Health();
    }
}
