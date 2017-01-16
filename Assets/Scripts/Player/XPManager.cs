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
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> origin/master
    public int playerxp = 102599; // waarde normaal gehaald uit playerprefs
    public double startxpbound = 100; // level 1 => XP van 0-100
    public int playerlvl = 1;
    public int xptonext = 0;
<<<<<<< HEAD
=======
    public double hpscaler = 0.10;
    public int playerxp = 5098; // waarde normaal gehaald uit save
    public double startxpbound = 100; // level 1 => XP van 0-100
    public int playerlvl = 1;
    public int xptonext = 0;
    public int playermaxhp = 100;
    public float playercurrhp = 100; // uit save
>>>>>>> origin/master
=======
>>>>>>> origin/master

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
<<<<<<< HEAD
<<<<<<< HEAD
=======

    public int Health() {
        double calchp = playermaxhp;
        for (int i = 0; i < playerlvl_(); i++) {
            calchp = calchp + calchp * hpscaler;
        }
        playermaxhp = (int)calchp;
        return playermaxhp;
    }
>>>>>>> origin/master
=======
>>>>>>> origin/master

    public int xptonext_() {
        Level();
        return xptonext;
    }

    public int playerlvl_() {
        Level();
        return xptonext;
    }

}
