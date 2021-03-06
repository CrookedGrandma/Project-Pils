﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour {

    public static XPManager xpmanager;

    public double xpscaler = 1.25;
    public double hpscaler = 0.10;
    public int playerxp = 0; // waarde wordt in Start() uit save gehaald
    public double startxpbound = 100; // level 1 => XP van 0-100
    private int playerlvl = 1;
    private int xptonext = 0;
    private int playermaxhp = 100;

    void Awake() {
        if (xpmanager != null) {
            Destroy(gameObject);
        }
        else {
            xpmanager = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }

    void Start() {
        playerxp = PlayerPrefsManager.GetPlayerXP();
        Level();
        Health();
    }

    // berekent level opnieuw op basis van xp
    public void Level() {

        double calcxp = startxpbound;
        double currxp = (double)playerxp;
        double xpleft = 0;

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
            calcxp = (calcxp + calcxp * xpscaler);
            currxp = currxp - calcxp;
        }
    }

    public int Health() {
        double calchp = 100;
        int currLvl = playerlvl_();
        for (int i = 0; i < currLvl; i++) {
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

    public void addxp(int xpgain) {
        playerxp += xpgain;
    }
}
