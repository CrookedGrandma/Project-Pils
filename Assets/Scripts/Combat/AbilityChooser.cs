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

    private List<Ability> UsableAbilities = new List<Ability>();
    private List<Ability> AbilityDatabase = new List<Ability>();
    private JsonData Abilities;

    private string[] weapon = new string[2];
    private string Ability1Text;
    private string Ability2Text;
    private string AbilityHText;

    public static int selectedAbility = -1;

    // Use this for initialization
    void Start () {
        Abilities = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/Abilities.json"));
        ConstructAbilityDatabase();
        FindUsables();
        try {
            weapon[0] = PersistentInventoryScript.instance.itemList[UsableAbilities[0].WeaponID, 0].ToString();
        }
        catch (Exception e) { }
        try {
            weapon[1] = PersistentInventoryScript.instance.itemList[UsableAbilities[1].WeaponID, 0].ToString();
        }
        catch (Exception e) { }
        WhiteText();
        selectedAbility = 1;
        //SelectAbility(1);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Keypad0)) {
            SelectAbility(-1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad1)) {
            SelectAbility(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2)) {
            SelectAbility(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3)) {
            SelectAbility(3);
        }

        A1T.text = Ability1Text;
        A2T.text = Ability2Text;
        AHT.text = AbilityHText;
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
            }
            if (AbilityDatabase[i].WeaponID == equippedRanged) {
                UsableAbilities[1] = AbilityDatabase[i];
            }
        }
    }

    private void WhiteText() {
        try {
            Ability1Text = UsableAbilities[0].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon[0] + "</color>";
        }
        catch (Exception e) { }
        try {
            Ability2Text = UsableAbilities[1].Title + " <color=#bbbbbb>using your</color> <color=#cccccc>" + weapon[1] + "</color>";
        }
        catch (Exception e) { }
        AbilityHText = "Heal <color=#bbbbbb>with</color> <color=#cccccc>Drinks</color> <color=#bbbbbb>or</color> <color=#cccccc>Food</color>";
    }

    public void SelectAbility(int a) {
        if (a == -1) {
            WhiteText();
            selectedAbility = -1;
        }
        if (a == 1) {
            try {
                Ability1Text = "<color=red>" + UsableAbilities[0].Title + "</color> <color=#ff7777>using your</color> <color=#ff8888>" + weapon[0] + "</color>";
                selectedAbility = 1;
            }
            catch (Exception e) {
                SelectAbility(2);
            }
        }
        if (a == 2) {
            try {
                Ability2Text = "<color=red>" + UsableAbilities[1].Title + "</color> <color=#ff7777>using your</color> <color=#ff8888>" + weapon[1] + "</color>";
                selectedAbility = 2;
            }
            catch (Exception e) {
                if (selectedAbility == 1) {
                    SelectAbility(3);
                }
                if (selectedAbility == 3) {
                    SelectAbility(1);
                }
            }
        }
        if (a == 3) {
            AbilityHText = "<color=red>Heal</color> <color=#ff7777>with</color> <color=#ff8888>Drinks</color> <color=#ff7777>or</color> <color=#ff8888>Food</color>";
            selectedAbility = 3;
        }
    }
}