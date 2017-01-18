using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability {

    public int ID { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public int WeaponID { get; set; }

    //Constructor for an ability
    /// <summary>
    /// Create a new ability
    /// </summary>
    public Ability(int id, string type, string title, int weaponid) {
        this.ID = id;
        this.Type = type;
        this.Title = title;
        this.WeaponID = weaponid;
    }

    //Constructor for fists
    /// <summary>
    /// Create the fist ability
    /// </summary>
    public Ability(int id, string type, string title) {
        this.ID = id;
        this.Type = type;
        this.Title = title;
    }
}
