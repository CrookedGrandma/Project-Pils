using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using LitJson;

public class AbilityChooser : MonoBehaviour {
    
    public Text A1T;
    public Text A2T;
    public Text AHT;

    private Ability[] UsableAbilities = new Ability[2];
    private List<Ability> AbilityDatabase = new List<Ability>();
    private JsonData Abilities;
    private ItemDatabase database;

    private Item[] weapon = new Item[2];
    private string Ability1Text;
    private string Ability2Text;
    private string AbilityHText;

    private bool started = false;
    private bool FoundPrimaryWeapon = false;
    public int selectedAbility = -1;

    // Use this for initialization, CALLED BY STATEHANDLER
    public void GetStarted () {
        started = true;
        Abilities = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Abilities.json"));
        database = GetComponent<ItemDatabase>();
        ConstructAbilityDatabase();
        FindUsables();
        weapon[0] = database.FetchItemById(UsableAbilities[0].WeaponID);
        try {
            weapon[1] = database.FetchItemById(UsableAbilities[1].WeaponID);
        }
        catch (Exception e) { }
        WhiteText();
        selectedAbility = 1;
        SelectAbility(1);
    }

    private void Update() {
        if (started) {
            //Debug {
            if (Input.GetKeyDown(KeyCode.Keypad0)) {
                SelectAbility(-1);
            }
            //}

            A1T.text = Ability1Text;
            A2T.text = Ability2Text;
            AHT.text = AbilityHText;
        }
    }

    private void ConstructAbilityDatabase() {
        for (int i = 0; i < Abilities.Count; i++) {
            AbilityDatabase.Add(new Ability((int)Abilities[i]["id"], Abilities[i]["type"].ToString(),
                                            Abilities[i]["title"].ToString(), (int)Abilities[i]["weaponid"]));
        }
    }

    private void FindUsables() {
        int equippedWeapon = PersistentInventoryScript.instance.equipmentList[0, 0];
        int equippedRanged = PersistentInventoryScript.instance.equipmentList[1, 0];
        for (int i = 0; i < AbilityDatabase.Count; i++) {
            if (AbilityDatabase[i].WeaponID == equippedWeapon) {
                UsableAbilities[0] = AbilityDatabase[i];
                FoundPrimaryWeapon = true;
            }
            if (AbilityDatabase[i].WeaponID == equippedRanged) {
                UsableAbilities[1] = AbilityDatabase[i];
            }
        }
        if (!FoundPrimaryWeapon) {
            UsableAbilities[0] = AbilityDatabase[0];
        }
    }

    private void WhiteText() {
        Ability1Text = UsableAbilities[0].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon[0].Title + "</color> (Damage: " + weapon[0].Damage + ")";
        try {
            Ability2Text = UsableAbilities[1].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon[1].Title + "</color> (Damage: " + weapon[1].Damage + ")";
        }
        catch (Exception e) { }
        AbilityHText = "Heal <color=#bbbbbb>with</color> <color=#cccccc>Drinks</color> <color=#bbbbbb>or</color> <color=#cccccc>Food</color>";
    }

    public void SelectAbility(int a) {
        WhiteText();
        if (a == -1) {
            selectedAbility = -1;
        }
        if (a == 1) {
            Ability1Text = "<color=red>" + UsableAbilities[0].Title + "</color> <color=#ff7777>using your</color> <color=#ff8888>" + weapon[0].Title + "</color> <color=red>(Damage: " + weapon[0].Damage + ")</color>";
            selectedAbility = 1;
        }
        if (a == 2) {
            try {
                Ability2Text = "<color=red>" + UsableAbilities[1].Title + "</color> <color=#ff7777>using your</color> <color=#ff8888>" + weapon[1].Title + "</color> <color=red>(Damage: " + weapon[1].Damage + ")</color>";
                selectedAbility = 2;
            }
            catch (Exception e) {
                if (selectedAbility == 1) {
                    SelectAbility(3);
                }
                else if (selectedAbility == 3) {
                    SelectAbility(1);
                }
            }
        }
        if (a == 3) {
            AbilityHText = "<color=red>Heal</color> <color=#ff7777>with</color> <color=#ff8888>Drinks</color> <color=#ff7777>or</color> <color=#ff8888>Food</color>";
            selectedAbility = 3;
        }
    }

    public int GetLastDoneDamage() {
        return weapon[selectedAbility - 1].Damage;
    }
}