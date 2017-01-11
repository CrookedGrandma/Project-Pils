using UnityEngine;
using System.Collections;

public class Enemy {

    public int ID { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public int HP { get; set; }
    public int Strength { get; set; }
    public string Attack1Title { get; set; }
    public int Attack1Damage { get; set; }
    public double Attack1Scaling { get; set; }
    public string Attack2Title { get; set; }
    public int Attack2Damage { get; set; }
    public double Attack2Scaling { get; set; }
    public string Attack3Title { get; set; }
    public int Attack3Damage { get; set; }
    public double Attack3Scaling { get; set; }

    //Constructor for 2-attack enemy
    /// <summary>
    /// Load an enemy with two attacks
    /// </summary>
    public Enemy(int id, string title, string type, int hp, int strength, string attack1title, int attack1damage, double attack1scaling, string attack2title, int attack2damage, double attack2scaling) {
        double add1 = (double)strength * attack1scaling; // additional damage for ability 1, based on scaling% multiplied by enemy strength 
        double add2 = (double)strength * attack2scaling; // additional damage for ability 2, based on scaling% multiplied by enemy strength
        this.ID = id;
        this.Title = title;
        this.Type = type;
        this.HP = hp;
        this.Strength = strength;
        this.Attack1Title = attack1title;
        this.Attack1Damage = attack1damage + (int)add1; // damage + additional damage for ability 1
        this.Attack1Scaling = attack1scaling;
        this.Attack2Title = attack2title;
        this.Attack2Damage = attack2damage + (int)add2; // damage + additional damage for ability 2
        this.Attack2Scaling = attack2scaling;
        this.Attack3Title = null;
        this.Attack3Damage = 0;
        this.Attack3Scaling = 0;
    }
    //Constructor for nothing
    /// <summary>
    /// Load a completely empty enemy
    /// </summary>
    public Enemy() {
        this.ID = -1;
    }
}
