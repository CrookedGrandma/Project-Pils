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

    private static List<Ability> UsableAbilities = new List<Ability>();
    private static List<Ability> AbilityDatabase = new List<Ability>();
    private static JsonData Abilities;

    private static string[] weapon = new string[2];
    private static string Ability1Text;
    private static string Ability2Text;
    private static string AbilityHText;

    private static bool started = false;
    private static bool WeAreNumberOne = false;
    public static int selectedAbility = -1;

    // Use this for initialization, CALLED BY STATEHANDLER
    public static void GetStarted () {
        started = true;
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

    private static void ConstructAbilityDatabase() {
        for (int i = 0; i < Abilities.Count; i++) {
            AbilityDatabase.Add(new Ability((int)Abilities[i]["id"], Abilities[i]["type"].ToString(),
                                            Abilities[i]["title"].ToString(), (int)Abilities[i]["weaponid"]));
        }
    }

    private static void FindUsables() {
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

    private static void WhiteText() {
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

    public static void SelectAbility(int a) {
        if (a == -1) {
            WhiteText();
            selectedAbility = -1;
        }
        if (a == 1) {
            WeAreNumberOne = true;
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
                if (selectedAbility == 1 || WeAreNumberOne) {
                    SelectAbility(3);
                }
                else if (selectedAbility == 3) {
                    SelectAbility(1);
                }
            }
        }
        if (a == 3) {
            WeAreNumberOne = false;
            AbilityHText = "<color=red>Heal</color> <color=#ff7777>with</color> <color=#ff8888>Drinks</color> <color=#ff7777>or</color> <color=#ff8888>Food</color>";
            selectedAbility = 3;
        }
    }
}